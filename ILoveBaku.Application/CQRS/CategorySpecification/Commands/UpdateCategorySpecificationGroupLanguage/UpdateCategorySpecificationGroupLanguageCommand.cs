using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationGroupLanguage
{
    public class UpdateCategorySpecificationGroupLanguageCommand:BaseRequest<ApiResult<int?>>
    {
        public string Name { get; set; }
        public int CategorySpecificationGroupId { get; set; }
        public class UpdateCategorySpecificationGroupCommandHandler : IRequestHandler<UpdateCategorySpecificationGroupLanguageCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategorySpecificationGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationGroupLanguageCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                CategoriesSpecificationsGroupsLangs data = await _context.CategoriesSpecificationsGroupsLangs
                                                                    .FirstOrDefaultAsync(c => c.CategoriesSpecificationsGroupsId == request.CategorySpecificationGroupId
                                                                                              &&
                                                                                              c.LangsId == 10);
                 if(data == null)
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
