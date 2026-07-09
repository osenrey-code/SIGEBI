using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class BibliotecarioConfiguration : IEntityTypeConfiguration<Bibliotecario>
    {
        public void Configure(EntityTypeBuilder<Bibliotecario> builder)
        {
            builder.Property(b => b.CodigoEmpleado)
                .HasMaxLength(30)
                .HasColumnName("CodigoEmpleado");
        }
    }
}