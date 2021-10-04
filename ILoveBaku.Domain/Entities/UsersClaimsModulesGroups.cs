using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersClaimsModulesGroups
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
