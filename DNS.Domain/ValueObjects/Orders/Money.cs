using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Domain.ValueObjects.Orders
{
    public sealed record Money(decimal Amount, string Currency)
    {
        public static Money Zero(string currency) => new(0m, currency);

        public static Money operator +(Money left, Money right)
        {
            if (!string.Equals(left.Currency, right.Currency, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Cannot add money with different currencies.");

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator *(Money money, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            return new Money(money.Amount * quantity, money.Currency);
        }

        public override string ToString() => $"{Amount:0.00} {Currency}";
    }
}
