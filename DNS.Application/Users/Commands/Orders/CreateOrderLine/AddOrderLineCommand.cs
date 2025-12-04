using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.CreateOrderLine
{
    public record AddOrderLineCommand(
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        string Currency
    ) : IRequest;
}
