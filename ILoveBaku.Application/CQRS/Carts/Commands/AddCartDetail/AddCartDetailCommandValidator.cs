using FluentValidation;

namespace ILoveBaku.Application.CQRS.Carts.Commands.AddCartDetail
{
    public class AddCartDetailCommandValidator : AbstractValidator<AddCartDetailCommand>
    {
        public AddCartDetailCommandValidator()
        {
            RuleFor(x => x.Model).Must(c => c.Count >= 1).WithMessage("Məhsulun sayını düzgün daxil edin.");
        }
    }
}
