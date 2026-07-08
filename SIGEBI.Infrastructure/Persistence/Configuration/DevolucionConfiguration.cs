using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class DevolucionConfiguration : IEntityTypeConfiguration<Devolucion>
    {
        public void Configure(EntityTypeBuilder<Devolucion> builder)
        {
            builder.ToTable("Devoluciones");

            builder.HasKey(d => d.DevolucionId);

            builder.Property(d => d.DevolucionId)
                .ValueGeneratedOnAdd();
            builder.Property(d => d.BibliotecarioId)
                .IsRequired();
            builder.Property(d => d.FechaDevolucion)
                .IsRequired();

            builder.Property(d => d.Observacion)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(d => d.Prestamo)
                .WithOne(p => p.Devolucion)
                .HasForeignKey<Devolucion>(d => d.PrestamoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(d => d.BibliotecarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.PrestamoId)
                .IsUnique();

            builder.HasIndex(d => d.BibliotecarioId);
            builder.HasIndex(d => d.FechaDevolucion);
        }
    }
}
