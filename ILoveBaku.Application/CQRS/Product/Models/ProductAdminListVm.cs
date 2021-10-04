using ILoveBaku.Domain.Stored_Procedures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class ProductAdminListVm
    {
        public List<ProductList_sp> Products { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
    }
}
