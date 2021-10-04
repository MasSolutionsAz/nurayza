using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Transactions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Transactions.Queries.GetProductStockTransactionDetails
{
    public class GetProductStockTransactionDetailsQuery:BaseRequest<ApiResult<List<ProductTransactionDetailsDto>>>
    {
        public int TransactionId { get; set; }
        public class GetProductStockTransactionDetailsQueryHandler : IRequestHandler<GetProductStockTransactionDetailsQuery, ApiResult<List<ProductTransactionDetailsDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductStockTransactionDetailsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductTransactionDetailsDto>>> Handle(GetProductStockTransactionDetailsQuery request, CancellationToken cancellationToken)
            {
                var transactionDetails = await _context.ProductsTransactionDetails.Where(c => c.ProductsTransactionsId == request.TransactionId)
                                                                                           .Select(c => new ProductTransactionDetailsDto
                                                                                           {
                                                                                               Barcode = _context.ProductSpecificationValuesBarcodes.Where(a=>a.ProductsId == c.ProductsId).Select(a=>a.Value).FirstOrDefault(),
                                                                                               BuyAmount = c.BuyAmount,
                                                                                               CostAmount = c.CostAmount,
                                                                                               Count = c.Count,
                                                                                               DiscountAmount = c.DiscountPercent,
                                                                                               Name = c.Products.Name,
                                                                                               PayAmount = c.PayAmount,
                                                                                               ProductTransactionDetailId = c.Id,
                                                                                               ProductTransactionId = c.ProductsTransactionsId,
                                                                                               TotalAmount = c.BuyAmount*c.Count
                                                                                           }).ToListAsync();

                
                transactionDetails = transactionDetails ??= new List<ProductTransactionDetailsDto>();
                return ApiResult<List<ProductTransactionDetailsDto>>.CreateResponse(transactionDetails);
            }
        }
    }
}
