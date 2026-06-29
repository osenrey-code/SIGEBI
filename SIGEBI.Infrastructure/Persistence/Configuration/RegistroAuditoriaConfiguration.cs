using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class RegistroAuditoriaConfiguration : IEntityTypeConfiguration<RegistroAuditoria>
    {
        public void Configure(EntityTypeBuilder<RegistroAuditoria> builder)
        {
            builder.ToTable("RegistroAuditorias");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.UsuarioId)
                .IsRequired(false);

            builder.Property(r => r.Usuario)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.Accion)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.EntidadAfectada)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.EntidadId)
                .IsRequired(false);

            builder.Property(r => r.Resultado)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Detalle)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(r => r.FechaRegistro)
                .IsRequired();

            builder.Property(r => r.ValoresAnteriores)
                .IsRequired(false);

            builder.Property(r => r.ValoresNuevos)
                .IsRequired(false);
        }
    }
}