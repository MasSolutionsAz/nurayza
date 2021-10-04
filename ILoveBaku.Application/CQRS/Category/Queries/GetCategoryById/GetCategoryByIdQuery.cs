using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryList;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : BaseRequest<ApiResult<CategoryDto>>
    {
        public int CategoryId { get; set; }
        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResult<CategoryDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<ApiResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<Categories, CategoryDto>(await _context.Categories
                                                                                .Where(c => c.Id == request.CategoryId)
                                                                                .FirstOrDefaultAsync());

                if (data == null)
                    return ApiResult<CategoryDto>.CreateResponse(null,request.Errors);
                else
                    return ApiResult<CategoryDto>.CreateResponse(data);
            }
        }
    }
}
