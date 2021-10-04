using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class UserController : BaseController
    {
        public async Task<IActionResult> List(int page=1)
        {
            var userList = await API.GetAsync<ApiResult<UserListVm>>($"users/{page}/20");
            if (userList == null || !userList.Succeeded)
                return RedirectToAction("Error", "Home");

            return View(userList.Response);
        }

        public async Task<IActionResult> Detail(Guid userId)
        {
            var getUserResult = await API.GetAsync<ApiResult<UserDto>>($"users/{userId}");
            if (getUserResult == null || !getUserResult.Succeeded)
                return RedirectToAction("Error", "Home");
            
            return View(getUserResult.Response);
        }


    }
}
