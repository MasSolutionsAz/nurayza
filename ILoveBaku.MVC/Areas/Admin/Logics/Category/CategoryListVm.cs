using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Category
{
    public class CategoryListVm
    {
        public List<CategoryChildrenDto> Categories { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
        public int ParentId { get; set; }

    }
}
