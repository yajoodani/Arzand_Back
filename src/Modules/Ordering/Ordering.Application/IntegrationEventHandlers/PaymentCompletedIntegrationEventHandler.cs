using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Shared.Contracts.IntegrationEvents;
using DotNetCore.CAP;

namespace Arzand.Modules.Ordering.Application.IntegrationEventHandlers;

public class PaymentCompletedIntegrationEventHandler : ICapSubscribe
{
    private readonly IOrderRepository _orderRepository;

    public PaymentCompletedIntegrationEventHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [CapSubscribe("payment.payment_completed")]
    public async Task HandleAsync(PaymentCompletedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(@event.OrderId);
        if (order is null)
        {
            throw new Exception("Order does not exist.");
        }

        order.MarkAsPaid();
        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
