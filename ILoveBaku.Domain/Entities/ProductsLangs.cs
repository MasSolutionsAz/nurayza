using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class ProductsLangs
    {
        public int Id { get; set; }
        public virtual Products Products { get; set; }
        public int ProductsId { get; set; }
        public virtual Langs Langs { get; set; }
        public byte LangsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
