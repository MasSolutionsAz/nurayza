namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class SearchedProductStock
    {
        public string Name { get; set; }
        public string RouteName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string Image { get; set; }
    }
}
