using ILoveBaku.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUser
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FatherName { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public bool? Gender { get; set; }
        public Guid UserId { get; set; }
        public DateTime RegisterDate { get; set; }

        public List<string> Addresses { get; set; }

        public byte UserStatusId { get; set; }
        public string GenderName { get; set; }
        public string Birthday { get; set; }
        public string Provider { get; set; }
        public UserDto()
        {
            Addresses = new List<string>();
        }
    }
}