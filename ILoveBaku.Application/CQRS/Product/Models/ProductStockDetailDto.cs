using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductStockDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RouteName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int StockCount { get; set; }
        public bool IsWishlist { get; set; }
        public List<string> Images { get; set; }
        public List<long> Barcodes { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? CostAmount { get; set; }
        public decimal? BuyAmount { get; set; }
        public decimal Tax { get; set; }
        public byte ProductStockStatusId { get; set; }
        public int ProductId { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
    }
}
