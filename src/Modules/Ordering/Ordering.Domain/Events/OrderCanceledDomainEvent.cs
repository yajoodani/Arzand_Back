using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using MediatR;

namespace Arzand.Modules.Ordering.Domain.Events;

public class OrderCanceledDomainEvent : INotification
{
    public Order Order { get; }
    
    public OrderCanceledDomainEvent(Order order)
    {
        Order = order;
    }
}
