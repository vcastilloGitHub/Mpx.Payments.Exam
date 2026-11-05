using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Data.Configurations;

public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
{
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("Cuentas");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Saldo).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(c => c.Moneda).HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(c => c.RowVersion).IsRowVersion();

        builder.HasOne(c => c.Cliente)
               .WithMany(cl => cl.Cuentas)
               .HasForeignKey(c => c.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
