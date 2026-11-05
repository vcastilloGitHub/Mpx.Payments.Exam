using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Documento).HasMaxLength(20).IsRequired();
        builder.Property(c => c.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasIndex(c => c.Documento).IsUnique();
    }
}
