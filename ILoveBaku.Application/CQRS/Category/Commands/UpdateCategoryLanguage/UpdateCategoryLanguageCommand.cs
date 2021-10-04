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

namespace ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage
{
    public class UpdateCategoryLanguageCommand : BaseRequest<ApiResult<int?>>
    {
        public CategoryLanguageVm Model { get; set; }
        public class UpdateCategoryLanguageCommandHandler : IRequestHandler<UpdateCategoryLanguageCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategoryLanguageCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategoryLanguageCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                CategoriesLangs categoriesLangs = await _context.CategoriesLangs
                                                                .FirstOrDefaultAsync(c => c.Id == request.Model.Id 
                                                                                          &&
                                                                                          c.LangsId == request.Model.LangsId);

                if (categoriesLangs == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                categoriesLangs.Name = request.Model.Name;
                categoriesLangs.Description = request.Model.Description;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(categoriesLangs.Id);

            }
        }
    }
}
