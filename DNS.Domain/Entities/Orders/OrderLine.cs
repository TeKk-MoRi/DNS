using DNS.Domain.Entities.Orders;
using DNS.Domain.ValueObjects.Orders;


namespace DNS.Domain.Entities.Orders;

public class OrderLine
{
    public int LineNumber { get; private set; }
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public Money SubTotal => UnitPrice * Quantity;

    internal OrderLine(
        int lineNumber,
        ProductId productId,
        string productName,
        int quantity,
        Money unitPrice)
    {
        LineNumber = lineNumber;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Quantity += amount;
    }
}
