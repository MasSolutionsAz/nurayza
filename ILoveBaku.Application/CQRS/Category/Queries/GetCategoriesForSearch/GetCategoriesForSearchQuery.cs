using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Category.Models;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoriesForSearch
{
    public class GetCategoriesForSearchQuery : BaseRequest<ApiResult<List<SearchCategoryDto>>>
    {
        public class GetCategoriesForSearchQueryHandler : IRequestHandler<GetCategoriesForSearchQuery, ApiResult<List<SearchCategoryDto>>>
        {
            private readonly IApplicationDbContext _context;

            public GetCategoriesForSearchQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<List<SearchCategoryDto>>> Handle(GetCategoriesForSearchQuery request, CancellationToken cancellationToken)
            {
                List<SearchCategoryDto> categories = await _context.CategoriesLangs.OrderBy(cl => cl.Category.Priority)
                                                                      .Where(c => c.Lang.Culture == request.Culture &&
                                                                                  c.Category.IsActive && c.Category.ParentId == 0).Take(5)
                                                                         .Select(c => new SearchCategoryDto()
                                                                         {
                                                                             Id = c.CategoriesId,
                                                                             Name = c.Name,
                                                                             Svg = c.Category.CategoriesFiles
                                                                                        .FirstOrDefault(cf => cf.Files.FilesTypesId == (byte)FileType.svg)
                                                                                            .Files.Name,
                                                                             Title = c.Category.Title
                                                                         }).ToListAsync();

                return ApiResult<List<SearchCategoryDto>>.CreateResponse(categories);
            }
        }
    }
}
