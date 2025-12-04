using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.PlaceOrder
{
    public class PlaceOrderCommandHandler
        : IRequestHandler<PlaceOrderCommand>
    {
        private readonly IApplicationUnitOfWork _uow;

        public PlaceOrderCommandHandler(IApplicationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _uow.Orders.FindAsync(new OrderId(request.OrderId));

            if (order == null)
                throw new Exception("Order not found");

            order.Place();

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
