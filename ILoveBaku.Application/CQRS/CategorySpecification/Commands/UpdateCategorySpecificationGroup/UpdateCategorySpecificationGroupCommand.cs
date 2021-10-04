using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationGroup;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationGroup
{
    public class UpdateCategorySpecificationGroupCommand : BaseRequest<ApiResult<int?>>
    {
        public int Id { get; set; }
        public CategorySpecificationGroupCommandDto Model { get; set; }
        public class UpdateCategorySpecificationGroupCommandHandler : IRequestHandler<UpdateCategorySpecificationGroupCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public UpdateCategorySpecificationGroupCommandHandler(IApplicationDbContext context,
                                                                  IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(UpdateCategorySpecificationGroupCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                CategoriesSpecificationsGroups data = await _context.CategoriesSpecificationsGroups
                                                                    .FirstOrDefaultAsync(c => c.Id == request.Id);

                if (data == null)
                {
                    request.Errors.Add("", "Belə bir data yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });
                }

                data = _mapper.Map<CategorySpecificationGroupCommandDto, CategoriesSpecificationsGroups>(request.Model);
                if (data == null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Update error"
                    });

                data.Id = request.Id;
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(data.Id);
            }
        }
    }
}
