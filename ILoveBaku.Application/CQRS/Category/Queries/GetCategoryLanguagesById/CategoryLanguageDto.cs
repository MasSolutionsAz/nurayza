using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryLanguagesById
{
    public class CategoryLanguageDto:IMapFrom<CategoriesLangs>
    {
        [Required]
        public int CategoriesId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public byte LangsId { get; set; }
    }
}
