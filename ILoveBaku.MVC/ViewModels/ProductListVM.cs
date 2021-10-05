using ILoveBaku.Application.CQRS.Category.Models;
using ILoveBaku.Application.CQRS.Product.Models;

namespace ILoveBaku.MVC.ViewModels
{
    public class ProductListVM
    {
        public CategoryDto Category { get; set; }

        public ProductListDto ProductList { get; set; }

        public ProductFiltersDto ProductFilters { get; set; }

        public int ShownItemCount { get; set; }

        public int CurrentPage { get; set; }
    }
}
