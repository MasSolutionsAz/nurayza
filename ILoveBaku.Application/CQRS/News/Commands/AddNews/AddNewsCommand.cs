using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Commands.AddNews
{
    public class AddNewsCommand : BaseRequest<ApiResult<int>>
    {
        public NewsVm Model { get; set; }
        public class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;
            public AddNewsCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }


            public async Task<ApiResult<int>> Handle(AddNewsCommand request, CancellationToken cancellationToken)
            {
                ILoveBaku.Domain.Entities.News news = new ILoveBaku.Domain.Entities.News
                {
                    Title = request.Model.Title,
                    ShowDate = request.Model.ShowDate,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    IsActive = request.Model.Status
                };

                await _context.News.AddAsync(news);
                await _context.SaveChangesAsync();

                var langs = await _context.Langs.ToListAsync();

                for (int i = 0; i < langs.Count; i++)
                {
                    NewsLangs newsLang = new NewsLangs
                    {
                        NewsLangsStatusesId = Convert.ToByte(ILoveBaku.Domain.Enums.NewsLangStatus.Active),
                        ContentHtml = news.Title,
                        CreatedDate = DateTime.Now,
                        Description = news.Title,
                        LangsId = langs[i].Id,
                        NewsId = news.Id,
                        Title = news.Title,
                        TitleUrl = news.Title,
                        UpdatedDate = DateTime.Now,
                        ViewCount = 0
                    };

                    await _context.NewsLangs.AddAsync(newsLang);
                }

                await _context.SaveChangesAsync();
                return ApiResult<int>.CreateResponse(news.Id);
            }
        }
    }
}
