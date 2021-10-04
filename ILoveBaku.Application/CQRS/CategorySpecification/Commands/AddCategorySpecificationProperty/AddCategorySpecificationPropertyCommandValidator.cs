using FluentValidation;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationProperty
{
    public class AddCategorySpecificationPropertyCommandValidator:AbstractValidator<AddCategorySpecificationPropertyCommand>
    {
        public AddCategorySpecificationPropertyCommandValidator()
        {
            RuleFor(c => c.Model.Title).NotEmpty().WithMessage("Ad boş qala bilməz");
            RuleFor(c => c.Model.ParentId).NotEmpty().WithMessage("ParentId boş qala bilməz");
        }
    }
}
