using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class RecursoConfiguration : IEntityTypeConfiguration<RecursoBibliografico>
    {
        public void Configure(EntityTypeBuilder<RecursoBibliografico> builder)
        {
            builder.ToTable("RecursosBibliograficos");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Identificador)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(r => r.Identificador)
                .IsUnique();

            builder.Property(r => r.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Autor)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.Categoria)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.NumeroEjemplares)
                .IsRequired();

            builder.Property(r => r.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}