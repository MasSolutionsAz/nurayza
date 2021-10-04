using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsTransactionsStatuses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}
