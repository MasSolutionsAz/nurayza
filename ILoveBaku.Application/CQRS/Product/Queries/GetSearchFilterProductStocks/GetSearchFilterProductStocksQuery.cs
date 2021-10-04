using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ILoveBaku.Domain.Enums;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetSearchFilterProductStocks
{
    public class GetSearchFilterProductStocksQuery : BaseRequest<ApiResult<ProductListDto>>
    {
        public string CategoryName { get; set; }

        public Dictionary<string, string> Filters { get; set; }

        public GetSearchFilterProductStocksQuery(string categoryName, Dictionary<string, string> filters)
        {
            CategoryName = categoryName;
            Filters = filters;
        }

        public class GetSearchFilterProductStocksQueryHandler : IRequestHandler<GetSearchFilterProductStocksQuery, ApiResult<ProductListDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetSearchFilterProductStocksQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<ProductListDto>> Handle(GetSearchFilterProductStocksQuery request, CancellationToken cancellationToken)
            {
                #region Old
                //string categoryName = request.CategoryName;

                //Dictionary<string, string> filters = request.Filters;

                //List<ProductsStock> searchedProducts = new List<ProductsStock>();

                //IQueryable<ProductsStock> productsStocks = _context.ProductsStock
                //                                                              .Where(ps => ps.BranchesId == 1 &&
                //                                                                           ps.PublishDate <= DateTime.Now &&
                //                                                                           ps.ProductStockStatusesId == (byte)ProductStockStatus.Active);

                #region Search
                //if (!filters.Remove("key", out string key)) key = string.Empty;

                //if (key.IsEmpty())
                //    searchedProducts = await productsStocks.OrderByDescending(ps => ps.CreatedDate).ToListAsync();
                //else
                //{
                //    productsStocks = productsStocks.OrderBy(ps => ps.Product.Name.IndexOf(key));

                //    searchedProducts = await productsStocks.Where(ps => ps.Product.Name.StartsWith(key)).ToListAsync();

                //    searchedProducts.AddRange(await productsStocks.Where(ps => !ps.Product.Name.StartsWith(key) &&
                //                                                                ps.Product.Name.Contains(key)).ToListAsync());
                //}
                #endregion

                #region Filter by Base Category
                //if (!categoryName.IsNullOrEmpty())
                //    searchedProducts = searchedProducts
                //                           .Where(ps => HasParentCategory(ps.Product.ProductGroup.CategoriesId, categoryName).Result)
                //                               .ToList();
                #endregion

                #region Filter by Categories
                //if (filters.Remove("categories", out string categories))
                //    searchedProducts = searchedProducts
                //                           .Where(ps => HasParentCategory(ps.Product.ProductGroup.CategoriesId, categories.Split(',')).Result)
                //                               .ToList();
                #endregion

                #region Filter by Price
                //if (filters.Remove("price", out string price))
                //{
                //    string[] prices = price.Split('-');
                //    if (prices.Length == 2 && int.TryParse(prices.First(), out int min) &&
                //                              int.TryParse(prices.Last(), out int max))
                //        searchedProducts = searchedProducts.Where(ps => ps.GetDiscountedPrice() >= min &&
                //                                                        ps.GetDiscountedPrice() <= max).ToList();
                //}
                #endregion

                //int take = 20;

                //int page = 1;

                //if (!filters.Remove("page", out string _page)) _page = string.Empty;

                //if (!_page.IsEmpty()) int.TryParse(_page, out page);

                #region Filter By Specifications
                //foreach (var filter in filters)
                //{
                //    searchedProducts = searchedProducts.Where(ps => ps.Product.ProductGroup.Category
                //                                                         .CategoriesSpecificationsRelations
                //                                                            .Any(csr => csr.CategoriesSpecification
                //                                                                             .CategoriesSpecificationsLangs.Any(cs => cs.Name.ToParameterizingRoute() == filter.Key) &&
                //                                                                        csr.CategoriesSpecification.CategoriesSpecificationsProperties.Any(csp => csp.CategoriesSpecificationsPropertiesLangs.Any(cspl => filter.Value.Split(',').Any(v => v == cspl.Name.ToParameterizingRoute() && csp.ProductsStockSpecificationsValuesLists.Any(pssvl => pssvl.ProductsId == ps.ProductId && !pssvl.IsDeleted)))))).ToList();
                //}
                #endregion

                //int productCount = searchedProducts.Count();

                //#region Pagination
                //IEnumerable<ProductsStock> takenProductStocks = searchedProducts.Skip(take * (page - 1)).Take(take);

                //if (takenProductStocks.Count().IsZore())
                //    page = 1;

                //searchedProducts = takenProductStocks.ToList();
                //#endregion

                //var products = searchedProducts.Select((ps) =>
                //                                  new ProductStockDto()
                //                                  {
                //                                      ProductGroupId = ps.Product.ProductGroupsId,
                //                                      Id = ps.Id,
                //                                      ProductId = ps.ProductId,
                //                                      Name = ps.Product.ProductsLangs.Where(c => c.Langs.Culture == request.Culture).FirstOrDefault()?.Name,
                //                                      RouteName = ps.Product.ProductsLangs.Where(c => c.Langs.Culture == request.Culture).FirstOrDefault()?.Name.ToParameterizingRoute(),
                //                                      Price = ps.GetPrice(ProductStockSaleAmountType.Retail),
                //                                      DiscountedPrice = ps.GetDiscountedPrice(),
                //                                      IsWishlist = _context.WishLists.Any(w => w.UsersId == request.UserId && w.ProductsStockId == ps.Id),
                //                                      Image = ps.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain)?.Files?.Path,
                //                                      Count = ps.Count,
                //                                  }).ToList();

                //ProductListDto model = new ProductListDto()
                //{
                //    ProductCount = productCount,
                //    Page = page,
                //    Products = products
                //};
                #endregion

                #region New
                //string categoryName = request.CategoryName;

                //Dictionary<string, string> filters = request.Filters;

                //IQueryable<ProductsStock> productsStocks = _context.ProductsStock
                //                                                      .Where(ps => ps.BranchesId == 1 &&
                //                                                                   ps.PublishDate <= DateTime.Now &&
                //                                                                   ps.ProductStockStatusesId == (byte)ProductStockStatus.Active);

                //#region Search
                //if (!filters.Remove("key", out string key)) key = string.Empty;

                //if (key.IsEmpty())
                //    productsStocks = productsStocks.OrderByDescending(ps => ps.CreatedDate);
                //else
                //{
                //    productsStocks = productsStocks.Where(ps => ps.Product.Name.Contains(key));

                //    productsStocks = productsStocks.OrderBy(ps => ps.Product.Name.IndexOf(key));
                //}
                //#endregion

                //#region Filter by Base Category
                ////if (!categoryName.IsNullOrEmpty())
                ////    productsStocks = productsStocks.Where(ps => HasParentCategoryNew(ps.Product.ProductGroup.CategoriesId, categoryName).Result);
                //#endregion

                //#region Filter by Categories
                //if (filters.Remove("categories", out string categories))
                //{
                //    string[] parentCategoryNames = categories.Split(',');
                //    productsStocks = productsStocks
                //                           .Where(ps => HasParentCategoryNew(ps.Product.ProductGroup.CategoriesId, parentCategoryNames).Result);
                //}
                //#endregion

                //#region Filter by Price
                //if (filters.Remove("price", out string price))
                //{
                //    string[] prices = price.Split('-');
                //    if (prices.Length == 2 && int.TryParse(prices.First(), out int min) &&
                //                              int.TryParse(prices.Last(), out int max))
                //        productsStocks = productsStocks.Where(ps => ps.GetDiscountedPrice() >= min && ps.GetDiscountedPrice() <= max);
                //}
                //#endregion

                //#region Filter By Specifications
                ////foreach (var filter in filters)
                ////{
                ////    string[] values = filter.Value.Split(',');

                ////    productsStocks = productsStocks.Where(ps => ps.Product.ProductGroup.Category
                ////                                                         .CategoriesSpecificationsRelations
                ////                                                            .Any(csr => csr.CategoriesSpecification
                ////                                                                             .CategoriesSpecificationsLangs.Any(cs => cs.Name.ToParameterizingRoute() == filter.Key) &&
                ////                                                                        csr.CategoriesSpecification.CategoriesSpecificationsProperties.Any(csp => csp.CategoriesSpecificationsPropertiesLangs.Any(cspl => values.Any(v => v == cspl.Name.ToParameterizingRoute() && csp.ProductsStockSpecificationsValuesLists.Any(pssvl => pssvl.ProductsId == ps.ProductId && !pssvl.IsDeleted))))));
                ////}
                //#endregion

                //#region Pagination
                //int take = 20;

                //int page = 1;

                //if (!filters.Remove("page", out string _page)) _page = string.Empty;

                //if (!_page.IsEmpty()) int.TryParse(_page, out page);

                //int productCount = await productsStocks.CountAsync();

                //int skip = take * (page - 1);

                //if (productCount > skip)
                //    productsStocks = productsStocks.Skip(skip).Take(take);
                //else
                //{
                //    productsStocks = productsStocks.Take(take);
                //    page = 1;
                //}
                //#endregion

                //List<ProductStockDto> products = await productsStocks.Select((ps) => new ProductStockDto()
                //{
                //    ProductGroupId = ps.Product.ProductGroupsId,
                //    Id = ps.Id,
                //    ProductId = ps.ProductId,
                //    Name = ps.Product.ProductsLangs.Where(c => c.Langs.Culture == request.Culture).FirstOrDefault().Name,
                //    RouteName = ps.Product.ProductsLangs.Where(c => c.Langs.Culture == request.Culture).FirstOrDefault().Name.ToParameterizingRoute(),
                //    Price = ps.GetPrice(ProductStockSaleAmountType.Retail),
                //    DiscountedPrice = ps.GetDiscountedPrice(),
                //    IsWishlist = _context.WishLists.Any(w => w.UsersId == request.UserId && w.ProductsStockId == ps.Id),
                //    Image = ps.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain).Files.Path,
                //    Count = ps.Count,
                //}).ToListAsync();

                //ProductListDto model = new ProductListDto()
                //{
                //    ProductCount = productCount,
                //    Page = page,
                //    Products = products
                //};
                #endregion

                #region Stored Procedure
                Dictionary<string, string> filters = request.Filters;

                #region Parameters
                short langId = (await _context.Langs.FirstOrDefaultAsync(l => l.Culture == request.Culture)).Id;

                int take = 20;

                int page = 1;

                if (!filters.Remove("page", out string _page)) _page = string.Empty;

                if (!_page.IsEmpty()) int.TryParse(_page, out page);

                int from = (take * (page - 1)) + 1;

                int to = from + take - 1;

                if (!filters.Remove("key", out string key)) key = "-1";

                string categoryName = request.CategoryName ?? "-1";

                if (!filters.Remove("categories", out string categories)) categories = "-1";

                int min = -1;

                int max = -1;

                if (filters.Remove("price", out string price))
                {
                    string[] prices = price.Split('-');

                    if (prices.Length == 2)
                    {
                        if (!int.TryParse(prices.First(), out min) || !int.TryParse(prices.Last(), out max))
                        {
                            min = -1;
                            max = -1;
                        }
                    }
                }

                string specifications = null;
                foreach (var filter in filters)
                {
                    specifications += filter.Key + "=" + filter.Value + "&";
                }

                if (specifications != null)
                    specifications = specifications.Substring(0, specifications.Length - 1);
                else
                    specifications = "-1";
                #endregion



                List<ProductStockDto> products = (await _context.Exec<ProductStockDto>("GetFilteredProducts", $"{langId}", $"{request.UserId}", "1",
                                                                                                              $"{from}", $"{to}", key, categoryName,
                                                                                                              $"{categories}", $"{specifications}", $"{min}", $"{max}"));

                products.ForEach(p => { p.RouteName = p.Title.ToParameterizingRoute(); });

                int productCount = products?.Count > 0 ? products[0].TotalCount : 0;

                ProductListDto model = new ProductListDto()
                {
                    ProductCount = productCount,
                    Page = page,
                    Products = products
                };
                #endregion

                return ApiResult<ProductListDto>.CreateResponse(model);
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

            public async Task<bool> HasParentCategory(int categoryId, string parentCategoryName)
            {
                Categories category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (!category.IsNull())
                {
                    if (category.CategoriesLangs.Any(cl => cl.Name.ToParameterizingRoute() == parentCategoryName))
                        return true;
                    else
                        return await HasParentCategory(category.ParentId ?? 0, parentCategoryName);
                }

                return false;
            }

            public async Task<bool> HasParentCategory(int categoryId, string[] parentCategoryNames)
            {
                Categories category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (!category.IsNull())
                {
                    if (category.CategoriesLangs.Any(cl => parentCategoryNames.Contains(cl.Name.ToParameterizingRoute())))
                        return true;
                    else
                        return await HasParentCategory(category.ParentId ?? 0, parentCategoryNames);
                }

                return false;
            }

            public async Task<bool> HasParentCategoryNew(int categoryId, string parentCategoryName)
            {
                Categories category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (!category.IsNull())
                {
                    if (category.Title == parentCategoryName)
                        return true;
                    else
                        return await HasParentCategoryNew(category.ParentId ?? 0, parentCategoryName);
                }

                return false;
            }

            public async Task<bool> HasParentCategoryNew(int categoryId, string[] parentCategoryNames)
            {
                Categories category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (!category.IsNull())
                {
                    if (parentCategoryNames.Contains(category.Title))
                        return true;
                    else
                        return await HasParentCategoryNew(category.ParentId ?? 0, parentCategoryNames);
                }

                return false;
            }
        }
    }
}

//List<ProductsStock> result = new List<ProductsStock>();
//foreach (var ps in searchedProducts)
//{
//    foreach (var csr in ps.Product.ProductGroup.Category.CategoriesSpecificationsRelations)
//    {
//        foreach (var cs in csr.CategoriesSpecification.CategoriesSpecificationsLangs)
//        {
//            if (cs.Name.ToParameterizingRoute() == filter.Key)
//            {
//                foreach (var csp in csr.CategoriesSpecification.CategoriesSpecificationsProperties)
//                {
//                    foreach (var cspl in csp.CategoriesSpecificationsPropertiesLangs)
//                    {
//                        foreach (var v in filter.Value.Split(','))
//                        {
//                            if (v == cspl.Name.ToParameterizingRoute() && csp.ProductsStockSpecificationsValuesLists.Any(pssvl => pssvl.ProductsId == ps.ProductId && !pssvl.IsDeleted))
//                            {
//                                result.Add(ps);
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}