using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecifications
    {
        public int Id { get; set; }
        public int CategoriesSpecificationGroupId { get; set; }
        public byte CategoriesSpecificationsTypeId { get; set; }
        public virtual CategoriesSpecificationsTypes CategoriesSpecificationsType { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public byte CategoriesSpecificationsStatusesId { get; set; }

        public virtual ICollection<CategoriesSpecificationsLangs> CategoriesSpecificationsLangs { get; set; }

        public virtual ICollection<CategoriesSpecificationsRelations> CategoriesSpecificationsRelations { get; set; }

        public virtual ICollection<CategoriesSpecificationsProperties> CategoriesSpecificationsProperties { get; set; }
    }
}
