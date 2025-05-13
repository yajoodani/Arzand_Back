using System;
using Arzand.Shared.Application;

namespace Arzand.Shared.Contracts.IntegrationEvents;

public class CartCheckedOutIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public MoneyDto TotalPrice { get; set; }
    public DateTime CheckedOutAt { get; set; }

    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public Guid VariantId { get; set; }
        public string ProductName { get; set; } = default!;
        public string VariantName { get; set; } = default!;
        public int Quantity { get; set; }
        public MoneyDto Price { get; set; }
    }
}