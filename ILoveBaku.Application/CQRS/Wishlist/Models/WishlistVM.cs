using System.Collections.Generic;
using ILoveBaku.Application.CQRS.Product.Models;

namespace ILoveBaku.Application.CQRS.Wishlist.Models
{
    public class WishlistVM
    {
        public WishlistVM() => Wishlist = new List<ProductStockDto>();

        public int WishlistCount { get; set; }

        public int Page { get; set; }

        public List<ProductStockDto> Wishlist { get; set; }
    }
}
