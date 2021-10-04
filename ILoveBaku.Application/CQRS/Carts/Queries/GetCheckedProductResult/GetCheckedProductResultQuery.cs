using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Carts.Queries.GetCheckedProductResult
{
    public class GetCheckedProductResultQuery:BaseRequest<ApiResult<CartDetailDto>>
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Last { get; set; }
        public class GetCheckedProductResultQueryHandler : IRequestHandler<GetCheckedProductResultQuery, ApiResult<CartDetailDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetCheckedProductResultQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<CartDetailDto>> Handle(GetCheckedProductResultQuery request, CancellationToken cancellationToken)
            {

                int productId = request.ProductId;

                ProductsStock productsStock = await _context.ProductsStock
                                                               .FirstOrDefaultAsync(ps => ps.BranchesId == 1 && ps.ProductId == productId &&
                                                                                          ps.ProductStockStatusesId == (byte)ProductStockStatus.Active);


                if (productsStock == null)
                {
                    request.Errors.Add("", "Məhsul tapılmadı.");
                    return ApiResult<CartDetailDto>.CreateResponse(null, request.Errors,
                            new ErrorDetail()
                            {
                                ErrorMessage = "This product is not in stock."
                            });
                }


                if (productsStock.Count <= 0)
                {
                    request.Errors.Add("", "Məhsul anbarda mövcud deyil.");
                    return ApiResult<CartDetailDto>.CreateResponse(null, request.Errors,
                            new ErrorDetail()
                            {
                                ErrorMessage = "This product is not in stock."
                            });
                }

                if (productsStock.Count < request.Count)
                {
                    request.Errors.Add("", "Almaq istədiyiniz qədər məhsul anbarda mövcud deyil.");
                    return ApiResult<CartDetailDto>.CreateResponse(null, request.Errors,
                               new ErrorDetail()
                               {
                                   ErrorMessage = "There is not enough product in stock."
                               });
                }

                var id = ++request.Last;
                var cartDetail = new CartDetailDto()
                {
                    Id = id,
                    Image = productsStock.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain)?.Files?.Path,
                    Name = productsStock.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                    Count = request.Count,
                    StockCount = (int)productsStock.Count,
                    Price = productsStock.Sales.FirstOrDefault(s => s.ProductStockSaleAmountsTypesId == (byte)ProductStockSaleAmountType.Retail).Amount
                                                 .PercentReductionOf(productsStock.ProductsStockDiscountsDetails.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                                                                                                  .Sum(ps => ps.ProductsStockDiscounts.DiscountValue)).Round(2),
                    ProductStockId = productsStock.Id,
                    ProductId = request.ProductId,
                    RootName = productsStock.Product.Name.ToParameterizingRoute()
                };

                return ApiResult<CartDetailDto>.CreateResponse(cartDetail, null);
            }
        }
    }
}
