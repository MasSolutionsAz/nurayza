using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Transactions.Models
{
    public class ProductTransactionFilter
    {
        public byte ProductTransactionType { get; set; }
        public byte? ProductTransactionStatus { get; set; }
    }
}
