using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
    {
        public void Configure(EntityTypeBuilder<Notificacion> builder)
        {
            builder.ToTable("Notificaciones");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.UsuarioDestinatarioId)
                .IsRequired(false);

            builder.Property(n => n.CorreoDestinatario)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(n => n.TipoEvento)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(n => n.Mensaje)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.FechaRegistro)
                .IsRequired();

            builder.Property(n => n.EstadoEnvio)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}