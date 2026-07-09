using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class SolicitudConfiguration : IEntityTypeConfiguration<Solicitud>
    {
        public void Configure(EntityTypeBuilder<Solicitud> builder)
        {
            builder.ToTable("Solicitud");

            builder.HasKey(s => s.SolicitudId);

            builder.Property(s => s.SolicitudId)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.UsuarioId)
                .IsRequired();

            builder.Property(s => s.EjemplarId)
                .IsRequired();

            builder.Property(s => s.FechaSolicitud)
                .IsRequired();

            builder.Property(s => s.Estado)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(s => s.MotivoRechazo)
                .HasMaxLength(180);

            builder.HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Ejemplar)
                .WithMany()
                .HasForeignKey(s => s.EjemplarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.UsuarioId);

            builder.HasIndex(s => s.EjemplarId);

            builder.HasIndex(s => s.Estado);

            builder.HasIndex(s => s.FechaSolicitud);
        }
    }
}