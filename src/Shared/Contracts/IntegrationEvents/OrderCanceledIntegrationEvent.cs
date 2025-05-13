using System;
using Arzand.Shared.Application;

namespace Arzand.Shared.Contracts.IntegrationEvents;

public class OrderCanceledIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public DateTime CanceledAt { get; set; }

    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
    }
}
