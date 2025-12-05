using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Orders.PayOrder
{
    public class PayOrderCommandHandler
        : IRequestHandler<PayOrderCommand>
    {
        private readonly IApplicationUnitOfWork _uow;

        public PayOrderCommandHandler(IApplicationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(PayOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _uow.Orders.FindAsync(new OrderId(request.OrderId));

            if (order == null)
                throw new Exception("Order not found");

            order.MarkAsPaid();

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
