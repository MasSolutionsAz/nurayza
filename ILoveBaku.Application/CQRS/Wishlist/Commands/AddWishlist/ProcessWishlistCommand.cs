using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Wishlist.Commands.AddWishlist
{
    public class ProcessWishlistCommand : BaseRequest<ApiResult<bool?>>
    {
        public int ProductStockId { get; set; }

        public ProcessWishlistCommand(int productStockId) => ProductStockId = productStockId;

        public class ProcessWishlistCommandHandler : IRequestHandler<ProcessWishlistCommand, ApiResult<bool?>>
        {
            private readonly IApplicationDbContext _context;

            public ProcessWishlistCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<bool?>> Handle(ProcessWishlistCommand request, CancellationToken cancellationToken)
            {
                Guid userId = request.UserId;

                int productStockId = request.ProductStockId;

                WishLists wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.UsersId == userId &&
                                                                                       w.ProductsStockId == productStockId);

                bool? status = null;

                if (wishList.IsNull())
                {
                    if (await _context.ProductsStock.AnyAsync(ps => ps.Id == productStockId))
                    {
                        _ = await _context.WishLists.AddAsync(new WishLists()
                        {
                            UsersId = userId,
                            ProductsStockId = productStockId,
                            CreatedDate = DateTime.Now
                        });
                        status = true;
                    }
                }
                else
                {
                    _ = _context.WishLists.Remove(wishList);
                    status = false;
                }

                _ = await _context.SaveChangesAsync();

                return ApiResult<bool?>.CreateResponse(status);
            }
        }
    }
}
