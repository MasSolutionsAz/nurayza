using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.News.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Queries.GetNewsLangs
{
    public class GetNewsLangsQuery:BaseRequest<ApiResult<List<NewsLangVm>>>
    {
        public int NewsId { get; set; }
        public class GetNewsLangsQueryHandler : IRequestHandler<GetNewsLangsQuery, ApiResult<List<NewsLangVm>>>
        {
            private readonly IApplicationDbContext _context;
            public GetNewsLangsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<NewsLangVm>>> Handle(GetNewsLangsQuery request, CancellationToken cancellationToken)
            {
                var news = await _context.News.Where(c => c.Id == request.NewsId).FirstOrDefaultAsync();

                if(news == null)
                {
                    request.Errors.Add("", "Belə bir portfolio yoxdur!");
                }

                var newsLangs = await _context.NewsLangs.Where(c => c.NewsId == news.Id).Select(c => new NewsLangVm
                {
                    Id = c.Id,
                    LangName = c.Lang.DisplayName,
                    Description = c.Description,
                    ContentHtml = c.ContentHtml,
                    ViewCount = c.ViewCount,
                    Title = c.Title,
                    Status = c.NewsLangsStatusesId,
                    TitleUrl = c.TitleUrl
                }).ToListAsync();


                return ApiResult<List<NewsLangVm>>.CreateResponse(newsLangs);
            }
        }
    }
}
