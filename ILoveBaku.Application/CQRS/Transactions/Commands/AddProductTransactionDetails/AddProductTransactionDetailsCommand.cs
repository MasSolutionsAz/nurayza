using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Transactions.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Transactions.Commands.AddProductTransactionDetails
{
    public class AddProductTransactionDetailsCommand:BaseRequest<ApiResult<int?>>
    {
        public int TransactionId { get; set; }
        public ProductTransactionDetailsModel Model { get; set; }
        public class AddProductTransactionDetailsCommandHandler : IRequestHandler<AddProductTransactionDetailsCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductTransactionDetailsCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddProductTransactionDetailsCommand request, CancellationToken cancellationToken)
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


                var productId = await _context.ProductSpecificationValuesBarcodes.Where(c => !c.IsDeleted && c.Value.ToString() == request.Model.Barcode).Select(c => c.ProductsId).FirstOrDefaultAsync();

                if (productId == 0)
                {
                    request.Errors.Add("error", "Belə bir məhsul yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }
                
                var productStock  = await _context.ProductsStock
                                                  .Where(c => c.ProductStockStatusesId == (byte)ProductStockStatus.Active
                                                              &&
                                                              c.ProductId == productId)
                                                             .FirstOrDefaultAsync();

                if(productStock == null)
                {
                    request.Errors.Add("error", "Bu məhsul stockda yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                if(transaction.Details.Any(c=>c.ProductsId == productId))
                {
                    request.Errors.Add("error", "Məhsul artıq əlavə edilib.Düzəliş etmək üçün tranzaksiyanı silib yenidən əlavə edin.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                ProductsTransactionDetails transactionDetail = new ProductsTransactionDetails
                {
                    BuyAmount = request.Model.BuyAmount,
                    CostAmount = request.Model.CostAmount,
                    Count = request.Model.Count,
                    CreatedDate = DateTime.Now,
                    DiscountPercent = request.Model.Discount,
                    DiscountAmount = (request.Model.BuyAmount * request.Model.Count) * request.Model.Discount / 100,
                    PayAmount = (request.Model.BuyAmount * request.Model.Count) - ((request.Model.BuyAmount* request.Model.Count)* request.Model.Discount/100),
                    ProductsId = productId,
                    ProductsTransactionsId = transaction.Id,
                };

                await _context.ProductsTransactionDetails.AddAsync(transactionDetail);
                await _context.SaveChangesAsync();


                transaction.TotalPayAmount = transaction.Details.Sum(c => c.PayAmount);
                transaction.TotalDiscountAmount = transaction.Details.Sum(c => c.DiscountAmount);
                transaction.TotalAmount = transaction.Details.Sum(c => c.DiscountAmount + c.PayAmount);
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(transactionDetail.Id);
            }
        }
    }
}
