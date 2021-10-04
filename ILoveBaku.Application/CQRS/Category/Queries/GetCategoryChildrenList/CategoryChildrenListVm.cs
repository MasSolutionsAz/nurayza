using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList
{
    public class CategoryChildrenListVm
    {
        public List<CategoryChildrenDto> Children { get; set; }
        public int Total { get; set; }
    }
}