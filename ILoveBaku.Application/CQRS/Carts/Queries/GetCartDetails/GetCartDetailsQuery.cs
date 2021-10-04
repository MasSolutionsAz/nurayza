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
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Carts.Queries.GetCartDetails
{
    public class GetCartDetailsQuery : BaseRequest<ApiResult<List<CartDetailDto>>>
    {
        public class GetCartDetailsQueryHandler : IRequestHandler<GetCartDetailsQuery, ApiResult<List<CartDetailDto>>>
        {
            private readonly IApplicationDbContext _context;

            public GetCartDetailsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResult<List<CartDetailDto>>> Handle(GetCartDetailsQuery request, CancellationToken cancellationToken)
            {
                List<CartDetailDto> cartDetailDtos = new List<CartDetailDto>();

                Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == request.UserId &&
                                                                         !c.IsDeleted &&
                                                                          c.CartStatusId == (byte)CartStatus.OnHold);

                if (cart.IsNull())
                    return ApiResult<List<CartDetailDto>>.CreateResponse(null, null, new ErrorDetail()
                    {
                        ErrorMessage = "Cartda məhsul yoxdur."
                    });

                foreach (CartDetail cartDetail in cart.CartDetails?.OrderByDescending(cd => cd.CreatedDate).ToList())
                {
                    ProductsStock productStock = cartDetail.Product.ProductsStocks
                                                                .FirstOrDefault(ps => ps.BranchesId == 1 && ps.Sales.Any() &&
                                                                                      ps.ProductStockStatusesId == (byte)ProductStockStatus.Active);
                    cartDetailDtos.Add(new CartDetailDto()
                    {
                        Id = cartDetail.Id,
                        Image = cartDetail.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain)?.Files?.Path,
                        Name = cartDetail.Product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                        Count = cartDetail.Count,
                        StockCount = (int)productStock.Count,
                        Price = productStock.Sales.FirstOrDefault(s => s.ProductStockSaleAmountsTypesId == (byte)ProductStockSaleAmountType.Retail).Amount
                                                     .PercentReductionOf(productStock.ProductsStockDiscountsDetails.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                                                                                                      .Sum(ps => ps.ProductsStockDiscounts.DiscountValue)).Round(2),
                        ProductStockId = productStock.Id,
                        RootName = cartDetail.Product.Name.ToParameterizingRoute()
                    });
                }

                return ApiResult<List<CartDetailDto>>.CreateResponse(cartDetailDtos);
            }
        }
    }
}
