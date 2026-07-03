using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            builder.ToTable("Auditorias");

            builder.HasKey(a => a.IdAuditoria);

            builder.Property(a => a.UsuarioId)
                .IsRequired();

            builder.Property(a => a.EntidadAfectada)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Accion)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Detalle)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.FechaRegistro)
                .IsRequired();
        }
    }
}