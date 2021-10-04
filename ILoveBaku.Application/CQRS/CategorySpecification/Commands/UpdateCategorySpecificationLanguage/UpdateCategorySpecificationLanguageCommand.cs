using AutoMapper;
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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategoryLanguageSpecification
{
    public class UpdateCategorySpecificationLanguageCommand:BaseRequest<ApiResult<int?>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public class UpdateCategorySpecificationLanguageCommandHandler : IRequestHandler<UpdateCategorySpecificationLanguageCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public UpdateCategorySpecificationLanguageCommandHandler(IApplicationDbContext context,
                                                                     IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationLanguageCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                CategoriesSpecificationsLangs categorySpecificationLangs = await _context.CategoriesSpecificationsLangs
                                    .FirstOrDefaultAsync(c => c.CategoriesSpecificationsId == request.Id && c.LangsId == 10);

                int id = categorySpecificationLangs.Id;

                if (categorySpecificationLangs == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                categorySpecificationLangs.Name = request.Name;
                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(categorySpecificationLangs.Id);
            }
        }
    }
}
