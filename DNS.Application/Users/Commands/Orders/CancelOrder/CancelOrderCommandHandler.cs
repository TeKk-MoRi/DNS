using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.CancelOrder
{
    public class CancelOrderCommandHandler
        : IRequestHandler<CancelOrderCommand>
    {
        private readonly IApplicationUnitOfWork _uow;

        public CancelOrderCommandHandler(IApplicationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _uow.Orders.FindAsync(new OrderId(request.OrderId));

            if (order == null)
                throw new NotFoundException(nameof(Order), request.OrderId);

            order.Cancel();

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
