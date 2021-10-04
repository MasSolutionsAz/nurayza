using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersClaimsModules
    {
        public int Id { get; set; }
        public int UsersClaimsModulesGroupsId { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public byte Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
