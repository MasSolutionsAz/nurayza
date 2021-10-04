using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class ProductsSpecificationsValuesStringsLangs
    {
        public int Id { get; set; }
        public virtual ProductsStockSpecificationsValuesStrings ProductsStockSpecificationsValuesStrings { get; set; }
        public int ProductsStockSpecificationsValuesStringsId { get; set; }
        public virtual Langs Langs { get; set; }
        public byte LangsId { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
    }
}
