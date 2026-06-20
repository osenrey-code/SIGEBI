using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Identificacion).IsUnique();
            builder.Property(u => u.NombreCompleto).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Correo).IsRequired().HasMaxLength(100);

            builder.HasOne(u => u.PerfilLector)
                .WithOne()
                .HasForeignKey<PerfilLector>(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
