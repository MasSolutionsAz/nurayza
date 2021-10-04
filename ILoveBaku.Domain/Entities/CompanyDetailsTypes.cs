using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CompanyDetailsTypes
    {
        public byte Id { get; set; }
        public byte? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
