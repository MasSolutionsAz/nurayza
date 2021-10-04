using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Menus.Commands.AddBanner;
using ILoveBaku.Application.CQRS.Menus.Commands.AddMenu;
using ILoveBaku.Application.CQRS.Menus.Commands.DeleteBanner;
using ILoveBaku.Application.CQRS.Menus.Commands.UpdateMenu;
using ILoveBaku.Application.CQRS.Menus.Commands.UpdateMenuLang;
using ILoveBaku.Application.CQRS.Menus.Models;
using ILoveBaku.Application.CQRS.Menus.Queries.GetBanners;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenu;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuTypes;
using ILoveBaku.Application.CQRS.Menus.Queries.GetNavbar;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<MenuDto>>> Navbar(int menuTypeId)
        {
            return await Mediator.Send(new GetNavbarQuery() { MenuTypeId = menuTypeId });
        }
        [HttpGet("{menuId}/details")]
        public async Task<ActionResult<ApiResult<MenuItemDto>>> GetMenu(int menuId)
        {
            return await Mediator.Send(new GetMenuQuery { MenuId = menuId });
        }
        [HttpGet("{parentId}")]
        public async Task<ActionResult<ApiResult<List<MenuItemDto>>>> GetMenus(int page, int menuTypeId, bool? isActive, int parentId = 0)
        {
            return await Mediator.Send(new GetMenusQuery() { ParentId = parentId ,Page = page,MenuTypeId= menuTypeId,IsActive = isActive });
        }
        [HttpGet("types")]
        public async Task<ActionResult<ApiResult<List<MenuTypes>>>> GetMenuTypes()
        {
            return await Mediator.Send(new GetMenuTypesQuery());
        }
        [HttpGet("{menuId}/langs")]
        public async Task<ActionResult<ApiResult<List<MenuLangDto>>>> GetMenuLangs(int menuId)
        {
            return await Mediator.Send(new GetMenuLangsQuery { MenuId = menuId });
        }
        [HttpGet("{menuId}/banners")]
        public async Task<ActionResult<ApiResult<List<ProductFileDto>>>> GetBanner(int menuId)
        {
            return await Mediator.Send(new GetBannersQuery() { MenuId = menuId });
        }
        [HttpPost]
        public async Task<ActionResult<ApiResult<int?>>> AddMenu(MenuItemDto model)
        {
            return await Mediator.Send(new AddMenuCommand { Model = model });
        }
        [HttpPost("{menuId}/banners")]
        public async Task<ActionResult<ApiResult<PhotoModel>>> AddBanner(int menuId,ProductFileDto model)
        {
            return await Mediator.Send(new AddBannerCommand { MenuId = menuId, Model = model });
        }

        [HttpPut("{menuId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateMenu(MenuItemDto model,int menuId)
        {
            return await Mediator.Send(new UpdateMenuCommand { MenuId = menuId, Model = model });
        }
        [HttpPut("langs/{menuLangId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateMenuLang(MenuLangDto model,int menuLangId)
        {
            return await Mediator.Send(new UpdateMenuLangCommand { MenuLangId = menuLangId, Model = model });
        }
        [HttpDelete("{menuId}/banners")]
        public async Task<ActionResult<ApiResult<string>>> DeleteBanner(string name,int menuId)
        {
            return await Mediator.Send(new DeleteBannerCommand() { MenuId = menuId, Name = name });
        }
    }
}