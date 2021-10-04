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

namespace ILoveBaku.Application.CQRS.News.Queries.GetNewsById
{
    public class GetNewsByIdQuery : BaseRequest<ApiResult<NewsVm>>
    {
        public int NewsId { get; set; }
        public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, ApiResult<NewsVm>>
        {
            private readonly IApplicationDbContext _context;

            public GetNewsByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<NewsVm>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
            {
                var news = await _context.News.Where(c => c.Id == request.NewsId).Select(c => new NewsVm()
                {
                    ShowDate = c.ShowDate,
                    Date = c.ShowDate.ToString("yyyy-MM-dd"),
                    Title = c.Title,
                    Status = c.IsActive
                }).FirstOrDefaultAsync();


                if (news == null)
                {
                    request.Errors.Add("", "Portfolio tapılmadı.");
                    return ApiResult<NewsVm>.CreateResponse(news,request.Errors);
                }

                return ApiResult<NewsVm>.CreateResponse(news);
            }
        }
    }
}
