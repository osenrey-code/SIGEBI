using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Identificacion)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Identificacion)
                .IsUnique();

            builder.Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(u => u.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(u => u.PerfilLector)
                .WithOne()
                .HasForeignKey<PerfilLector>(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}