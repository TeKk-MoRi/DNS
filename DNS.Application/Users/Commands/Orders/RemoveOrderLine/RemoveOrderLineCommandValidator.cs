using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.RemoveOrderLine
{
    public class RemoveOrderLineCommandValidator
        : AbstractValidator<RemoveOrderLineCommand>
    {
        public RemoveOrderLineCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.LineNumber).GreaterThan(0);
        }
    }

}
