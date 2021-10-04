using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Commands.UpdateProductStock
{
    public class UpdateProductStockCommandValidator:AbstractValidator<UpdateProductStockCommand>
    {
        public UpdateProductStockCommandValidator()
        {
            RuleFor(c => c.Model.Price).NotNull().WithMessage("Satış qiyməti boş qala bilməz.");
            RuleFor(c => c.Model.CostAmount).NotNull().WithMessage("Maya dəyəri boş qala bilməz.");
            RuleFor(c => c.Model.BuyAmount).NotNull().WithMessage("Alış qiyməti boş qala bilməz.");
            RuleFor(c => c.Model.ProductStockStatusId).NotNull().WithMessage("Status boş qala bilməz.");
            RuleFor(c => c.Model.Tax).NotNull().WithMessage("ƏDV boş qala bilməz.");
        }
    }
}
