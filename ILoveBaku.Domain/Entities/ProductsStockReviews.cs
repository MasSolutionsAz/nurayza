using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class ProductsStockReviews
    {
        public int Id { get; set; }
        public virtual ProductsStock ProductsStock { get; set; }
        public int ProductsStockId { get; set; }
        public virtual Users Users { get; set; }
        public Guid UsersId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProductsStockReviewStatusesId { get; set; }
    }
}
