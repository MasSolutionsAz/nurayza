using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(r => r.Model.Name).NotEmpty().WithMessage("Ad boş qala bilməz.");
            RuleFor(r => r.Model.Surname).NotEmpty().WithMessage("Soyad boş qala bilməz.");
            RuleFor(r => r.Model.RegisterEmail).NotEmpty().WithMessage("Email boş qala bilməz.");
            RuleFor(r => r.Model.RegisterPassword).NotEmpty().WithMessage("Şifrə boş qala bilməz.");
            //RuleFor(r => r.Model.BirthDate.Day).NotEmpty().WithMessage("Gün boş qala bilməz.");
            //RuleFor(r => r.Model.BirthDate.Month).NotEmpty().WithMessage("Ay boş qala bilməz.");
            //RuleFor(r => r.Model.BirthDate.Year).NotEmpty().WithMessage("İl boş qala bilməz.");
        }
    }
}
