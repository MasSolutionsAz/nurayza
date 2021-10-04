using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
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

namespace ILoveBaku.Application.CQRS.Product.Commands.UpdateProductStock
{
    public class UpdateProductStockCommand:BaseRequest<ApiResult<int?>>
    {
        public ProductStockDetailDto Model { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProductStockCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail()
                    {
                        ErrorMessage = "Validation error"
                    });

                ProductsStock productsStock = await _context.ProductsStock.Where(c=>c.ProductId == request.ProductId && c.BranchesId == request.BranchId)
                                                                            .FirstOrDefaultAsync();

                if(productsStock==null)
                {
                    request.Errors.Add("Product", "Belə bir məhsul yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail()
                    {
                        ErrorMessage = "Update error"
                    });
                }

                productsStock.BuyAmount = request.Model.BuyAmount;
                productsStock.PublishDate = request.Model.PublishDate;
                productsStock.CostAmount = request.Model.CostAmount;
                productsStock.Sales.Where(c => c.ProductStockSaleAmountsTypesId == (int)ProductStockSaleAmountType.Retail).FirstOrDefault().Amount = request.Model.Price;
                productsStock.UpdateDate = DateTime.Now;
                productsStock.TaxPercent = request.Model.Tax;
                productsStock.ProductStockStatusesId = request.Model.ProductStockStatusId;
                //productsStock.Description = request.Model.Description;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(productsStock.Id);
            }
        }
    }
}
