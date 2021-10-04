using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.CartOrders.Models;
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

namespace ILoveBaku.Application.CQRS.CartOrders.Queries.GetLastCartOrder
{
    public class GetLastCartOrderQuery : BaseRequest<ApiResult<CartOrderDto>>
    {
        public Guid RequestUsersId { get; set; }
        public class GetLastCartOrderQueryHandler : IRequestHandler<GetLastCartOrderQuery, ApiResult<CartOrderDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetLastCartOrderQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<CartOrderDto>> Handle(GetLastCartOrderQuery request, CancellationToken cancellationToken)
            {
                List<Cart> carts = await _context.Carts.Where(c => c.UserId == request.RequestUsersId && !c.IsDeleted &&
                                                                          c.CartStatusId == (byte)CartStatus.OnPayment).ToListAsync();

                var cart = carts.LastOrDefault();
                if (cart.IsNull())
                    return ApiResult<CartOrderDto>.CreateResponse(null, null, new ErrorDetail()
                    { ErrorMessage = "Cart is empty." });

                CartOrder cartOrder = await _context.CartOrders.OrderByDescending(co => co.CreatedDate)
                                                       .FirstOrDefaultAsync(co => co.CartId == cart.Id &&
                                                                                  co.CartOrderStatusId == (byte)KapitalOrderStatus.CREATED);
                if (cartOrder.IsNull())
                    return ApiResult<CartOrderDto>.CreateResponse(null, null, new ErrorDetail()
                    { ErrorMessage = "Siz ödəniş səhifəsinə keçməmisiniz." });

                CartOrderDto model = new CartOrderDto()
                {
                    OrderId = cartOrder?.OrderId,
                    SessionId = cartOrder?.SessionId
                };
                return ApiResult<CartOrderDto>.CreateResponse(model);
            }
        }
    }
}
