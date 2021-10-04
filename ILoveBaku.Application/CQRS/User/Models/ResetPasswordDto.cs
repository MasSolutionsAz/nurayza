using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Models
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
