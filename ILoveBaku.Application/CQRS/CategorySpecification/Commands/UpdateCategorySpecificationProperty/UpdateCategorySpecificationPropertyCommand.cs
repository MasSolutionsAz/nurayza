using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationProperty;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationProperty
{
    public class UpdateCategorySpecificationPropertyCommand : BaseRequest<ApiResult<int?>>
    {
        public int CategorySpecificationId { get; set; }
        public CategorySpecificationPropertyCommandDto Model { get; set; }
        public class UpdateCategorySpecificationPropertyCommandHandler : IRequestHandler<UpdateCategorySpecificationPropertyCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public UpdateCategorySpecificationPropertyCommandHandler(IApplicationDbContext context,
                                                                     IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationPropertyCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                CategoriesSpecificationsProperties data = await _context.CategoriesSpecificationsProperties
                                                                    .FirstOrDefaultAsync(c => c.CategoriesSpecificationId == request.CategorySpecificationId);
                if (data == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }
                int dataId = data.Id;

                data = _mapper.Map<CategorySpecificationPropertyCommandDto, CategoriesSpecificationsProperties>(request.Model);
                if (data == null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                data.CategoriesSpecificationId = request.CategorySpecificationId;
                data.Id = dataId;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(data.CategoriesSpecificationId);
            }
        }
    }
}
