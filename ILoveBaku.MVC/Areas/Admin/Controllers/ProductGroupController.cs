using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Application.CQRS.ProductGroup.Models;
using ILoveBaku.MVC.Areas.Admin.Models;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class ProductGroupController : BaseController
    {

        public async Task<IActionResult> List(int page = 1)
        {
            var productGroups = await API.GetAsync<ApiResult<ProductGroupListVm>>($"groups/{page}/20");
            if (productGroups == null || !productGroups.Succeeded)
                return RedirectToAction("Error", "Home");

            return View(productGroups.Response);
        }

        [HttpGet]
        public async Task<IActionResult> Process(int? groupId)
        {
            if (groupId == null)
            {
                var categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>($"categories/0/children/?page=0");
                return View(new ProductGroupProccessVm() { Categories = categories.Response.Children });
            }
            else
            {
                var productGroup = await API.GetAsync<ApiResult<ProductGroupDto>>($"groups/{groupId}");
                var categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>($"categories/0/children/?page=0");
                if (productGroup == null || !productGroup.Succeeded)
                    return RedirectToAction("Error", "Home");

                ProductGroupProccessVm vm = new ProductGroupProccessVm
                {
                    ProuctGroup = productGroup.Response,
                    GroupId = (int)groupId,
                    Categories = categories.Response.Children
                };

                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Process(ProductGroupProccessVm model)
        {
            if (model.GroupId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>($"categories/0/children/?page=0");
                    model.Categories = categories.Response.Children;
                    return View(model);
                }

                var result = await AddGroup(model);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Məlumat əlavə edilərkən xəta baş verdi.");
                    return View(model);
                }

                return RedirectToAction("Process", "ProductGroup", new { groupId = result });
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>($"categories/0/children/?page=0");
                    model.Categories = categories.Response.Children;
                    return View(model);
                }

                var result = await EditGroup(model);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Məlumat update edilərkən xəta baş verdi.");
                    return View(model);
                }

                return RedirectToAction("Process", "ProductGroup", new { groupId = result });

            }

        }

        public async Task<int> AddGroup(ProductGroupProccessVm model)
        {
            var addGroupResult = await API.PostAsync<ProductGroupDto, ApiResult<int?>>($"groups", model.ProuctGroup);
            if (addGroupResult == null || !addGroupResult.Succeeded)
            {
                return 0;
            }

            return (int)addGroupResult.Response;
        }

        public async Task<int> EditGroup(ProductGroupProccessVm model)
        {
            model.ProuctGroup.Id = model.GroupId;
            var editGroupResult = await API.PutAsync<ProductGroupDto, ApiResult<int?>>($"groups", model.ProuctGroup);
            if (editGroupResult == null || !editGroupResult.Succeeded)
            {
                return 0;
            }

            return (int)editGroupResult.Response;
        }
    }
}
