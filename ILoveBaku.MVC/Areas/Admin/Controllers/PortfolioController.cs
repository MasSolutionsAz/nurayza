using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class PortfolioController : BaseController
    {
        
        public async Task<IActionResult> List(int page = 1)
        {
            int take = 10;

            ApiResult<AllNewsVM> allNews = await API.GetAsync<ApiResult<AllNewsVM>>($"news?take={take}&page={page}");
            if (allNews != null && allNews.Succeeded)
            {
                allNews.Response.AllNews = allNews.Response.AllNews.OrderByDescending(c => c.ShowDate).ToList();
                return View(allNews.Response);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Process(int portfolioId)
        {
            return View(portfolioId);
        }


        [HttpPost]
        public async Task<JsonResult> Process(int? newsId,NewsProcessVm model)
        {
            if(newsId == null)
            {

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        status = 400,
                        errors = ModelState.GetErrorsWithKey()
                    });


                //Create news
                ApiResult<int> createNewsResult = await API.PostAsync<NewsVm, ApiResult<int>>("news", model.News);
                if(createNewsResult!=null && !createNewsResult.Succeeded)
                {
                    return Json(new
                    {
                        status = 400,
                        errors = createNewsResult.ErrorList
                    });
                }

                if(createNewsResult == null)
                {
                    return Json(new
                    {
                        status = 400,
                        errors = new Dictionary<string, string>() { { "", "Xəta baş verdi." } }
                    });
                }

                return Json(new
                {
                    status = 200,
                    data = createNewsResult.Response
                });
            }
            

            return Json(new
            {
                status = 400
            });
        }

        public async Task<IActionResult> GetNews(int? newsId)
        {
            if (newsId == null) return Json(new { status = 400, errors = new Dictionary<string, string> { { "", "Xeta bas verdi" } } });

            var getNewsLangResult =  await API.GetAsync<ApiResult<List<NewsLangVm>>>($"news/{newsId}/langs");
            var getNewsResult = await API.GetAsync<ApiResult<NewsVm>>($"news/getNews/{newsId}");
            var getNewsLangsStatuses = await API.GetAsync<ApiResult<List<NewsLangsStatuses>>>("news/langs/statuses");
            if(getNewsLangResult!=null && getNewsLangResult.Succeeded && getNewsResult!=null && getNewsResult.Succeeded && getNewsLangsStatuses!=null && getNewsLangsStatuses.Succeeded)
            {
                NewsProcessVm vm = new NewsProcessVm
                {
                    News = getNewsResult.Response,
                    NewsLangs = getNewsLangResult.Response,
                    Statuses = getNewsLangsStatuses.Response
                };

                return Json(new
                {
                    status = 200,
                    data = vm
                });
            }
            return Json(new
            {
                status = 200,
                errors = new Dictionary<string, string> { {"","Xəta baş verdi." } }
            });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateNews(int? newsId,NewsVm model)
        {
            if (newsId == null)
                return Json(new { status = 400 });

            if (!ModelState.IsValid)
                return Json(new { status = 400 });

            var updateNewsResult = await API.PutAsync<NewsVm, ApiResult<int?>>($"news/{newsId}", model);
            if (updateNewsResult != null && !updateNewsResult.Succeeded)
            {
                return Json(new
                {
                    status = 400,
                    errors = updateNewsResult.ErrorList
                });
            }

            if (updateNewsResult == null)
            {
                return Json(new
                {
                    status = 400,
                    errors = new Dictionary<string, string>() { { "", "Xəta baş verdi." } }
                });
            }

            return Json(new
            {
                status = 200,
                data = updateNewsResult.Response
            });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateNewsLang(NewsLangVm model)
        {
            if (model.Id == 0 || model.Title == null || model.Description == null || model.ContentHtml == null)
                return Json(new { status = 400 });

            var updateNewsLangResult = await API.PutAsync<NewsLangVm, ApiResult<int?>>($"news/langs/{model.Id}", model);
            if (updateNewsLangResult != null && !updateNewsLangResult.Succeeded)
            {
                return Json(new
                {
                    status = 400,
                    errors = updateNewsLangResult.ErrorList
                });
            }

            if (updateNewsLangResult == null)
            {
                return Json(new
                {
                    status = 400,
                    errors = new Dictionary<string, string>() { { "", "Xəta baş verdi." } }
                });
            }

            return Json(new
            {
                status = 200,
                data = updateNewsLangResult.Response
            });
        }


        [HttpPost]
        public async Task<JsonResult> UploadPhoto(List<PhotoModel> photos, List<string> deletePhotos, int newsId)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        "uploads",
                                        "portfolios");

            var result = await UploadAsync(path, photos, deletePhotos, newsId, "/uploads/portfolios");
            return Json(result);
        }

        public async Task<object> UploadAsync(string path, List<PhotoModel> upload, List<string> delete, int newsId, string url)
        {
            var response = new List<PhotoModel>();
            foreach (var file in upload)
            {
                ProductFileDto fileDto = new ProductFileDto
                {
                    ContentType = file.File.ContentType,
                    IsMain = file.IsMain,
                    Length = file.File.Length,
                    Name = file.File.FileName,
                    Path = url
                };

                var data = await API.PostAsync<ProductFileDto, ApiResult<PhotoModel>>($"news/{newsId}/files", fileDto);
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data != null && data.Succeeded)
                {

                    string photoPath = path + "/" + data.Response.Name;
                    using (FileStream stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await file.File.CopyToAsync(stream);
                    }
                    response.Add(new PhotoModel { IsMain = data.Response.IsMain, Name = data.Response.Name });
                }
            }

            foreach (var file in delete)
            {
                var data = await API.DeleteAsync<ApiResult<string>>($"news/{newsId}/files/?name={file}");
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data.Succeeded)
                {
                    string photoPath = path + "/" + data.Response;
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }
            }
            return new
            {
                status = 200,
                data = response
            };
        }
        public async Task<JsonResult> GetPhotos(int? newsId)
        {
            if (newsId == null)
                return Json(new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Xəta baş verdi." } } });

            var data = await API.GetAsync<ApiResult<List<ProductFileDto>>>($"news/{newsId}/files");
            if (data != null && data.Succeeded)
                return Json(new { status = 200, data = data.Response });

            return Json( new { status = 400 });
        }


    }
}
