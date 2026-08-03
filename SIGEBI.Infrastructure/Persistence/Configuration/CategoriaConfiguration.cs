using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categoria");

            builder.HasKey(c => c.CategoriaId);

            builder.Property(c => c.CategoriaId)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Nombre)
                .IsUnique();

            builder.Property(c => c.Descripcion)
                .IsRequired()
                .HasMaxLength(180);

            builder.HasMany(c => c.Libros)
                .WithOne(r => r.Categoria)
                .HasForeignKey(r => r.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}