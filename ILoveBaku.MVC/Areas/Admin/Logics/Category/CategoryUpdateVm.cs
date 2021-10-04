using ILoveBaku.Application.CQRS.Category.Commands.AddCategory;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Category
{
    public class CategoryUpdateVm
    {
        public CategoryVm CategoryVm { get; set; }
        public List<CategoryLanguageVm> CategoryLanguageVm { get; set; }
        public CategorySpecificationDto Specification { get; set; }
    }
}
