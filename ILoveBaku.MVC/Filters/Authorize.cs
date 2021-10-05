using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Filters
{
    public class Authorize : ActionFilterAttribute
    {
        private readonly string Roles;

        private readonly bool IsAjax;

        public Authorize(string roles = null, bool isAjax = false)
        {
            Roles = roles;
            IsAjax = isAjax;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if ((context.HttpContext.User.Identity.IsAuthenticated && Roles == null) || (Roles != null && context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && Roles.ToLower().Contains(c.Value.ToLower()))) && context.HttpContext.Session.GetString("branchId") != null)
                await next();
            else
            {

                if (IsAjax)
                    context.Result = new JsonResult(new
                    {
                        status = 401,
                        error = "İlk öncə sayta login olmalısız."
                    });
                else
                    context.Result = new RedirectToRouteResult(new
                    {
                        controller = "Account",
                        action = "Login",
                        area = "Admin"
                    });
            }
        }
    }
}
