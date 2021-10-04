using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses
{
    public class UserAddressInfoDto
    {
        public int AddressId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Address { get; set; }
        public string Phone { get; set; }
        [Required]
        public int RegionId { get; set; }
        public int CountryId { get; set; }
    }
}
