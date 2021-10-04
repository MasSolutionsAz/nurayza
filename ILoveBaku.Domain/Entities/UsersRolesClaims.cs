using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersRolesClaims
    {
        public Guid Id { get; set; }
        public Guid UsersRolesId { get; set; }
        public int ClaimsId { get; set; }
    }
}
