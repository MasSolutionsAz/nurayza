using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStockSpecificationsValuesList
    {
        public int Id { get; set; }
        public virtual Products Products { get; set; }
        public int ProductsId { get; set; }
        public virtual CategoriesSpecificationsProperties CategoriesSpecificationsProperties { get; set; }
        public int CategoriesSpecificationsPropertiesId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
