using ILoveBaku.Domain.Stored_Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Models
{
    public class ProductListVm
    {
        public List<ProductList_sp> Products { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
        public int ParentId { get; set; }
    }
}
