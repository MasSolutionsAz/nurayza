using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsTransactionsTypes
    {
        public byte Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}
