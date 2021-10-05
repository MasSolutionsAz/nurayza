using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext _httpContext;

        public IdentityService(IHttpContextAccessor httpContextAccessors)
        {
            _httpContext = httpContextAccessors.HttpContext;
        }

        public async Task SignInAsync(UserResponse user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role,user.Roles==null?" ":user.Roles),
                new Claim("token",user.Token),
            };

            _httpContext.Session.SetString("token", user.Token);
            _httpContext.Session.SetString("branchId", user.BranchId.ToString());
            _httpContext.Response.Cookies.Append("token", user.Token);
            _httpContext.Response.Cookies.Append("branchId", user.BranchId.ToString());

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        public async Task SignOutAsync()
        {
            _httpContext.Response.Cookies.Delete("token");
            _httpContext.Response.Cookies.Delete("branchId");
            await _httpContext.SignOutAsync();
            _httpContext.Session.Clear();
        }
    }
}
