using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutStatuses
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
