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

namespace ILoveBaku.Application.CQRS.Transactions.Queries.GetProductStockTransactions
{
    public class GetProductStockTransactionsQuery : BaseRequest<ApiResult<List<ProductTransactionDto>>>
    {
        public ProductTransactionFilter Model { get; set; }
        public class GetProductStockTransactionQueryHandler : IRequestHandler<GetProductStockTransactionsQuery, ApiResult<List<ProductTransactionDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductStockTransactionQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductTransactionDto>>> Handle(GetProductStockTransactionsQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(c => c.Id == request.UserId).FirstOrDefaultAsync();
                var transactions = await _context.ProductsTransactions.Where(c => c.BranchesId == user.BranchesId
                                                                                &&
                                                                                c.ProductsTransactionsTypesId == request.Model.ProductTransactionType
                                                                                &&
                                                                                (request.Model.ProductTransactionStatus!=null?c.ProductsTransactionsStatusesId == request.Model.ProductTransactionStatus:true)
                                                                                )
                                                                            .Select(c => new ProductTransactionDto
                                                                            {
                                                                                Id = c.Id,
                                                                                SupplierId = c.SuppliersId,
                                                                                Supplier =  _context.Suppliers.Where(cc=>cc.Id == c.SuppliersId).Select(cc=>cc.CompanyDetails.Name).FirstOrDefault(),
                                                                                ReceiptNumber = c.ReceiptsNumber.ToString(),
                                                                                ReceiptDate = c.ReceipDate.ToShortDateString(),
                                                                                Discount = c.TotalDiscountAmount,
                                                                                Note = c.Description,
                                                                                PayAmount = c.TotalPayAmount,
                                                                                TotalAmount = c.TotalAmount
                                                                            }).OrderByDescending(k=>k.Id).ToListAsync();

                transactions = transactions ??= new List<ProductTransactionDto>();

                return ApiResult<List<ProductTransactionDto>>.CreateResponse(transactions);
            }
        }
    }
}
