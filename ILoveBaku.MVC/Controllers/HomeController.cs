using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>("categories/0/children/?page=0");
            var commingSoonProducts = await API.GetAsync<ApiResult<List<ProductStockDto>>>("products/soon");
            var portfolios = await API.GetAsync<ApiResult<AllNewsVM>>("news/?page=1");
            var about = $"contents/{(int)ContentCategory.About}";
            ApiResult<ContentDto> aboutResponse = await API.GetAsync<ApiResult<ContentDto>>(about);

            HomeVM vm = new HomeVM();
            if(categories!= null && commingSoonProducts != null && portfolios!= null && aboutResponse != null)
            {
                vm.Categories = categories.Succeeded ? categories.Response.Children.Where(c=>c.IsActive).Take(5).ToList() : new List<CategoryChildrenDto>();
                vm.CommingSoonProducts = commingSoonProducts.Succeeded ? commingSoonProducts.Response : new List<ProductStockDto>();
                vm.Portfolios = portfolios.Succeeded ? portfolios.Response.AllNews : new List<NewsDto>();
                vm.About = aboutResponse.Succeeded ? aboutResponse.Response : new ContentDto();
                return View(vm);
            }

            return View(vm);
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}