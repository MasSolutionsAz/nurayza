using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Areas.Admin.Logics.Menu;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public async Task<IActionResult> List(int parentId = 0, int page = 1)
        {
            ViewBag.MenuId = parentId;
            return View(await _menuService.GetMenuItems(parentId, page, (int)MenuType.All));
        }

        public IActionResult Process(int? parentId, int? menuId)
        {
            if (parentId == null && menuId == null)
                return RedirectToAction("List", "Menu");

            MenuProcessVm vm = new MenuProcessVm
            {
                IsUpdate = menuId != null,
                ParentId = parentId != null ? (int)parentId : 0,
                MenuId = menuId != null ? (int)menuId : 0
            };
            return View(vm);
        }

        public async Task<JsonResult> GetCategories(int? parentId)
        {
            return Json(await _menuService.GetCategories(parentId));
        }
        public async Task<JsonResult> GetMenuTypes()
        {
            return Json(await _menuService.GetMenuTypes());
        }
        public async Task<JsonResult> GetMenuLangs(int? menuId)
        {
            return Json(await _menuService.GetMenuLangs(menuId));
        }
        public async Task<JsonResult> GetMenu(int? menuId)
        {
            return Json(await _menuService.GetMenu(menuId));
        }

        [HttpPost]
        public async Task<JsonResult> AddMenu(MenuItemDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = 400, errors = ModelState.GetErrorsWithKey() });
            return Json(await _menuService.AddMenu(model));
        }
        [HttpPost]
        public async Task<JsonResult> UpdateMenu(MenuItemDto model,int? menuId)
        {
            if (!ModelState.IsValid)
                return Json(new { status = 400 });

            return Json(await _menuService.UpdateMenu(model,menuId));
        }
        [HttpPost]
        public async Task<object> UpdateMenuLang(MenuLangDto model, int? menuLangId)
        {
            if (!ModelState.IsValid)
                return Json(new { status = 400 });

            return Json(await _menuService.UpdateMenuLang(model, menuLangId));
        }

        [HttpPost]
        public async Task<JsonResult> uploadBanner(List<PhotoModel> photos, List<string> deletePhotos, int menuId)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        "uploads",
                                        "banners");

            var result = await _menuService.UploadPhoto(path, photos, deletePhotos, menuId);
            return Json(result);
        }

        public async Task<JsonResult> GetPhotos(int? menuId)
        {
            var result = await _menuService.GetPhotos(menuId);
            return Json(result);
        }
    }
}
