using DNS.Domain.Entities.Orders;
using DNS.Domain.Exceptions;
using DNS.Domain.ValueObjects.Orders;


namespace DNS.Domain.Entities.Orders;

public class OrderLine
{
    private OrderLine() { } // EF Core

    public OrderId OrderId { get; private set; }    // required for EF Core
    public int LineNumber { get; private set; }
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public Money SubTotal => UnitPrice * Quantity;

    internal OrderLine(
        OrderId orderId,
        int lineNumber,
        ProductId productId,
        string productName,
        int quantity,
        Money unitPrice)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name is required.");

        OrderId = orderId;
        LineNumber = lineNumber;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
    }

    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Increase amount must be positive.");

        Quantity += amount;
    }
}

