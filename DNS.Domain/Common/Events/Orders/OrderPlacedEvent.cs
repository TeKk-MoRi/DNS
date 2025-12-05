using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Domain.Common.Events.Orders
{
    public class OrderPlacedEvent : DomainEventBase
    {
        public OrderId OrderId { get; }

        public OrderPlacedEvent(OrderId orderId)
        {
            OrderId = orderId;
        }
    }
}
