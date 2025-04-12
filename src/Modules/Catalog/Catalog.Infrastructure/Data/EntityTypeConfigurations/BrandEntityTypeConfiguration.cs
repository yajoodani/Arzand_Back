using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arzand.Modules.Catalog.Infrastructure.Data.EntityTypeConfigurations;

public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}
