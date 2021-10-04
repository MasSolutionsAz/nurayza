using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CashDeskSeanceManualProducts
    {
        public int Id { get; set; }
        public int BranchesId { get; set; }
        public int ProductsId { get; set; }
        public int Priority { get; set; }
    }
}
