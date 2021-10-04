using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ProductsCashOutShippingsPackets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte DeliveryCompaniesId { get; set; }
        public string ResponsablePerson { get; set; }
        public byte ProductsCashOutShippingsPacketsStatusesId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
