using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Core.Pagination
{
    public abstract class Pagination
    {
        public static ClassicPagination Classic(int totalItemCount, int shownItemCount = 10, int currentPage = 1)
        {
            return new ClassicPagination(totalItemCount, shownItemCount, currentPage);
        }

        public static SymmetricPagination Symmetric(int totalItemCount, int shownItemCount = 10,
                                                    int currentPage = 1, int shownPageCount = 5)
        {
            return new SymmetricPagination(totalItemCount, shownItemCount, currentPage, shownPageCount);
        }
    }

    public class ClassicPagination : Pagination
    {
        public ClassicPagination(int totalItemCount, int shownItemCount = 10, int currentPage = 1)
        {
            TotalPageCount = (int)Math.Ceiling((decimal)totalItemCount / shownItemCount);
            CurrentPage = (currentPage < 1) ? 1 : currentPage;
        }

        public int TotalPageCount { get; }

        public int CurrentPage { get; }
    }

    public class SymmetricPagination : Pagination
    {
        public SymmetricPagination(int totalItemCount, int shownItemCount = 10, int currentPage = 1, int shownPageCount = 5)
        {
            TotalPageCount = (int)Math.Ceiling((decimal)totalItemCount / shownItemCount);
            CurrentPage = (currentPage < 1 || currentPage > TotalPageCount) ? 1 : currentPage;
            ShownPageCount = shownPageCount;
            if (TotalPageCount == 1)
            {
                StartPage = 1;
                EndPage = 1;
            }
            else
            {
                StartPage = (currentPage < shownPageCount) ? 1 :
                                (currentPage <= TotalPageCount - shownPageCount + 1) ? currentPage - shownPageCount / 2 : TotalPageCount - shownPageCount + 1;
                EndPage = (currentPage < shownPageCount || currentPage <= TotalPageCount - shownPageCount + 1) ?
                                                                                             StartPage + shownPageCount - 1 : TotalPageCount;
            }
        }

        public int TotalPageCount { get; set; }

        public int CurrentPage { get; }

        public int ShownPageCount { get; }

        public int StartPage { get; }

        public int EndPage { get; }
    }

    public enum PaginationType : byte
    {
        Classic,
        Symmetric
    }
}
