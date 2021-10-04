using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductInvoice
{
    public class GetProductInvoiceQuery : BaseRequest<ApiResult<ProductInvoiceVm>>
    {
        public int CashOutId { get; set; }
        public class GetProductInvoiceQueryHandler : IRequestHandler<GetProductInvoiceQuery, ApiResult<ProductInvoiceVm>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductInvoiceQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductInvoiceVm>> Handle(GetProductInvoiceQuery request, CancellationToken cancellationToken)
            {
                var productCashOut = await _context.ProductsCashOut.Where(c => c.Id == request.CashOutId).FirstOrDefaultAsync();
                if (productCashOut == null)
                    return ApiResult<ProductInvoiceVm>.CreateResponse(null, request.Errors);

                var productInvoices = new List<ProductInvoiceDto>();
                foreach (var productDetail in productCashOut.ProductsCashOutDetails)
                {
                    var productInvoice = new ProductInvoiceDto
                    {
                        Name = productDetail.Products.Name,
                        Description = _context.ProductsStock.Where(c => c.ProductId == productDetail.Products.Id).Select(c => c.Description).FirstOrDefault(),
                        Count = productDetail.Count,
                        Price = _context.ProductsStock.Where(c => c.ProductId == productDetail.Products.Id).Select(c => c.Sales.FirstOrDefault().Amount).FirstOrDefault(),
                        ImageUrl = _context.ProductsFiles.Where(c => c.ProductsId == productDetail.Products.Id).Select(c => c.Files.Path).FirstOrDefault()
                    };
                    productInvoices.Add(productInvoice);
                }

                var model = new ProductInvoiceVm
                {
                    Invoices = productInvoices,
                    ToEmail = _context.UsersLogins.Where(c=>c.UsersId == request.UserId).Select(c=>c.Email).FirstOrDefault()
                };
                return ApiResult<ProductInvoiceVm>.CreateResponse(model);
            }
        }
    }
}
