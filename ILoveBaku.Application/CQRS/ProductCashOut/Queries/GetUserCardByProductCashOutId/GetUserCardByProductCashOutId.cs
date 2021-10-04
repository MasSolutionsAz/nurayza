using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOut.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetUserCardByProductCashOutId
{
    public class GetUserCardByProductCashOutId : BaseRequest<ApiResult<ProductCashOutCardDto>>
    {
        public int? ProductCashOutId { get; set; }
        public class GetUserCardByProductCashOutIdHandler : IRequestHandler<GetUserCardByProductCashOutId, ApiResult<ProductCashOutCardDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetUserCardByProductCashOutIdHandler(IApplicationDbContext context,
                                                        IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<ApiResult<ProductCashOutCardDto>> Handle(GetUserCardByProductCashOutId request, CancellationToken cancellationToken)
            {
                ProductCashOutCardDto data = _mapper.Map<ProductsCashOutCards, ProductCashOutCardDto>(await _context.ProductsCashOutCards.Where(c => c.ProductsCashOutId == request.ProductCashOutId).FirstOrDefaultAsync());
                if(data!=null)
                {
                    data.Address =  _context.ProductsCashOutAddresses
                                                        .Where(c => c.ProductsCashOutId == request.ProductCashOutId)
                                                        .Select(c=>c.UsersAddressInfo.Address)    
                                                        .FirstOrDefault();

                    return ApiResult<ProductCashOutCardDto>.CreateResponse(data);
                }

                return ApiResult<ProductCashOutCardDto>.CreateResponse(null);
            }
        }
    }
}
