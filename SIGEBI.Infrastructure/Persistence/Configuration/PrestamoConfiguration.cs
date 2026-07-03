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

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PerfilLectorId)
                .IsRequired();

            builder.Property(p => p.RecursoId)
                .IsRequired();

            builder.Property(p => p.FechaSolicitud)
                .IsRequired();

            builder.Property(p => p.FechaInicio)
                .IsRequired(false);

            builder.Property(p => p.FechaLimite)
                .IsRequired(false);

            builder.Property(p => p.FechaDevolucion)
                .IsRequired(false);

            builder.Property(p => p.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(p => p.MotivoRechazo)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.HasOne(p => p.PerfilLector)
                .WithMany(pl => pl.prestamos)
                .HasForeignKey(p => p.PerfilLectorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Recurso)
                .WithMany()
                .HasForeignKey(p => p.RecursoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}