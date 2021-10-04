using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguages;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<LanguageDto>>>> GetLanguages()
        {
            return await Mediator.Send(new GetLanguagesQuery());
        }

        [HttpGet("{culture}")]
        public async Task<ActionResult<ApiResult<LanguageDto>>> Get(string culture)
        {
            return await Mediator.Send(new GetLanguageQuery(culture));
        }
    }
}