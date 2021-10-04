using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications
{
    public class GetCategorySpecificationsQuery : BaseRequest<ApiResult<List<CategorySpecificationDto>>>
    {
        public int CategoryId { get; set; }
        public class GetCategorySpecificationsQueryHandler : IRequestHandler<GetCategorySpecificationsQuery, ApiResult<List<CategorySpecificationDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategorySpecificationsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<CategorySpecificationDto>>> Handle(GetCategorySpecificationsQuery request, CancellationToken cancellationToken)
            {
                var data = (from categorySpecLang in _context.CategoriesSpecificationsLangs

                            where categorySpecLang.Lang.Culture == request.Culture && categorySpecLang.CategorySpecification.CategoriesSpecificationsStatusesId == (byte)CategorySpecificationStatus.ShowEveryWhere

                            join categorySpecRelation in _context.CategoriesSpecificationsRelations

                            on new { SpecId = categorySpecLang.CategoriesSpecificationsId, CatId= request.CategoryId } equals new { SpecId = categorySpecRelation.CategoriesSpecificationId, CatId =categorySpecRelation.CategoriesId }
                            into Details
                            from categorySpecRelationDetail in Details.DefaultIfEmpty()

                            select new CategorySpecificationDto
                            {
                                CategorySpecificationId = categorySpecLang.CategoriesSpecificationsId,
                                CategorySpecificationRelationId = categorySpecRelationDetail.Id,
                                CategoryId = categorySpecRelationDetail.CategoriesId,
                                Name = categorySpecLang.Name
                            });


                return ApiResult<List<CategorySpecificationDto>>.CreateResponse(data.ToList());
            }
        }
    }
}
