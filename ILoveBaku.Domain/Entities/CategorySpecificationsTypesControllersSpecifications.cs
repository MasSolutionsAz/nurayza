using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategorySpecificationsTypesControllersSpecifications
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public int Priority { get; set; }
    }
}
