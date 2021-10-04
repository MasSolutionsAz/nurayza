namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductLangs
{
    public class ProductLangDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public int Id { get; set; }
        public string LangName { get; set; }
    }
}