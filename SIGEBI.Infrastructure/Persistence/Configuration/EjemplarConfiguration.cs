using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class EjemplarConfiguration : IEntityTypeConfiguration<Ejemplar>
    {
        public void Configure(EntityTypeBuilder<Ejemplar> builder)
        {
            builder.ToTable("Ejemplares");

            builder.HasKey(e => e.EjemplarId);

            builder.Property(e => e.Identificador)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => new { e.RecursoBibliograficoId, e.Identificador })
                .IsUnique();

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(e => e.Observacion)
                .HasMaxLength(300);

            builder.HasOne(e => e.RecursoBibliografico)
                .WithMany(r => r.Ejemplares)
                .HasForeignKey(e => e.RecursoBibliograficoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}