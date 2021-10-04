using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductStockDto
    {
        public int Id { get; set; }
        [NotMapped]
        public int ProductGroupId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string RouteName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public bool IsWishlist { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public string BranchName { get; set; }
        [NotMapped]
        public List<long> Barcodes { get; set; }
        [NotMapped]
        public decimal SaleAmount { get; set; }
        public decimal Count { get; set; }
        [NotMapped]
        public bool Status { get; set; }

        public int TotalCount { get; set; }

    }
}
