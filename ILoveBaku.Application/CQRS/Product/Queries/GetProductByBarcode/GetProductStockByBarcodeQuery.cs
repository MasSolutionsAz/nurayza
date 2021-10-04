using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductByBarcode
{
    public class GetProductStockByBarcodeQuery:BaseRequest<ApiResult<ProductStockDetailDto>>
    {
        public string Barcode { get; set; }
        public class GetProductStockByBarcodeQueryHandler : IRequestHandler<GetProductStockByBarcodeQuery, ApiResult<ProductStockDetailDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductStockByBarcodeQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductStockDetailDto>> Handle(GetProductStockByBarcodeQuery request, CancellationToken cancellationToken)
            {
                var productStockBarcodeValue = await _context.ProductSpecificationValuesBarcodes.Where(c => !c.IsDeleted && c.Value == Convert.ToInt64(request.Barcode)).FirstOrDefaultAsync();

                if (productStockBarcodeValue == null)
                {
                    request.Errors.Add("Error", "Belə bir məhsul yoxdur.");
                    return ApiResult<ProductStockDetailDto>.CreateResponse(null, request.Errors);
                }

                var productStock = await _context.ProductsStock
                                                    .Where(c => c.ProductStockStatusesId == (byte)ProductStockStatus.Active
                                                                &&
                                                                c.ProductId == productStockBarcodeValue.ProductsId)
                                                                .Select(c => new ProductStockDetailDto
                                                                         {
                                                                              BuyAmount = c.BuyAmount,
                                                                              CostAmount = c.CostAmount
                                                                         }).FirstOrDefaultAsync();

                return ApiResult<ProductStockDetailDto>.CreateResponse(productStock);
            }
        }
    }
}
