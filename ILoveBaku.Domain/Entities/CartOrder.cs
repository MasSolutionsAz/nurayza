using System;

namespace ILoveBaku.Domain.Entities
{
    public class CartOrder
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public byte CartOrderStatusId { get; set; }
    }
}
