using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsTransactionsCount
    {
        public int Id { get; set; }
        public decimal Count { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int ProductsTransactionsDetailsId { get; set; }
        public virtual ProductsTransactionDetails ProductsTransactionsDetails { get; set; }

        public virtual ICollection<ProductsCashOutDetails> ProductsCashOutDetails { get; set; }
    }
}
