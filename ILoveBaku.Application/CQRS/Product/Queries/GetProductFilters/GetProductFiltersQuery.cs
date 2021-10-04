using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Domain.Enums;
using System;
using System.Diagnostics;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductFilters
{
    public class GetProductFiltersQuery : BaseRequest<ApiResult<ProductFiltersDto>>
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public Dictionary<string, string> Filters { get; set; }

        public GetProductFiltersQuery(int categoryId) => CategoryId = categoryId;

        public GetProductFiltersQuery(string categoryName, Dictionary<string, string> filters)
        {
            CategoryName = categoryName;
            Filters = filters;
        }

        public class GetProductFiltersQueryHandler : IRequestHandler<GetProductFiltersQuery, ApiResult<ProductFiltersDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetProductFiltersQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<ProductFiltersDto>> Handle(GetProductFiltersQuery request, CancellationToken cancellationToken)
            {
                int categoryId = request.CategoryId;

                string culture = request.Culture;

                Dictionary<string, string> filters = request.Filters;

                List<CategoryFilterDto> categoryFilters = await _context.CategoriesLangs
                                                                           .Where(cl => cl.Category.ParentId == categoryId &&
                                                                                        cl.Lang.Culture == culture && cl.Category.IsActive)
                                                                              .Select(cl => new CategoryFilterDto()
                                                                              {
                                                                                  Id = cl.CategoriesId,
                                                                                  Name = cl.Name,
                                                                                  RouteName = cl.Category.Title.ToParameterizingRoute()
                                                                              }).ToListAsync();

                List<SpecificationFilterDto> specifications = await (from CSL in _context.CategoriesSpecificationsLangs

                                                                     join CS in _context.CategoriesSpecifications
                                                                     on CSL.CategoriesSpecificationsId equals CS.Id

                                                                     join CSR in _context.CategoriesSpecificationsRelations
                                                                     on CS.Id equals CSR.CategoriesSpecificationId

                                                                     where CS.CategoriesSpecificationGroupId == 1 &&
                                                                     CS.CategoriesSpecificationsTypeId == (byte)CategorySpecificationType.List &&
                                                                     CS.CategoriesSpecificationsStatusesId == (byte)CategorySpecificationStatus.ShowEveryWhere &&
                                                                     CSR.CategoriesId == categoryId &&
                                                                     CSL.Lang.Culture == culture

                                                                     select new SpecificationFilterDto()
                                                                     {
                                                                         Id = CSL.CategoriesSpecificationsId,
                                                                         Name = CSL.Name,
                                                                         RouteName = CSL.CategorySpecification.Title.ToParameterizingRoute(),
                                                                         Values = (from CSPL in _context.CategoriesSpecificationsPropertiesLangs

                                                                                   join CSP in _context.CategoriesSpecificationsProperties
                                                                                   on CSPL.CategoriesSpecificationsPropertiesId equals CSP.Id

                                                                                   where CSP.CategoriesSpecificationId == CS.Id &&
                                                                                   CSPL.Lang.Culture == culture

                                                                                   select new SpecificationValueFilterDto()
                                                                                   {
                                                                                       Value = CSPL.Name,
                                                                                       RouteValue = CSP.Title.ToParameterizingRoute()
                                                                                   }).ToList()
                                                                     }).ToListAsync();

                IQueryable<ProductsStockSaleAmounts> productsStockSaleAmounts = from PSSA in _context.ProductsStockSaleAmounts

                                                                                join PS in _context.ProductsStock
                                                                                on PSSA.ProductsStockId equals PS.Id

                                                                                join P in _context.Products
                                                                                on PS.ProductId equals P.Id

                                                                                join PG in _context.ProductGroups
                                                                                on P.ProductGroupsId equals PG.Id

                                                                                join C in _context.Categories
                                                                                on PG.CategoriesId equals C.Id

                                                                                where PSSA.ProductStockSaleAmountsTypesId == (byte)ProductStockSaleAmountType.Retail &&
                                                                                PS.BranchesId == 1 &&
                                                                                PS.ProductStockStatusesId == (byte)ProductStockStatus.Active &&
                                                                                PS.PublishDate <= DateTime.Now &&
                                                                                (PG.CategoriesId == categoryId || (C.ParentId ?? 0) == categoryId)

                                                                                select PSSA;

                #region Old Price
                //List<ProductsStock> productStocks = await _context.ProductsStockSaleAmounts
                //                                                     .Where(pssa => pssa.ProductsStock.BranchesId == 1 &&
                //                                                                    pssa.ProductsStock.PublishDate <= DateTime.Now &&
                //                                                                    pssa.ProductsStock.ProductStockStatusesId == (byte)ProductStockStatus.Active &&
                //                                                                    pssa.ProductStockSaleAmountsTypesId == (byte)ProductStockSaleAmountType.Retail)
                //                                                        .Select(pssa => pssa.ProductsStock)
                //                                                        .ToListAsync();

                //if (!categoryId.IsZore())
                //    productStocks = productStocks.Where(ps => HasParentCategory(ps.Product.ProductGroup.CategoriesId, categoryId).Result)
                //                                   .ToList();

                //PriceFilterDto priceFilter = new PriceFilterDto()
                //{
                //    Min = (int)Math.Ceiling(productStocks.Min(ps => ps?.GetDiscountedPrice()) ?? 0),
                //    Max = (int)Math.Ceiling(productStocks.Max(ps => ps?.GetDiscountedPrice()) ?? 0)
                //};
                #endregion

                PriceFilterDto priceFilter = new PriceFilterDto()
                {
                    Min = (int)Math.Ceiling(productsStockSaleAmounts.Min(pssa => pssa.Amount)),
                    Max = (int)Math.Ceiling(productsStockSaleAmounts.Max(pssa => pssa.Amount))
                };

                ProductFiltersDto model = new ProductFiltersDto()
                {
                    CategoryFilters = categoryFilters,
                    SpecificationFilters = specifications,
                    PriceFilter = priceFilter
                };

                return ApiResult<ProductFiltersDto>.CreateResponse(model);
            }

            public async Task<bool> HasParentCategory(int categoryId, int parentCategoryId)
            {
                Categories category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (!category.IsNull())
                {
                    if (category.Id == parentCategoryId)
                        return true;
                    else
                        return await HasParentCategory(category.ParentId ?? 0, parentCategoryId);
                }

                return false;
            }
        }
    }
}
