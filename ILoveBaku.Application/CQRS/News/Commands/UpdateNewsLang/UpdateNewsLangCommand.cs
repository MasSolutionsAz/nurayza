using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.News.Models;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Commands.UpdateNewsLang
{
    public class UpdateNewsLangCommand : BaseRequest<ApiResult<int?>>
    {
        public NewsLangVm Model { get; set; }
        public int NewsLangId { get; set; }
        public class UpdateNewsLangCommandHandler : IRequestHandler<UpdateNewsLangCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateNewsLangCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateNewsLangCommand request, CancellationToken cancellationToken)
            {
                var newsLang = await _context.NewsLangs.Where(c => c.Id == request.NewsLangId).FirstOrDefaultAsync();
                if (newsLang == null)
                {
                    request.Errors.Add("", "Belə bir mılumat tapılmadı.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                newsLang.Title = request.Model.Title;
                newsLang.Description = request.Model.Description;
                newsLang.ContentHtml = request.Model.ContentHtml;
                newsLang.NewsLangsStatusesId = (byte)request.Model.Status;

                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(newsLang.Id);
            }
        }
    }
}
