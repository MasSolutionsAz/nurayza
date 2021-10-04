using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecLangs
{
    public class GetProductSpecLangsQuery : BaseRequest<ApiResult<List<ProductSpecLangsDto>>>
    {
        public int ProductId { get; set; }
        public class GetProductSpecLangsQueryHandler : IRequestHandler<GetProductSpecLangsQuery, ApiResult<List<ProductSpecLangsDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductSpecLangsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductSpecLangsDto>>> Handle(GetProductSpecLangsQuery request, CancellationToken cancellationToken)
            {
                var productSpecLangs = await (from stringSpecs in _context.ProductsStockSpecificationsValuesStrings
                                              where stringSpecs.ProductsId == request.ProductId && !stringSpecs.IsDeleted
                                              select new ProductSpecLangsDto
                                              {
                                                  Id = stringSpecs.Id,
                                                  ProductId = stringSpecs.ProductsId,
                                                  SpecificationId = stringSpecs.CategoriesSpecificationsProperties.CategoriesSpecificationId,
                                                  Name = stringSpecs.CategoriesSpecificationsProperties
                                                                          .CategoriesSpecification
                                                                              .CategoriesSpecificationsLangs
                                                                                  .Where(a => a.Lang.Culture == request.Culture)
                                                                                      .Select(a => a.Name)
                                                                                          .FirstOrDefault(),
                                                  PropertyId = stringSpecs.CategoriesSpecificationsPropertiesId,
                                                  Values = (from stringSpecsLangs in _context.ProductsSpecificationsValuesStringsLangs
                                                            where stringSpecsLangs.ProductsStockSpecificationsValuesStringsId == stringSpecs.Id && !stringSpecsLangs.IsDeleted
                                                            select new ProductSpecLangValue
                                                            {
                                                                LangId = stringSpecsLangs.LangsId,
                                                                Value = stringSpecsLangs.Value,
                                                                LangName = stringSpecsLangs.Langs.DisplayName
                                                            }).ToList()
                                              }).ToListAsync();

                if(productSpecLangs==null)
                {
                    request.Errors.Add("", "Xeta bas verdi");
                    return ApiResult<List<ProductSpecLangsDto>>.CreateResponse(null, request.Errors);
                }

                return ApiResult<List<ProductSpecLangsDto>>.CreateResponse(productSpecLangs);

            }
        }
    }
}
