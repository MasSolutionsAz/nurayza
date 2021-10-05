using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using ILoveBaku.Infrastructure.Extensions;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Core.Localization
{
    public class RequestCultureProvider : IRequestCultureProvider
    {
        public async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext.IsNull())
                throw new ArgumentNullException();

            CultureService cultureService = await httpContext.GetServiceAsync<CultureService>();

            string defaultCulture = await httpContext.GetDefaultCulture();

            var cultureOnCookie = await httpContext.GetCultureFromCookie();

            // check has culture on database
            if (!cultureService.IsCulture(cultureOnCookie)) cultureOnCookie = defaultCulture;

            httpContext.Response.Cookies.Append("culture", cultureOnCookie);

            ProviderCultureResult providerCultureResult = new ProviderCultureResult(cultureService.GetCultureOriginalName(cultureOnCookie));

            if (!httpContext.Request.Path.HasValue) return await Task.FromResult(providerCultureResult);

            //Quick and dirty parsing of language from url path, which looks like /api/es-ES/hello-world
            var segments = httpContext.Request.Path.Value
                                        .Split('/')
                                            .Where(p => !p.IsNullOrWhiteSpace())
                                                .ToList();

            if (segments.Count == 0) return await Task.FromResult(providerCultureResult);

            var cultureSegmentIndex = segments.Contains("api") ? 1 : 0;

            string cultureOnUrl = segments[cultureSegmentIndex];

            if (!cultureService.IsValid(cultureOnUrl)) return await Task.FromResult(providerCultureResult);

            // check has culture on database
            if (!cultureService.IsCulture(cultureOnUrl)) cultureOnUrl = defaultCulture;

            httpContext.Response.Cookies.Append("culture", cultureOnUrl);

            cultureService.CurrentCulture = cultureService.GetCultureOriginalName(cultureOnUrl);

            providerCultureResult = new ProviderCultureResult(cultureService.GetCultureOriginalName(cultureOnUrl));

            return await Task.FromResult(providerCultureResult);
        }
    }
}
