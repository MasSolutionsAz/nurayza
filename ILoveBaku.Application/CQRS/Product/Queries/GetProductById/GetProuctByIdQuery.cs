using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Stored_Procedures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductById
{
    public class GetProuctByIdQuery : BaseRequest<ApiResult<ProductVm>>
    {
        public int ProductId { get; set; }
        public class GetProuctByIdQueryHandler : IRequestHandler<GetProuctByIdQuery, ApiResult<ProductVm>>
        {
            private readonly IApplicationDbContext _context;
            public GetProuctByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductVm>> Handle(GetProuctByIdQuery request, CancellationToken cancellationToken)
            {
                Products product = await _context.Products
                                                        .Include(c => c.ProductGroup)
                                                        .FirstOrDefaultAsync(c => c.Id == request.ProductId);
                if (product == null)
                {
                    request.Errors.Add("product", "Belə bir məhsul yoxdur.");
                    return ApiResult<ProductVm>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }


                ProductVm vm = new ProductVm
                {
                    Title = product.Name,
                    DefaultBuyAmount = product.DefaultBuyAmount,
                    DefaultSaleAmount = product.DefaultSaleAmount,
                    DefaultCostAmount = product.DefaultCostAmount,
                    IsActive = (bool)product.IsActive,
                    ProductGroupId = product.ProductGroupsId,
                    CategoryId = product.ProductGroup.CategoriesId,
                    DefaultPublishDate = product.DefaultPublishDate,
                    DefaultPublishDateDay = product.DefaultPublishDate?.Day.ToString(),
                    DefaultPublishDateMonth = product.DefaultPublishDate?.Month.ToString(),
                    DefaultPublishDateYear = product.DefaultPublishDate?.Year.ToString(),
                    Description = product.Description
                };

                List<ProductValueTable> tables = (await _context.GetValuesTablesByCategoryId(vm.CategoryId)).ToList();

                List<ProductSpecificationValueDto> values = new List<ProductSpecificationValueDto>();

                foreach (var table in tables)
                {
                    var x = _context.GetTable(table.TableName);
                    var data = _context.GetTable(table.TableName).ToList();
                    values.AddRange(data.Where(c => (int)c.GetType().GetProperty("ProductsId").GetValue(c) == request.ProductId && !(bool)c.GetType().GetProperty("IsDeleted").GetValue(c)).Select(c => new ProductSpecificationValueDto
                    {
                        PropertyId = (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c),
                        SpecificationId =(int)_context.CategoriesSpecificationsProperties.Where(a=>a.Id == (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c)).FirstOrDefault()?.CategoriesSpecificationId,
                        TableName = table.TableName,
                        Value = (c.GetType().GetProperty("Value") != null ? c.GetType().GetProperty("Value").GetValue(c) : c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c)).ToString(),
                        IsManual = (bool)(c.GetType().GetProperty("IsManual") != null ? c.GetType().GetProperty("IsManual").GetValue(c) : false),
                    }).Distinct().ToList());
                }

                vm.Values = values;
                return ApiResult<ProductVm>.CreateResponse(vm);
            }
        }
    }
}
