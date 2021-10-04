namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications
{
    public class SizeDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductCount { get; set; }
    }
}