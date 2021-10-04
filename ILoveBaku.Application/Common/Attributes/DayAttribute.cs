using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.Common.Attributes
{
    public class DayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is byte day)
            {
                if (day == 0)
                    return new ValidationResult("Zəhmət olmasa günü seçin.");

                if (day > 31)
                    return new ValidationResult("Düzgün gün daxil edin.");

                return ValidationResult.Success;
            }

            return new ValidationResult("Xananı düzgün doldurun.");
        }
    }
}
