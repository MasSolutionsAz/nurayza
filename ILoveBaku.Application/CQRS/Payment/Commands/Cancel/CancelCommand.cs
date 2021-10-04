using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Payment.Commands.Paid;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Payment.Commands.Cancel
{
    
    public class CancelCommand : BaseRequest<ApiResult<int>>
    {
        public PaymentRequestModel Model { get; set; }
        public class CancelCommandHandler : IRequestHandler<CancelCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;

            public CancelCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<int>> Handle(CancelCommand request, CancellationToken cancellationToken)
            {
                List<Cart> carts = await _context.Carts.Where(c => c.UserId == request.Model.UsersId && !c.IsDeleted &&
                                                                          c.CartStatusId == (byte)CartStatus.OnPayment).ToListAsync();
                var cart = carts.LastOrDefault();

                if (cart.IsNull())
                    return ApiResult<int>.CreateResponse(400, null, new ErrorDetail()
                    { ErrorMessage = "Cart is empty." });

                CartOrder cartOrder = await _context.CartOrders.OrderByDescending(co => co.CreatedDate)
                                                       .FirstOrDefaultAsync(co => co.CartId == cart.Id);

                if (cartOrder.IsNull())
                    return ApiResult<int>.CreateResponse(400, null, new ErrorDetail()
                    { ErrorMessage = "Sizin sifarişiniz yoxdur." });

                cartOrder.CartOrderStatusId = (byte)KapitalOrderStatus.CANCELED;


                var productCashOut = await _context.ProductsCashOut.FirstOrDefaultAsync(c => c.Id == request.Model.ProductCashOutId);
                productCashOut.ProductsCashOutStatusesId = (int)ProductCashOutStatus.Refused;

                await _context.SaveChangesAsync();

                return ApiResult<int>.CreateResponse(200);
            }
        }
    }
}
