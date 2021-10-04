using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CashDesk
    {
        public int Id { get; set; }
        public virtual BranchesFloorsRelations BranchesFloorsRelations { get; set; }
        public int BranchesFloorsRelationsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
