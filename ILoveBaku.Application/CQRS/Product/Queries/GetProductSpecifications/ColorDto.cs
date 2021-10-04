using System.Collections;
using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications
{
    public class ColorDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public int PropertyId { get; set; }
        public int ListValueId { get; set; }
        public string ProductName { get; set; }
        public List<SizeDto> Sizes { get; set; }
      
    }
}