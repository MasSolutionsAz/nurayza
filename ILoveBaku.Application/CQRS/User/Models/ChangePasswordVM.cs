using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.User.Models
{
    public class ChangePasswordVM
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Parolu düzgün daxil edin."), DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(NewPassword), ErrorMessage = "Təkrar parolu düzgün daxil edin.")]
        public string ConfirmNewPassword { get; set; }
    }
}
