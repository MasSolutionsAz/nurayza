using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.CategorySpecification
{
    public class CategorySpecificationService : BaseService,ICategorySpecificationService
    {
        public CategorySpecificationService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }
        
        public async Task<object> GetFullDto(int id)
        {
            var data = await API.GetAsync<ApiResult<List<CategorySpecificationGroupDto>>>("specifications/" + id);
            if (data != null && data.Succeeded)
                return new { status = 200, data = data.Response };

            return   new { status = 400};
        }
    }
}
