using AutoMapper;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Language.Queries.GetLanguage
{
    public class GetLanguageQuery : BaseRequest<ApiResult<LanguageDto>>
    {
        public string CultureOfLanguage { get; }

        public Expression<Func<Langs, bool>> Expression { get; }

        public GetLanguageQuery(string culture)
        {
            CultureOfLanguage = culture;
        }

        public GetLanguageQuery(Expression<Func<Langs, bool>> expression)
        {
            Expression = expression;
        }

        public class GetCheckHasLanguageQueryHandler : IRequestHandler<GetLanguageQuery, ApiResult<LanguageDto>>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            public GetCheckHasLanguageQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ApiResult<LanguageDto>> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
            {
                LanguageDto response  = _mapper.Map<LanguageDto>(await _context.GetLanguage(request.Expression ?? 
                                                                                           (l => l.Culture == request.CultureOfLanguage)));

                return ApiResult<LanguageDto>.CreateResponse(response);
            }
        }
    }
}
