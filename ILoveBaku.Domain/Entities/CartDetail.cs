using System;

namespace ILoveBaku.Domain.Entities
{
    public partial class CartDetail
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ProductId { get; set; }

        public virtual Products Product { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
