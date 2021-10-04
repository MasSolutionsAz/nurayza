using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Stored_Procedures
{
    public class ProductList_sp
    {
        public string ProductName { get; set; }
        public DateTime ProductCreatedDate { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public DateTime ProductGroupCreatedDate { get; set; }
        public string ProductPhoto { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
