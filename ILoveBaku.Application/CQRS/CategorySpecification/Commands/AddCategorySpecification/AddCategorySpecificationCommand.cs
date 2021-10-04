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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecification
{
    public class AddCategorySpecificationCommand : BaseRequest<ApiResult<int?>>
    {
        public CategorySpecificationCommandDto Model { get; set; }
        public class AddCategorySpecificationCommandHandler : IRequestHandler<AddCategorySpecificationCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public AddCategorySpecificationCommandHandler(IApplicationDbContext context,
                                                          IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(AddCategorySpecificationCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });

                CategoriesSpecifications categorySpecification = _mapper.Map<CategorySpecificationCommandDto, CategoriesSpecifications>(request.Model);

                if (categorySpecification == null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });

                await _context.CategoriesSpecifications.AddAsync(categorySpecification);
                await _context.SaveChangesAsync();

                var langs = await _context.Langs.ToListAsync();
                for (int i = 0; i < langs.Count; i++)
                {
                    CategoriesSpecificationsLangs categoriesSpecificationsLangs = new CategoriesSpecificationsLangs
                    {
                        CategoriesSpecificationsId = categorySpecification.Id,
                        LangsId = langs[i].Id,
                        Name = categorySpecification.Title,
                    };
                    await _context.CategoriesSpecificationsLangs.AddAsync(categoriesSpecificationsLangs);
                }

                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(categorySpecification.Id);
            }
        }
    }
}
