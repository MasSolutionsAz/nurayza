using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket
{
    public class CreateShippingPacketCommand:BaseRequest<ApiResult<int?>>
    {
        public string Name { get; set; }
        public class CreateShippingPacketCommandHandler : IRequestHandler<CreateShippingPacketCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public CreateShippingPacketCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(CreateShippingPacketCommand request, CancellationToken cancellationToken)
            {
                ProductsCashOutShippingsPackets packet = new ProductsCashOutShippingsPackets
                {
                    ProductsCashOutShippingsPacketsStatusesId = (byte)ProductCashOutShippingPacketStatus.Hazırlanır,
                    CreatedDate = DateTime.Now,
                    Name = request.Name,
                    ResponsablePerson = ""
                };

                await _context.ProductsCashOutShippingsPackets.AddAsync(packet);
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(packet.Id);
            }
        }
    }
}
