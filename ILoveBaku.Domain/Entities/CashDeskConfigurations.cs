using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CashDeskConfigurations
    {
        public int Id { get; set; }
        public int CashDeskId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
