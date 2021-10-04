using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class CategorySpecificationGroupDto
    {
        public List<CategorySpecificationDto> CategorySpecifications { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
