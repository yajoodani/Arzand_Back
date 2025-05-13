using System;

namespace Arzand.Shared.Application;

public abstract class IntegrationEvent
{
    public Guid Id { get; private set; }
    public DateTime OccurredOn { get; private set; }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}
