using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage;
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

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryLanguagesById
{
    public class GetCategoryLanguagesByIdQuery : BaseRequest<ApiResult<List<CategoryLanguageVm>>>
    {
        public int CategoryId { get; set; }
        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryLanguagesByIdQuery, ApiResult<List<CategoryLanguageVm>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<CategoryLanguageVm>>> Handle(GetCategoryLanguagesByIdQuery request, CancellationToken cancellationToken)
            {
                var categoryLangs = _mapper.Map<List<CategoriesLangs>, List<CategoryLanguageVm>>(await _context.CategoriesLangs.Include(c => c.Category)
                                                                    .Where(c => c.CategoriesId == request.CategoryId
                                                                                              )
                                                                    .ToListAsync());
                return ApiResult<List<CategoryLanguageVm>>.CreateResponse(categoryLangs);
            }
        }
    }
}
