using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Domain.ValueObjects.Orders
{
    public sealed record Address(
        string Country,
        string City,
        string PostalCode,
        string Street
    )
    {
        public override string ToString() => $"{Street}, {PostalCode} {City}, {Country}";
    }
}
