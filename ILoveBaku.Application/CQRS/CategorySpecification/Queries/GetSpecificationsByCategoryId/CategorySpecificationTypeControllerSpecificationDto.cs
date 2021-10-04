using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class CategorySpecificationTypeControllerSpecificationDto
    {
        public string TableName { get; set; }
        public string Name { get; set; }
        public List<SpecificationValueDto> Values { get; set; }
    }
}
