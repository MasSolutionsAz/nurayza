using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
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

namespace ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets
{
    public class GetProductCashOutShippingPacketsQuery:BaseRequest<ApiResult<List<ProductCashOutShippingPacketDto>>>
    {
        public class GetProductCashOutShippingPacketsQueryHandler : IRequestHandler<GetProductCashOutShippingPacketsQuery, ApiResult<List<ProductCashOutShippingPacketDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProductCashOutShippingPacketsQueryHandler(IApplicationDbContext context,
                                                                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<ProductCashOutShippingPacketDto>>> Handle(GetProductCashOutShippingPacketsQuery request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<List<ProductsCashOutShippingsPackets>,List<ProductCashOutShippingPacketDto>> (await _context.ProductsCashOutShippingsPackets.Where(c=>c.ProductsCashOutShippingsPacketsStatusesId == (byte)ProductCashOutShippingPacketStatus.Hazırlanır).ToListAsync());
                return ApiResult<List<ProductCashOutShippingPacketDto>>.CreateResponse(data);
            }
        }
    }
}
