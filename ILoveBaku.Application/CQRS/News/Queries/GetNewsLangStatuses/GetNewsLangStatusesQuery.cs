using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Queries.GetNewsLangStatuses
{
    public class GetNewsLangStatusesQuery:BaseRequest<ApiResult<List<NewsLangsStatuses>>>
    {
        public class GetNewsLangStatusesQueryHandler : IRequestHandler<GetNewsLangStatusesQuery, ApiResult<List<NewsLangsStatuses>>>
        {
            private readonly IApplicationDbContext _context;
            public GetNewsLangStatusesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            
            public async Task<ApiResult<List<NewsLangsStatuses>>> Handle(GetNewsLangStatusesQuery request, CancellationToken cancellationToken)
            {
                var newsLangsStatuses = await _context.NewsLangsStatuses.ToListAsync();

                return ApiResult<List<NewsLangsStatuses>>.CreateResponse(newsLangsStatuses);
            }
        }
    }
}
