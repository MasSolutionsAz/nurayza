using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategorySpecificationsTypesSpecificationsRelations
    {
        public int Id { get; set; }
        public byte CategorySpecificationsTypesControllersId { get; set; }
        public int CategorySpecificationsTypesControllersSpecificationsId { get; set; }
        public virtual CategorySpecificationsTypesControllersSpecifications CategorySpecificationsTypesControllersSpecification { get; set; }
    }
}
