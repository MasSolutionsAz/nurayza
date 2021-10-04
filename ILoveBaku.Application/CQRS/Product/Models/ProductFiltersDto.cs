using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductFiltersDto
    {
        public List<CategoryFilterDto> CategoryFilters { get; set; }

        public List<SpecificationFilterDto> SpecificationFilters { get; set; }

        public PriceFilterDto PriceFilter { get; set; }
    }
}
