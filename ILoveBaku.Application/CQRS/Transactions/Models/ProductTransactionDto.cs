using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Transactions.Models
{
    public class ProductTransactionDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal TotalBuyAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal PayAmount { get; set; }
        public string Note { get; set; }
        public string ReceiptDate { get; set; }
        public byte TransactionStatus { get; set; }


        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
