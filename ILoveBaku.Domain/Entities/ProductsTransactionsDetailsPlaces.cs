using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsTransactionsDetailsPlaces
    {
        public int Id { get; set; }
        public int ProductsStockLocationsId { get; set; }
        public int ProductsTransactionsDetailsId { get; set; }
    }
}
