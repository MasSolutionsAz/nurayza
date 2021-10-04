using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Transactions.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Transactions.Commands.AddProductTransaction
{
    public class AddProductTransactionCommand : BaseRequest<ApiResult<int?>>
    {
        public ProductTransactionCreateDto Model { get; set; }
        public class AddProductTransactionCommandHandler : IRequestHandler<AddProductTransactionCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductTransactionCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddProductTransactionCommand request, CancellationToken cancellationToken)
            {
                ProductsTransactions transaction = new ProductsTransactions
                {
                    SuppliersId = request.Model.SupplierId,
                    ProductsTransactionsStatusesId = request.Model.ProductTransactionStatus,
                    BranchesId = await _context.Users.Where(c => c.Id == request.UserId).Select(c => c.BranchesId).FirstOrDefaultAsync(),
                    CreatedDate = DateTime.Now,
                    Description = request.Model.Description,
                    ProductsTransactionsTypesId = request.Model.ProductTransactionType,
                    ReceipDate = DateTime.Parse(request.Model.ReceiptDate),
                    ReceiptsNumber = Convert.ToInt32(request.Model.ReceiptNumber),
                    TotalAmount = 0,
                    TotalDiscountAmount = 0,
                    TotalPayAmount = 0,
                    UpdatedDate = DateTime.Now
                };


                await _context.ProductsTransactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(transaction.Id);
            }
        }
    }
}
