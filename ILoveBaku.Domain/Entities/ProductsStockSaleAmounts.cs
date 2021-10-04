using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStockSaleAmounts
    {
        public int Id { get; set; }
        public byte ProductStockSaleAmountsTypesId { get; set; }
        public decimal Amount { get; set; }

        public int ProductsStockId { get; set; }

        public virtual ProductsStock ProductsStock { get; set; }
    }
}
