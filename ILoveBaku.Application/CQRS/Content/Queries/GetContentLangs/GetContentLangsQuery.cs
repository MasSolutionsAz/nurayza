using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Content.Queries.GetContentLangs
{
    public class GetContentLangsQuery:BaseRequest<ApiResult<List<ContentDto>>>
    {
        public int ContentCategoryId { get; set; }
        public class GetContentLangsQueryHandler : IRequestHandler<GetContentLangsQuery, ApiResult<List<ContentDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetContentLangsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ContentDto>>> Handle(GetContentLangsQuery request, CancellationToken cancellationToken)
            {
                List<ContentDto> contentLangs = await _context.ContentsLangs
                                                            .Where(cl => cl.Content.ContentsCategoriesId == request.ContentCategoryId).Select(c=>new ContentDto
                                                                                       {
                                                                                           Title = c.Title,
                                                                                           Content = c.ContentHtml,
                                                                                           Image = c.Content.File.Path,
                                                                                           ImageName = c.Content.File.Name,
                                                                                           ContentId = c.ContentsId,
                                                                                           ContentLangId = c.Id,
                                                                                           LangName = c.Lang.DisplayName
                                                                                       }).ToListAsync();


                return ApiResult<List<ContentDto>>.CreateResponse(contentLangs);
            }
        }
    }
}
