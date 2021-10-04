using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.Common.Attributes
{
    public class MonthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is byte month)
            {
                if (month == 0)
                    return new ValidationResult("Zəhmət olmasa ayı seçin.");

                if (month > 12)
                    return new ValidationResult("Düzgün ay daxil edin.");

                return ValidationResult.Success;
            }

            return new ValidationResult("Xananı düzgün doldurun.");
        }
    }
}
