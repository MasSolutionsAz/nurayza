using ILoveBaku.Application.CQRS.Branches.Queries.GetBranches;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryLanguagesById;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.ViewModels
{
    public class HomeVM
    {
        public List<CategoryChildrenDto> Categories { get; set; }
        public List<ProductStockDto> CommingSoonProducts { get; set; }
        public List<NewsDto> Portfolios { get; set; }
        public ContentDto About { get; set; }
    }
}
