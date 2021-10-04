using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersClaims
    {
        public int Id { get; set; }
        public byte UsersClaimsTypesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DisplayName { get; set; }
    }
}
