using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Carts.Commands.UpdateCartDetail
{
    public class UpdateCartDetailCommand : BaseRequest<ApiResult<int>>
    {
        public int CartDetailId { get; set; }

        public UpdateCartDetailVM Model { get; set; }

        public UpdateCartDetailCommand(int cartDetailId, UpdateCartDetailVM model)
        {
            CartDetailId = cartDetailId;
            Model = model;
        }

        public class UpdateCartDetailCommandHandler : IRequestHandler<UpdateCartDetailCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;

            public UpdateCartDetailCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<int>> Handle(UpdateCartDetailCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int>.CreateResponse(400, request.Errors, new ErrorDetail()
                    {
                        ErrorMessage = "Update Cart error"
                    });

                int cartDetailId = request.Model.CartDetailId;

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

                cartDetail.Count = request.Model.Count;

                await _context.SaveChangesAsync();

                return ApiResult<int>.CreateResponse(cartDetailId);
            }
        }
    }
}
