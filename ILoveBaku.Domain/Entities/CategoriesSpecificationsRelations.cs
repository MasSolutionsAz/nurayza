using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsRelations
    {
        public int Id { get; set; }
        public bool? IsActive { get; set; }

        public int CategoriesSpecificationId { get; set; }

        public virtual CategoriesSpecifications CategoriesSpecification { get; set; }

        public int CategoriesId { get; set; }

        public virtual Categories Category { get; set; }
    }
}
