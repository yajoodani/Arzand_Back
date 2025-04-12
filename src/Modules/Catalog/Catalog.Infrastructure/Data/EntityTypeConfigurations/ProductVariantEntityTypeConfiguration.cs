using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arzand.Modules.Catalog.Infrastructure.Data.EntityTypeConfigurations;

public class ProductVariantEntityTypeConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.ProductId)
            .IsRequired();

        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.Variants)
            .HasForeignKey(pv => pv.ProductId);

        // Configuration for the Money value object
        builder.OwnsOne(pv => pv.Price, price =>
        {
            price.Property(p => p.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("Price_Currency");

            price.Property(p => p.Amount)
                .IsRequired()
                .HasColumnName("Price_Amount");
        });

        builder.OwnsMany(p => p.Attributes, a =>
        {
            a.WithOwner().HasForeignKey("ProductVariantId");
            a.Property<int>("Id"); // Shadow property as PK
            a.HasKey("Id");

            a.ToTable("ProductVariantAttributes");
            a.HasIndex(a => a.Name).IsUnique();

        });

        builder.Navigation(p => p.Attributes).Metadata.SetField("_attributes");
        builder.Navigation(p => p.Attributes).UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}
