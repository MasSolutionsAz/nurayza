using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecification;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecification
{
    public class UpdateCategorySpecificationCommand:BaseRequest<ApiResult<int?>>
    {
        public int Id { get; set; }
        public CategorySpecificationCommandDto Model { get; set; }
        public class UpdateCategorySpecificationCommandHandler : IRequestHandler<UpdateCategorySpecificationCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public UpdateCategorySpecificationCommandHandler(IApplicationDbContext context,
                                                             IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                CategoriesSpecifications categorySpecification = await _context.CategoriesSpecifications
                                    .FirstOrDefaultAsync(c => c.Id == request.Id);

                if(categorySpecification == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                categorySpecification = _mapper.Map<CategorySpecificationCommandDto, CategoriesSpecifications>(request.Model);

                categorySpecification.Id = request.Id;
                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(categorySpecification.Id);
            }
        }
    }
}
