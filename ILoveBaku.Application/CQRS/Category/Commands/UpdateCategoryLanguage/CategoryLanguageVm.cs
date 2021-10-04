using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage
{
    public class CategoryLanguageVm:IMapFrom<CategoriesLangs>
    {
        public int Id { get; set; }
        [Required]
        public int CategoriesId { get; set; }
        [Required(ErrorMessage = "Ad boş qala bilməz")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public byte LangsId { get; set; }
    }
}
