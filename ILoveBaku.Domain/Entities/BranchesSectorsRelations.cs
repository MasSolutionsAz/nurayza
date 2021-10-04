using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class BranchesSectorsRelations
    {
        public int Id { get; set; }
        public int BranchesFloorsRelationsId { get; set; }
        public int BranchesSectorsId { get; set; }
    }
}
