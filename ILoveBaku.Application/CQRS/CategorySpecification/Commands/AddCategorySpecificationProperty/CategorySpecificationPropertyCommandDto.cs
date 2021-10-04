using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationProperty
{
    public class CategorySpecificationPropertyCommandDto:IMapFrom<CategoriesSpecificationsProperties>
    {
        [Required]
        public int? ParentId { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
