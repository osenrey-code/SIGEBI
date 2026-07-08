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

            builder.HasKey(n => n.NotificacionId);

            builder.Property(n => n.NotificacionId)
                .ValueGeneratedOnAdd();

            builder.Property(n => n.UsuarioId)
                .IsRequired();

            builder.Property(n => n.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(n => n.Mensaje)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.FechaRegistro)
                .IsRequired();

            builder.HasOne(n => n.Usuario)
                .WithMany()
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}