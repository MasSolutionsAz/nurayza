using ILoveBaku.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductStocksVM
    {
        public ProductStocksVM() => Products = new List<ProductStockDto>();

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int ProductCount { get; set; }

        public int Page { get; set; }
        public int Total { get; set; }

        public List<ProductStockDto> Products { get; set; }
    }
}
