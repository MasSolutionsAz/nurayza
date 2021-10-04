using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Payment.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Payment.Queries
{
    public class GetPaymentTypesQuery : BaseRequest<ApiResult<List<PaymentTypeDto>>>
    {
        public class GetPaymentTypesQueryHandler : IRequestHandler<GetPaymentTypesQuery, ApiResult<List<PaymentTypeDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetPaymentTypesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<PaymentTypeDto>>> Handle(GetPaymentTypesQuery request, CancellationToken cancellationToken)
            {
                var paymentTypes = await _context.ProductsCashOutPaymentsTypes.Where(c => c.IsActive).Select(c => new PaymentTypeDto
                {
                    Id = (int)c.Id,
                    Name = c.Name
                }).ToListAsync();

                paymentTypes = paymentTypes == null ? new List<PaymentTypeDto>() : paymentTypes;
                return ApiResult<List<PaymentTypeDto>>.CreateResponse(paymentTypes);
            }
        }
    }
}
