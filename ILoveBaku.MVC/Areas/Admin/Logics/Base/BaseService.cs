using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Base
{
    public class BaseService
    {
        protected ApiRequestService API { get ;private set ; }
        protected HttpContext  HttpContext { get; private set; }

        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
            API = HttpContext.GetServiceAsync<ApiRequestService>().Result;

            API.Configure((options) =>
            {
                options.AddHeader("culture", HttpContext.GetServiceAsync<CultureService>().Result.CurrentCulture);
                //options.AddHeader("token", HttpContext.Session.GetString("token"));
                options.AddHeader("token", HttpContext.User.FindFirstValue("Token"));
            });
        }
        
    }
}
