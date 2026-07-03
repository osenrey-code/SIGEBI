using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class PerfilLectorConfiguration : IEntityTypeConfiguration<PerfilLector>
    {
        public void Configure(EntityTypeBuilder<PerfilLector> builder)
        {
            builder.ToTable("PerfilesLectores");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.UsuarioId)
                .IsRequired();

            builder.Property(p => p.LimitePrestamos)
                .IsRequired();

            builder.Property(p => p.DiasPrestamosPermitidos)
                .IsRequired();

            builder.HasMany(p => p.prestamos)
                .WithOne(p => p.PerfilLector)
                .HasForeignKey(p => p.PerfilLectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Penalizaciones)
                .WithOne(p => p.PerfilLector)
                .HasForeignKey(p => p.PerfilLectorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}