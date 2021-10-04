using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Transactions.Models
{
    public class ProductTransactionDetailsDto
    {
        public int ProductTransactionDetailId { get; set; }
        public int ProductTransactionId { get; set; }
        public long Barcode { get; set; }
        public string Name { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal CostAmount { get; set; }
        public decimal Count { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PayAmount { get; set; }
    }
}
