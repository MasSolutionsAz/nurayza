using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Models
{
    public class SearchCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public string Svg { get; set; }
    }
}
