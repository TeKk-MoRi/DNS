using DNS.Domain.Entities.Orders;
using DNS.Domain.Enums.Orders;
using DNS.Domain.ValueObjects.Orders;

namespace DNS.Domain.Entities.Orders;

public class Order 
{
    private readonly List<OrderLine> _lines = new();

    public OrderId Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public ValueObjects.Orders.Address ShippingAddress { get; private set; }
    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    // Compute total dynamically
    public Money Total => _lines.Count == 0
        ? Money.Zero("USD")
        : _lines.Select(l => l.SubTotal)
                .Aggregate((a, b) => a + b);

    private Order() { } // For EF Core later

    private Order(OrderId id, CustomerId customerId, ValueObjects.Orders.Address shippingAddress)
    {
        Id = id;
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Draft;
    }

    public static Order CreateDraft(CustomerId customerId, ValueObjects.Orders.Address shippingAddress)
        => new Order(OrderId.New(), customerId, shippingAddress);

    // ------------------------------
    // BUSINESS BEHAVIOR (IMPORTANT)
    // ------------------------------

    public void AddLine(ProductId productId, string productName, int quantity, Money unitPrice)
    {
        EnsureDraft();

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        var existing = _lines.SingleOrDefault(x => x.ProductId == productId);

        if (existing is null)
        {
            _lines.Add(new OrderLine(
                lineNumber: _lines.Count + 1,
                productId,
                productName,
                quantity,
                unitPrice));
        }
        else
        {
            existing.IncreaseQuantity(quantity);
        }
    }

    public void RemoveLine(int lineNumber)
    {
        EnsureDraft();

        var line = _lines.SingleOrDefault(l => l.LineNumber == lineNumber)
                   ?? throw new InvalidOperationException("Line not found.");

        _lines.Remove(line);
    }

    public void Place()
    {
        EnsureDraft();

        if (!_lines.Any())
            throw new InvalidOperationException("Cannot place an empty order.");

        Status = OrderStatus.Placed;
        // Raise domain event OrderPlacedEvent here (later)
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Placed)
            throw new InvalidOperationException("Order must be Placed before Paid.");

        Status = OrderStatus.Paid;
        // Raise OrderPaidEvent
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid order.");

        Status = OrderStatus.Cancelled;
        // Raise OrderCancelledEvent
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Only Draft orders may be modified.");
    }
}
