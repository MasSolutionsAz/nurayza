using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductSpecificationDto
    {
        public int SpecificationId { get; set; }
        public string SpecificationName { get; set; }
        public bool MultiData { get; set; }
        public string Value { get; set; }
        public List<PropertyDto> Properties { get; set; }
    }
}
