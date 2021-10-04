using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Companies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Voen { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedIp { get; set; }
    }
}
