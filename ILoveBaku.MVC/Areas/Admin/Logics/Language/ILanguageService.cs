using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Language
{
    public interface ILanguageService
    {
        Task<List<LanguageDto>> GetLanguages();
    }
}
