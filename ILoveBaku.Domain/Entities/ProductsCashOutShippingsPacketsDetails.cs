using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutShippingsPacketsDetails
    {
        public int Id { get; set; }
        public int ProductsCashOutShippingsPacketsId { get; set; }
        public int ProductsCashOutsId { get; set; }
        public Guid TrackingNumber { get; set; }
        public decimal Price { get; set; }
        public string WantedShipmentDate { get; set; }
        public byte ProductsCashOutShippingsPacketsDetailsStatuses { get; set; }
    }
}
