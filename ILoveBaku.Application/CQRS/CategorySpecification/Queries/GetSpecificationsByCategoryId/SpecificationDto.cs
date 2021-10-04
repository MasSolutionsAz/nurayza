using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public  class SpecificationDto
    {
        public string Name { get; set; }
        public List<PropertyDto> Properties { get; set; }
    }
}
