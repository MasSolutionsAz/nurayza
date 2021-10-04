using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ILoveBaku.Application.Common.Extension;
using System.Threading.Tasks;
using ILoveBaku.Application.CQRS.Base;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryParentsList
{
    public class GetCategoryParentsListQuery : BaseRequest<ApiResult<List<object>>>
    {
        public int CategoryId { get; set; }

        public class GetCategoryParentsListQueryHandler : IRequestHandler<GetCategoryParentsListQuery, ApiResult<List<object>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategoryParentsListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<object>>> Handle(GetCategoryParentsListQuery request, CancellationToken cancellationToken)
            {
                //heleki dil el ile verilir
                List<object> result = await Recursive(request.CategoryId, request.Culture);
                return ApiResult<List<object>>.CreateResponse(result);
            }
            public async Task<List<object>> Recursive(int categoryId, string culture)
            {
                //Valideyni saxlamaq ucun massiv
                var result = new List<object>();
                //get secilmis kateqoriyani tap
                var categoryLang = await _context.CategoriesLangs
                                                        .Include(c => c.Category)
                                                        .FirstOrDefaultAsync(c => c.CategoriesId == categoryId
                                                                                  &&
                                                                                  c.Lang.Culture == culture
                                                                                  );

                if (categoryLang != null)
                {
                    //eger en boyuk valideyn deyilse
                    if (categoryLang.Category.ParentId != 0)
                    {
                        //onu elave ele
                        result.Add(new
                        {
                            id = categoryLang.Category.ParentId,
                            name = categoryLang.Name
                        });
                        //ve bir yuxari qalx
                        result.AddRange(await Recursive((int)categoryLang.Category.ParentId, culture));
                    }
                }

                return result;
            }
        }
    }
}
