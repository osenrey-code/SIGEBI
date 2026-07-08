using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
    {
        public void Configure(EntityTypeBuilder<Prestamo> builder)
        {
            builder.ToTable("Prestamos");

            builder.HasKey(p => p.PrestamoId);

            builder.Property(p => p.PrestamoId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.SolicitudId)
                .IsRequired();

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            builder.Property(p => p.EjemplarId)
                .IsRequired();

            builder.Property(p => p.FechaInicio)
                .IsRequired();

            builder.Property(p => p.FechaLimite)
                .IsRequired();

            builder.Property(p => p.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(p => p.Solicitud)
                .WithMany()
                .HasForeignKey(p => p.SolicitudId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.Prestamos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Ejemplar)
                .WithMany()
                .HasForeignKey(p => p.EjemplarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Devolucion)
                .WithOne(d => d.Prestamo)
                .HasForeignKey<Devolucion>(d => d.PrestamoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}