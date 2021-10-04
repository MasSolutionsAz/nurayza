using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class SpecificationFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string RouteName { get; set; }

        public List<SpecificationValueFilterDto> Values { get; set; }
    }
}
