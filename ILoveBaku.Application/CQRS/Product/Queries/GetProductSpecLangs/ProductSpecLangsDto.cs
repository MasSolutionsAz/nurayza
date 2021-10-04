using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecLangs
{
    public class ProductSpecLangsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PropertyId { get; set; }
        public int SpecificationId { get; set; }
        public int ProductId { get; set; }
        public List<ProductSpecLangValue> Values { get; set; }
    }
}