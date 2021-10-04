using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId;
using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Enums;
using ILoveBaku.Domain.Stored_Procedures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications
{
    public class GetProductSpecificationsQuery : BaseRequest<ApiResult<ProductSpecificationModel>>
    {
        public int ProductId { get; set; }
        public class GetProductSpecificationsQueryHandler : IRequestHandler<GetProductSpecificationsQuery, ApiResult<ProductSpecificationModel>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IConfiguration _configuration;
            public GetProductSpecificationsQueryHandler(IApplicationDbContext context, IConfiguration configuration)
            {
                _configuration = configuration;
                _context = context;
            }
            public async Task<ApiResult<ProductSpecificationModel>> Handle(GetProductSpecificationsQuery request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.Where(c => c.Id == request.ProductId).FirstOrDefaultAsync();
                if (product == null)
                    return ApiResult<ProductSpecificationModel>.CreateResponse(null);

                var categoryId = product.ProductGroup.CategoriesId;
                var tables = await _context.GetValuesTablesByCategoryId(categoryId);
                List<ProductSpecificationValueDto> values = new List<ProductSpecificationValueDto>();

                foreach (var table in tables)
                {
                    var x = _context.GetTable(table.TableName);
                    var data = _context.GetTable(table.TableName).ToList();
                    values.AddRange(data.Where(c => (int)c.GetType().GetProperty("ProductsId").GetValue(c) == request.ProductId && !(bool)c.GetType().GetProperty("IsDeleted").GetValue(c)).Select(c => new ProductSpecificationValueDto
                    {
                        PropertyId = (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c),
                        SpecificationId = (int)_context.CategoriesSpecificationsProperties.Where(a => a.Id == (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c)).FirstOrDefault()?.CategoriesSpecificationId,
                        TableName = table.TableName,
                        Value = (c.GetType().GetProperty("Value") != null ? c.GetType().GetProperty("Value").GetValue(c) : c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c)).ToString(),
                        MultiData = c.GetType().GetProperty("Value") == null ? true : false,
                        IsManual = (bool)(c.GetType().GetProperty("IsManual") != null ? c.GetType().GetProperty("IsManual").GetValue(c) : false),
                        Type = _context.CategoriesSpecifications.Where(b=> b.Id  == (int)_context.CategoriesSpecificationsProperties.Where(a => a.Id == (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c)).FirstOrDefault().CategoriesSpecificationId).Select(c=>c.CategoriesSpecificationsType.Name).FirstOrDefault()
                    }).Distinct().ToList());
                }

                List<ProductSpecificationDto> productSpecificationDtos = new List<ProductSpecificationDto>();
                foreach (var item in values)
                {
                    productSpecificationDtos.Add(new ProductSpecificationDto
                    {
                        SpecificationId = await _context.CategoriesSpecificationsLangs.Where(c => c.Lang.Culture == request.Culture && c.CategoriesSpecificationsId == item.SpecificationId).Select(c => c.CategoriesSpecificationsId).FirstOrDefaultAsync(),
                        SpecificationName = await _context.CategoriesSpecificationsLangs.Where(c => c.Lang.Culture == request.Culture && c.CategoriesSpecificationsId == item.SpecificationId).Select(c => c.Name).FirstOrDefaultAsync(),
                        Value = item.MultiData ? await _context.CategoriesSpecificationsPropertiesLangs.Where(l => l.Lang.Culture == request.Culture && l.CategoriesSpecificationsPropertiesId == Convert.ToInt32(item.Value)).Select(l => l.Name).FirstOrDefaultAsync() : (item.Type=="string"?_context.ProductsSpecificationsValuesStringsLangs.Where(c=> c.ProductsStockSpecificationsValuesStrings.CategoriesSpecificationsPropertiesId == item.PropertyId && c.Langs.Culture == request.Culture && !c.IsDeleted && c.ProductsStockSpecificationsValuesStrings.ProductsId == product.Id).FirstOrDefault()?.Value: item.Value),
                        MultiData = false
                    });
                }


                var productIds = _context.Products.Where(c => c.ProductGroupsId == product.ProductGroupsId).Select(c => c.Id).ToList();

                var result = new List<ColorDto>();
                foreach (var item in await _context.ProductsStockSpecificationsValuesList.Where(c => !c.IsDeleted
                                                                                     &&
                                                                                     c.CategoriesSpecificationsProperties.CategoriesSpecificationId == Convert.ToInt32(_configuration["Specifications:Color"])
                                                                                     &&
                                                                                     productIds.Any(a => a == c.ProductsId)).ToListAsync())
                {

                    var color = new ColorDto
                    {
                        ListValueId = item.Id,
                        Name = _context.CategoriesSpecificationsLangs.Where(a => a.CategoriesSpecificationsId == item.CategoriesSpecificationsProperties.CategoriesSpecificationId &&
                                                                                                                                                a.Lang.Culture == request.Culture).Select(a => a.Name).FirstOrDefault(),
                        ProductId = item.ProductsId,
                        PropertyId = item.CategoriesSpecificationsPropertiesId,
                        ProductName = item.Products.Name.ToParameterizingRoute(),
                        Value = _context.CategoriesSpecificationsPropertiesLangs.Where(b => b.Lang.Culture == request.Culture && b.CategoriesSpecificationsPropertiesId == item.CategoriesSpecificationsPropertiesId).Select(b => b.Name).FirstOrDefault(),
                        Sizes = await _context.ProductsStockSpecificationsValuesList.Where(c => !c.IsDeleted && c.CategoriesSpecificationsProperties.CategoriesSpecificationId == Convert.ToInt32(_configuration["Specifications:Size"])
                                                                                                && c.Products.ProductGroupsId == product.ProductGroupsId && _context.ProductsStockSpecificationsValuesList.Any(a => !a.IsDeleted && a.CategoriesSpecificationsPropertiesId == item.CategoriesSpecificationsPropertiesId
                                                                                                                                                                                                                    && a.ProductsId == c.ProductsId)).Select(b => new SizeDto
                                                                                                                                                                                                                    {
                                                                                                                                                                                                                        Name = _context.CategoriesSpecificationsLangs.Where(a => a.CategoriesSpecificationsId == b.CategoriesSpecificationsProperties.CategoriesSpecificationId &&
                                                                                                                                                                                                                                                                                                                                a.Lang.Culture == request.Culture).Select(a => a.Name).FirstOrDefault(),

                                                                                                                                                                                                                        Value = _context.CategoriesSpecificationsPropertiesLangs.Where(a => a.Lang.Culture == request.Culture && a.CategoriesSpecificationsPropertiesId == b.CategoriesSpecificationsPropertiesId).Select(a => a.Name).FirstOrDefault(),
                                                                                                                                                                                                                        ProductId = b.ProductsId,
                                                                                                                                                                                                                        ProductCount = _context.ProductsStock.Where(k => k.ProductId == b.ProductsId).Select(k => k.Count).FirstOrDefault(),
                                                                                                                                                                                                                        ProductName = b.Products.Name.ToParameterizingRoute()
                                                                                                                                                                                                                    }).ToListAsync()
                    };
                    result.Add(color);
                }



                ProductSpecificationModel model = new ProductSpecificationModel
                {
                    Colors = result,
                    Specifications = productSpecificationDtos
                };
                return ApiResult<ProductSpecificationModel>.CreateResponse(model);
            }

            private int GetProductIdByProperty(int propertyId, List<ProductValueTable> tables)
            {
                int productId = 0;
                foreach (var table in tables)
                {
                    var data = _context.GetTable(table.TableName).ToList();
                    object result = data.Where(c => (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c) == propertyId && !(bool)c.GetType().GetProperty("IsDeleted").GetValue(c)).Select(c => c.GetType().GetProperty("PropertyId")).FirstOrDefault();
                    if (result != null)
                    {
                        productId = (int)result;
                        break;
                    }
                }

                return productId;
            }
        }
    }
}
