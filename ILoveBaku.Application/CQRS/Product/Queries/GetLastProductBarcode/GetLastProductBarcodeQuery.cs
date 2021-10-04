using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetLastProductBarcode
{
    public class GetLastProductBarcodeQuery : BaseRequest<ApiResult<string>>
    {
        public class GetLastProductBarcodeQueryHandler : IRequestHandler<GetLastProductBarcodeQuery, ApiResult<string>>
        {
            private readonly IApplicationDbContext _context;
            public GetLastProductBarcodeQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<string>> Handle(GetLastProductBarcodeQuery request, CancellationToken cancellationToken)
            {
                var barcode = await _context.ProductSpecificationValuesBarcodes
                                                    .OrderByDescending(c=>c.Id)
                                                        .FirstOrDefaultAsync(c=>c.IsManual);
                if(barcode==null)
                return ApiResult<string>.CreateResponse("");


                return ApiResult<string>.CreateResponse(barcode.Value.ToString());
            }
        }
    }
}
