using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductGroupsByCategoryId
{
    public class GetProductGroupsByCategoryIdQuery:BaseRequest<ApiResult<List<ProductGroupDto>>>
    {
        public int CategoryId { get; set; }
        public class GetProductGroupsByCategoryIdQueryHandler : IRequestHandler<GetProductGroupsByCategoryIdQuery, ApiResult<List<ProductGroupDto>>>
        {
            public readonly IApplicationDbContext _context;
            public IMapper _mapper;
            public GetProductGroupsByCategoryIdQueryHandler(IApplicationDbContext context,
                                                            IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<ProductGroupDto>>> Handle(GetProductGroupsByCategoryIdQuery request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<List<ProductGroups>, List<ProductGroupDto>>(await _context.ProductGroups.Where(p => p.CategoriesId == request.CategoryId).ToListAsync());
                return ApiResult<List<ProductGroupDto>>.CreateResponse(data);
            }
        }
    }
}
