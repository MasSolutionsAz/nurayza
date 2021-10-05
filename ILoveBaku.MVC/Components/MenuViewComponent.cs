using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Menus.Models;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Core.Localization;
using ILoveBaku.MVC.Services;
using ILoveBaku.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApiRequestService API;

        public MenuViewComponent(ApiRequestService apiRequestService, CultureService cultureService)
        {
            API = apiRequestService;
            API.Configure(options =>
            {
                options.AddHeader("culture", cultureService.CurrentCulture);
            });
        }
        public IViewComponentResult Invoke()
        {
            var response = API.GetAsync<ApiResult<List<MenuItemDto>>>("menu/0/?menuTypeId=" + (byte)MenuType.Header+"&isActive=true").Result;
            return View(response!=null?response.Response:new List<MenuItemDto>());
        }
    }
}
