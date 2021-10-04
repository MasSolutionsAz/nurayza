using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductReviews
{
    public class GetProductReviewsQuery : BaseRequest<ApiResult<List<ProductReviewDto>>>
    {
        public string ProductName { get; set; }
        public int BranchId { get; set; }
        public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, ApiResult<List<ProductReviewDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductReviewsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductReviewDto>>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
            {
                var productStocks = await _context.ProductsStock.ToListAsync();
                var id = 0;
                foreach (var item in productStocks)
                {
                    if(item.Product.Name.ToParameterizingRoute() == request.ProductName)
                    {
                        id = item.Id;
                        break;
                    }
                }


                var reviews = await _context.ProductsStockReviews.Where(c => c.ProductsStock.BranchesId == request.BranchId
                                                                             &&
                                                                             c.ProductsStockId == id
                                                                             &&
                                                                             (request.UserId == c.UsersId ? true : c.ProductsStockReviewStatusesId == (byte)ProductStockReviewStatus.Approved))
                                                                               .Select(c => new ProductReviewDto
                                                                               {
                                                                                   Name = c.Users.Name,
                                                                                   Surname = c.Users.Surname,
                                                                                   Text = c.Text,
                                                                                   CreatedDate = c.CreatedDate
                                                                               })
                                                                                .ToListAsync();


                reviews = reviews ??= new List<ProductReviewDto>();
                return ApiResult<List<ProductReviewDto>>.CreateResponse(reviews);

            }
        }
    }
}
