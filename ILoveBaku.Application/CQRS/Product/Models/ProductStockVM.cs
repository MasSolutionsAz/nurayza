using ILoveBaku.Application.CQRS.Carts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductStockVM
    {
        public ProductStockDetailDto Product { get; set; }

        public List<NestedCategory> NestedCategories { get; set; }
    }

    public class NestedCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
    }
}
