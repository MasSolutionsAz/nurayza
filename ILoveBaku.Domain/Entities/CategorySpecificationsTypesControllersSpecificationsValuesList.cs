using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategorySpecificationsTypesControllersSpecificationsValuesList
    {
        public int Id { get; set; }
        public int CategorySpecificationsId { get; set; }
        public int CategorySpecificationsTypesControllersSpecificationsPropertiesId { get; set; }
    }
}
