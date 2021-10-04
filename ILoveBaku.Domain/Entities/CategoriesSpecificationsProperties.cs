using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsProperties
    {
        public int Id { get; set; }
        public int CategoriesSpecificationId { get; set; }
        public virtual CategoriesSpecifications CategoriesSpecification { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<CategoriesSpecificationsPropertiesLangs> CategoriesSpecificationsPropertiesLangs { get; set; }

        public virtual ICollection<ProductsStockSpecificationsValuesList> ProductsStockSpecificationsValuesLists { get; set; }
    }
}
