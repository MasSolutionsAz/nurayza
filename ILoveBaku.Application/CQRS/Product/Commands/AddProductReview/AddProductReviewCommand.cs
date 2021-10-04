using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductReview
{
    public class AddProductReviewCommand:BaseRequest<ApiResult<ProductReviewDto>>
    {
        public ProductReviewDto Model { get; set; }
        public int BranchId { get; set; }
        public string ProductName { get; set; }
        public class AddProductReviewCommandHandler : IRequestHandler<AddProductReviewCommand, ApiResult<ProductReviewDto>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductReviewCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductReviewDto>> Handle(AddProductReviewCommand request, CancellationToken cancellationToken)
            {
                var productStocks = await _context.ProductsStock.ToListAsync();
                var id = 0;
                foreach (var item in productStocks)
                {
                    if (item.Product.Name.ToParameterizingRoute() == request.ProductName)
                    {
                        id = item.Id;
                        break;
                    }
                }
                if (id == 0)
                    return ApiResult<ProductReviewDto>.CreateResponse(null);

                ProductsStockReviews review = new ProductsStockReviews
                {
                    ProductsStockId = (int)id,
                    CreatedDate = DateTime.Now,
                    ProductsStockReviewStatusesId = (byte)ProductStockReviewStatus.Rejected,
                    Text = request.Model.Text,
                    UsersId = request.UserId
                };

                await _context.ProductsStockReviews.AddAsync(review);
                await _context.SaveChangesAsync();

                ProductReviewDto response = new ProductReviewDto
                {
                    Name = review.Users.Name,
                    Surname = review.Users.Surname,
                    CreatedDate = review.CreatedDate,
                    Date = review.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                    Text = review.Text
                };

                return ApiResult<ProductReviewDto>.CreateResponse(response);
            }
        }
    }
}
