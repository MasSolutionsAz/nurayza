using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Regions
    {
        public int Id { get; set; }
        public virtual Countries Country { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int IsActive { get; set; }
    }
}
