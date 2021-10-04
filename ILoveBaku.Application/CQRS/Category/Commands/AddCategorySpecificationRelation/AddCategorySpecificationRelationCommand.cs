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

namespace ILoveBaku.Application.CQRS.Category.Commands.AddCategorySpecificationRelation
{
    public class AddCategorySpecificationRelationCommand : BaseRequest<ApiResult<int?>>
    {
        public CategorySpecificationVm Model { get; set; }
        public class AddCategorySpecificationRelationCommandHandler : IRequestHandler<AddCategorySpecificationRelationCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddCategorySpecificationRelationCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddCategorySpecificationRelationCommand request, CancellationToken cancellationToken)
            {
                CategoriesSpecificationsRelations relation = await _context.CategoriesSpecificationsRelations
                                                                                .FirstOrDefaultAsync(c => c.CategoriesId == request.Model.CategoryId
                                                                                                        &&
                                                                                                        c.CategoriesSpecificationId == request.Model.CategorySpecificationId);
                if (relation != null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Create error"
                    });

                relation = new CategoriesSpecificationsRelations
                {
                    CategoriesId = request.Model.CategoryId,
                    CategoriesSpecificationId = request.Model.CategorySpecificationId,
                    IsActive = request.Model.IsActive
                };
                await _context.CategoriesSpecificationsRelations.AddAsync(relation);
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(relation.Id);
            }
        }
    }
}
