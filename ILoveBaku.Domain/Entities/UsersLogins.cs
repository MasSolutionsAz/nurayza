using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersLogins
    {
        public Guid Id { get; set; }
        public virtual Users User { get; set; }
        public Guid UsersId { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public DateTimeOffset LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? CreatedIp { get; set; }
    }
}
