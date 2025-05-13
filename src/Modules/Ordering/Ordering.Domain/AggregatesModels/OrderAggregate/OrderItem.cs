using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;

public class OrderItem : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid VariantId { get; private set; }
    public string ProductName { get; private set; }
    public string VariantName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public Money TotalPrice => new Money(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    public static OrderItem Create(Guid productId, Guid variantId, string productName, string variantName, Money unitPrice, int quantity)
    {
        return new OrderItem
        {
            ProductId = productId,
            VariantId = variantId,
            ProductName = productName,
            VariantName = variantName,
            UnitPrice = unitPrice,
            Quantity = quantity
        };
    }
}
