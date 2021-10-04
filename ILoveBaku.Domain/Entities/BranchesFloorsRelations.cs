using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class BranchesFloorsRelations
    {
        public int Id { get; set; }
        public byte BranchesFloorsId { get; set; }
        public virtual BranchesPlaces BranchesPlaces { get; set; }
        public int? BranchesPlacesId { get; set; }
    }
}
