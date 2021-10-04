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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecificationsTypes
{
    public class GetCategorySpecificationsTypesQuery : IRequest<ApiResult<List<CategoriesSpecificationsTypes>>>
    {
        public int? ControllerId { get; set; }
        public string Name { get; set; }
        public class GetCategorySpecificationsTypesQueryHandler : IRequestHandler<GetCategorySpecificationsTypesQuery, ApiResult<List<CategoriesSpecificationsTypes>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategorySpecificationsTypesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<CategoriesSpecificationsTypes>>> Handle(GetCategorySpecificationsTypesQuery request, CancellationToken cancellationToken)
            {
                var categorySpecificationsTypes = await _context.CategoriesSpecificationsTypes
                                                                 .Where(c => request.ControllerId != null ?
                                                                        c.CategorySpecificationsTypesControllersId == request.ControllerId : true
                                                                        &&
                                                                        request.Name != null ?
                                                                        c.Name == request.Name : true).ToListAsync(cancellationToken);

                return ApiResult<List<CategoriesSpecificationsTypes>>.CreateResponse(categorySpecificationsTypes);
            }
        }
    }
}
