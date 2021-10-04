using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetBanners
{
    public class GetBannersQuery : BaseRequest<ApiResult<List<ProductFileDto>>>
    {
        public int MenuId { get; set; }

        public class GetBannersQueryHandler : IRequestHandler<GetBannersQuery, ApiResult<List<ProductFileDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetBannersQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductFileDto>>> Handle(GetBannersQuery request, CancellationToken cancellationToken)
            {
                var photos = await _context.MenuBannerItems.Where(c => c.MenuId == request.MenuId).Select(c => new ProductFileDto
                {
                    Name = c.File.Name,
                    Path = c.File.Path
                }).ToListAsync();


                return ApiResult<List<ProductFileDto>>.CreateResponse(photos);
            }
        }
    }
}
