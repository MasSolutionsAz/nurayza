using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Content.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Content.Queries.GetContent
{
    public class GetContentQuery : BaseRequest<ApiResult<ContentDto>>
    {
        public int ContentCategoryId { get; set; }

        public GetContentQuery(int contentCategoryId) => ContentCategoryId = contentCategoryId;

        public class GetContentQueryHandler : IRequestHandler<GetContentQuery, ApiResult<ContentDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetContentQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<ContentDto>> Handle(GetContentQuery request, CancellationToken cancellationToken)
            {
                ContentsLangs contentLang = await _context.ContentsLangs
                                                            .FirstOrDefaultAsync(cl => cl.Content.ContentsCategoriesId == request.ContentCategoryId &&
                                                                                       cl.Lang.Culture == request.Culture);

                ContentDto content = new ContentDto()
                {
                    Title = contentLang.Title,
                    Content = contentLang.ContentHtml,
                    Image = contentLang.Content.File?.Path
                };

                return ApiResult<ContentDto>.CreateResponse(content);
            }
        }
    }
}
