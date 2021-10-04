using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Country.Queries.GetCitiesByCountryId
{
    public class GetCitiesByCountryIdQuery : BaseRequest<ApiResult<List<Regions>>>
    {
        public int CountryId { get; set; }
        public class GetCitiesByCountryIdQueryHandler : IRequestHandler<GetCitiesByCountryIdQuery, ApiResult<List<Regions>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCitiesByCountryIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<Regions>>> Handle(GetCitiesByCountryIdQuery request, CancellationToken cancellationToken)
            {
                var cities = await _context.Regions.Where(c => c.CountryId == request.CountryId).ToListAsync();
                return ApiResult<List<Regions>>.CreateResponse(cities);
            }
        }
    }
}
