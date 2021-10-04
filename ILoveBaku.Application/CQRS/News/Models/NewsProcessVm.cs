using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.News.Models
{
    public class NewsProcessVm
    {
       public NewsVm News { get; set; }
       public NewsLangVm NewsLang { get; set; }

        public List<NewsLangsStatuses> Statuses { get; set; }
        public List<NewsLangVm> NewsLangs { get; set; }
    }
}
