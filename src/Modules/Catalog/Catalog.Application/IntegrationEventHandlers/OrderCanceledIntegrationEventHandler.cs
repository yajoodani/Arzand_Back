using System;
using Arzand.Shared.Contracts;
using Arzand.Shared.Contracts.IntegrationEvents;
using DotNetCore.CAP;

namespace Arzand.Modules.Catalog.Application.IntegrationEventHandlers;

public class OrderCanceledIntegrationEventHandler : ICapSubscribe
{
    private readonly ICatalogService _catalogService;

    public OrderCanceledIntegrationEventHandler(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [CapSubscribe("ordering.order_canceled")]
    public async Task Handle(OrderCanceledIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var reserveStockDtos = @event.Items.Select(i => new ReserveStockDto{
            ProductId = i.ProductId,
            VariantId = i.VariantId,
            Quantity = -i.Quantity,
        }).ToList();
        // Releasing reserved stock
        await _catalogService.HandleReserveStockAsync(reserveStockDtos, cancellationToken);
    }
}
