using DNS.Domain.Common.Events.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Orders.Events
{
    public class SendOrderConfirmationEmailHandler
       : INotificationHandler<OrderPlacedEvent>
    {
        public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"OrderPlacedEvent received. OrderId: {notification.OrderId}");

            // TODO: Email service or logging or SMS
            return Task.CompletedTask;
        }
    }
}
