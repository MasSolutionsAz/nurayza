using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class BranchesSectorsShelfsRelations
    {
        public int Id { get; set; }
        public int BranchesSectorsRelationsId { get; set; }
        public string Name { get; set; }
    }
}
