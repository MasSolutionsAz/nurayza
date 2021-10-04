using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.News.Models
{
    public class AllNewsVM
    {
        public int NewsCount { get; set; }

        public int Page { get; set; }

        public List<NewsDto> AllNews { get; set; }
    }
}
