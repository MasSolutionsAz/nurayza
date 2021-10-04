using AutoMapper;
using ILoveBaku.Application.Common.Extension;
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

namespace ILoveBaku.Application.CQRS.Category.Commands.AddCategory
{
    public class AddCategoryCommand : BaseRequest<ApiResult<int?>>
    {
        public CategoryVm Model { get; set; }

        public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            public AddCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ApiResult<int?>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Create error"
                    });
                }

                //Categories category = _mapper.Map<Categories>(request.Model);
                Categories category = new Categories
                {
                    Title = request.Model.Title.ToParameterizingRoute(),
                    Priority = request.Model.Priority,
                    IsActive = request.Model.IsActive,
                    ParentId = request.Model.ParentId
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                var langs = await _context.Langs.ToListAsync();
                for (int i = 0; i < langs.Count; i++)
                {
                    CategoriesLangs categoryLang = new CategoriesLangs
                    {
                        Name = request.Model.Name,
                        CategoriesId = category.Id,
                        LangsId = langs[i].Id,
                    };

                    await _context.CategoriesLangs.AddAsync(categoryLang);
                }

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(category.Id);
            }
        }
    }
}
