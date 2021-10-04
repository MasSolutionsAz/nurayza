using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductToStock
{
    public class AddProductToStockCommand : BaseRequest<ApiResult<int?>>
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public class AddProductToStockCommandHandler : IRequestHandler<AddProductToStockCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductToStockCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddProductToStockCommand request, CancellationToken cancellationToken)
            {
                Products products = await _context.Products.FirstOrDefaultAsync(c => c.Id == request.ProductId);
                if (products == null)
                {
                    request.Errors.Add("product", "Belə bir məhsul yoxdur");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });
                }

                ProductsStock productStock = await _context.ProductsStock.FirstOrDefaultAsync(c => c.ProductId == request.ProductId && c.BranchesId == request.BranchId);
                if (productStock != null)
                {
                    request.Errors.Add("product", "Bu məhsul artıq ambardadır.Səhifəni yeniləyin.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });
                }

                productStock = new ProductsStock
                {
                    BranchesId = request.BranchId,
                    ProductId = products.Id,
                    BuyAmount = products.DefaultBuyAmount,
                    CostAmount = products.DefaultCostAmount,
                    ProductStockStatusesId = (byte)ProductStockStatus.Active,
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Count = 0,
                    PriorityDate = DateTime.Now,
                    MinCount =0,
                    TaxPercent = products.TaxPercent==null?0:(decimal)products.TaxPercent,
                    PublishDate = products.DefaultPublishDate
                };

                await _context.ProductsStock.AddAsync(productStock);
                await _context.SaveChangesAsync();

                ProductsStockSaleAmounts amount = new ProductsStockSaleAmounts
                {
                    ProductsStockId = productStock.Id,
                    ProductStockSaleAmountsTypesId = (byte)ProductStockSaleAmountType.Retail,
                    Amount = products.DefaultSaleAmount==null?0:(decimal)products.DefaultSaleAmount
                };

                await _context.ProductsStockSaleAmounts.AddAsync(amount);
                await _context.SaveChangesAsync();


                return ApiResult<int?>.CreateResponse(productStock.Id);
            }
        }
    }
}
