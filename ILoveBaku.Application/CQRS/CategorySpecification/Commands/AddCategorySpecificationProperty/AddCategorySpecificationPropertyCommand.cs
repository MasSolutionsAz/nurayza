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

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationProperty
{
    public class AddCategorySpecificationPropertyCommand : BaseRequest<ApiResult<int?>>
    {
        public CategorySpecificationPropertyCommandDto Model { get; set; }
        public int CategorySpecificationId { get; set; }
        public class AddCategorySpecificationPropertyCommandHandler : IRequestHandler<AddCategorySpecificationPropertyCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public AddCategorySpecificationPropertyCommandHandler(IApplicationDbContext context,
                                                                  IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<int?>> Handle(AddCategorySpecificationPropertyCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });

                CategoriesSpecificationsProperties data = _mapper.Map<CategorySpecificationPropertyCommandDto, CategoriesSpecificationsProperties>(request.Model);
                if(data == null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Add error"
                    });

                data.CategoriesSpecificationId = request.CategorySpecificationId;
                await _context.CategoriesSpecificationsProperties.AddAsync(data);

                var langs = await _context.Langs.ToListAsync();
                for (int i = 0; i < langs.Count; i++)
                {
                    CategoriesSpecificationsPropertiesLangs dataLang = new CategoriesSpecificationsPropertiesLangs()
                    {
                        CategoriesSpecificationsPropertiesId = data.Id,
                        LangsId = langs[i].Id,
                        Name = data.Title
                    };
                    await _context.CategoriesSpecificationsPropertiesLangs.AddAsync(dataLang);
                }


                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(data.Id);
            }
        }
    }
}
