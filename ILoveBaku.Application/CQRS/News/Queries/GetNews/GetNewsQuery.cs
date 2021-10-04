using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Queries.GetNews
{
    public class GetNewsQuery : BaseRequest<ApiResult<NewsDetailsDto>>
    {
        public int Id { get; set; }

        public GetNewsQuery(int id) => Id = id;

        public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, ApiResult<NewsDetailsDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetNewsQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<NewsDetailsDto>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
            {
                NewsLangs newsLangs = await _context.NewsLangs.FirstOrDefaultAsync(nl => nl.NewsId == request.Id &&
                                                                                         nl.Lang.Culture == request.Culture);

                if (newsLangs.IsNull())
                    return ApiResult<NewsDetailsDto>.CreateResponse(null, null, new ErrorDetail()
                    {
                        ErrorMessage = "News not found."
                    });

                NewsDetailsDto newsDetailsDto = new NewsDetailsDto()
                {
                    Id = newsLangs.NewsId,
                    Title = newsLangs.Title,
                    ContentHTML = newsLangs.ContentHtml,
                    ShowDate = newsLangs.News.ShowDate
                };

                return ApiResult<NewsDetailsDto>.CreateResponse(newsDetailsDto);
            }
        }
    }
}
