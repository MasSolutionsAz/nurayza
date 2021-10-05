using ILoveBaku.Application.Common.Models;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Components
{
    public class ProductStockNotificationViewComponent:ViewComponent
    {
        private readonly ApiRequestService API;

        public ProductStockNotificationViewComponent(ApiRequestService apiRequestService)
        {
            API = apiRequestService;
        }

        public IViewComponentResult Invoke(string branchId)
        {
            if (branchId == null)
                return View(0);
            var result =  API.GetAsync<ApiResult<int?>>($"products/{branchId}/outOfStockCount").Result;
            if (result != null && result.Succeeded)
                return View(result.Response);

            return View(0);
        }
    }
}
