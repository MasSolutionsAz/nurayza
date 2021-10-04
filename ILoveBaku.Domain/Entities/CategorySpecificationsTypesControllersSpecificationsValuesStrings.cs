using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategorySpecificationsTypesControllersSpecificationsValuesStrings
    {
        public int Id { get; set; }
        public int CategorySpecificationsId { get; set; }
        public int CategorySpecificationsTypesControllersSpecificationsPropertiesId { get; set; }
        public string Value { get; set; }
    }
}
