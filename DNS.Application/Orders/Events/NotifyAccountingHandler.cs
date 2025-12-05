using DNS.Domain.Common.Events.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Orders.Events
{
    public class NotifyAccountingHandler
        : INotificationHandler<OrderPaidEvent>
    {
        public Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"OrderPaidEvent received. OrderId: {notification.OrderId}");
            return Task.CompletedTask;
        }
    }

}
