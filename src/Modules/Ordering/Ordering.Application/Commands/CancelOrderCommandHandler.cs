using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using MediatR;

namespace Arzand.Modules.Ordering.Application.Commands;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.Id, cancellationToken);
        if (order is null || order.BuyerId != command.UserId)
        {
            throw new Exception("Order not found.");
        }

        order.Cancel();

        _orderRepository.Update(order);
        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
