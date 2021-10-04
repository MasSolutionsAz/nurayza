using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Language
{
    public class LanguageService : BaseService, ILanguageService
    {
        public LanguageService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }
        public async Task<List<LanguageDto>> GetLanguages()
        {
            ApiResult<List<LanguageDto>> langs = await API.GetAsync<ApiResult<List<LanguageDto>>>("languages");
            if (langs != null && langs.Succeeded)
                return langs.Response;
            else
                return null;
        }
    }
}
