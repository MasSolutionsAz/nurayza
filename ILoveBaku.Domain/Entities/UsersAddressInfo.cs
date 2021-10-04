using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class UsersAddressInfo
    {
        public int Id { get; set; }
        public virtual Users Users { get; set; }
        public Guid UsersId { get; set; }
        public virtual Regions Regions { get; set; }
        public int RegionsId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public bool IsActive { get; set; }
    }
}
