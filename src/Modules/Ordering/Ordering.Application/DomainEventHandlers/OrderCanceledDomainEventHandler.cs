using System;
using Arzand.Modules.Ordering.Domain.Events;
using Arzand.Shared.Contracts.IntegrationEvents;
using DotNetCore.CAP;
using MediatR;

namespace Arzand.Modules.Ordering.Application.DomainEventHandlers;

public class OrderCanceledDomainEventHandler : INotificationHandler<OrderCanceledDomainEvent>
{
    private readonly ICapPublisher _capPublisher;

    public OrderCanceledDomainEventHandler(ICapPublisher capPublisher)
    {
        _capPublisher = capPublisher;
    }

    public async Task Handle(OrderCanceledDomainEvent @event, CancellationToken cancellationToken)
    {
        var order = @event.Order;
        var integrationEvent = new OrderCanceledIntegrationEvent
        {
            UserId = order.BuyerId,
            Items = order.Items.Select(i => new OrderCanceledIntegrationEvent.CartItemDto{
                ProductId = i.ProductId,
                VariantId = i.VariantId,
                Quantity = i.Quantity,
            }).ToList(),
            CanceledAt = DateTime.UtcNow,
        };

        await _capPublisher.PublishAsync(name: "ordering.order_canceled", contentObj: integrationEvent, cancellationToken: cancellationToken);
    }
}
