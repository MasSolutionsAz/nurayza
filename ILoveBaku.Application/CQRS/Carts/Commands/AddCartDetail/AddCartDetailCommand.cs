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
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Carts.Commands.AddCartDetail
{
    public class AddCartDetailCommand : BaseRequest<ApiResult<int>>
    {
        public AddCartDetailVM Model { get; set; }

        public AddCartDetailCommand(AddCartDetailVM model) => Model = model;

        public class AddToCartDetailCommandHandler : IRequestHandler<AddCartDetailCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;

            public AddToCartDetailCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<int>> Handle(AddCartDetailCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int>.CreateResponse(400, request.Errors, new ErrorDetail()
                    {
                        ErrorMessage = "Add Cart error"
                    });

                Guid userId = request.UserId;

                int productId = request.Model.ProductId;

                ProductsStock productsStock = await _context.ProductsStock
                                                               .FirstOrDefaultAsync(ps => ps.BranchesId == 1 && ps.ProductId == productId &&
                                                                                          ps.ProductStockStatusesId == (byte)ProductStockStatus.Active);

                if (productsStock.Count <= 0)
                    return ApiResult<int>.CreateResponse(404, null,
                            new ErrorDetail()
                            {
                                ErrorMessage = "This product is not in stock."
                            });

                if (productsStock.Count < request.Model.Count)
                    return ApiResult<int>.CreateResponse(406, null,
                                new ErrorDetail()
                                {
                                    ErrorMessage = "There is not enough product in stock."
                                });

                if (await _context.CartDetails.AnyAsync(cd => cd.ProductId == productId && cd.Cart.UserId == userId &&
                                                              cd.Cart.CartStatusId == (byte)CartStatus.OnHold && !cd.Cart.IsDeleted))
                    return ApiResult<int>.CreateResponse(208, null,
                        new ErrorDetail()
                        {
                            ErrorMessage = "This product is already in cart."
                        });

                Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted &&
                                                                          c.CartStatusId == (byte)CartStatus.OnHold);

                if (cart.IsNull())
                {
                    cart = new Cart()
                    {
                        UserId = userId,
                        CartStatusId = (byte)CartStatus.OnHold,
                        CreatedDate = DateTime.Now
                    };

                    await _context.Carts.AddAsync(cart);

                    await _context.SaveChangesAsync();
                }

                CartDetail cartDetail = new CartDetail()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Count = request.Model.Count,
                    CreatedDate = DateTime.Now
                };

                await _context.CartDetails.AddAsync(cartDetail);

                await _context.SaveChangesAsync();

                return ApiResult<int>.CreateResponse(cartDetail.Id);
            }
        }
    }
}
