using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications
{
    public class CategorySpecificationDto
    {
        public int CategorySpecificationId { get; set; }
        public int? CategorySpecificationRelationId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }

    }
}
