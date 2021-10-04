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

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductStock
{
    public class GetProductStockQuery : BaseRequest<ApiResult<ProductStockVM>>
    {
        public int BranchId { get; set; }

        public int ProductStockId { get; set; }
        public string ProductName { get; set; }

        public ProductStockStatus ProductStockStatus { get; set; } = ProductStockStatus.NonSelected;

        public GetProductStockQuery(int branchId, int productStockId, string productName, ProductStockStatus productStockStatus)
        {
            BranchId = branchId;
            ProductStockId = productStockId;
            ProductName = productName;
            ProductStockStatus = productStockStatus;
        }

        public class GetProductStockQueryHandler : IRequestHandler<GetProductStockQuery, ApiResult<ProductStockVM>>
        {
            private readonly IApplicationDbContext _context;

            public GetProductStockQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<ProductStockVM>> Handle(GetProductStockQuery request, CancellationToken cancellationToken)
            {
                int branchId = request.BranchId;

                int productStockId = request.ProductStockId;

                string productName = request.ProductName;

                ProductStockStatus productStockStatus = request.ProductStockStatus;

                ProductsStock productStock = null;

                foreach (ProductsStock ps in _context.ProductsStock.Where(ps => branchId.IsZore() ? ps.ProductStockStatusesId == (byte)ProductStockStatus.Active :
                                                                                                    ps.BranchesId == branchId && (((int)productStockStatus).IsZore() || ps.ProductStockStatusesId == (byte)productStockStatus)))
                    if (productStockId.IsZore() ? ps.Product.Name.ToParameterizingRoute() == request.ProductName:
                                                  ps.Id == productStockId)
                    {
                        productStock = ps; break;
                    }

                if (productStock.IsNull())
                    return ApiResult<ProductStockVM>.CreateResponse(null, null, new ErrorDetail()
                    {
                        ErrorMessage = "Product not found."
                    });

                decimal price = productStock.GetPrice();

                ProductStockDetailDto productStockDetail = new ProductStockDetailDto()
                {
                    Id = productStock.Id,
                    Name = productStock.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                    RouteName = productStock.Product.Name.ToParameterizingRoute(),
                    Price = price,
                    DiscountedPrice = price.PercentReductionOf(productStock.ProductsStockDiscountsDetails?.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                              .Sum(ps => ps.ProductsStockDiscounts.DiscountValue) ?? default).Round(2),
                    StockCount = (int)productStock.Count,
                    IsWishlist = await _context.WishLists.AnyAsync(w => w.UsersId == request.UserId && w.ProductsStockId == productStock.Id),
                    Images = productStock.Product.ProductsFiles?.OrderBy(pf => pf.IsMain ? 0 : 1).Select(pf => pf.Files?.Path).ToList() ?? new List<string>(),
                    Barcodes = _context.ProductSpecificationValuesBarcodes
                                                    .Where(c => !c.IsDeleted && c.ProductsId == productStock.ProductId)
                                                    .Select(c => c.Value)
                                                    .ToList(),
                    CategoryId = productStock.Product.ProductGroup.CategoriesId,
                    CategoryName = productStock.Product.ProductGroup.Category.CategoriesLangs.FirstOrDefault(c => c.Lang.Culture == request.Culture).Name,
                    BuyAmount = productStock.BuyAmount,
                    CostAmount = productStock.CostAmount,
                    Tax = productStock.TaxPercent,
                    ProductStockStatusId = productStock.ProductStockStatusesId,
                    ProductId = productStock.ProductId,
                    PublishDate = productStock.PublishDate == null ? default(DateTime) : (DateTime)productStock.PublishDate,
                    Description = productStock.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture)?.Description
                };

                ProductStockVM model = new ProductStockVM()
                {
                    Product = productStockDetail,
                    NestedCategories = (await GetParentCategories(productStock.Product.ProductGroup.CategoriesId, request.Culture))
                                                  .Reverse<NestedCategory>().ToList()
                };

                return ApiResult<ProductStockVM>.CreateResponse(model);
            }

            public async Task<List<NestedCategory>> GetParentCategories(int categoryId, string culture)
            {
                List<NestedCategory> result = new List<NestedCategory>();

                CategoriesLangs categoryLang = await _context.CategoriesLangs
                                                                .FirstOrDefaultAsync(c => c.CategoriesId == categoryId &&
                                                                                          c.Lang.Culture == culture);

                if (!categoryLang.IsNull())
                {
                    result.Add(new NestedCategory()
                    {
                        Id = categoryLang.CategoriesId,
                        Name = categoryLang.Name,
                        Title = categoryLang.Category.Title
                    });

                    result.AddRange(await GetParentCategories(categoryLang.Category.ParentId ?? 0, culture));
                }

                return result;
            }
        }
    }
}
