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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationPropertyLanguage
{
    public class UpdateCategorySpecificationPropertyLanguageCommand : BaseRequest<ApiResult<int?>>
    {
        public int CategorySpecificationId { get; set; }
        public string Name { get; set; }
        public class UpdateCategorySpecificationPropertyLanguageCommandHandler : IRequestHandler<UpdateCategorySpecificationPropertyLanguageCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategorySpecificationPropertyLanguageCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationPropertyLanguageCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                CategoriesSpecificationsPropertiesLangs data = await _context.CategoriesSpecificationsPropertiesLangs
                                                                                    .FirstOrDefaultAsync(c => c.LangsId == 10
                                                                                               &&
                                                                                               c.CategorySpecificationProperty.CategoriesSpecificationId == request.CategorySpecificationId);
                if (data == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                data.Name = request.Name;
                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(data.Id);
            }
        }
    }
}
