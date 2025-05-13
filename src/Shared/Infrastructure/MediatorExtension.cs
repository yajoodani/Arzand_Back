using System;
using Arzand.Shared.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Shared.Infrastructure;

public static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
    {
        await DispatchDomainEventsAsync<Guid>(mediator, ctx);
        await DispatchDomainEventsAsync<int>(mediator, ctx);
    }

    private static async Task DispatchDomainEventsAsync<T>(IMediator mediator, DbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity<T>>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
