using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class EjemplarConfiguration : IEntityTypeConfiguration<Ejemplar>
    {
        public void Configure(EntityTypeBuilder<Ejemplar> builder)
        {
            builder.ToTable("Ejemplar");

            builder.HasKey(e => e.EjemplarId);

            builder.Property(e => e.EjemplarId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Identificador)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.Identificador)
                .IsUnique();

            builder.Property(e => e.RecursoBibliograficoId)
                .IsRequired();

            builder.Property(e => e.Estado)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.Observacion)
                .HasMaxLength(200);

            builder.HasOne(e => e.RecursoBibliografico)
                .WithMany(r => r.Ejemplares)
                .HasForeignKey(e => e.RecursoBibliograficoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}