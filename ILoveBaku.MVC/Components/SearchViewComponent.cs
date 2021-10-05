using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Services;
using ILoveBaku.MVC.ViewModels;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Category.Models;

namespace ILoveBaku.MVC.Components
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ApiRequestService API;

        private readonly IConfiguration _configuration;

        public SearchViewComponent(ApiRequestService apiRequestService, IConfiguration configuration, CultureService cultureService)
        {
            API = apiRequestService;
            API.Configure(options =>
            {
                options.AddHeader("culture", cultureService.CurrentCulture);
            });
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            ApiResult<List<SearchCategoryDto>> categories = API.GetAsync<ApiResult<List<SearchCategoryDto>>>("categories/forSearch").Result;

            string URL = $"products/{_configuration["OnlineBranchId"]}/stocks?categoryId=0&productStockSaleAmountType={ProductStockSaleAmountType.Retail}&productStockStatus={ProductStockStatus.Active}&take=10&page=1";
            
            ApiResult<ProductStocksVM> products = API.GetAsync<ApiResult<ProductStocksVM>>(URL).Result;

            SearchVM model = new SearchVM()
            {
                Categories = categories?.Response ?? new List<SearchCategoryDto>(),
                Products = products?.Response?.Products ?? new List<ProductStockDto>()
            };

            return View(model);
        }
    }
}
