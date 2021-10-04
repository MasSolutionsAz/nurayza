using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Content.Commands.UpdateContentLang;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Application.CQRS.Content.Queries.GetContent;
using ILoveBaku.Application.CQRS.Content.Queries.GetContentLangs;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/contents")]
    public class ContentController : BaseController
    {
        [HttpGet("{contentCategoryId}")]
        public async Task<ActionResult<ApiResult<ContentDto>>> Get(int contentCategoryId)
        {
            return await Mediator.Send(new GetContentQuery(contentCategoryId));
        }

        [HttpGet("{contentCategoryId}/langs")]
        public async Task<ActionResult<ApiResult<List<ContentDto>>>> GetLangs(int contentCategoryId)
        {
            return await Mediator.Send(new GetContentLangsQuery { ContentCategoryId = contentCategoryId });
        }

        [HttpPost("{contentCategoryId}/{contentLangId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateContentLang(int contentCategoryId,int contentLangId,ContentDto model)
        {
            return await Mediator.Send(new UpdateContentLangCommand { ContentCategoryId = contentCategoryId, ContentLangId = contentLangId, Model = model });
        }
    }
}
