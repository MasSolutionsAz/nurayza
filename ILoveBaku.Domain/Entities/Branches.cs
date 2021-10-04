using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Branches
    {
        public int Id { get; set; }
        public virtual CompanyDetails CompanyDetails { get; set; }
        public int CompanyDetailsId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedIp { get; set; }
    }
}
