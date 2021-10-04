using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationGroup
{
    public class UpdateCategorySpecificationGroupValidator:AbstractValidator<UpdateCategorySpecificationGroupCommand>
    {
        public UpdateCategorySpecificationGroupValidator()
        {
            RuleFor(c => c.Model.IsActive).NotEmpty().WithMessage("Status boş qala bilməz.");
            RuleFor(c => c.Model.Title).NotEmpty().WithMessage("Ad boş qala bilməz.");
        }
    }
}
