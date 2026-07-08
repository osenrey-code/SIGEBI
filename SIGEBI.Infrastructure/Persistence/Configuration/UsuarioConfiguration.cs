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

            builder.HasKey(u => u.UsuarioId);

            builder.Property(u => u.UsuarioId)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Correo)
                .IsUnique();

            builder.Property(u => u.PassWord)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(u => u.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasDiscriminator<string>("TipoUsuario")
                .HasValue<Estudiante>("Estudiante")
                .HasValue<Docente>("Docente")
                .HasValue<Bibliotecario>("Bibliotecario")
                .HasValue<Administrador>("Administrador")
                .HasValue<Auditor>("Auditor");

            builder.HasMany(u => u.Prestamos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.penalizciones)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.notificaciones)
                .WithOne(n => n.Usuario)
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}