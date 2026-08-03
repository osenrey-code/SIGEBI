using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
    {
        public void Configure(EntityTypeBuilder<Notificacion> builder)
        {
            builder.ToTable("Notificacion");

            builder.HasKey(n => n.NotificacionId);

            builder.Property(n => n.NotificacionId)
                .ValueGeneratedOnAdd();

            builder.Property(n => n.UsuarioId)
                .IsRequired();

            builder.Property(n => n.Tipo)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(n => n.Mensaje)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(n => n.FechaRegistro)
                .IsRequired();

            builder.Property(n => n.Leida)
                .IsRequired();

            builder.HasOne(n => n.Usuario)
                .WithMany(u => u.Notificaciones)
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(n => n.UsuarioId);

            builder.HasIndex(n => n.Tipo);

            builder.HasIndex(n => n.Leida);

            builder.HasIndex(n => n.FechaRegistro);
        }
    }
}