using DNS.Domain.Entities.Orders;
using DNS.Domain.ValueObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.CreateOrderDraft
{
    public class CreateOrderDraftCommandHandler
        : IRequestHandler<CreateOrderDraftCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _uow;

        public CreateOrderDraftCommandHandler(IApplicationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Country,
                request.City,
                request.PostalCode,
                request.Street);

            var order = Order.CreateDraft(
                new CustomerId(request.CustomerId),
                address);

            _uow.Orders.Add(order);

            await _uow.SaveChangesAsync(cancellationToken);

            return order.Id.Value;
        }
    }
}
