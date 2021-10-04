using ILoveBaku.Application.CQRS.Product.Models;
using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications
{
    public class ProductSpecificationModel
    {
        public List<ProductSpecificationDto> Specifications { get; set; }
        public List<ColorDto> Colors { get; set; }
    }
}