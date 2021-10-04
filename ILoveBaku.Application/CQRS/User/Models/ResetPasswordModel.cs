using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Yeni şifrə boş qala bilməz.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Təkrar şifrə boş qala bilməz.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
