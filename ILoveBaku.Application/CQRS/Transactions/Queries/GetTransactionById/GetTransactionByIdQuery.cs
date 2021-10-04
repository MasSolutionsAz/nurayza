using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Transactions.Models;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : BaseRequest<ApiResult<ProductTransactionDto>>
    {
        public int TransactionId { get; set; }
        public byte TransactionType { get; set; }
        public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, ApiResult<ProductTransactionDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetTransactionByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductTransactionDto>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
            {
                var transaction = await _context.ProductsTransactions.Where(c => c.Id == request.TransactionId && c.ProductsTransactionsTypesId == request.TransactionType)
                                                                              .Select(c => new ProductTransactionDto
                                                                              {
                                                                                  Id = c.Id,
                                                                                  SupplierId = c.SuppliersId,
                                                                                  Note = c.Description,
                                                                                  TotalBuyAmount = c.Details.Sum(a => a.BuyAmount * a.Count),
                                                                                  Discount = c.TotalDiscountAmount,
                                                                                  PayAmount = c.TotalPayAmount,
                                                                                  ReceiptNumber = c.ReceiptsNumber.ToString(),
                                                                                  ReceiptDate = c.ReceipDate.ToShortDateString(),
                                                                                  TotalAmount = c.TotalAmount,
                                                                                  TotalCostAmount = c.Details.Sum(a => a.CostAmount * a.Count),
                                                                                  Day = c.ReceipDate.Day,
                                                                                  Month = c.ReceipDate.Month,
                                                                                  TransactionStatus = (byte)c.ProductsTransactionsStatusesId,
                                                                                  Year = c.ReceipDate.Year
                                                                              }).OrderByDescending(c => c.Id)
                                                                                .FirstOrDefaultAsync();


                transaction = transaction ??= new ProductTransactionDto();
                return ApiResult<ProductTransactionDto>.CreateResponse(transaction);
            }
        }
    }
}
