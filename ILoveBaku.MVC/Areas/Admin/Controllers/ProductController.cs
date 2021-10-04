using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup;
using ILoveBaku.Application.CQRS.Product.Commands.UpdateProductSpecLang;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductLangs;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecLangs;
using ILoveBaku.MVC.Areas.Admin.Logics.Category;
using ILoveBaku.MVC.Areas.Admin.Logics.Photo;
using ILoveBaku.MVC.Areas.Admin.Logics.Product;
using ILoveBaku.MVC.Areas.Admin.Models;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IPhotoService _photoService;
        private readonly ICategoryService<int?> _categoryService;
        public ProductController(IProductService productService, ICategoryService<int?> categoryService,IPhotoService photoService) { _productService = productService; _categoryService = categoryService;_photoService = photoService; }
        public async Task<IActionResult> List(int page=1)
        {
            var data = await _productService.GetProducts(page);
           
            if (data.Products != null)
                return View(data);
            else
                return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> Notification()
        {
            int? branchId = Convert.ToInt32(HttpContext.Session.GetString("branchId"));
            if (branchId == null || branchId == 0)
                return RedirectToAction("Login", "Account", new { area = "Admin" });

            var result = await _productService.GetNewProducts(branchId);
            if (result == null)
                return RedirectToAction("Error", "Home");

            return View(result);
        }
        public IActionResult Process(int? productId)
        {
            if (productId == null)
                return View();
            else
                return View(productId);
        }

        [HttpPost]
        public async Task<JsonResult> Process(ProductVm model, int? productId)
        {
            if (productId == null)
                return Json(await _productService.CreateProduct(model, ModelState));
            else
                return Json(await _productService.UpdateProduct(model, productId, ModelState));
        }

        //Ajax actions
        public async Task<JsonResult> GetCategories()
        {
            var result = await _categoryService.GetCategoryChildrenByParentId(0,0);
            if (result == null)
                return Json(new { status = 400 });

            return Json(new { status = 200, data = result });
        }

        public async Task<JsonResult> GetGroupsByCategoryId(int? categoryId)
        {
            if (categoryId == null)
                return Json(new { status = 400 });

            var result = await _productService.GetGroupsByCategoryId(categoryId);
            if (result == null)
                return Json(new { status = 400 });

            return Json(new { status = 200, data = result });
        }
        [HttpPost]
        public async Task<JsonResult> CreateGroup(ProductGroupVm model)
        {
            var result = await _productService.CreateGroup(model, ModelState);
            return Json(result);
        }

        public async Task<JsonResult> GetBarcode()
        {
            var barcode = await _productService.MakeBarcode();
            if (barcode == null)
                return Json(new { status = 400 });

            return Json(new
            {
                status = 200,
                data = barcode
            });
        }
        public async Task<object> GetProduct(int? productId)
        {
            var result = await _productService.GetProduct(productId);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> UploadPhoto(List<PhotoModel> photos, List<string> deletePhotos, int productId)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        "uploads",
                                        "productimages");

            var result = await _photoService.Process(path, photos, deletePhotos, productId, "/uploads/productimages");
            return Json(result);
        }


        public async Task<JsonResult> GetPhotos(int? productId)
        {
            var result = await _productService.GetPhotos(productId);
            return Json(result);
        }


        [HttpGet]
        public async Task<JsonResult> GetProductLangs(int? productId)
        {
            if (productId == null)
                return Json(new
                {
                    status = 400
                });

            var productLangs = await API.GetAsync<ApiResult<List<ProductLangDto>>>($"products/{productId}/langs");
            if (productLangs == null || (productLangs!=null && !productLangs.Succeeded))
                return Json(new
                {
                    status = 400
                });


            return Json(new
            {
                status = 200,
                data = productLangs.Response
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetProductSpecLangs(int? productId)
        {
            if (productId == null)
                return Json(new
                {
                    status = 400
                });

            var productSpecLangs = await API.GetAsync<ApiResult<List<ProductSpecLangsDto>>>($"products/{productId}/specifications/langs");
            if (productSpecLangs == null || (productSpecLangs != null && !productSpecLangs.Succeeded))
                return Json(new
                {
                    status = 400
                });

            return Json(new
            {
                status = 200,
                data = productSpecLangs.Response
            });
        }

        [HttpPost]
        public async Task<JsonResult> updateProductLang(int? id,ProductLangDto model)
        {
            if (id == null || (model.Name == null || model.Description == null))
                return Json(new
                {
                    status = 400
                });


            var updateProductLang = await API.PutAsync<ProductLangDto, ApiResult<int?>>($"products/langs/{id}", model);
            if (updateProductLang == null || (updateProductLang != null && !updateProductLang.Succeeded))
                return Json(new
                {
                    status = 400
                });

            return Json(new
            {
                status = 200,
                data = updateProductLang.Response
            });
        }

        public async Task<JsonResult> updateProductSpecLang(string langName, ProductSpecLangVm model)
        {
            if (langName == null || model == null)
                return Json(new
                {
                    status = 200
                });

            var updateSpecLang = await API.PutAsync<List<ProductSpecLangValueDto>, ApiResult<int?>>($"products/specifications/langs/{langName}", model.Values);
            if (updateSpecLang == null || (updateSpecLang != null && !updateSpecLang.Succeeded))
                return Json(new
                {
                    status = 400
                });

            return Json(new
            {
                status = 200,
                data = updateSpecLang.Response
            });
        }
    }

    public class ProductSpecLangVm
    {
        public List<ProductSpecLangValueDto> Values { get; set; }
    }

}