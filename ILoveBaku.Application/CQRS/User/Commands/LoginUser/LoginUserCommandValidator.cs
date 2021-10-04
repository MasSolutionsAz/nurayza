using FluentValidation;

namespace ILoveBaku.Application.CQRS.User.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(r => r.Model.Email).NotEmpty().WithMessage("Email boş qala bilməz.");
            RuleFor(r => r.Model.Password).NotEmpty().WithMessage("Şifrə boş qala bilməz.");
        }
    }
}
