using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.RemoveOrderLine
{
    public class RemoveOrderLineCommandHandler
        : IRequestHandler<RemoveOrderLineCommand>
    {
        private readonly IApplicationUnitOfWork _uow;

        public RemoveOrderLineCommandHandler(IApplicationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(RemoveOrderLineCommand request, CancellationToken cancellationToken)
        {
            var order = await _uow.Orders
                .Include(o => o.Lines)
                .FirstOrDefaultAsync(o => o.Id == new OrderId(request.OrderId), cancellationToken);

            if (order == null)
                throw new Exception("Order not found");

            order.RemoveLine(request.LineNumber);

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
