using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Wishlist.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Wishlist.Queries.GetWishlist
{
    public class GetWishlistQuery : BaseRequest<ApiResult<WishlistVM>>
    {
        public int Take { get; set; }

        public int Page { get; set; }

        public GetWishlistQuery(int take = 10, int page = 1)
        {
            Take = take;
            Page = (page > 0) ? page : 1;
        }

        public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, ApiResult<WishlistVM>>
        {
            private readonly IApplicationDbContext _context;

            public GetWishlistQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<WishlistVM>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
            {
                Guid userId = request.UserId;

                int take = request.Take;

                int page = request.Page;

                IQueryable<ProductsStock> productStocks = _context.WishLists.Where(w => w.UsersId == userId).Select(w => w.ProductsStock);

                int wishlistCount = productStocks.Count();

                if (!(await productStocks.Skip(take * (page - 1)).Take(take).CountAsync()).IsZore())
                {
                    productStocks = productStocks.Skip(take * (page - 1)).Take(take);
                }
                else
                {
                    page = 1;
                    productStocks = productStocks.Skip(take * (page - 1)).Take(take);
                }

                List<ProductStockDto> wishlist = await productStocks
                                                          .Select(ps => new ProductStockDto()
                                                          {
                                                              RouteName = ps.Product.Name.ToParameterizingRoute(),
                                                              BranchName = ps.Branches.CompanyDetails.Name,
                                                              ProductId = ps.ProductId,
                                                              Id = ps.Id,
                                                              Name = ps.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                                                              Price = ps.GetPrice(ProductStockSaleAmountType.Retail),
                                                              DiscountedPrice = ps.GetPrice(ProductStockSaleAmountType.Retail)
                                                                                                .PercentReductionOf(ps.ProductsStockDiscountsDetails.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                                                                                                                                             .Sum(ps => ps.ProductsStockDiscounts.DiscountValue)).Round(2),
                                                              IsWishlist = _context.WishLists.Any(w => w.UsersId == userId && w.ProductsStockId == ps.Id),
                                                              Image = ps.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain).Files.Path,
                                                              CategoryName = ps.Product.ProductGroup.Category.CategoriesLangs.FirstOrDefault(c => c.Lang.Culture == request.Culture).Name,
                                                              Barcodes = _context.ProductSpecificationValuesBarcodes.Where(c => c.ProductsId == ps.ProductId && !c.IsDeleted).Select(c => c.Value).ToList(),
                                                              Count = ps.Count
                                                          }).ToListAsync();

                WishlistVM model = new WishlistVM()
                {
                    WishlistCount = wishlistCount,
                    Page = page,
                    Wishlist = wishlist
                };

                return ApiResult<WishlistVM>.CreateResponse(model);
            }
        }
    }
}
