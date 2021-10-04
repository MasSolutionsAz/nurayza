using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductReviewDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
        public string ProductName { get; set; }
    }
}
