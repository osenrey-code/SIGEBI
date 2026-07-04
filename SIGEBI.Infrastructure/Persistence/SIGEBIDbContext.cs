
using SIGEBI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SIGEBI.Infrastructure.Persistence
{

    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        public DbSet<RecursoBibliografico> RecursosBibliograficos { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Ejemplar> Ejemplares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SIGEBIDbContext).Assembly);
        }
    }
      
    
}

