using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Infrastructure.Persistence
{
    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Bibliotecario> Bibliotecarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Auditor> Auditores { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<RecursoBibliografico> RecursosBibliograficos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Ejemplar> Ejemplares { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Devolucion> Devolucion { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SIGEBIDbContext).Assembly);
        }
    }
}