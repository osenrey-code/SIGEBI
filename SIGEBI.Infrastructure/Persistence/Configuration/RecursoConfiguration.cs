using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class RecursoConfiguration : IEntityTypeConfiguration<RecursoBibliografico>
    {
        public void Configure(EntityTypeBuilder<RecursoBibliografico> builder)
        {
            builder.ToTable("RecursoBibliografico");

            builder.HasKey(r => r.RecursoBibliograficoId);

            builder.Property(r => r.RecursoBibliograficoId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(r => r.ISBN)
                .IsUnique();

            builder.Property(r => r.Titulo)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.Autor)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.AnioPublicado)
                .IsRequired();

            builder.Property(r => r.ImagenUrl)
                .HasMaxLength(200);

            builder.Property(r => r.CategoriaId)
                .IsRequired();

            builder.Ignore(r => r.TotalEjemplares);
            builder.Ignore(r => r.CopiasDisponibles);

            builder.Property(r => r.Activo)
                .IsRequired()
                .HasDefaultValue(true);
            builder.HasQueryFilter(r => r.Activo);

            builder.Property(r => r.Descripcion)
                .HasMaxLength(500);

            builder.HasOne(r => r.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(r => r.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Ejemplares)
                .WithOne(e => e.RecursoBibliografico)
                .HasForeignKey(e => e.RecursoBibliograficoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(r => r.Ejemplares)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}