using ILoveBaku.Application.CQRS.Category.Commands.AddCategory;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Category
{
    public class CategoryProcessVm
    {
        public bool IsUpdate { get; set; }
        public int? ParentId { get; set; }
        public int? CategoryId { get; set; }

        public CategoryVm CategoryVm {get;set;}
        public List<CategoryLanguageVm> CategoryLanguageVm { get; set; }
        public List<LanguageDto> Languages { get; set; }
        public List<CategorySpecificationDto> Specifications { get; set; }

        public CategoryProcessVm()
        {
            CategoryLanguageVm = new List<CategoryLanguageVm>();
            Languages = new List<LanguageDto>();
            Specifications = new List<CategorySpecificationDto>();
        }
    }
}
