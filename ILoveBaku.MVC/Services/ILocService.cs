using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public interface ILocService
    {
        LocalizedString GetLocalizedHtmlString(string key);
    }
}
