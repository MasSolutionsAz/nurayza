using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Carts.Commands.AddCartsToUserFromSession
{
    public class AddCartsToUserFromSessionCommand : BaseRequest<ApiResult<int?>>
    {
        public List<CartDetailDto> Model { get; set; }
        public Guid RequestUsersId { get; set; }
        public class AddCartsToUserFromSessionCommandHandler : IRequestHandler<AddCartsToUserFromSessionCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddCartsToUserFromSessionCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddCartsToUserFromSessionCommand request, CancellationToken cancellationToken)
            {

                Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == request.RequestUsersId && !c.IsDeleted &&
                                                          c.CartStatusId == (byte)ILoveBaku.Domain.Enums.CartStatus.OnHold);

                if (cart == null)
                {
                    //Map carts to the user
                    cart = new Cart()
                    {
                        UserId = request.RequestUsersId,
                        CartStatusId = (byte)Domain.Enums.CartStatus.OnHold,
                        CreatedDate = DateTime.Now
                    };

                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();
                }


              

                foreach (var sessionCartItems in request.Model)
                {
                    CartDetail cartDetail = null;
                    if (cart.CartDetails!= null && cart.CartDetails.Any(c=>c.ProductId == sessionCartItems.ProductId))
                    {
                        cartDetail =  cart.CartDetails.FirstOrDefault(c => c.ProductId == sessionCartItems.ProductId);
                        cartDetail.Count += sessionCartItems.Count;
                    }
                    else
                    {
                        cartDetail = new CartDetail()
                        {
                            CartId = cart.Id,
                            ProductId = sessionCartItems.ProductId,
                            Count = sessionCartItems.Count,
                            CreatedDate = DateTime.Now
                        };
                        await _context.CartDetails.AddAsync(cartDetail);
                    }
                    
                }
                //end map carts to the user

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(200);
            }
        }
    }
}
