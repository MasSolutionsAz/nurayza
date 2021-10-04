using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Commands.AddCategorySpecificationRelation
{
    public class CategorySpecificationVm
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int CategorySpecificationId { get; set; }
        public bool IsActive { get; set; }
    }
}
