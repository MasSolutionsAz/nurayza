using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutPaymentsCards
    {
        public int Id { get; set; }
        public int ProductsCashOutPaymentsId { get; set; }
        public int UsersCardsId { get; set; }
    }
}
