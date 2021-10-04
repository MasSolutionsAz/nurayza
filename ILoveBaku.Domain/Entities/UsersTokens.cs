using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersTokens
    {
        public Guid Id { get; set; }
        public virtual Users User { get; set; }
        public Guid UsersId { get; set; }
        public byte UsersTokensStatusesId { get; set; }
        public byte? UsersTokensTypesId { get; set; }
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
