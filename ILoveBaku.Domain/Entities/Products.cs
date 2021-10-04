using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Products
    {
        public Products()
        {
            ProductsFiles = new HashSet<ProductsFiles>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public decimal? DefaultSaleAmount { get; set; }
        public decimal? DefaultBuyAmount { get; set; }
        public decimal? DefaultCostAmount { get; set; }
        public DateTime? DefaultPublishDate { get; set; }
        public decimal? TaxPercent { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreatedIp { get; set; }
        public string Description { get; set; }
        public int ProductGroupsId { get; set; }
        public virtual ProductGroups ProductGroup { get; set; }

        public virtual ICollection<ProductsStock> ProductsStocks { get; set; }

        public virtual ICollection<ProductsFiles> ProductsFiles { get; set; }
        public virtual ICollection<ProductsLangs> ProductsLangs { get; set; }
    }
}
