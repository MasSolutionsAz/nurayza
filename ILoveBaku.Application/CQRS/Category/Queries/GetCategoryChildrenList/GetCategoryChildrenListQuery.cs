using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList
{
    public class GetCategoryChildrenListQuery : BaseRequest<ApiResult<CategoryChildrenListVm>>
    {
        public int CategoryId { get; set; }
        public int Page { get; set; }
        public class GetCategoryChildsListQueryHandler : IRequestHandler<GetCategoryChildrenListQuery, ApiResult<CategoryChildrenListVm>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IConfiguration _configuration;
            public GetCategoryChildsListQueryHandler(IApplicationDbContext context,
                                                     IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
            public async Task<ApiResult<CategoryChildrenListVm>> Handle(GetCategoryChildrenListQuery request, CancellationToken cancellationToken)
            {
                int take = Convert.ToInt32(_configuration["Default:CategoryList"]);
                List<CategoryChildrenDto> data = await Recursive(request.Culture, request.CategoryId);
                int total = (int)Math.Ceiling(data.Count / (decimal)take);
                data = request.Page == 0 ? data : data.Skip((request.Page - 1) * take).Take(take).ToList();

                CategoryChildrenListVm vm = new CategoryChildrenListVm
                {
                    Children = data.ToList(),
                    Total = total
                };
                return ApiResult<CategoryChildrenListVm>.CreateResponse(vm);
            }

            public async Task<List<CategoryChildrenDto>> Recursive(string Culture, int categoryId = 0)
            {
                var result = new List<CategoryChildrenDto>();

                var categoryLangs = await _context.CategoriesLangs
                                                        .Include(c => c.Category)
                                                        .Where(c => c.Category.ParentId == categoryId
                                                                                  &&
                                                                                  c.Lang.Culture == Culture
                                                                                  ).OrderByDescending(c=>c.Category.IsActive).ToListAsync();

                foreach (var item in categoryLangs)
                {
                    var categoryFile = await _context.CategoriesFiles.Where(c => c.CategoriesId == item.CategoriesId
                                                                        &&
                                                                        c.Files.FileTypes.FilesTypesGroupsId == (byte)FileTypeGroup.Picture).FirstOrDefaultAsync();
                    result.Add(new CategoryChildrenDto
                    {
                        Id = item.CategoriesId,
                        Priority = item.Category.Priority,
                        Title = item.Category.Title,
                        Name = item.Name,
                        RootName = item.Category.Title.ToParameterizingRoute(),
                        IsActive = item.Category.IsActive,
                        Image = item.Category.CategoriesFiles.Where(c=>c.Files.FileTypes.FilesTypesGroupsId == (byte)FileTypeGroup.Picture).FirstOrDefault()?.Files?.Path,
                        Children = await Recursive(Culture, item.CategoriesId),
                    });
                }

                return result;
            }
        }
    }
}
