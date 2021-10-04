using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersRolesRelations
    {
        //public virtual Users Users { get; set; }
        public Guid UsersId { get; set; }
        //public virtual UsersRoles UsersRoles { get; set; }
        public Guid UsersRolesId { get; set; }
    }
}
