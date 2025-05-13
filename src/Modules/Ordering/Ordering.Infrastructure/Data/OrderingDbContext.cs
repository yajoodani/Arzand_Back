using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Modules.Ordering.Infrastructure.Data.EntityTypeConfigurations;
using Arzand.Shared.Domain;
using Arzand.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Arzand.Modules.Ordering.Infrastructure.Data;

public class OrderingDbContext : DbContext, IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }

    private readonly IMediator _mediator;

    public OrderingDbContext(DbContextOptions<OrderingDbContext> options, IMediator mediator) : base(options) 
    { 
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }

    private void UpdateCreationAndModificationDates()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity<int> || e.Entity is Entity<Guid>)
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                // Handling CreationDate and LastModifiedDate for added entities
                if (entry.Entity is Entity<int> entityInt)
                {
                    entityInt.CreationDate = DateTime.UtcNow;
                    entityInt.LastModifiedDate = DateTime.UtcNow;
                }
                else if (entry.Entity is Entity<Guid> entityGuid)
                {
                    entityGuid.CreationDate = DateTime.UtcNow;
                    entityGuid.LastModifiedDate = DateTime.UtcNow;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                // Handling LastModifiedDate for modified entities
                if (entry.Entity is Entity<int> entityInt)
                {
                    entityInt.LastModifiedDate = DateTime.UtcNow;
                }
                else if (entry.Entity is Entity<Guid> entityGuid)
                {
                    entityGuid.LastModifiedDate = DateTime.UtcNow;
                }
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateCreationAndModificationDates();

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Added before SaveChangesAsync, so that only one save will be done.
        // If added after, eventual consistency must be handled.
        await _mediator.DispatchDomainEventsAsync(this);

        // Save changes to the database
        await SaveChangesAsync(cancellationToken);

        return true;
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }
}
