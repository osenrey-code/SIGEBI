using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    internal class AuditorConfiguration : IEntityTypeConfiguration<Auditor>
    {
        public void Configure(EntityTypeBuilder<Auditor> builder)
        {
            builder.Property(a => a.CodigoEmpleado)
                .HasMaxLength(30)
                .HasColumnName("CodigoEmpleado");
        }
    }
}