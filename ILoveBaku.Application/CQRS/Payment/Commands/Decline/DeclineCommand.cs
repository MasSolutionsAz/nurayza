using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Payment.Commands.Paid;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
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
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Payment.Commands.Decline
{
    public class DeclineCommand : BaseRequest<ApiResult<int>>
    {
        public PaymentRequestModel Model { get; set; }
        public class DeclineCommandHandler : IRequestHandler<DeclineCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;

            public DeclineCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<int>> Handle(DeclineCommand request, CancellationToken cancellationToken)
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

                cartOrder.CartOrderStatusId = (byte)KapitalOrderStatus.DECLINED;



                var productCashOut = await _context.ProductsCashOut.FirstOrDefaultAsync(c => c.Id == request.Model.ProductCashOutId);
                productCashOut.ProductsCashOutStatusesId = (int)ProductCashOutStatus.Refused;

                await _context.SaveChangesAsync();

                return ApiResult<int>.CreateResponse(200);
            }
        }
    }
}
