using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public byte CartStatusId { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<CartDetail> CartDetails { get; set; }

        public virtual ICollection<CartOrder> CartOrders { get; set; }
    }
}
