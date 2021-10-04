using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsGroups
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}
