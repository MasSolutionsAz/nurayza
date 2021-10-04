using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOut
    {
        public int Id { get; set; }
        public int CashDeskSeanceId { get; set; }
        public string Description { get; set; }
        public Guid? TransactionId { get; set; }
        public int PaymentType { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ProductsCashOutStatuses ProductsCashOutStatuses { get; set; }
        public byte ProductsCashOutStatusesId { get; set; }
        public virtual ICollection<ProductsCashOutDetails> ProductsCashOutDetails { get; set; }
        public virtual ProductsCashOutCards ProductsCashOutCards { get; set; }
    }
}
