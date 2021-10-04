using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Menus.Models;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Menu
{
    public class MenuService : BaseService, IMenuService
    {
        public MenuService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        public async Task<object> AddMenu(MenuItemDto model)
        {
            var data = await API.PostAsync<MenuItemDto, ApiResult<int>>("menu", model);
            if (data == null)
                return new { status = 400 };

            if (data != null && !data.Succeeded)
                return new { status = 400 };

            return new { status = 200, data = data.Response };
        }

        public async Task<object> GetMenuTypes()
        {
            var data = await API.GetAsync<ApiResult<List<MenuTypes>>>("menu/types");

            if (data == null)
                return new { status = 400 };

            if (data != null && !data.Succeeded)
                return new { status = 400 };

            return new { status = 200, data = data.Response };
        }
        public async Task<object> GetCategories(int? parentId)
        {
            if (parentId == null)
                return new { status = 400 };

            ApiResult<CategoryChildrenListVm> categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>($"categories/{parentId}/children/?page=0");
            if (categories == null)
                return new { status = 400 };

            if (categories != null && !categories.Succeeded)
                return new { status = 400 };

            return new { status = 200, data = categories.Response };

        }

        public async Task<List<MenuItemDto>> GetMenuItems(int parentId, int page, int menuTypeId)
        {
            var data = await API.GetAsync<ApiResult<List<MenuItemDto>>>($"menu/{parentId}/?menuTypeId=" + menuTypeId + "&page=" + page);
            if (data != null && data.Succeeded)
                return data.Response;
            else
                return new List<MenuItemDto>();
        }

        public async Task<object> GetMenuLangs(int? menuId)
        {
            if (menuId == null)
                return new { status = 400 };

            var data = await API.GetAsync<ApiResult<List<MenuLangDto>>>($"menu/{menuId}/langs");
            if (data == null)
                return new { status = 400 };

            if (data != null && !data.Succeeded)
                return new { status = 400 };


            return new { status = 200, data = data.Response };
        }

        public async Task<object> GetMenu(int? menuId)
        {
            if (menuId == null)
                return new { status = 400, errors = new Dictionary<string, string>() { { "", "Məlumat tapılmadı." } } };

            var data = await API.GetAsync<ApiResult<MenuItemDto>>($"menu/{menuId}/details");
            if (data == null)
                return new { status = 400, errors = new Dictionary<string, string>() { { "", "Xəta baş verdi." } } };

            if (data != null && !data.Succeeded)
                return new { status = 400, errors = data.ErrorList };

            return new { status = 200, data = data.Response };
        }

       
        public async Task<object> UpdateMenu(MenuItemDto model, int? menuId)
        {
            var data = await API.PutAsync<MenuItemDto, ApiResult<int?>>($"menu/{menuId}", model);
            if (data == null)
                return new { status = 400 };

            if (data != null && !data.Succeeded)
                return new { status = 400 };

            return new { status = 200, data = data.Response };
        }
        
         public async Task<object> UpdateMenuLang(MenuLangDto model, int? menuLangId)
        {
            var data = await API.PutAsync<MenuLangDto, ApiResult<int?>>($"menu/langs/{menuLangId}", model);
            if (data == null)
                return new { status = 400 };

            if (data != null && !data.Succeeded)
                return new { status = 400 };

            return new { status = 200, data = data.Response };
        }

        public async Task<object> UploadPhoto(string path, List<PhotoModel> upload, List<string> delete, int menuId)
        {
            var response = new List<PhotoModel>();

            foreach (var file in upload)
            {
                ProductFileDto fileDto = new ProductFileDto
                {
                    ContentType = file.File.ContentType,
                    IsMain = file.IsMain,
                    Length = file.File.Length,
                    Name = file.File.FileName,
                    Path = "/uploads/banners"
                };

                var data = await API.PostAsync<ProductFileDto, ApiResult<PhotoModel>>($"menu/{menuId}/banners", fileDto);
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data != null && data.Succeeded)
                {
                    string photoPath = path + "/" + data.Response.Name;
                    using (FileStream stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await file.File.CopyToAsync(stream);
                    }
                    response.Add(new PhotoModel { IsMain = data.Response.IsMain, Name = data.Response.Name });

                }
            }

            foreach (var file in delete)
            {
                var data = await API.DeleteAsync<ApiResult<string>>($"menu/{menuId}/banners/?name={file}");
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data.Succeeded)
                {
                    string photoPath = path + "/" + data.Response;
                    if (System.IO.File.Exists(photoPath))
                    {
                        File.Delete(photoPath);
                    }
                }
            }
            return new
            {
                status = 200,
                data = response
            };
        }

        public async Task<object> GetPhotos(int? menuId)
        {
            if (menuId == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Xəta baş verdi." } } };

            var data = await API.GetAsync<ApiResult<List<ProductFileDto>>>($"menu/{menuId}/banners");
            if (data != null && data.Succeeded)
                return new { status = 200, data = data.Response };
            return new { status = 400 };
        }
    }
}
