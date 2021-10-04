using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Users
    {
        public Users()
        {
            Tokens = new HashSet<UsersTokens>();
            Claims = new HashSet<UsersClaimsRelations>();
            Roles = new HashSet<UsersRolesRelations>();
        }
        public Guid Id { get; set; }
        public virtual Branches Branches { get; set; }
        public int BranchesId { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string DocumentNumber { get; set; }
        public string Pin { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public byte UsersStatusesId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? Birthday { get; set; }
        public int CreatedIp { get; set; }
        public bool? Gender { get; set; }

        public virtual ICollection<UsersTokens> Tokens { get; private set; }
        public virtual ICollection<UsersClaimsRelations> Claims { get; private set; }
        public virtual ICollection<UsersRolesRelations> Roles { get; private set; }
    }
}
