using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class MenuCategoriesItems
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public int CategoriesId { get; set; }
        public virtual Categories Category { get; set; }
        public int? CategoriesParentId { get; set; }
    }
}
