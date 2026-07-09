using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class EstudianteConfiguration : IEntityTypeConfiguration<Estudiante>
    {
        public void Configure(EntityTypeBuilder<Estudiante> builder)
        {
            builder.Property(e => e.Matricula)
                .HasMaxLength(30)
                .HasColumnName("Matricula");

            builder.HasIndex(e => e.Matricula)
                .IsUnique()
                .HasFilter("[Matricula] IS NOT NULL")
                .HasDatabaseName("IX_Usuario_Matricula");
        }
    
    }
}
