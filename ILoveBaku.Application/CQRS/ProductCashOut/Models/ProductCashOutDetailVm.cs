using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPacket;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Models
{
    public class ProductCashOutDetailVm
    {
        public byte ProductCashOutStatusId { get; set; }
        public ProductCashOutCardDto UserCardDto { get; set; }
        public List<ProductCashOutDetailDto> ProductCashOutDetailDtos { get; set; }
        public List<ProductCashOutShippingPacketDto> Packets { get; set; }
        public ProductsCashOutShippingsPacketsDetailsDto PacketDetail { get; set; }
    }
}
