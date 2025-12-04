using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Users.Commands.Orders.RemoveOrderLine
{
    public record RemoveOrderLineCommand(
        Guid OrderId,
        int LineNumber
    ) : IRequest;
}
