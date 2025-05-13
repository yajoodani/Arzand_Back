using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arzand.Modules.Ordering.Infrastructure.Data.EntityTypeConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.Ignore(o => o.DomainEvents);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.BuyerId)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
            sa.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
            sa.Property(a => a.State).HasColumnName("State").HasMaxLength(100);
            sa.Property(a => a.Country).HasColumnName("Country").HasMaxLength(100);
            sa.Property(a => a.ZipCode).HasColumnName("ZipCode").HasMaxLength(20);
        });


        builder.HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
