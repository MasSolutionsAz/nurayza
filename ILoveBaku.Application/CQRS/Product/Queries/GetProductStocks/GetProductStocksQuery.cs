using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductStocks
{
    public class GetProductStocksQuery : BaseRequest<ApiResult<ProductStocksVM>>
    {
        //public bool 
        public int BranchId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ProductStockSaleAmountType ProductStockSaleAmountType { get; set; } = ProductStockSaleAmountType.NonSelected;

        public ProductStockStatus ProductStockStatus { get; set; } = ProductStockStatus.NonSelected;

        public int Take { get; set; }

        public int Page { get; set; }

        public class GetProductStocksQueryHandler : IRequestHandler<GetProductStocksQuery, ApiResult<ProductStocksVM>>
        {
            private readonly IApplicationDbContext _context;

            public GetProductStocksQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<ProductStocksVM>> Handle(GetProductStocksQuery request, CancellationToken cancellationToken)
            {
                int take = request.Take;

                int page = (request.Page > 0) ? request.Page : 1;

                Expression<Func<ProductsStock, bool>> productStockExpression = ps => ps.BranchesId == request.BranchId &&
                                                                                     ps.PublishDate <= DateTime.Now &&
                                                                         (((int)request.ProductStockStatus).IsZore() || ps.ProductStockStatusesId == (byte)request.ProductStockStatus) &&
                                                                         (request.CategoryId.IsZore() || ps.Product.ProductGroup.CategoriesId == request.CategoryId);

                IQueryable<ProductsStock> productStocks = _context.ProductsStock.OrderByDescending(c => c.CreatedDate).Where(productStockExpression);

                int productCount = productStocks.Count();
                int total = (int)Math.Ceiling(productCount / (decimal)request.Take);

                if (!(await productStocks.Skip(take * (page - 1)).Take(take).CountAsync()).IsZore())
                {
                    productStocks = productStocks.Skip(take * (page - 1)).Take(take);
                }
                else
                {
                    page = 1;
                    productStocks = productStocks.Skip(take * (page - 1)).Take(take);
                }

                List<ProductStockDto> products = await productStocks.Select((ps) =>
                                                                     new ProductStockDto()
                                                                     {
                                                                         Id = ps.Id,
                                                                         ProductId = ps.ProductId,
                                                                         BranchName = ps.Branches.CompanyDetails.Name,
                                                                         Name = ps.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                                                                         RouteName = ps.Product.Name.ToParameterizingRoute(),
                                                                         Price = ps.Product.DefaultSaleAmount.Value,
                                                                         DiscountedPrice = ps.GetDiscountedPrice(request.ProductStockSaleAmountType),
                                                                         //DiscountedPrice = ps.GetPrice(request.ProductStockSaleAmountType)
                                                                         //                       .PercentReductionOf(ps.ProductsStockDiscountsDetails.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                                                         //                                                                                    .Sum(ps => ps.ProductsStockDiscounts.DiscountValue)).Round(2),
                                                                         IsWishlist = _context.WishLists.Any(w => w.UsersId == request.UserId && w.ProductsStockId == ps.Id),
                                                                         Image = ps.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain).Files.Path,
                                                                         CategoryName = ps.Product.ProductGroup.Category.CategoriesLangs.FirstOrDefault(c => c.Lang.Culture == request.Culture).Name,
                                                                         Barcodes = _context.ProductSpecificationValuesBarcodes.Where(c => c.ProductsId == ps.ProductId && !c.IsDeleted).Select(c => c.Value).ToList(),
                                                                         Count = ps.Count,
                                                                         Status = ps.ProductStockStatusesId ==  (byte)ProductStockStatus.Active
                                                                     }).ToListAsync();

                ProductStocksVM model = new ProductStocksVM()
                {
                    CategoryId = request.CategoryId,
                    CategoryName = (await _context.CategoriesLangs
                                                     .FirstOrDefaultAsync(cl => cl.CategoriesId == request.CategoryId &&
                                                                                cl.Lang.Culture == request.Culture))?.Name,
                    ProductCount = productCount,
                    Page = page,
                    Products = products,
                    Total = total
                };

                return ApiResult<ProductStocksVM>.CreateResponse(model);
            }
        }
    }
}
