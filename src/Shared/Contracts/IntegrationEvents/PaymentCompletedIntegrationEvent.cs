using System;
using Arzand.Shared.Application;

namespace Arzand.Shared.Contracts.IntegrationEvents;

public class PaymentCompletedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}
