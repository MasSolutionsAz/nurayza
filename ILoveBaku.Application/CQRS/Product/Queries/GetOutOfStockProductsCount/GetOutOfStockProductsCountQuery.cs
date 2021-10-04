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

namespace ILoveBaku.Application.CQRS.Product.Queries.GetOutOfStockProductsCount
{
    public class GetOutOfStockProductsCountQuery:BaseRequest<ApiResult<int?>>
    {
        public int BranchId { get; set; }
        public class GetOutOfStockProductsCountQueryHandler : IRequestHandler<GetOutOfStockProductsCountQuery, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public GetOutOfStockProductsCountQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(GetOutOfStockProductsCountQuery request, CancellationToken cancellationToken)
            {
                var result=await _context.Products.Where(c => !c.ProductsStocks.Any(c => c.BranchesId == request.BranchId)).CountAsync();
                return ApiResult<int?>.CreateResponse(result);
            }
        }
    }
}
