using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguages;
using ILoveBaku.MVC.Core.Localization;
using ILoveBaku.MVC.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public class CultureService
    {
        private readonly ApiRequestService _apiService;

        private readonly HttpContext _httpContext;

        private List<Culture> _cultures;

        public List<Culture> Cultures => _cultures ??= GetCultures().Result;

        private SelectList _selectListOfCultures;

        public SelectList SelectListOfCultures => _selectListOfCultures ??= GetSelectListOfCultures().Result;

        private string _currentCulture;

        public string CurrentCulture
        {
            get { return _currentCulture ??= GetCultureOriginalName(_httpContext.GetCultureFromCookie().Result ?? DefaultCulture); }
            set { if (IsCulture(value)) _currentCulture = value; }
        }

        private string _shortCulture;

        public string ShortCulture
        {
            get { return _shortCulture ??= GetCultureShortName(_httpContext.GetCultureFromCookie().Result ?? GetCultureShortName(DefaultCulture)); }
            set { if (IsCulture(value)) _shortCulture = value; }
        }

        public string DefaultCulture { get; }

        public CultureService(ApiRequestService apiService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _apiService = apiService;
            _httpContext = httpContextAccessor.HttpContext;
            DefaultCulture = configuration["Default:Culture"];
        }

        public async Task<List<Culture>> GetCultures()
        {
            return (await _apiService?.GetAsync<ApiResult<List<LanguageGetDto>>>("languages"))?
                                        .Response?.Select(l => new Culture()
                                        {
                                            Name = l.Culture,
                                            ShortName = l.Culture?.Split('-')?[0],
                                            DisplayName = l.DisplayName,
                                            File = l.File
                                        })?.ToList();
        }

        public async Task<List<Culture>> GetCultures(ApiRequestService apiService)
        {
            return (await apiService?.GetAsync<ApiResult<List<LanguageGetDto>>>("languages"))?
                                        .Response?.Select(l => new Culture()
                                        {
                                            Name = l.Culture,
                                            ShortName = l.Culture?.Split('-')?[0],
                                            DisplayName = l.DisplayName,
                                            File = l.File
                                        })?.ToList();
        }

        private async Task<SelectList> GetSelectListOfCultures()
        {
            return await Task.FromResult(new SelectList(Cultures, "name", "displayName", CurrentCulture));
        }

        //functions
        public string GetCultureOriginalName(string culture)
        {
            return Cultures.FirstOrDefault(c => c.Name == culture || c.ShortName == culture)?.Name;
        }

        public string GetCultureShortName(string culture)
        {
            return Cultures.FirstOrDefault(c => c.Name == culture || c.ShortName == culture)?.ShortName;
        }

        public bool IsCulture(string culture)
        {
            return Cultures.Any(c => c.Name == culture || c.ShortName == culture);
        }

        public bool IsValid(string culture)
        {
            return Regex.IsMatch(culture, @"^[a-z]{2}(?:-[A-Z]{2})?$");
        }

        public bool HasCulture(string culture)
        {
            return Cultures.Any(c => c.Name == culture);
        }

        public bool HasUICulture(string culture)
        {
            return Cultures.Any(c => c.ShortName == culture);
        }
    }
}
