using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecification
{
    public class AddCategorySpecificationValidator:AbstractValidator<AddCategorySpecificationCommand>
    {
        public AddCategorySpecificationValidator()
        {
            RuleFor(c => c.Model.CategoriesSpecificationGroupId).NotEmpty().WithMessage("Qrup boş qala bilməz.");
            RuleFor(c => c.Model.CategoriesSpecificationsStatusesId).NotEmpty().WithMessage("Status boş qala bilməz.");
            RuleFor(c => c.Model.CategoriesSpecificationTypeId).NotEmpty().WithMessage("Type boş qala bilməz.");
            RuleFor(c => c.Model.Title).NotEmpty().WithMessage("Ad boş qala bilməz.");
        }
    }
}
