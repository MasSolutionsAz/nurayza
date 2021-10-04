using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Carts.Commands.DeleteCartDetail
{
    public class DeleteCartDetailCommand : BaseRequest<ApiResult<int>>
    {
        public int CartDetailId { get; set; }

        public DeleteCartDetailCommand(int cartDetailId)
        {
            CartDetailId = cartDetailId;
        }

        public class DeleteCartDetailCommandHandler : IRequestHandler<DeleteCartDetailCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;

            public DeleteCartDetailCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<int>> Handle(DeleteCartDetailCommand request, CancellationToken cancellationToken)
            {
                int cartDetailId = request.CartDetailId;

                CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(cd => cd.Id == cartDetailId && cd.Cart.CartStatusId == (byte)CartStatus.OnHold &&
                                                                                             cd.Cart.UserId == request.UserId && !cd.Cart.IsDeleted);

                if (cartDetail.IsNull())
                    return ApiResult<int>.CreateResponse(404, null,
                        new ErrorDetail()
                        {
                            ErrorMessage = "Cart not found."
                        });

                CartOrder cartOrder = cartDetail.Cart.CartOrders.OrderByDescending(co => co.CreatedDate)
                                                                    .FirstOrDefault();

                if (!cartOrder.IsNull() && cartOrder.CartOrderStatusId == (byte)KapitalOrderStatus.CREATED)
                    return ApiResult<int>.CreateResponse(400, null,
                        new ErrorDetail()
                        {
                            ErrorMessage = "Məhsul hal-hazırda ödənişdədir."
                        });

                _context.CartDetails.Remove(cartDetail);

                await _context.SaveChangesAsync();

                return ApiResult<int>.CreateResponse(cartDetailId);
            }
        }
    }
}
