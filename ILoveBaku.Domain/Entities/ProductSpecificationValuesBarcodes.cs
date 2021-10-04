using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductSpecificationValuesBarcodes
    {
        public int Id { get; set; }
        public int ProductsId { get; set; }
        public int CategoriesSpecificationsPropertiesId { get; set; }
        public long Value { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsManual { get; set; }
    }
}
