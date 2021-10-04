using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class CategorySpecificationTypeControllerDto
    {
        public string Name { get; set; }
        public List<CategorySpecificationTypeControllerSpecificationDto> Specifications { get; set; }
    }
}
