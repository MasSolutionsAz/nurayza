using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CompanyDebts
    {
        public int Id { get; set; }
        public int BranchesId { get; set; }
        public int SuppliersId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
