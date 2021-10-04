namespace ILoveBaku.Application.CQRS.Carts.Models
{
    public class CartDetailDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public int ProductStockId { get; set; }
        public string RootName { get; set; }

        public int ProductId { get; set; }
        public int Count { get; set; }

    }
}
