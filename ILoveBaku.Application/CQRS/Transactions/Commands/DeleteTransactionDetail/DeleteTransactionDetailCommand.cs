using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Transactions.Commands.DeleteTransactionDetail
{
    public class DeleteTransactionDetailCommand:BaseRequest<ApiResult<int?>>
    {
        public int TransactionId { get; set; }
        public int TransactionDetailId { get; set; }
        public class DeleteTransactionDetailCommandHandler : IRequestHandler<DeleteTransactionDetailCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public DeleteTransactionDetailCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(DeleteTransactionDetailCommand request, CancellationToken cancellationToken)
            {
                var transaction = await _context.ProductsTransactions.Where(c => c.ProductsTransactionsStatusesId == (byte)ProductTransactionStatus.OnHold
                                                                                &&
                                                                                c.ProductsTransactionsTypesId == (byte)ProductTransactionType.StockEntry
                                                                                &&
                                                                                c.Id == request.TransactionId).FirstOrDefaultAsync();

                if (transaction == null)
                {
                    request.Errors.Add("error", "Belə bir tranzaksiya tapılmadı.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }


                var transactionDetail = await _context.ProductsTransactionDetails.Where(c => c.ProductsTransactionsId == transaction.Id && c.Id == request.TransactionDetailId).FirstOrDefaultAsync();
                if (transactionDetail == null)
                {
                    request.Errors.Add("error", "Belə bir tranzaksiya detalı tapılmadı.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }


                var transactionDetailId = transactionDetail.Id;
                _context.ProductsTransactionDetails.Remove(transactionDetail);
                await _context.SaveChangesAsync();

                transaction.TotalPayAmount = transaction.Details.Sum(c => c.PayAmount);
                transaction.TotalDiscountAmount = transaction.Details.Sum(c => c.DiscountAmount);
                transaction.TotalAmount = transaction.Details.Sum(c => c.DiscountAmount + c.PayAmount);

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(transactionDetailId);

            }
        }
    }
}
