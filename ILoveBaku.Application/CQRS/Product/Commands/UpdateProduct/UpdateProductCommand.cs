using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.UpdateProduct
{
    public class UpdateProductCommand : BaseRequest<ApiResult<int?>>
    {
        public ProductVm Model { get; set; }
        public int ProductId { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProductCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Validation Error"
                    });

                Products products = await _context.Products.FirstOrDefaultAsync(c => c.Id == request.ProductId);
                if (products == null)
                {
                    request.Errors.Add("product", "Belə bir məhsul yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update Error"
                    });
                }

                products.DefaultBuyAmount = (decimal)request.Model.DefaultBuyAmount;
                products.DefaultCostAmount = (decimal)request.Model.DefaultCostAmount;
                products.DefaultSaleAmount = (decimal)request.Model.DefaultSaleAmount;
                products.UpdateDate = DateTime.Now;
                products.ProductGroupsId = (int)request.Model.ProductGroupId;
                products.IsActive = true;
                products.Name = request.Model.Title.ToParameterizingRoute();
                products.DefaultPublishDate = request.Model.DefaultPublishDate;
                products.Description = request.Model.Description;
                products.ProductGroupsId = (int)request.Model.ProductGroupId;

                var needToSave = true;
                if (request.Model.Values != null)
                {
                    foreach (var specificationValue in request.Model.Values)
                    {
                        needToSave = false;

                        var oldValue = _context.GetTable(specificationValue.TableName).ToList().Where(c =>
                        {
                            var a = !specificationValue.MultiData ? c.GetType().GetProperty("Value").GetValue(c).ToString() : "";
                            var result = (int)c.GetType().GetProperty("ProductsId").GetValue(c) == products.Id &&
                                                                                                             (int)c.GetType().GetProperty("CategoriesSpecificationsPropertiesId").GetValue(c) == (!specificationValue.MultiData ? specificationValue.PropertyId : Convert.ToInt32(specificationValue.OldValue))
                                                                                                             &&
                                                                                                             (!specificationValue.MultiData ? a == specificationValue.OldValue : true)
                                                                                                             && !(bool)c.GetType().GetProperty("IsDeleted").GetValue(c);
                            return result;

                        }).FirstOrDefault();

                        if (oldValue != null)
                        {
                            oldValue.GetType().GetProperty("IsDeleted").SetValue(oldValue, Convert.ChangeType(true, oldValue.GetType().GetProperty("IsDeleted").PropertyType));
                            _context.Entry(oldValue, EntityState.Modified);
                        }


                        if (specificationValue.ReCreate)
                        {
                            dynamic obj = null;
                            if (specificationValue.MultiData)
                                obj = new
                                {
                                    CategoriesSpecificationsPropertiesId = specificationValue.Value,
                                    ProductsId = products.Id

                                };
                            else
                                obj = new
                                {
                                    CategoriesSpecificationsPropertiesId = specificationValue.PropertyId,
                                    ProductsId = products.Id,
                                    Value = specificationValue.Value,
                                    IsManual = specificationValue.IsManual
                                };

                            var table = _context.GetType().GetProperty(specificationValue.TableName).GetValue(_context);
                            var addMethod = table.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
                            var paramType = addMethod.GetParameters()[0].ParameterType;

                            addMethod.Invoke(table, new object[] {
                                   ConvertTo(obj, paramType)
                            });
                        }


                        await _context.SaveChangesAsync();
                    }
                }

                if (needToSave)
                    await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(products.Id);
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
