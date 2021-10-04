using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Category.Commands.AddCategory;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : BaseRequest<ApiResult<int?>>
    {
        public int Id { get; set; }
        public CategoryVm Model { get; set; }
        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                }


                Categories category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id == request.Id);

                if (category == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                category.IsActive = request.Model.IsActive;
                category.ParentId = request.Model.ParentId;
                category.Priority = request.Model.Priority;
                category.Title = request.Model.Title.ToParameterizingRoute();

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(category.Id);
;            }
        }
    }
}
