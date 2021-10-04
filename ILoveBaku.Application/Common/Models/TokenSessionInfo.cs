using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.Common.Models
{
    public class TokenSessionInfo
    {
        public Guid UserId { get; set; }
        public List<UsersClaimsRelations> Claims { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
