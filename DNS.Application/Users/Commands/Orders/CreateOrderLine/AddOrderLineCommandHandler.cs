using DNS.Domain.Entities.Orders;
using DNS.Domain.ValueObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.CreateOrderLine
{
    public class AddOrderLineCommandHandler
        : IRequestHandler<AddOrderLineCommand>
    {
        private readonly IApplicationUnitOfWork _uow;

        public AddOrderLineCommandHandler(IApplicationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(AddOrderLineCommand request, CancellationToken cancellationToken)
        {
            var order = await _uow.Orders
                .Include(o => o.Lines)
                .FirstOrDefaultAsync(o => o.Id == new OrderId(request.OrderId), cancellationToken);

            if (order == null)
                throw new Exception("Order not found");

            order.AddLine(
                new ProductId(request.ProductId),
                request.ProductName,
                request.Quantity,
                new Money(request.UnitPrice, request.Currency));

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
