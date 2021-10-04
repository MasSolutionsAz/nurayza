using ILoveBaku.Application.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Commands.RegisterUser
{
    public class RegisterVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string RegisterEmail { get; set; }

        [Required, DataType(DataType.Password)]
        public string RegisterPassword { get; set; }

        [Required, DataType(DataType.Password), Compare("RegisterPassword", ErrorMessage = "Təkrar şifrəni düzgün daxil edin.")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        //public BirthDate BirthDate { get; set; }
    }

    public class BirthDate
    {
        [Range(1, 31, ErrorMessage = "Zəhmət olmasa günü seçin.")]
        public byte Day { get; set; }

        [Range(1, 12, ErrorMessage = "Zəhmət olmasa ayı seçin.")]
        public byte Month { get; set; }

        [Range(1950, 2010, ErrorMessage = "Zəhmət olmasa ayı seçin.")]
        public int Year { get; set; }
    }
}
