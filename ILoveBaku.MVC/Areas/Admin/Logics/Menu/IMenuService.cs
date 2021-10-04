using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Menu
{
    public interface IMenuService
    {
        Task<List<MenuItemDto>> GetMenuItems(int parentId,int page,int menuTyepId);
        Task<object> GetCategories(int? parentId);
        Task<object> GetMenuLangs(int? menuId);
        Task<object> AddMenu(MenuItemDto model);
        Task<object> GetMenuTypes();
        Task<object> GetMenu(int? menuId);
        Task<object> UpdateMenu(MenuItemDto model,int? menuId);
        Task<object> UpdateMenuLang(MenuLangDto model,int? menuLangId);
        Task<object> UploadPhoto(string path, List<PhotoModel> upload, List<string> delete, int productId);
        Task<object> GetPhotos(int? menuId);

    }
}
