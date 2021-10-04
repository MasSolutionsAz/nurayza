using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecification
{
    public class GetCategorySpecificationDto
    {
        public int? CategoriesSpecificationGroupId { get; set; }
        public byte? CategoriesSpecificationTypeId { get; set; }
        public byte? CategoriesSpecificationsStatusesId { get; set; }
    }
}
