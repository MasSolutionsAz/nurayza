using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class ProductsCashOutDetails
    {
        public int Id { get; set; }
        public virtual ProductsCashOut ProductsCashOut { get; set; }
        public int ProductsCashOutId { get; set; }
        public virtual Products Products { get; set; }
        public int ProductsId { get; set; }
        public int? ProductsTransactionsCountId { get; set; }
        public decimal Count { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal PayAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ProductsTransactionsCount ProductsTransactionsCount { get; set; }
    }
}
