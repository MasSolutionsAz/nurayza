using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetCoomingSoonProducts
{
    public class GetCommingSoonProductQuery : BaseRequest<ApiResult<List<ProductStockDto>>>
    {
        public class GetCoomingSoonProductQueryHandler : IRequestHandler<GetCommingSoonProductQuery, ApiResult<List<ProductStockDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCoomingSoonProductQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductStockDto>>> Handle(GetCommingSoonProductQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.ProductsStock.Where(c => c.PublishDate > DateTime.Now).Select(c => new ProductStockDto
                {
                    SaleAmount = c.GetPrice(Domain.Enums.ProductStockSaleAmountType.Retail),
                    Name = c.Product.Name,
                    Image = c.Product.ProductsFiles.FirstOrDefault(pf => pf.IsMain).Files.Path,
                    Id = c.Id,
                    IsWishlist =  _context.WishLists.Any(w => w.UsersId == request.UserId && w.ProductsStockId == c.Id)

                }).ToListAsync();

                return ApiResult<List<ProductStockDto>>.CreateResponse(data);
            }
        }
    }
}
