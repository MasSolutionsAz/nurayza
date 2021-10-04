using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CashDeskSeance
    {
        public int Id { get; set; }
        public virtual CashDesk CashDesk { get; set; }
        public int CashDeskId { get; set; }
        public Guid UsersId { get; set; }
        public decimal StartAmount { get; set; }
        public decimal EndAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public byte CashDeskSeanceStatusesId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
