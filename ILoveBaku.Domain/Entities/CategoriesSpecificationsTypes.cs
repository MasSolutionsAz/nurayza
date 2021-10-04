using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesSpecificationsTypes
    {
        public byte Id { get; set; }
        public byte? CategorySpecificationsTypesControllersId { get; set; }
        public virtual CategorySpecificationsTypesControllers CategoriesSpecificationsTypesController { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
    }
}
