using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.MVC.Areas.Admin.Logics.Category;
using ILoveBaku.MVC.Areas.Admin.Logics.Photo;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService<int?> _categoryService;
        public CategoryController(ICategoryService<int?> categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> List(int id = 0, int page = 1)
        {
            ViewBag.CategoryId = id;
            CategoryListVm vm = await _categoryService.CreateListVm(id, page);
            if (vm != null)
                return View(vm);
            else
                return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> Process(int? parentId, int? categoryId)
        {
            if (parentId == null && categoryId == null)
                return RedirectToAction("List", "Category");

            CategoryProcessVm vm = await _categoryService.CreateProcessVm(parentId,categoryId);
            if (vm == null)
                return RedirectToAction("List", "Category");

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Process(CategoryProcessVm model)
        {
            if (!ProcessValidation(model))
                return View(await _categoryService.CreateProcessVm(model.ParentId,model.CategoryId));

            var result = await _categoryService.Process(model);
            if (!result.Succeeded)
            {
                ModelState.FillErrors(result.Errors);
                return View(model);
            }
            else
                return RedirectToAction("Process", "Category", new { categoryId = result.Response });
        }

        [HttpPost]
        public async Task<JsonResult> Update(CategoryUpdateVm model, int? id)
        {
            if (model == null || id == null)
                return Json(new
                {
                    status = 400,
                    errorsObj = new Dictionary<string, string> { { "", "Xəta baş verdi" } }
                });

            var result = await _categoryService.Update(model,id,ModelState);
            return Json(result);

        }

        [HttpPost]
        public async Task<JsonResult> Upload(List<PhotoModel> photos, List<string> deletePhotos, int categoryId)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        "uploads",
                                        "categories");

            var result = await _categoryService.UploadPhoto(path, photos, deletePhotos, categoryId);
            return Json(result);
        }
        public bool ProcessValidation(CategoryProcessVm model)
        {
            if (model.IsUpdate)
                return ModelState.GetModelValidationState("CategoryLanguageVm") == ModelValidationState.Valid &&
                       ModelState.GetModelValidationState("CategoryVm") == ModelValidationState.Valid;
            else
                return ModelState.GetModelValidationState("CategoryVm") == ModelValidationState.Valid;
        }

        public async Task<JsonResult> GetPhotos(int? categoryId)
        {
            var result = await _categoryService.GetPhotos(categoryId);
            return Json(result);
        }
    }
}