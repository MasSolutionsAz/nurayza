using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsTransactions
    {
        public ProductsTransactions()
        {
            Details = new HashSet<ProductsTransactionDetails>();
        }
        public int Id { get; set; }
        public byte ProductsTransactionsTypesId { get; set; }
        public int BranchesId { get; set; }
        public int SuppliersId { get; set; }
        public int ReceiptsNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalPayAmount { get; set; }
        public string Description { get; set; }
        public DateTime ReceipDate { get; set; }
        public int ProductsTransactionsStatusesId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<ProductsTransactionDetails> Details { get; set; }
    }
}
