using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Country.Queries.GetCountries
{
    public class GetCountriesQuery:BaseRequest<ApiResult<List<Countries>>>
    {
        public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, ApiResult<List<Countries>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCountriesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<Countries>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
            {
                var countries = await _context.Countries.ToListAsync();
                countries = countries == null ? new List<Countries>() : countries;
                return ApiResult<List<Countries>>.CreateResponse(countries);
            }
        }
    }
}
