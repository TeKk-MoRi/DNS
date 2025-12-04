using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Domain.Enums.Orders
{
    public enum OrderStatus
    {
        Draft = 1,
        Placed = 2,
        Paid = 3,
        Cancelled = 4
    }
}
