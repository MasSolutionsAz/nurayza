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

namespace ILoveBaku.Application.CQRS.News.Queries.GetNewsPhotos
{
    public class GetNewsPhotosQuery : BaseRequest<ApiResult<List<ProductFileDto>>>
    {
        public int NewsId { get; set; }
        public class GetNewsPhotosQueryHandler : IRequestHandler<GetNewsPhotosQuery, ApiResult<List<ProductFileDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetNewsPhotosQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductFileDto>>> Handle(GetNewsPhotosQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.NewsFiles.Where(c => c.NewsId == request.NewsId).Select(c => new ProductFileDto
                {
                    IsMain = c.IsMain,
                    Name = c.Files.Name,
                    Path = c.Files.Path
                }).ToListAsync();

                return ApiResult<List<ProductFileDto>>.CreateResponse(data);
            }
        }
    }
}
