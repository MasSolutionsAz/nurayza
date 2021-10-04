using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersExternalLogins
    {
        public Guid Id { get; set; }
        public virtual Users Users { get; set; }
        public Guid UsersId { get; set; }
        public byte UsersExternalLoginsProvidersId { get; set; }
        public string ProviderKey { get; set; }
    }
}
