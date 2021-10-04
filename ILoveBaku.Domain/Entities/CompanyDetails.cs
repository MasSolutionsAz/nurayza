using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CompanyDetails
    {
        public int Id { get; set; }
        public int CompaniesId { get; set; }
        public byte CompanyDetailsTypesId { get; set; }
        public string Name { get; set; }
        public string Sun { get; set; }
        public string ContactsName { get; set; }
        public string Contacts { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedIp { get; set; }
    }
}
