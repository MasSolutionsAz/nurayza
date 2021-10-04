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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecificationProperties
{
    public class GetCategorySpecificationPropertiesQuery : IRequest<ApiResult<List<CategoriesSpecificationsPropertiesLangs>>>
    {
        public int? CategorySpecificationId { get; set; }
        public class GetCategorySpecificationPropertiesQueryHandler : IRequestHandler<GetCategorySpecificationPropertiesQuery, ApiResult<List<CategoriesSpecificationsPropertiesLangs>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategorySpecificationPropertiesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<CategoriesSpecificationsPropertiesLangs>>> Handle(GetCategorySpecificationPropertiesQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.CategoriesSpecificationsPropertiesLangs
                                        .Where(c => c.LangsId == 10 && c.CategorySpecificationProperty.CategoriesSpecificationId == request.CategorySpecificationId)
                                            .ToListAsync(cancellationToken);
                return ApiResult<List<CategoriesSpecificationsPropertiesLangs>>.CreateResponse(data);
            }
        }
    }
}
