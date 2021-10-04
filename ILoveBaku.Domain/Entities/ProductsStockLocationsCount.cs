using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStockLocationsCount
    {
        public int Id { get; set; }
        public int ProductSstockLocationsId { get; set; }
        public int Count { get; set; }
    }
}
