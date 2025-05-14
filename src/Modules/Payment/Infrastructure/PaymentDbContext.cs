using System;
using Arzand.Modules.Payment.Domain;
using Arzand.Modules.Payment.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Modules.Payment.Infrastructure;

public class PaymentDbContext : DbContext
{
    public DbSet<PaymentModel> Payments => Set<PaymentModel>();

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
}