using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.User.Commands.LogoutUser
{
    public class LogoutUserCommandValidator:AbstractValidator<LogoutUserCommand>
    {
        public LogoutUserCommandValidator()
        {
            RuleFor(r => r.Token).NotEmpty().WithMessage("Token göndərilməyib");
        }
    }
}
