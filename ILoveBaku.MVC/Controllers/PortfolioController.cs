using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Controllers
{
    public class PortfolioController : BaseController
    {
        [HttpGet("portfolio/list/{page:int?}")]
        public async Task<IActionResult> List(int page = 1)
        {
            int take = 10;

            ApiResult<AllNewsVM> allNews = await API.GetAsync<ApiResult<AllNewsVM>>($"news?nls={NewsLangStatus.Active}&take={take}&page={page}");

            ViewBag.CultureInfo = new CultureInfo(Culture);

            ViewBag.ShownItemCount = take;

            ViewBag.CurrentPage = allNews?.Response?.Page;

            return View(allNews.Response);
        }

        [HttpGet("portfolio/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ApiResult<NewsDetailsDto> response = await API.GetAsync<ApiResult<NewsDetailsDto>>($"news/{id}");

            if (response.IsNull() || !response.Succeeded) return NotFound(response.ErrorDetail.ErrorMessage);

            ViewBag.CultureInfo = new CultureInfo(Culture);

            int take = 4;

            ApiResult<AllNewsVM> relatedNews = await API.GetAsync<ApiResult<AllNewsVM>>($"news?nls={NewsLangStatus.Active}&take={take}");

            NewsDetailsVM model = new NewsDetailsVM()
            {
                News = response.Response,
                RelatedNews = (relatedNews?.Succeeded ?? false) ? relatedNews?.Response?.AllNews : new List<NewsDto>()
            };

            return View(model);
        }
    }
}