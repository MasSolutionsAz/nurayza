using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator:AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(a => a.Model.Title)
                           .MaximumLength(100)
                           .WithMessage("Ad max 100 ola bilər")
                           .NotEmpty()
                           .WithMessage("Ad boş qala bilməz.");

            RuleFor(a => a.Model.Priority).NotEmpty().WithMessage("Öncəlik boş qala bilməz.");
        }
    }
}
