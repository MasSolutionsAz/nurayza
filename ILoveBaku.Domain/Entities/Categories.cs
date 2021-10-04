using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Categories
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public byte? Priority { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }

        public virtual ICollection<CategoriesLangs> CategoriesLangs { get; private set; }

        public virtual ICollection<CategoriesFiles> CategoriesFiles { get; private set; }

        public virtual ICollection<CategoriesSpecificationsRelations> CategoriesSpecificationsRelations { get; private set; }
    }
}
