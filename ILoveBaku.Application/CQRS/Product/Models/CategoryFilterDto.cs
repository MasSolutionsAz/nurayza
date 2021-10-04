using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Models
{
    public class CategoryFilterDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RouteName { get; set; }

        public bool IsSelected { get; set; }
    }
}
