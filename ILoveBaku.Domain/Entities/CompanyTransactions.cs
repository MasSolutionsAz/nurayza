using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CompanyTransactions
    {
        public int Id { get; set; }
        public byte CompanyTransactionTypesId { get; set; }
        public int? ProductTransactionsId { get; set; }
        public decimal Amount { get; set; }
        public byte CompanyTransactionsStatusesId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedIp { get; set; }
    }
}
