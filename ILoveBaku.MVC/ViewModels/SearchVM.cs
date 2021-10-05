using ILoveBaku.Application.CQRS.Category.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.ViewModels
{
    public class SearchVM
    {
        public List<SearchCategoryDto> Categories { get; set; }

        public List<ProductStockDto> Products { get; set; }
    }
}
