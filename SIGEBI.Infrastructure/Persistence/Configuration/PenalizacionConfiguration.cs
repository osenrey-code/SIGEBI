using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class PenalizacionConfiguration : IEntityTypeConfiguration<Penalizacion>
    {
        public void Configure(EntityTypeBuilder<Penalizacion> builder)
        {
            builder.ToTable("Penalizacion");

            builder.HasKey(p => p.PenalizacionId);

            builder.Property(p => p.PenalizacionId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            builder.Property(p => p.PrestamoId)
                .IsRequired();

            builder.Property(p => p.DiasRetraso)
                .IsRequired();

            builder.Property(p => p.MontoMora)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(p => p.Motivo)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Estado)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(p => p.FechaGeneracion)
                .IsRequired();

            builder.Property(p => p.FechaResolucion)
                .IsRequired(false);

            builder.Property(p => p.UsuarioResolucionId)
                .IsRequired(false);

            builder.Property(p => p.MotivoResolucion)
                .HasMaxLength(500);

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Prestamo)
                .WithMany()
                .HasForeignKey(p => p.PrestamoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(p => p.UsuarioResolucionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.UsuarioId);

            builder.HasIndex(p => p.PrestamoId);

            builder.HasIndex(p => p.Estado);

            builder.HasIndex(p => p.FechaGeneracion);
        }
    }
}