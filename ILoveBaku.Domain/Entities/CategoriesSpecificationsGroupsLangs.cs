using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsGroupsLangs
    {
        public int Id { get; set; }
        public int CategoriesSpecificationsGroupsId { get; set; }
        public virtual CategoriesSpecificationsGroups CategoriesSpecificationsGroup { get; set; }
        public byte LangsId { get; set; }
        public virtual Langs Lang { get; set; }
        public string Name { get; set; }
    }
}
