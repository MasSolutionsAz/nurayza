using ILoveBaku.Application.CQRS.Branches.Queries.GetSuppliers;
using ILoveBaku.Application.CQRS.Transactions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Models
{
    public class ProductTransationVm
    {
        public List<SupplierDto> Suppliers { get; set; }
        public ProductTransactionDto Transaction { get; set; }
        public List<ProductTransactionDetailsDto> TransactionDetails { get; set; }


        public ProductTransationVm()
        {
            Suppliers = new List<SupplierDto>();
            Transaction = new ProductTransactionDto();
            TransactionDetails = new List<ProductTransactionDetailsDto>();
        }
    }
}
