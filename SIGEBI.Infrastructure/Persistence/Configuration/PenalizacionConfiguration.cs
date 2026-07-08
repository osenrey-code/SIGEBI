using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class PenalizacionConfiguration : IEntityTypeConfiguration<Penalizacion>
    {
        public void Configure(EntityTypeBuilder<Penalizacion> builder)
        {
            builder.ToTable("Penalizaciones");

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
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(p => p.Motivo)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.FechaGeneracion)
                .IsRequired();

            builder.Property(p => p.FechaResolucion);

            builder.Property(p => p.UsuarioResolucionId);

            builder.Property(p => p.MotivoResolucion)
                .HasMaxLength(300);

            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.penalizciones)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Prestamo)
                .WithMany()
                .HasForeignKey(p => p.PrestamoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}