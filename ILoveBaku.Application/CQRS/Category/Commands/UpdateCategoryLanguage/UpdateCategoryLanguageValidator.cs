using FluentValidation;
using ILoveBaku.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage
{
    public class UpdateCategoryLanguageValidator:AbstractValidator<UpdateCategoryLanguageCommand>
    {
        public UpdateCategoryLanguageValidator()
        {
            RuleFor(c => c.Model.Name).NotEmpty().WithMessage("Ad boş qala bilməz.");
        }
    }
}
