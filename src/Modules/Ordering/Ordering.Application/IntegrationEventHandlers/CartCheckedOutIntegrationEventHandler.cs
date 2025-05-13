using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Shared.Contracts.IntegrationEvents;
using Arzand.Shared.Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace Arzand.Modules.Ordering.Application.IntegrationEventHandlers;

public class CartCheckedOutIntegrationEventHandler : ICapSubscribe
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CartCheckedOutIntegrationEventHandler> _logger;

    public CartCheckedOutIntegrationEventHandler(
        IOrderRepository orderRepository,
        ILogger<CartCheckedOutIntegrationEventHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [CapSubscribe("cart.checkout")]
    public async Task HandleAsync(CartCheckedOutIntegrationEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CartCheckedOutIntegrationEvent for UserId: {UserId}", @event.UserId);

        // TODO: Get delivery address from Identity/another source
        var deliveryAddress = new Address("Fetched", "from", "Identity module", "or", "default"); 

        var orderItems = @event.Items.Select(i =>
            OrderItem.Create(
                i.ProductId,
                i.VariantId,
                i.ProductName,
                i.VariantName,
                new Money (i.Price.Amount, i.Price.Currency),
                i.Quantity
            )).ToList();

        var order = Order.Create(
            @event.UserId,
            OrderStatus.Created,
            deliveryAddress,
            orderItems
        );

        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        _logger.LogInformation("Order created for UserId: {UserId}", @event.UserId);
    }
}
