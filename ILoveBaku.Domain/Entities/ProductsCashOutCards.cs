using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutCards
    {
        public int Id { get; set; }
        public virtual ProductsCashOut ProductsCashOut { get; set; }
        public int ProductsCashOutId { get; set; }
        public virtual UsersCards UsersCards { get; set; }
        public int UsersCardsId { get; set; }
    }
}
