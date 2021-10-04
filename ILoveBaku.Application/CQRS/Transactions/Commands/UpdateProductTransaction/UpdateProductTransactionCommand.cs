using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.Transactions.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Transactions.Commands.UpdateProductTransaction
{
    public class UpdateProductTransactionCommand:BaseRequest<ApiResult<int?>>
    {
        public ProductTransactionDto Model { get; set; }
        public byte? TransactionStatus { get; set; }
        public class UpdateProductTransactionCommandHandler : IRequestHandler<UpdateProductTransactionCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProductTransactionCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateProductTransactionCommand request, CancellationToken cancellationToken)
            {
                var transaction = await _context.ProductsTransactions.Where(c => c.ProductsTransactionsStatusesId == (byte)ProductTransactionStatus.OnHold
                                                                                 &&
                                                                                 c.ProductsTransactionsTypesId == (byte)ProductTransactionType.StockEntry
                                                                                 &&
                                                                                 c.Id == request.Model.Id).FirstOrDefaultAsync();

                if (transaction == null)
                {
                    request.Errors.Add("xeta", "Tranzaksiya tapılmadı.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                if (request.TransactionStatus == null)
                {
                    bool numberValidation = false;
                    int receiptNumber = 0;
                    if (int.TryParse(request.Model.ReceiptNumber, out receiptNumber))
                        numberValidation = true;

                    if (!numberValidation)
                    {
                        request.Errors.Add("xeta", "Sənəd nömrəsi düzgün deyil.");
                        return ApiResult<int?>.CreateResponse(null, request.Errors);
                    }
                    transaction.ReceipDate = DateTime.Parse(request.Model.ReceiptDate);
                    transaction.ReceiptsNumber = receiptNumber;
                    transaction.SuppliersId = request.Model.SupplierId;
                    transaction.Description = request.Model.Note;

                    await _context.SaveChangesAsync();
                    return ApiResult<int?>.CreateResponse(transaction.Id);
                }
                else
                {
                    if(transaction.Details.Count == 0)
                    {
                        request.Errors.Add("xeta", "Tranzaksiya boşdur.");
                        return ApiResult<int?>.CreateResponse(null,request.Errors);

                    }
                    transaction.ProductsTransactionsStatusesId = (byte)ProductTransactionStatus.Finished;
                    //countlar yaranir
                    foreach (var transactionDetails in transaction.Details)
                    {
                        var productStock = await _context.ProductsStock.Where(c => c.ProductId == transactionDetails.ProductsId).FirstOrDefaultAsync();
                        productStock.Count += transactionDetails.Count;

                        //var productCashOutDetail = await _context.ProductsCashOutDetails.Where(c=>c.ProductsCashOut.)
                        ProductsTransactionsCount transactionCount = new ProductsTransactionsCount
                        {
                            Count = transactionDetails.Count,
                            CreatedDate = DateTime.Now,
                            ProductsTransactionsDetailsId = transactionDetails.Id,
                            UpdateDate = DateTime.Now
                        };
                        await _context.ProductsTransactionsCount.AddAsync(transactionCount);
                    }


                    //company transaction yaranir
                    CompanyTransactions companyTransactions = new CompanyTransactions
                    {
                        CompanyTransactionTypesId = (byte)CompanyTransactionType.InComing,
                        CompanyTransactionsStatusesId = (byte)CompanyTransactionStatus.Active,
                        Amount = transaction.TotalPayAmount,
                        CreatedDate = DateTime.Now,
                        CreatedIp = 1,
                        ProductTransactionsId = transaction.Id
                    };

                    await _context.CompanyTransactions.AddAsync(companyTransactions);

                    CompanyDebts debt = new CompanyDebts
                    {
                        SuppliersId = transaction.SuppliersId,
                        BranchesId = transaction.BranchesId,
                        Amount = transaction.TotalPayAmount,
                        CreatedDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };

                    await _context.CompanyDebts.AddAsync(debt);


                    await _context.SaveChangesAsync();
                    return ApiResult<int?>.CreateResponse(transaction.Id);
                }

            }
        }
    }
}
