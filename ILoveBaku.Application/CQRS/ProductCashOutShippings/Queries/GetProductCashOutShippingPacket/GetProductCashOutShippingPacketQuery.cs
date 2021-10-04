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

namespace ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPacket
{
    public class GetProductCashOutShippingPacketQuery : BaseRequest<ApiResult<ProductsCashOutShippingsPacketsDetailsDto>>
    {
        public int ProductCashOutId { get; set; }
        public class GetProductCashOutShippingPacketQueryHadnler : IRequestHandler<GetProductCashOutShippingPacketQuery, ApiResult<ProductsCashOutShippingsPacketsDetailsDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProductCashOutShippingPacketQueryHadnler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<ProductsCashOutShippingsPacketsDetailsDto>> Handle(GetProductCashOutShippingPacketQuery request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<ProductsCashOutShippingsPacketsDetails, ProductsCashOutShippingsPacketsDetailsDto>(await _context.ProductsCashOutShippingsPacketsDetails
                                                                                                                                            .Where(c => c.ProductsCashOutsId == request.ProductCashOutId
                                                                                                                                            &&
                                                                                                                                            c.ProductsCashOutShippingsPacketsDetailsStatuses == (byte)ProductCashOutShippingPacketDetailStatus.Aktiv)
                                                                                                                                            .FirstOrDefaultAsync());
                return ApiResult<ProductsCashOutShippingsPacketsDetailsDto>.CreateResponse(data);
            }

        }
    }
}
