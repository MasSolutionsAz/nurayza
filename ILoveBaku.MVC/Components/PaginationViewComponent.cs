using System;
using Microsoft.AspNetCore.Mvc;
using ILoveBaku.MVC.Core.Pagination;

namespace ILoveBaku.MVC.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationType type, int totalItemCount, int shownItemCount = 10, int currentPage = 1, int categoryId = 0)
        {
            ViewBag.CategoryId = categoryId;

            switch (type)
            {
                case PaginationType.Classic:
                    return View(Pagination.Classic(totalItemCount, shownItemCount, currentPage));
                case PaginationType.Symmetric:
                    return View(Pagination.Symmetric(totalItemCount, shownItemCount, currentPage));
            }

            throw new InvalidCastException("Yalniz Pagination tipli dəyər ötürməlisiz.");
        }
    }
}
