using ILoveBaku.MVC.Core.Localization;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Components
{
    public class CultureViewComponent : ViewComponent
    {
        private readonly CultureService _cultureService;

        public CultureViewComponent(CultureService cultureService) => _cultureService = cultureService;

        public IViewComponentResult Invoke()
        {
            CultureVCM model = new CultureVCM()
            {
                Cultures = _cultureService.Cultures,
                CurrentCulture = _cultureService.CurrentCulture,
                //Cultures = _cultureService.SelectListOfCultures,
                //ReturnUrl = string.IsNullOrEmpty(HttpContext.Request.Path) ? "~/" : $"~{HttpContext.Request.Path.Value}"
                ReturnUrl = $"~{HttpContext.Request.Path.Value}"
            };
            return View(model);
        }
    }
}
