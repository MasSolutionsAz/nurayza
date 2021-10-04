using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Models
{
    public class UserListVm
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
