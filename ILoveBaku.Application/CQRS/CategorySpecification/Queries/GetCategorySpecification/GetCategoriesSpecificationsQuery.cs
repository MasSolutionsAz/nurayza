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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecification
{
    public class GetCategoriesSpecificationsQuery : IRequest<ApiResult<List<CategoriesSpecificationsLangs>>>
    {
        public GetCategorySpecificationDto Model { get; set; }
        public class GetCategoriesSpecificationsHandler : IRequestHandler<GetCategoriesSpecificationsQuery, ApiResult<List<CategoriesSpecificationsLangs>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategoriesSpecificationsHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<CategoriesSpecificationsLangs>>> Handle(GetCategoriesSpecificationsQuery request, CancellationToken cancellationToken)
            {
                var categoriesSpecificationsLangs = await _context.CategoriesSpecificationsLangs
                                                           .Where(c => c.LangsId == 10
                                                                       &&
                                                                       request.Model.CategoriesSpecificationGroupId != null ?
                                                                       c.CategorySpecification.CategoriesSpecificationGroupId == request.Model.CategoriesSpecificationGroupId : true
                                                                       &&
                                                                       request.Model.CategoriesSpecificationsStatusesId != null ?
                                                                       c.CategorySpecification.CategoriesSpecificationsStatusesId == request.Model.CategoriesSpecificationsStatusesId : true
                                                                       &&
                                                                       request.Model.CategoriesSpecificationTypeId != null ?
                                                                       c.CategorySpecification.CategoriesSpecificationsTypeId == request.Model.CategoriesSpecificationTypeId : true
                                                                       )
                                                                       .ToListAsync(cancellationToken);
                return ApiResult<List<CategoriesSpecificationsLangs>>.CreateResponse(categoriesSpecificationsLangs);
            }
        }
    }
}
