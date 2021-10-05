using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Infrastructure.Extensions;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Controllers
{
    public class LanguageController : BaseController
    {
        public IActionResult Change(string culture, string returnUrl)
        {
            CultureService cultureService = HttpContext.RequestServices.GetService<CultureService>();

            if (culture.IsNull() || !cultureService.IsCulture(culture)) culture = Culture;

            Response.Cookies.Append("culture", culture);

            if (Url.IsLocalUrl(returnUrl))
            {
                if (!returnUrl.StartsWith("~"))
                    returnUrl = $"~{returnUrl}";

                var cultureOnUrl = returnUrl.Split('/')?[1];

                if (cultureOnUrl.IsNullOrEmpty() || cultureService.IsCulture(cultureOnUrl))
                    returnUrl = returnUrl.Replace($"~/{cultureOnUrl}", $"~/{culture}");

                if (Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home", new { culture = culture });
        }
    }
}