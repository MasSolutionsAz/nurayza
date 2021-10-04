using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Transactions.Models
{
    public class ProductTransactionCreateDto
    {
        [Required(ErrorMessage ="Firma qeyd edilməyib.")]
        public int SupplierId { get; set; }
        [Required(ErrorMessage = "Satış rəfi qeyd edilməyib.")]
        public int BranchPlaceId { get; set; }
        [Required(ErrorMessage = "Sənəd nömrəsi qeyd edilməyib.")]
        public string ReceiptNumber { get; set; }
        [Required(ErrorMessage = "Sənəd tarixi qeyd edilməyib.")]
        public string ReceiptDate { get; set; }
        public string Description { get; set; }

        public byte ProductTransactionType { get; set; }
        public byte ProductTransactionStatus { get; set; }
    }
}
