using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.ViewModels;
using System;
using System.Linq;
using System.Text;
using System.Globalization;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.Application.CQRS.Category.Models;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications;

namespace ILoveBaku.MVC.Controllers
{
    public class ProductController : BaseController
    {
        [HttpGet("products")]
        [HttpGet("{culture}/products")]
        [HttpGet("products/{categoryName}/{filters?}")]
        [HttpGet("{culture}/products/{categoryName}/{filters?}")]
        public async Task<IActionResult> List(string categoryName, string filters)
        {
            categoryName = categoryName?.ToLower(new CultureInfo("en-US"));

            ApiResult<CategoryDto> categoryResponse = await API.GetAsync<ApiResult<CategoryDto>>($"categories/{categoryName ?? "0"}");

            if (categoryResponse.IsNull() || !categoryResponse.Succeeded)
                return Redirect($"{Request.Host.Value}/products");

            string categoryRouteName = categoryResponse.Response?.RouteName;

            //if (!categoryName.IsNull() && categoryRouteName != categoryName)
            //    Response.Redirect($"{categoryRouteName}");

            int take = 20;

            string URL = GenerateSearchFilterUrl(categoryName, filters);

            ApiResult<ProductListDto> productResponse = await API.GetAsync<ApiResult<ProductListDto>>(URL);

            if (productResponse.IsNull() || !productResponse.Succeeded || productResponse.Response.Page != 1)
                return Redirect($"{Request.Host.Value}/products");

            ApiResult<ProductFiltersDto> filtersResponse = await API.GetAsync<ApiResult<ProductFiltersDto>>($"products/filters?categoryId={categoryResponse.Response?.Id ?? 0}");

            ProductListVM model = new ProductListVM()
            {
                Category = categoryResponse.Response,
                ProductList = productResponse.Response,
                ProductFilters = filtersResponse.Response,
                ShownItemCount = take,
                CurrentPage = productResponse.Response.Page
            };

            return View(model);
        }

        [HttpGet("product/{productName}")]
        [HttpGet("{culture}/product/{productName}")]
        public async Task<IActionResult> Details(string productName)
        {
            ApiResult<ProductStockVM> response = await API.GetAsync<ApiResult<ProductStockVM>>($"products/{Configuration["OnlineBranchId"]}/stocks/0?productName={productName}&productStockStatus={ProductStockStatus.Active}");

            if (response.IsNull() || !response.Succeeded) return NotFound(response.ErrorDetail.ErrorMessage);

            int categoryId = response.Response.Product.CategoryId;

            string URL = $"products/{Configuration["OnlineBranchId"]}/stocks?categoryId={categoryId}&productStockSaleAmountType={ProductStockSaleAmountType.Retail}&productStockStatus={ProductStockStatus.Active}&take=8&page=1";

            ApiResult<ProductStocksVM> relatedProducts = await API.GetAsync<ApiResult<ProductStocksVM>>(URL);
            ApiResult<List<ProductReviewDto>> reviewsResult = await API.GetAsync<ApiResult<List<ProductReviewDto>>>($"products/{Configuration["OnlineBranchId"]}/stocks/{productName}/reviews");
            ApiResult<ProductSpecificationModel> specificationsResponse = await API.GetAsync<ApiResult<ProductSpecificationModel>>($"products/{response.Response.Product.ProductId}/specifications");
            ProductDetailsVM model = new ProductDetailsVM()
            {
                Colors = (specificationsResponse!=null&& specificationsResponse.Succeeded)?specificationsResponse.Response.Colors:new List<ColorDto>(),
                ProductSpecifications = (specificationsResponse!=null&& specificationsResponse.Succeeded)?specificationsResponse.Response.Specifications:new List<ProductSpecificationDto>(),
                ProductRootName = productName,
                Reviews = (reviewsResult != null && reviewsResult.Succeeded) ? reviewsResult.Response : new List<ProductReviewDto>(),
                CategoryId = categoryId,
                Product = response.Response,
                RelatedProducts = (relatedProducts?.Succeeded ?? false) ? relatedProducts?.Response?.Products : new List<ProductStockDto>()
            };

            return View(model);
        }

        [HttpGet("products/search")]
        public async Task<JsonResult> Search(string key, string[] categories)
        {
            string cats = string.Join(',', categories);
            string URL = $"products/search?key={key}&categories={cats}";

            ApiResult<List<ProductStockDto>> products = await API.GetAsync<ApiResult<List<ProductStockDto>>>(URL);

            return Json(new
            {
                status = 200,
                products = products.Response
            });
        }

        [HttpGet("products/searchFilter")]
        [HttpGet("products/searchFilter/{categoryName}/{filters?}")]
        public async Task<ActionResult> SearchFilter(string categoryName, string filters)
        {
            string URL = GenerateSearchFilterUrl(categoryName, filters);

            ApiResult<ProductListDto> response = await API.GetAsync<ApiResult<ProductListDto>>(URL);
            int total = response.Response.ProductCount;

            return Json(new
            {
                data = this.RenderViewAsync("_ProductStockListPartial", response?.Response?.Products ?? new List<ProductStockDto>(),true),
                total = total
            });
        }

        [HttpGet("productss/searchFilter")]
        [HttpGet("productss/searchFilter/{categoryName}/{filters?}")]
        public ActionResult Test(string categoryName, string filters)
        {
            return Content(GenerateSearchFilterUrl(categoryName, filters));
        }

        private string GenerateSearchFilterUrl(string categoryName, string filters)
        {
            StringBuilder URL = new StringBuilder("products/searchFilter");

            if (!categoryName.IsNull())
                URL.Append($"/{categoryName}");

            URL.Append($"{(filters?.Length > 0 ? "?" : string.Empty)}{filters?.Replace('|', '&')}");

            return URL.ToString();
        }

        private string GenerateSearchFilterUrl(string categoryName, Dictionary<string, string> filters)
        {
            StringBuilder result = new StringBuilder("products/searchFilter");

            if (!categoryName.IsNull())
                result.Append($"/{categoryName}");

            result.Append($"{(filters?.Count > 0 ? "?" : string.Empty)}");

            foreach (var filter in filters)
            {
                result.Append($"{filter.Key}={filter.Value}&");
            }

            result.Length--;

            return result.ToString();
        }


        public async Task<JsonResult> AddReview(string productName, string text)
        {
            if (productName == null || text == null)
                return Json(new { status = 400 });
            ProductReviewDto productReviewDto = new ProductReviewDto
            {
                Text = text,
                ProductName = productName
            };

            var addReviewResult = await API.PostAsync<ProductReviewDto, ApiResult<ProductReviewDto>>($"products/{Configuration["OnlineBranchId"]}/stocks/{productName}/reviews", productReviewDto);
            if (addReviewResult != null && addReviewResult.Succeeded)
                return Json(new
                {
                    status = 200,
                    data = addReviewResult.Response
                });


            return Json(new { status = 400 });
        }


        //[HttpGet]
        //public async Task<JsonResult> GetPaginatedList(int page=1)
        //{

        //}
    }
}