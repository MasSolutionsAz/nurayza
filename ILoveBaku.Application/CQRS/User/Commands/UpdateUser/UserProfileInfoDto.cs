using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.User.Commands.UpdateUser
{
    public class UserProfileInfoDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Day { get; set; }
        [Required]
        public string Month { get; set; }
        [Required]
        public string Year { get; set; }
        public bool? Gender { get; set; }
    }
}