using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Application.Orders.CreateOrderDraft
{
    public record CreateOrderDraftCommand(
        Guid CustomerId,
        string Country,
        string City,
        string PostalCode,
        string Street
    ) : IRequest<Guid>;
}
