using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Queries.GetAllNews
{
    public class GetAllNewsQuery : BaseRequest<ApiResult<AllNewsVM>>
    {
        public NewsLangStatus NewsLangStatus { get; set; }

        public int Take { get; set; }

        public int Page { get; set; }

        public GetAllNewsQuery(NewsLangStatus newsLangStatus = NewsLangStatus.NonSelected, int take = 10, int page = 1)
        {
            NewsLangStatus = newsLangStatus;
            Take = take;
            Page = (page > 0) ? page : 1;
        }

        public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, ApiResult<AllNewsVM>>
        {
            private readonly IApplicationDbContext _context;

            public GetAllNewsQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<AllNewsVM>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
            {
                int take = request.Take;

                int page = request.Page;

                IQueryable<NewsLangs> newsLangs = _context.NewsLangs.Where(nl => nl.Lang.Culture == request.Culture &&
                                                                                 (((int)request.NewsLangStatus).IsZore() ||
                                                                                 nl.NewsLangsStatusesId == (byte)request.NewsLangStatus))
                                                                       .OrderByDescending(nl => nl.News.ShowDate);

                int newsCount = newsLangs.Count();

                if (!(await newsLangs.Skip(take * (page - 1)).Take(take).CountAsync()).IsZore())
                {
                    newsLangs = newsLangs.Skip(take * (page - 1)).Take(take);
                }
                else
                {
                    page = 1;
                    newsLangs = newsLangs.Skip(take * (page - 1)).Take(take);
                }

                List<NewsDto> allNews = await newsLangs.Select(nl => new NewsDto()
                {
                    Id = nl.NewsId,
                    Title = nl.Title,
                    Description = nl.Description,
                    ShowDate = nl.News.ShowDate,
                    Image = nl.News.NewsFiles.FirstOrDefault().Files.Path,
                    Status = (byte)(nl.News.IsActive?10:20),
                    CreatedDate = nl.News.CreatedDate
                }).ToListAsync();

                AllNewsVM model = new AllNewsVM()
                {
                    NewsCount = newsCount,
                    Page = page,
                    AllNews = allNews
                };

                return ApiResult<AllNewsVM>.CreateResponse(model);
            }
        }
    }
}
