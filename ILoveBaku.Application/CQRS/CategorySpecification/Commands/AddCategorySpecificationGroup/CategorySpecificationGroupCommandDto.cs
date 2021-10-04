using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationGroup
{
    public class CategorySpecificationGroupCommandDto:IMapFrom<CategoriesSpecificationsGroups>
    {
        [Required]
        public string Title { get; set; }
        public int Priority { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
}
