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
    public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
    {
        public void Configure(EntityTypeBuilder<Prestamo> builder)
        {
            builder.ToTable("Prestamos");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FechaInicio).IsRequired();
            builder.Property(p => p.FechaLimite).IsRequired();

            builder.HasOne(p => p.PerfilLector)
                .WithMany(pl => pl.prestamos)
                .HasForeignKey(p => p.PerfilLectorId)
                .OnDelete(DeleteBehavior.NoAction);
        } 
    }
}
