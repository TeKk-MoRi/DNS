using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Domain.Entities.Orders
{
    public sealed record CustomerId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
