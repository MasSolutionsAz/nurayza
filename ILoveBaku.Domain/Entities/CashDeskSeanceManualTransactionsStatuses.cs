using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CashDeskSeanceManualTransactionsStatuses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
