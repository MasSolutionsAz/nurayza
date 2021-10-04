using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductPhotos
{
    public class GetProductPhotosQuery:BaseRequest<ApiResult<List<ProductFileDto>>>
    {
        public int ProductId { get; set; }
        public class GetProductPhotosQueryHandler : IRequestHandler<GetProductPhotosQuery, ApiResult<List<ProductFileDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProductPhotosQueryHandler(IApplicationDbContext context,
                                                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<ProductFileDto>>> Handle(GetProductPhotosQuery request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<List<ProductsFiles>,List<ProductFileDto>>(await _context.ProductsFiles.Where(c => c.ProductsId == request.ProductId).ToListAsync());
                return ApiResult<List<ProductFileDto>>.CreateResponse(data);
            }
        }
    }
}
