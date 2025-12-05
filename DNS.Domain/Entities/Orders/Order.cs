using DNS.Domain.Common.Events.Orders;
using DNS.Domain.Entities.Orders;
using DNS.Domain.Enums.Orders;
using DNS.Domain.Exceptions;
using DNS.Domain.ValueObjects.Orders;
using Address = DNS.Domain.ValueObjects.Orders.Address;

namespace DNS.Domain.Entities.Orders;

public class Order : Entity
{
    // -----------------------------
    // EF Core requirements
    // -----------------------------
    private Order() { }  // For EF Core

    // Backing field for OrderLines
    private readonly List<OrderLine> _lines = new();

    // -----------------------------
    // Aggregate Root Properties
    // -----------------------------
    public OrderId Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Address Address { get; private set; }
    public OrderStatus Status { get; private set; }

    // Read-only access for outside world
    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    // Derived property (not mapped)
    public Money Total =>
        _lines.Count == 0
            ? Money.Zero("USD")
            : _lines.Select(l => l.SubTotal)
                    .Aggregate((a, b) => a + b);

    // -----------------------------
    // Constructor used by static factory
    // -----------------------------
    private Order(
        OrderId id,
        CustomerId customerId,
        Address address)
    {
        Id = id;
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Status = OrderStatus.Draft;
    }

    // -----------------------------
    // Factory Method (Always preferred for aggregates)
    // -----------------------------
    public static Order CreateDraft(CustomerId customerId, Address address)
    {
        var newOrderId = new OrderId(Guid.NewGuid());

        return new Order(newOrderId, customerId, address);
    }

    // -----------------------------
    // ADD LINE
    // -----------------------------
    public void AddLine(
        ProductId productId,
        string productName,
        int quantity,
        Money unitPrice)
    {
        if (Status != OrderStatus.Draft)
            throw new DomainException("Cannot modify order lines after placing the order.");

        var existing = _lines.FirstOrDefault(l => l.ProductId == productId);

        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
            return;
        }

        var lineNumber = _lines.Count + 1;

        var orderLine = new OrderLine(
            Id,
            lineNumber,
            productId,
            productName,
            quantity,
            unitPrice);

        _lines.Add(orderLine);
    }

    // -----------------------------
    // REMOVE LINE
    // -----------------------------
    public void RemoveLine(int lineNumber)
    {
        if (Status != OrderStatus.Draft)
            throw new DomainException("Cannot modify order lines after placing the order.");

        var line = _lines.FirstOrDefault(l => l.LineNumber == lineNumber);
        if (line is null)
            throw new DomainException($"Order line {lineNumber} does not exist.");

        _lines.Remove(line);
    }

    // -----------------------------
    // PLACE ORDER
    // -----------------------------
    public void Place()
    {
        if (Status != OrderStatus.Draft)
            throw new DomainException("Only draft orders can be placed.");

        if (_lines.Count == 0)
            throw new DomainException("Cannot place an empty order.");

        Status = OrderStatus.Placed;

        AddDomainEvent(new OrderPlacedEvent(Id));
    }

    // -----------------------------
    // MARK AS PAID
    // -----------------------------
    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Placed)
            throw new DomainException("Only placed orders can be paid.");

        Status = OrderStatus.Paid;

        AddDomainEvent(new OrderPaidEvent(Id));
    }

    // -----------------------------
    // CANCEL ORDER
    // -----------------------------
    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
            throw new DomainException("Paid orders cannot be canceled.");

        Status = OrderStatus.Cancelled;
    }
}
