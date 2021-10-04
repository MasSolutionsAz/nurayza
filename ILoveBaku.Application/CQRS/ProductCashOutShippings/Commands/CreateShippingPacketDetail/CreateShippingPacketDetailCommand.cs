using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
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

namespace ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacketDetail
{
    public class CreateShippingPacketDetailCommand : BaseRequest<ApiResult<int?>>
    {
        public int ProductCashOutId { get; set; }
        public int PacketId { get; set; }

        public class CreateShippingPacketDetailCommandHandler : IRequestHandler<CreateShippingPacketDetailCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public CreateShippingPacketDetailCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(CreateShippingPacketDetailCommand request, CancellationToken cancellationToken)
            {

                var detail = await _context.ProductsCashOutShippingsPacketsDetails.Where(c => c.ProductsCashOutsId == request.ProductCashOutId
                                                                                              && c.ProductsCashOutShippingsPacketsDetailsStatuses == (byte)ProductCashOutShippingPacketDetailStatus.Aktiv)
                                                                                                .FirstOrDefaultAsync();

                if (detail != null)
                {
                    detail.ProductsCashOutShippingsPacketsDetailsStatuses = (byte)ProductCashOutShippingPacketDetailStatus.Deaktiv;
                    await _context.SaveChangesAsync();
                }

                var newDetail = new ProductsCashOutShippingsPacketsDetails
                {
                    ProductsCashOutShippingsPacketsDetailsStatuses = (byte)ProductCashOutShippingPacketDetailStatus.Aktiv,
                    ProductsCashOutShippingsPacketsId = request.PacketId,
                    ProductsCashOutsId = request.ProductCashOutId,
                    TrackingNumber = Guid.NewGuid(),
                    Price = 0
                };



                await _context.ProductsCashOutShippingsPacketsDetails.AddAsync(newDetail);
                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(newDetail.Id);
            }
        }
    }
}
