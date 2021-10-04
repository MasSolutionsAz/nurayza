using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecificationsGroups
{
   
    public class GetCategorySpecificationsGroupsQuery : IRequest<ApiResult<List<CategoriesSpecificationsGroupsLangs>>>
    {
        public bool IsActive { get; set; }
        public class GetCategorySpecificationsGroupsQueryHandler : IRequestHandler<GetCategorySpecificationsGroupsQuery, ApiResult<List<CategoriesSpecificationsGroupsLangs>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategorySpecificationsGroupsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<CategoriesSpecificationsGroupsLangs>>> Handle(GetCategorySpecificationsGroupsQuery request, CancellationToken cancellationToken)
            {
                var categorySpecificationsGroupsLangs = await _context.CategoriesSpecificationsGroupsLangs.Where(c => c.LangsId == 10
                                                                                                                    &&
                                                                                                                    c.CategoriesSpecificationsGroup.IsActive == request.IsActive)
                                                                                                                    .ToListAsync(cancellationToken);

                return ApiResult<List<CategoriesSpecificationsGroupsLangs>>.CreateResponse(categorySpecificationsGroupsLangs);
            }
        }
    }
}
