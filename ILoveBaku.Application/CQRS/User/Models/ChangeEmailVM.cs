using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.User.Models
{
    public class ChangeEmailVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
