using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetSearchedProductStocks
{
    public class GetSearchedProductStocksQuery : BaseRequest<ApiResult<List<ProductStockDto>>>
    {
        public string Key { get; set; }

        public string Categories { get; set; }


        public GetSearchedProductStocksQuery(string key, string categories)
        {
            Key = key;
            Categories = categories;
        }

        public class GetSearchedProductStocksQueryHandler : IRequestHandler<GetSearchedProductStocksQuery, ApiResult<List<ProductStockDto>>>
        {
            private readonly IApplicationDbContext _context;

            public GetSearchedProductStocksQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<List<ProductStockDto>>> Handle(GetSearchedProductStocksQuery request, CancellationToken cancellationToken)
            {
                //string key = request.Key ?? string.Empty;

                //int[] categories = request.Categories;

                //List<ProductsStock> searchedProducts = new List<ProductsStock>();

                //IQueryable<ProductsStock> productsStocks = _context.ProductsStock
                //                                                             .Where(ps => ps.BranchesId == 1 &&
                //                                                                          ps.PublishDate <= DateTime.Now &&
                //                                                                          ps.ProductStockStatusesId == (byte)ProductStockStatus.Active)
                //                                                                .OrderBy(ps => ps.Product.ProductsLangs.FirstOrDefault(p => p.Langs.Culture == request.Culture).Name.IndexOf(key));

                //searchedProducts = await productsStocks.Where(ps => ps.Product.ProductsLangs.FirstOrDefault(p => p.Langs.Culture == request.Culture).Name.StartsWith(key)).ToListAsync();

                //searchedProducts.AddRange(await productsStocks.Where(ps => !ps.Product.ProductsLangs.FirstOrDefault(p=>p.Langs.Culture == request.Culture).Name.StartsWith(key) &&
                //                                                            ps.Product.ProductsLangs.FirstOrDefault(p => p.Langs.Culture == request.Culture).Name.Contains(key)).ToListAsync());

                //if (!categories.Length.IsZore())
                //    searchedProducts = searchedProducts
                //                           .Where(ps => HasParentCategory(ps.Product.ProductGroup.CategoriesId, categories).Result)
                //                               .ToList();

                //List<SearchedProductStock> products = searchedProducts.Select(ps => new SearchedProductStock()
                //{
                //    Name = ps.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                //    RouteName = ps.Product.Name.ToParameterizingRoute(),
                //    Price = ps.GetPrice(ProductStockSaleAmountType.Retail),
                //    DiscountedPrice = ps.GetDiscountedPrice(),
                //    Image = ps.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain)?.Files?.Path
                //}).ToList();

                //return ApiResult<List<SearchedProductStock>>.CreateResponse(products);


                #region Parameters
                short langId = (await _context.Langs.FirstOrDefaultAsync(l => l.Culture == request.Culture)).Id;

                int take = 10;

                int page = 1;

                //if (!filters.Remove("page", out string _page)) _page = string.Empty;

                //if (!_page.IsEmpty()) int.TryParse(_page, out page);

                int from = (take * (page - 1)) + 1;

                int to = from + take - 1;

                if (request.Key == null) request.Key = "-1";


                if (request.Categories == null) request.Categories = "-1";

                int min = -1;

                int max = -1;

                
                #endregion



                List<ProductStockDto> products = (await _context.Exec<ProductStockDto>("GetFilteredProducts", $"{langId}", $"{request.UserId}", "1",
                                                                                                              $"{from}", $"{to}", request.Key, request.Categories,
                                                                                                              $"-1", $"-1", $"{min}", $"{max}"));

                products.ForEach(p => { p.RouteName = p.Title.ToParameterizingRoute(); });

                //int productCount = products?.Count > 0 ? products[0].TotalCount : 0;

                //ProductListDto model = new ProductListDto()
                //{
                //    ProductCount = productCount,
                //    Page = page,
                //    Products = products
                //};


                return ApiResult<List<ProductStockDto>>.CreateResponse(products);
            }

            public async Task<bool> HasParentCategory(int categoryId, int[] parentCategories)
            {
                Categories category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (!category.IsNull())
                {
                    if (parentCategories.Contains(category.Id))
                        return true;
                    else
                        return await HasParentCategory(category.ParentId ?? 0, parentCategories);
                }

                return false;
            }
        }
    }
}
