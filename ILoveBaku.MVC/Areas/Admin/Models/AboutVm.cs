using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Models
{
    public class AboutVm
    {
        public List<LanguageDto> Languages { get; set; }
        public List<ContentDto> Contents { get; set; }
    }
}
