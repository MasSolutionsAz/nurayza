using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets
{
    public class ProductCashOutShippingPacketDto:IMapFrom<ProductsCashOutShippingsPackets>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte ProductsCashOutShippingsPacketsStatusesId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
