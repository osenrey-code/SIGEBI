
using SIGEBI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SIGEBI.Infrastructure.Persistencia
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PerfilLector> PerfilesLectores { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        public DbSet<RecursoBibliografico> RecursoBibliograficos { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
        }
    }
}
