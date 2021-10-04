using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStockLocations
    {
        public int Id { get; set; }
        public int ProductsStockId { get; set; }
        public int BranchesSectorsShelfsRelationsId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
