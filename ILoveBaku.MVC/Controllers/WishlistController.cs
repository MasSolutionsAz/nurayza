using System.Threading.Tasks;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Wishlist.Models;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Controllers
{
    public class WishlistController : BaseController
    {
        [Authorize]
        [HttpGet("wishlist/list/{page:int?}")]
        public async Task<IActionResult> List(int page = 1)
        {
            int take = 16;

            ApiResult<WishlistVM> wishlist = await API.GetAsync<ApiResult<WishlistVM>>($"wishlist?take={take}&page={page}");

            if (wishlist.IsNull() || !wishlist.Succeeded) wishlist.Response = new WishlistVM();

            ViewBag.ShownItemCount = take;

            ViewBag.CurrentPage = wishlist?.Response?.Page;

            return View(wishlist.Response);
        } 

        [HttpPost]
        [Authorize(isAjax: true)]
        public async Task<JsonResult> Process(int productStockId)
        {
            ApiResult<bool?> process = await API.PostAsync<ApiResult<bool?>>($"wishlist/{productStockId}");

            if (process.IsNull() || !process.Succeeded)
                return Json(new
                {
                    status = 400,
                    error = "Xəta baş verdi"
                });

            if (process.Response.IsNull())
                return Json(new
                {
                    status = 404,
                    error = "Product not found"
                });

            return Json(new
            {
                status = 200,
                process = process.Response
            });
        }
    }
}