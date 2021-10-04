using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStockDiscounts
    {
        public int Id { get; set; }
        public int ProductsStockDiscountsTypesId { get; set; }
        public decimal DiscountValue { get; set; }
        public string Description { get; set; }
        public decimal? MinimumOrder { get; set; }
        public DateTime ExpireDate { get; set; }
        public byte ProductsStockDiscountsStatusesId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
