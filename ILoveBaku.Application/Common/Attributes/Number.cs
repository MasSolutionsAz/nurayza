using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.Common.Attributes
{
    public class Number : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            byte data;
            bool result= byte.TryParse(value.ToString(), out data);

            if (result)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
            
        }
    }
}
