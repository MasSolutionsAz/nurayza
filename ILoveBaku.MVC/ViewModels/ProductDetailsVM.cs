using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications;
using System.Collections.Generic;

namespace ILoveBaku.MVC.ViewModels
{
    public class ProductDetailsVM
    {
        public int CategoryId { get; set; }
        public ProductStockVM Product { get; set; }
        public List<ProductStockDto> RelatedProducts { get; set; }
        public List<ProductReviewDto> Reviews { get; set; }
        public List<ProductSpecificationDto> ProductSpecifications { get; set; }
        public string ProductRootName { get; set; }
        public List<ColorDto> Colors { get; set; }
    }
}
