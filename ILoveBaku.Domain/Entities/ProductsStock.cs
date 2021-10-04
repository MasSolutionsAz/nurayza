using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStock
    {
        public ProductsStock()
        {
            Sales = new HashSet<ProductsStockSaleAmounts>();
            ProductsStockDiscountsDetails = new HashSet<ProductsStockDiscountsDetails>();
        }

        public int Id { get; set; }
        public virtual Branches Branches { get; set; }
        public int BranchesId { get; set; }
        public decimal Count { get; set; }
        public string Description { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal? BuyAmount { get; set; }
        public decimal? CostAmount { get; set; }
        public byte ProductStockStatusesId { get; set; }
        public int MinCount { get; set; }
        public DateTime PriorityDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public virtual ICollection<ProductsStockSaleAmounts> Sales { get; set; }
        public virtual ICollection<ProductsStockDiscountsDetails> ProductsStockDiscountsDetails { get; set; }
    }
}
