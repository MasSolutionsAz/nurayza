using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Controllers
{
    public class AboutController : BaseController
    {
        public async Task<IActionResult> Index()
        {

            string URL = $"contents/{(int)ContentCategory.About}";

            ApiResult<ContentDto> response = await API.GetAsync<ApiResult<ContentDto>>(URL);

            return View(response.Response);
        }
    }
}