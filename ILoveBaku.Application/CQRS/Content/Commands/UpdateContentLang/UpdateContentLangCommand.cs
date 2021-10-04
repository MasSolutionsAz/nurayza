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

namespace ILoveBaku.Application.CQRS.Content.Commands.UpdateContentLang
{
    public class UpdateContentLangCommand:BaseRequest<ApiResult<int?>>
    {
        public int ContentCategoryId { get; set; }
        public int ContentLangId { get; set; }
        public ContentDto Model { get; set; }
        public class UpdateContentCommandHandler : IRequestHandler<UpdateContentLangCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateContentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateContentLangCommand request, CancellationToken cancellationToken)
            {
                var checkAbout = await _context.Contents.AnyAsync(c => c.ContentsCategoriesId == request.ContentCategoryId);
                var contentLang = await _context.ContentsLangs.Where(c => c.Id == request.ContentLangId && c.Content.ContentsCategoriesId == request.ContentCategoryId).FirstOrDefaultAsync();
                if (contentLang == null && !checkAbout)
                {
                    request.Errors.Add("", "Xeta");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }
                else if(contentLang == null && checkAbout)
                {
                    Contents content = await _context.Contents.Where(c => c.ContentsCategoriesId == request.ContentCategoryId).FirstOrDefaultAsync();
                    contentLang = new ContentsLangs
                    {
                        Title = content.Title,
                        SubTitle = content.Title,
                        ContentsId  = content.Id,
                        ContentHtml = request.Model.Content,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        LangsId = await _context.Langs.Where(c=>c.DisplayName == request.Model.LangName).Select(c=>c.Id).FirstOrDefaultAsync(),
                        UpdatedDate = DateTime.Now,
                        VisitorCount = 0
                    };
                    await _context.ContentsLangs.AddAsync(contentLang);
                }
                else
                {
                    contentLang.ContentHtml = request.Model.Content;
                }

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(contentLang.Id);

            }
        }
    }
}
