namespace ILoveBaku.Domain.Entities
{
    public class ProductsStockDiscountsDetails
    {
        public int Id { get; set; }

        public int ProductsStockDiscountsId { get; set; }

        public virtual ProductsStockDiscounts ProductsStockDiscounts { get; set; }

        public int ProductsStockId { get; set; }

        public virtual ProductsStock ProductsStock { get; set; }

        public int ProductsCount { get; set; }

        public bool IsActive { get; set; }
    }
}
