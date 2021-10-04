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

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductLangs
{
    public class GetProductLangsQuery : BaseRequest<ApiResult<List<ProductLangDto>>>
    {
        public int ProductId { get; set; }
        public class GetProductLangsQueryHandler : IRequestHandler<GetProductLangsQuery, ApiResult<List<ProductLangDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductLangsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductLangDto>>> Handle(GetProductLangsQuery request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.Where(c => c.Id == request.ProductId).FirstOrDefaultAsync();
                if (product == null)
                {
                    request.Errors.Add("", "Belə bir məhsul yoxdur!");
                }

                var productLangs = await _context.ProductsLangs.Where(c => c.ProductsId == request.ProductId).Select(c => new ProductLangDto()
                {
                    ProductId = c.Products.Id,
                    Description = c.Description,
                    Name = c.Name,
                    Id = c.Id,
                    LangName = c.Langs.DisplayName
                }).ToListAsync();

                return ApiResult<List<ProductLangDto>>.CreateResponse(productLangs);
            }
        }
    }
}
