using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup
{
    public class AddProductGroupValidator:AbstractValidator<AddProductGroupCommand>
    {
        public AddProductGroupValidator()
        {
            RuleFor(c => c.Model.Name).NotNull().WithMessage("Ad boş qala bilməz");
            RuleFor(c => c.Model.CategoryId).NotNull().WithMessage("Kateqoriya seçilməyib");
        }
    }
}
