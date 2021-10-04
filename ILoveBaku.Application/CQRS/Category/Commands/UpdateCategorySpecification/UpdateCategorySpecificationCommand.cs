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

namespace ILoveBaku.Application.CQRS.Category.Commands.UpdateCategorySpecification
{
    public class UpdateCategorySpecificationCommand : BaseRequest<ApiResult<int?>>
    {
        public int CategorySpecificationId { get; set; }
        public int CategoryId { get; set; }
        public class UpdateCategorySpecificationCommandHandler : IRequestHandler<UpdateCategorySpecificationCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategorySpecificationCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    request.Errors.Add("", "Server xətası");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail()
                    {
                        ErrorMessage = "Update error"
                    });
                }

                var relation = await _context.CategoriesSpecificationsRelations.FirstOrDefaultAsync(c => c.CategoriesId == request.CategoryId && c.CategoriesSpecificationId == request.CategorySpecificationId);
                if (relation != null)
                {
                    var id = relation.Id;
                    _context.CategoriesSpecificationsRelations.Remove(relation);
                    await _context.SaveChangesAsync();
                    return ApiResult<int?>.CreateResponse(relation.Id);
                }
                else
                {
                    relation = new CategoriesSpecificationsRelations
                    {
                        CategoriesId = request.CategoryId,
                        CategoriesSpecificationId = request.CategorySpecificationId,
                        IsActive = true
                    };
                    await _context.CategoriesSpecificationsRelations.AddAsync(relation);
                    await _context.SaveChangesAsync();
                    return ApiResult<int?>.CreateResponse(relation.Id);
                }


            }
        }
    }
}
