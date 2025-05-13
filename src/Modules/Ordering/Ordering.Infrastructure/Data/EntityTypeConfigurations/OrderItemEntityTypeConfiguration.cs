using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arzand.Modules.Ordering.Infrastructure.Data.EntityTypeConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Ignore(p => p.DomainEvents);

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.ProductId)
            .IsRequired();

        builder.Property(oi => oi.VariantId)
            .IsRequired();

        builder.Property(oi => oi.ProductName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(oi => oi.VariantName)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(oi => oi.UnitPrice, up =>
        {
            up.Property(p => p.Amount)
                .HasColumnName("UnitPriceAmount")
                .IsRequired();

            up.Property(p => p.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(oi => oi.Quantity)
            .IsRequired();
    }
}
