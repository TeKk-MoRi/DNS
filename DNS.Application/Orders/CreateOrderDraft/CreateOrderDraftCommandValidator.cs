using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Orders.CreateOrderDraft
{
    public class CreateOrderDraftCommandValidator
        : AbstractValidator<CreateOrderDraftCommand>
    {
        public CreateOrderDraftCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();

            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
        }
    }

}
