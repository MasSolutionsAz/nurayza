using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecification
{
    public class CategorySpecificationCommandDto : IMapFrom<CategoriesSpecifications>
    {
        [Required]
        public int CategoriesSpecificationGroupId { get; set; }
        [Required]
        public byte CategoriesSpecificationTypeId { get; set; }
        [Required]
        public string Title { get; set; }
        public int Priority { get; set; }
        [Required]
        public byte CategoriesSpecificationsStatusesId { get; set; }
    }
}
