using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutBonusInfo
    {
        public int Id { get; set; }
        public int ProductsCashOutCardsId { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal BonusCount { get; set; }
    }
}
