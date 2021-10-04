using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class WishLists
    {
        public int Id { get; set; }
        public Guid UsersId { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ProductsStockId { get; set; }

        public virtual ProductsStock ProductsStock { get; set; }
    }
}
