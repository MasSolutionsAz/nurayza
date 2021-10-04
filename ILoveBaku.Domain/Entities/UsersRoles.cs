using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersRoles
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UsersRolesGroupsId { get; set; }
    }
}
