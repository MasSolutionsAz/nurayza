using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList
{
    public class CategoryChildrenDto
    {
        public string Name { get; set; }
        public string RootName { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public byte? Priority { get; set; }
        public string Image { get; set; }
        public List<CategoryChildrenDto> Children { get; set; }
        public bool IsActive { get; set; }
    }
}
