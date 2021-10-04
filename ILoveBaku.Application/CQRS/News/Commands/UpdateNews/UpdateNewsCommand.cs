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

namespace ILoveBaku.Application.CQRS.News.Commands.UpdateNews
{
    public  class UpdateNewsCommand:BaseRequest<ApiResult<int?>>
    {
        public int NewsId { get; set; }
        public NewsVm Model { get; set; }
        public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, ApiResult<int?>>
        {
            private IApplicationDbContext _context;
            public UpdateNewsCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
            {
                var news = await _context.News.Where(c => c.Id == request.NewsId).FirstOrDefaultAsync();
                if(news == null)
                {
                    request.Errors.Add("", "Belə bir məlumat tapılmadı");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                news.Title = request.Model.Title;
                news.ShowDate = request.Model.ShowDate;
                news.IsActive = request.Model.Status;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(news.Id);
            }
        }
    }
}
