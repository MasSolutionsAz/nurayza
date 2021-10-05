using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Menus.Models;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Components
{
    public class FooterMenuViewComponent:ViewComponent
    {
        private readonly ApiRequestService API;

        public FooterMenuViewComponent(ApiRequestService apiRequestService, CultureService cultureService)
        {
            API = apiRequestService;
            API.Configure(options =>
            {
                options.AddHeader("culture", cultureService.CurrentCulture);
            });
        }

        public IViewComponentResult Invoke()
        {
            var response = API.GetAsync<ApiResult<List<MenuItemDto>>>("menu/0/?menuTypeId=" + (byte)MenuType.Footer + "&isActive=true").Result;
            return View(response != null ? response.Response : new List<MenuItemDto>());
        }
    }
}
