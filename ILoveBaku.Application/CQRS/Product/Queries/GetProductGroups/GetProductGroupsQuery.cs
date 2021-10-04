using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups
{
    public class GetProductGroupsQuery:BaseRequest<ApiResult<List<ProductGroupDto>>>
    {
        public class GetProductGroupsQueryHandler : IRequestHandler<GetProductGroupsQuery, ApiResult<List<ProductGroupDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProductGroupsQueryHandler(IApplicationDbContext context,
                                                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<ProductGroupDto>>> Handle(GetProductGroupsQuery request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<List<ProductGroups>, List<ProductGroupDto>>(await _context.ProductGroups.ToListAsync());

                return ApiResult<List<ProductGroupDto>>.CreateResponse(data);
            }
        }
    }
}
