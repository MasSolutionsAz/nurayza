using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Models
{
    public class ProductGroupProccessVm
    {
        public ProductGroupDto ProuctGroup { get; set; }
        public int GroupId { get; set; }
        public List<CategoryChildrenDto> Categories { get; set; }

        public ProductGroupProccessVm()
        {
            Categories = new List<CategoryChildrenDto>();
        }
    }
}
