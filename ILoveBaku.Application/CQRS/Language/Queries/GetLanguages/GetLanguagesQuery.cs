using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
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

namespace ILoveBaku.Application.CQRS.Language.Queries.GetLanguages
{
    public class GetLanguagesQuery:BaseRequest<ApiResult<List<LanguageDto>>>
    {
        public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, ApiResult<List<LanguageDto>>>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            public GetLanguagesQueryHandler(IApplicationDbContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ApiResult<List<LanguageDto>>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
            {
                var a = await _context.Langs
                                          .Where(c => c.LangsStatusesId == (int)LanguageStatuses.Active)
                                            .ToListAsync();
                List<LanguageDto> test = _mapper.Map<List<Langs>, List<LanguageDto>>(a);
                List <LanguageDto> languages = _mapper.Map<List<Langs>, List<LanguageDto>>(await _context.Langs
                                                                                                          .Where(c => c.LangsStatusesId == (int)LanguageStatuses.Active)
                                                                                                            .ToListAsync());

                return ApiResult<List<LanguageDto>>.CreateResponse(languages);
            }
        }
    }
}
