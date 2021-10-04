using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class BranchesPlaces
    {
        public int Id { get; set; }
        public int BranchesId { get; set; }
        public string Name { get; set; }
        public bool IsSalesRows { get; set; }
        public byte Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}
