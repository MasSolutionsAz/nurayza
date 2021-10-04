using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersCards
    {
        public int Id { get; set; }
        public byte UserCardsTypesId { get; set; }
        public virtual Users Users { get; set; }
        public Guid UsersId { get; set; }
        public long CardNumber { get; set; }
        public byte UserCardsStatusesId { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
