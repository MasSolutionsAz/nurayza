using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsStockSpecificationValuesInt
    {
        public int ProductsId { get; set; }
        public int CategoriesSpecificationsPropertiesId { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
