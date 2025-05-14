using System;
using Arzand.Modules.Payment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arzand.Modules.Payment.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel>
{
    public void Configure(EntityTypeBuilder<PaymentModel> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Method).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
    }
}
