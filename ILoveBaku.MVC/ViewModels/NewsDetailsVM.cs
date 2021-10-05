using ILoveBaku.Application.CQRS.News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.ViewModels
{
    public class NewsDetailsVM
    {
        public NewsDetailsDto News { get; set; }

        public List<NewsDto> RelatedNews { get; set; }
    }
}
