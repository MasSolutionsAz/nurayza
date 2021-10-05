using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ILoveBaku.MVC.Extensions;
using System.Linq;

namespace ILoveBaku.MVC.Components
{
    public class AddToCartViewComponent : ViewComponent
    {
        private readonly ApiRequestService API;

        public AddToCartViewComponent(ApiRequestService apiRequestService) => API = apiRequestService;

        public IViewComponentResult Invoke()
        {
            API.Configure(options =>
            {
                options.AddHeader("token", HttpContext.Request.Cookies["token"]);
            });

            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            List<CartDetailDto> cartDetails = new List<CartDetailDto>();
            if (isAuthenticated)
            {
                ApiResult<List<CartDetailDto>> response = API.GetAsync<ApiResult<List<CartDetailDto>>>($"carts").Result;
                if (!response.Succeeded)
                    return View(new List<CartDetailDto>());

                cartDetails = response.Response;
            }
            else
            {
                cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails") ?? new List<CartDetailDto>();
            }

            return View(cartDetails.OrderByDescending(c=>c.Id).ToList());
        }
    }
}
