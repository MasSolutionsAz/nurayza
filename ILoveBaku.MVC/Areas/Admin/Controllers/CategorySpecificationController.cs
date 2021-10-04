using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.MVC.Areas.Admin.Logics.CategorySpecification;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class CategorySpecificationController : BaseController
    {
        private readonly ICategorySpecificationService _categorySpecificationService;
        public CategorySpecificationController(ICategorySpecificationService categorySpecificationService) => _categorySpecificationService = categorySpecificationService;


        public async Task<JsonResult> Get(int id)
        {
            var result = await _categorySpecificationService.GetFullDto(id);
            return Json(result);
        }

    }
}