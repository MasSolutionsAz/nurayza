using ILoveBaku.Application.Common.Extension;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Extensions
{
    public static class HttpContextExtension
    {
        public static async Task<TService> GetServiceAsync<TService>(this HttpContext httpContext)
        {
            return await Task.FromResult(httpContext.RequestServices.GetService<TService>());
        }

        public static async Task<string> GetCultureFromCookie(this HttpContext httpContext)
        {
            string value = null;
            httpContext?.Request.Cookies.TryGetValue("culture", out value);
            return await Task.FromResult(value);
        }

        public static async Task<string> GetDefaultCulture(this HttpContext httpContext)
        {
            return await Task.FromResult(httpContext?.RequestServices.GetService<IConfiguration>()["Default:Culture"]);
        }

        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.TryParse(claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId) ? userId : default;
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value,new JsonSerializerSettings { 
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
