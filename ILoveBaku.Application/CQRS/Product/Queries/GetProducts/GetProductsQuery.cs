using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ILoveBaku.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using ILoveBaku.Domain.Stored_Procedures;
using ILoveBaku.Application.CQRS.Product.Models;
using System;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProducts
{
    public class GetProductsQuery:BaseRequest<ApiResult<ProductAdminListVm>>
    {
        public int Page { get; set; }
        public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery,ApiResult<ProductAdminListVm>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductAdminListVm>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.ProductListStoredProcedure(request.Culture);
                var total = (int)Math.Ceiling(data.Count / (decimal)20);
                data = data.Skip(20 * (request.Page - 1)).Take(20).ToList();

                ProductAdminListVm vm = new ProductAdminListVm
                {
                    Products = data,
                    Current = request.Page,
                    Total = total
                };
                return ApiResult<ProductAdminListVm>.CreateResponse(vm);
            }
        }
    }
}
