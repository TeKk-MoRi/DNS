using DNS.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Domain.Common.Events.Orders
{
    public class OrderPaidEvent : DomainEventBase
    {
        public OrderId OrderId { get; }

        public OrderPaidEvent(OrderId orderId)
        {
            OrderId = orderId;
        }
    }

}
