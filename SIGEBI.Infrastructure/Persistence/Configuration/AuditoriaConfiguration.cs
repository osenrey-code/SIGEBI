using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {

            builder.ToTable("Auditoria");

            builder.HasKey(a => a.AuditoriaId);

            builder.Property(a => a.AuditoriaId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.UsuarioId)
                .IsRequired();

            builder.Property(a => a.EntidadAfectada)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Accion)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Detalle)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(a => a.FechaRegistro)
                .IsRequired();

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.UsuarioId);

            builder.HasIndex(a => a.EntidadAfectada);

            builder.HasIndex(a => a.Accion);

            builder.HasIndex(a => a.FechaRegistro);
        }
    }
}