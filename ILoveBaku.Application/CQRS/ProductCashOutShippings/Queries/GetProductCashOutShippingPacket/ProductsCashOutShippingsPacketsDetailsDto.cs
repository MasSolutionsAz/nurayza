using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPacket
{
    public class ProductsCashOutShippingsPacketsDetailsDto:IMapFrom<ProductsCashOutShippingsPacketsDetails>
    {
        public int Id { get; set; }
        public int ProductsCashOutShippingsPacketsId { get; set; }
        public int ProductsCashOutsId { get; set; }
        public string TrackingNumber { get; set; }
        public byte ProductsCashOutShippingsPacketsDetailsStatuses { get; set; }
        public decimal Price { get; set; }
    }
}
