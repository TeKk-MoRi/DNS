using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.PayOrder
{
    public record PayOrderCommand(Guid OrderId) : IRequest;
}
