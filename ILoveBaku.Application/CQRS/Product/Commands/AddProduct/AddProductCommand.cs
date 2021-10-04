using AutoMapper;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProduct
{
    public class AddProductCommand : BaseRequest<ApiResult<int?>>
    {
        public ProductVm Model { get; set; }
        public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public AddProductCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(AddProductCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Create error"
                    });

                Products product = new Products
                {
                    Name = request.Model.Title.ToParameterizingRoute(),
                    DefaultBuyAmount = (decimal)request.Model.DefaultBuyAmount,
                    DefaultCostAmount = (decimal)request.Model.DefaultCostAmount,
                    DefaultSaleAmount = (decimal)request.Model.DefaultSaleAmount,
                    ProductGroupsId =(int)request.Model.ProductGroupId,
                    CreatedDate = DateTime.Now,
                    CreatedIp = 1,
                    UpdateDate = DateTime.Now,
                    IsActive = true,
                    DefaultPublishDate = request.Model.DefaultPublishDate,
                    Description = request.Model.Description
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                var langs = await _context.Langs.ToListAsync();
                for (int i = 0; i < langs.Count; i++)
                {
                    var productLang = new ProductsLangs()
                    {
                        Name = request.Model.ProductName,
                        Description = product.Description,
                        ProductsId = product.Id,
                        LangsId = langs[i].Id
                    };

                    await _context.ProductsLangs.AddAsync(productLang);
                }

                await _context.SaveChangesAsync();

                foreach (var specificationValue in request.Model.Values)
                {
                   
                    if(specificationValue.Value != null || specificationValue.Type == "string")
                    {
                        dynamic obj = null;
                        if (specificationValue.MultiData)
                            obj = new
                            {
                                CategoriesSpecificationsPropertiesId = specificationValue.PropertyId,
                                ProductsId = product.Id

                            };
                        else
                            obj = new
                            {
                                CategoriesSpecificationsPropertiesId = specificationValue.PropertyId,
                                ProductsId = product.Id,
                                Value = specificationValue.Type=="string"&&specificationValue.Value==null?"":specificationValue.Value,
                                IsManual = specificationValue.IsManual
                            };

                        var table = _context.GetType().GetProperty(specificationValue.TableName).GetValue(_context);
                        var addMethod = table.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
                        var paramType = addMethod.GetParameters()[0].ParameterType;

                        addMethod.Invoke(table, new object[] {
                        ConvertTo(obj, paramType)
                    });
                    }
                    
                }

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(product.Id);
            }
            public object ConvertTo(object obj, Type type)
            {
                var data = JsonConvert.SerializeObject(obj);
                var newObj = JsonConvert.DeserializeObject(data, type);
                return newObj;
            }
        }
    }
}
