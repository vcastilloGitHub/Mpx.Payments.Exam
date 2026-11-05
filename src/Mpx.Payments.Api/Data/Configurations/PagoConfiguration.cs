using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Data.Configurations;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pagos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Monto).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.Referencia).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Proveedor).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.FechaCreacion).HasColumnType("datetime2(3)").IsRequired();
        builder.Property(p => p.FechaAplicacion).HasColumnType("datetime2(3)");
        builder.Property(p => p.FechaReversa).HasColumnType("datetime2(3)");
        builder.Property(p => p.IdempotencyKey).HasMaxLength(100).IsRequired();

        builder.HasIndex(p => p.IdempotencyKey).IsUnique();
        builder.HasIndex(p => new { p.CuentaId, p.FechaCreacion });

        builder.HasOne(p => p.Cuenta)
               .WithMany(c => c.Pagos)
               .HasForeignKey(p => p.CuentaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
