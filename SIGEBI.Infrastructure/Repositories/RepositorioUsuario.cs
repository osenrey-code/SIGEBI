using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioUsuario : IUsuario 
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<Usuario> _dbSet;

        public RepositorioUsuario(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Usuario>();
        }

        public async Task ActualizarAsync(Usuario entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Usuario entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task DesactivarUsuarioAsync(string IdUsuario)
        {
            var usuario = await ObtenerUsuarioPorIdentificacionAsync(IdUsuario);

            if (usuario != null)
            {
                usuario.Estado = EstadoUsuario.Inactivo;
                _dbSet.Update(usuario);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Correo == correo);
        }

        public async Task<Usuario?> ObtenerporIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioConDetallesAsync(string Identificacion)
        {
            return await _dbSet
      
         .Include(u => u.Prestamos)
         .Include(u => u.Penalizciones) 
         .Include(u => u.Notificaciones)

         .FirstOrDefaultAsync(u =>
             (u is Estudiante && ((Estudiante)u).Matricula == Identificacion) ||
             (u is Docente && ((Docente)u).CodigoEmpleado == Identificacion) ||
             (u is Administrador && ((Administrador)u).CodigoEmpleado == Identificacion) ||
             (u is Bibliotecario && ((Bibliotecario)u).CodigoEmpleado == Identificacion) ||
             (u is Auditor && ((Auditor)u).CodigoEmpleado == Identificacion)
         );
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdentificacionAsync(string identificacion)
        {
             return await _dbSet.FirstOrDefaultAsync(u =>
            (u is Estudiante && ((Estudiante)u).Matricula == identificacion) ||
            (u is Docente && ((Docente)u).CodigoEmpleado == identificacion) ||
            (u is Administrador && ((Administrador)u).CodigoEmpleado == identificacion) ||
            (u is Bibliotecario && ((Bibliotecario)u).CodigoEmpleado == identificacion) ||
            (u is Auditor && ((Auditor)u).CodigoEmpleado == identificacion)
            );
        }

        public async Task<IEnumerable<Usuario?>> ConsultarPorFiltrosAsync(string? nombre, string? tipoUsuario, string? estado)
        {
            IQueryable<Usuario> query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(u => u.NombreCompleto.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                if (Enum.TryParse<EstadoUsuario>(estado, true, out var estadoEnum))
                {
                    query = query.Where(u => u.Estado == estadoEnum);
                }
            }

            if (!string.IsNullOrWhiteSpace(tipoUsuario))
            {
                query = tipoUsuario.ToLower() switch
                {
                    "estudiante" => query.OfType<Estudiante>(),
                    "docente" => query.OfType<Docente>(),
                    "administrador" => query.OfType<Administrador>(),
                    "bibliotecario" => query.OfType<Bibliotecario>(),
                    "auditor" => query.OfType<Auditor>(),
                    _ => query // Si envían un tipo no válido, no filtramos por tipo
                };
            }
            return await query.ToListAsync();
        }

       
    }
}
