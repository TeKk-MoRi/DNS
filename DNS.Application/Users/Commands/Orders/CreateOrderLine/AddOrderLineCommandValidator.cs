using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.CreateOrderLine
{
    public class AddOrderLineCommandValidator
        : AbstractValidator<AddOrderLineCommand>
    {
        public AddOrderLineCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be at least 1.");
            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Price must be positive.");
            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .WithMessage("Currency must be 3 letters, e.g. USD");
        }
    }

}
