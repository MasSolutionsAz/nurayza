using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductGroups
    {
        public ProductGroups()
        {
            Products = new List<Products>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoriesId { get; set; }

        public virtual Categories Category { get; set; }

        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedIp { get; set; }

        public virtual ICollection<Products> Products { get; private set; }
    }
}
