using ILoveBaku.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BranchId { get; set; }
        public string Roles { get; set; }
        public int AddressId { get; set; }
    }
}
