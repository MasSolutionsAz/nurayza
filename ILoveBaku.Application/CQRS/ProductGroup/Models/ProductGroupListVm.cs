using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductGroup.Models
{
    public class ProductGroupListVm
    {
        public List<ProductGroupDto> Groups { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
    }
}
