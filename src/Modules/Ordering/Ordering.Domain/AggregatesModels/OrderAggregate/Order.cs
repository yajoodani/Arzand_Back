using System;
using Arzand.Modules.Ordering.Domain.Events;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;

public class Order : Entity<Guid>, IAggregateRoot
{
    public Guid BuyerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public Address ShippingAddress { get; private set; }

    private List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Money TotalAmount => new Money(_items.Sum(item => item.TotalPrice.Amount), _items.FirstOrDefault()?.UnitPrice.Currency ?? "USD");

    private Order() {}

    public static Order Create(Guid buyerId, OrderStatus status, Address shippingAddress, List<OrderItem> items)
    {
        return new Order 
        { 
            BuyerId = buyerId,
            Status = status,
            ShippingAddress = shippingAddress,
            _items = items
        };
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Created && Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Only Created or Pending orders can be cancelled ");
        }

        Status = OrderStatus.Cancelled;

        AddDomainEvent(new OrderCanceledDomainEvent(this));
    }
 
    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be paid");

        Status = OrderStatus.Paid;
    }
}