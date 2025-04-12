using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Modules.Catalog.Infrastructure.Data.EntityTypeConfigurations;
using Arzand.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Modules.Catalog.Infrastructure.Data;

public class CatalogDbContext : DbContext, IUnitOfWork
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BrandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
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
        await DispatchDomainEventsAsync();

        // Save changes to the database
        await SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task DispatchDomainEventsAsync()
    {
        // TODO: Dispatch domain events here
        // Placeholder for domain event logic
        await Task.CompletedTask;
    }
}
