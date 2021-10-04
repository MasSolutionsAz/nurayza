using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersCardsValues
    {
        public int Id { get; set; }
        public int UsersCardsId { get; set; }
        public byte UsersCardsValuesTypesId { get; set; }
        public decimal Value { get; set; }
    }
}
