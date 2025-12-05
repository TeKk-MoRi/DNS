using DNS.Application.Common.Exceptions;
using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Orders.RemoveOrderLine
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
                throw new NotFoundException(nameof(Order), request.OrderId);

            order.RemoveLine(request.LineNumber);

            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
