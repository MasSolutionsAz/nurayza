using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutPayments
    {
        public int Id { get; set; }
        public int ProductsCashOutPaymentsTypesId { get; set; }
        public int ProductsCashOutId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
