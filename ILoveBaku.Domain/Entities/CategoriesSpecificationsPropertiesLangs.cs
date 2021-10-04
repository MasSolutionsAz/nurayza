using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsPropertiesLangs
    {
        public int Id { get; set; }
        public int CategoriesSpecificationsPropertiesId { get; set; }
        public virtual CategoriesSpecificationsProperties CategorySpecificationProperty { get; set; }
        public virtual Langs Lang { get; set; }
        public byte LangsId { get; set; }
        public string Name { get; set; }
    }
}
