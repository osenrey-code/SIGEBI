using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SIGEBI.Infrastructure.Persistence.Configuration
{
    public class PerfilLectorConfiguration : IEntityTypeConfiguration<PerfilLector>
    {
        public void Configure(EntityTypeBuilder<PerfilLector> builder)
        {
            builder.ToTable("PerfilesLectores");
            builder.HasKey(p => p.Id);
        }
    }
}
