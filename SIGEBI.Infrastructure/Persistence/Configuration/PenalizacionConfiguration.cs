using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class PenalizacionConfiguration : IEntityTypeConfiguration<Penalizacion>
    {
        public void Configure(EntityTypeBuilder<Penalizacion> builder)
        {
            builder.ToTable("Penalizaciones");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PerfilLectorId)
                .IsRequired();

            builder.Property(p => p.DiasRetraso)
                .IsRequired();

            builder.Property(p => p.MontoMora)
                .IsRequired();

            builder.Property(p => p.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(p => p.FechaGeneracion)
                .IsRequired();

            builder.Property(p => p.FechaResolucion)
                .IsRequired(false);

            builder.Property(p => p.UsuarioResolucionId)
                .IsRequired(false);
        }
    }
}