using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class DocenteConfiguration : IEntityTypeConfiguration<Docente>
    {
        public void Configure(EntityTypeBuilder<Docente> builder)
        {
            builder.Property(d => d.CodigoEmpleado)
                .HasMaxLength(30)
                .HasColumnName("CodigoEmpleado");

            builder.HasIndex(d => d.CodigoEmpleado)
                .IsUnique()
                .HasFilter("[CodigoEmpleado] IS NOT NULL")
                .HasDatabaseName("IX_Usuario_CodigoEmpleado");
        }
    }
}