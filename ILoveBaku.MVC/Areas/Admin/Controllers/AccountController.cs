using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ILoveBaku.MVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        private readonly IIdentityService _identityService;
        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            ApiResult<UserResponse> result = await API.PostAsync<LoginVM, ApiResult<UserResponse>>("account/login", dto);

            if (result != null)
            {
                if (result.Succeeded)
                {
                    await _identityService.SignInAsync(result.Response);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.FillErrors(result.ErrorList);
            }

            return View(dto);
        }

        public async Task<IActionResult> Logout()
        {
            string token = HttpContext.Request.Cookies["token"];

            ApiResult<string> result = await API.PutAsync<string, ApiResult<string>>("account/logout", token);
            if (result != null)
            {
                await _identityService.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
            else
                return RedirectToAction("Index", "Home");
        }


        
    }
}