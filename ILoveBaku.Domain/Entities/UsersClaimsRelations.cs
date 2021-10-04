using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersClaimsRelations
    {
        public Guid Id { get; set; }
        public virtual Users User { get; set; }
        public Guid UsersId { get; set; }
        public virtual UsersClaims UserClaim { get; set; }
        public int UsersClaimsId { get; set; }
    }
}
