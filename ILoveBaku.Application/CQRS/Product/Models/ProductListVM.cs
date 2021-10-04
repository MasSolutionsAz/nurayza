using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductListDto
    {
        public ProductListDto() => Products = new List<ProductStockDto>();

        public int ProductCount { get; set; }

        public int Page { get; set; }

        public List<ProductStockDto> Products { get; set; }
    }
}
