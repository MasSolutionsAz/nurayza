using AutoMapper;
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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationGroup
{
    public class AddCategorySpecificationGroupCommand : BaseRequest<ApiResult<int?>>
    {
        public CategorySpecificationGroupCommandDto Model { get; set; }

        public class AddCategorySpecificationGroupCommandHandler : IRequestHandler<AddCategorySpecificationGroupCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public AddCategorySpecificationGroupCommandHandler(IApplicationDbContext context,
                                                                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(AddCategorySpecificationGroupCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });

                CategoriesSpecificationsGroups categorySpecificationGroup = _mapper.Map<CategorySpecificationGroupCommandDto, CategoriesSpecificationsGroups>(request.Model);
                
                if (categorySpecificationGroup == null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });

                await _context.CategoriesSpecificationsGroups.AddAsync(categorySpecificationGroup);
                await _context.SaveChangesAsync();


                var langs = await _context.Langs.ToListAsync();

                for (int i = 0; i < langs.Count; i++)
                {
                    CategoriesSpecificationsGroupsLangs data = new CategoriesSpecificationsGroupsLangs
                    {
                        CategoriesSpecificationsGroupsId = categorySpecificationGroup.Id,
                        LangsId = langs[i].Id,
                        Name = categorySpecificationGroup.Title
                    };

                    await _context.CategoriesSpecificationsGroupsLangs.AddAsync(data);
                }


                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(categorySpecificationGroup.Id);
            }
        }
    }
}
