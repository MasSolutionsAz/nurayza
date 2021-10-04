using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsTransactionDetails
    {
        public int Id { get; set; }
        public virtual Products Products { get; set; }
        public int ProductsId { get; set; }
        public decimal Count { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal CostAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PayAmount { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ProductsTransactionsId { get; set; }
        public virtual ProductsTransactions ProductsTransactions { get; set; }
    }
}
