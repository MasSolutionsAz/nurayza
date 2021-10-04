using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryList
{
    public class GetCategoryListQuery : BaseRequest<ApiResult<List<CategoryFullDto>>>
    {
        public CategoryFullDto Model { get; set; }
        public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, ApiResult<List<CategoryFullDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetCategoryListQueryHandler(IApplicationDbContext context,
                                                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<CategoryFullDto>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
            {
                var categories = _mapper.Map<List<CategoriesLangs>,List<CategoryFullDto>>(await _context.CategoriesLangs
                                                            .Where(c => c.Lang.Culture == request.Culture
                                                                        &&
                                                                        c.Category.IsActive == request.Model.IsActive
                                                                        &&
                                                                        (request.Model.Name != null) ? c.Name == request.Model.Name : true
                                                                        &&
                                                                        (request.Model.ParentId != null) ? c.Category.ParentId == request.Model.ParentId : true
                                                                        )
                                                                        .ToListAsync(cancellationToken));


                return ApiResult<List<CategoryFullDto>>.CreateResponse(categories);
            }
        }
    }
}
