using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsLangs
    {
        public int Id { get; set; }
        public int CategoriesSpecificationsId { get; set; }
        public virtual CategoriesSpecifications CategorySpecification { get; set; }
        public virtual Langs Lang { get; set; }
        public byte LangsId { get; set; }
        public string Name { get; set; }
    }
}
