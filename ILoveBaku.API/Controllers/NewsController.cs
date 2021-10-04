using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.News.Commands.AddNews;
using ILoveBaku.Application.CQRS.News.Commands.AddNewsFile;
using ILoveBaku.Application.CQRS.News.Commands.DeleteNewsFile;
using ILoveBaku.Application.CQRS.News.Commands.UpdateNews;
using ILoveBaku.Application.CQRS.News.Commands.UpdateNewsLang;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Application.CQRS.News.Queries.GetAllNews;
using ILoveBaku.Application.CQRS.News.Queries.GetNews;
using ILoveBaku.Application.CQRS.News.Queries.GetNewsById;
using ILoveBaku.Application.CQRS.News.Queries.GetNewsLangs;
using ILoveBaku.Application.CQRS.News.Queries.GetNewsLangStatuses;
using ILoveBaku.Application.CQRS.News.Queries.GetNewsPhotos;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<AllNewsVM>>> GetAllNews(NewsLangStatus nls, int take = 10, int page = 1)
        {
            return await Mediator.Send(new GetAllNewsQuery(nls, take, page));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<NewsDetailsDto>>> GetNews(int id)
        {
            return await Mediator.Send(new GetNewsQuery(id));
        }
        [HttpGet("getNews/{newsId}")]
        public async Task<ActionResult<ApiResult<NewsVm>>> GetNewsById(int newsId)
        {
            return await Mediator.Send(new GetNewsByIdQuery() { NewsId = newsId });
        }

        [HttpGet("{newsId}/langs")]
        public async Task<ActionResult<ApiResult<List<NewsLangVm>>>> GetNewsLangs(int newsId)
        {
            return await Mediator.Send(new GetNewsLangsQuery { NewsId = newsId });
        }
        [HttpGet("langs/statuses")]
        public async Task<ActionResult<ApiResult<List<NewsLangsStatuses>>>> GetNewsLangsStatuses()
        {
            return await Mediator.Send(new GetNewsLangStatusesQuery());
        }


        [HttpGet("{newsId}/files")]
        public async Task<ActionResult<ApiResult<List<ProductFileDto>>>> GetNewsPhotos(int newsId)
        {
            return await Mediator.Send(new GetNewsPhotosQuery { NewsId = newsId });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<int>>> AddNews(NewsVm model)
        {
            return await Mediator.Send(new AddNewsCommand { Model = model });
        }

        [HttpPost("{newsId}/files")]
        public async Task<ActionResult<ApiResult<PhotoModel>>> AddNewsFile(int newsId,ProductFileDto model)
        {
            return await Mediator.Send(new AddNewsFileCommand { NewsId = newsId, Model = model });
        }

        [HttpPut("{newsId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateNews(int newsId,NewsVm model)
        {
            return await Mediator.Send(new UpdateNewsCommand { NewsId = newsId, Model = model });
        }


        [HttpPut("langs/{newsLangId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateNewsLang(int newsLangId,NewsLangVm model)
        {
            return await Mediator.Send(new UpdateNewsLangCommand { Model = model, NewsLangId = newsLangId });
        }

        [HttpDelete("{newsId}/files")]
        public async Task<ActionResult<ApiResult<string>>> DeletePhotos(int newsId,string name)
        {
            return await Mediator.Send(new DeletNewsFileCommand { Name = name, NewsId = newsId });
        }

    }
}