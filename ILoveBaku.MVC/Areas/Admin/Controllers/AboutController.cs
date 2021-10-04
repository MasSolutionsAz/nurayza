using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Areas.Admin.Models;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class AboutController : BaseController
    {
        public async Task<IActionResult> Detail()
        {
            string URL = $"contents/{(int)ContentCategory.About}/langs";
            ApiResult<List<ContentDto>> response = await API.GetAsync<ApiResult<List<ContentDto>>>(URL);
            ApiResult<List<LanguageDto>> languagesResponse = await API.GetAsync<ApiResult<List<LanguageDto>>>("languages");
            AboutVm vm = new AboutVm
            {
                Languages = languagesResponse.Response,
                Contents = response.Response
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> Update(int? contentLangId,ContentDto model)
        {
            if (contentLangId == null || model.Content == null)
                return Json(new
                {
                    status = 400
                });

            var updateAboutResult = await API.PostAsync<ContentDto, ApiResult<int?>>($"contents/{(int)ContentCategory.About}/{contentLangId}",model);
            if(updateAboutResult==null || updateAboutResult!=null && !updateAboutResult.Succeeded)
            {
                return Json(new { status = 400 });
            }

            return Json(new
            {
                status = 200
            });
        }
    }
}
