using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.UseCase
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IApplicationDbContext _db;

        public AuditoriaService(IRepositorioAuditoria auditoria, IApplicationDbContext db)
        {
            _auditoria = auditoria;
            _db = db;
        }

        public async Task RegistrarAsync(
            int UsuarioId,
            string Accion,
            string EntidadAfectada,
            string detalles = "")
        {
            var registro = new Auditoria(
                UsuarioId,
                EntidadAfectada,
                Accion,
                detalles
            );

            await _auditoria.AgregarAsync(registro);
        }

        public async Task<IEnumerable<LogAuditoriaResponse>> ListarHistorialAsync(
            string? UsuarioId = null,
            string? EntidadAfectada = null)
        {
           
            var registros = await _auditoria.ConsultarAsync(
                UsuarioId,
                null,
                EntidadAfectada,
                null,
                null
            );

            return registros.Select(r => new LogAuditoriaResponse
            {
                AuditoriaId = r.AuditoriaId,
                UsuarioId = r.UsuarioId,
                Accion = r.Accion,
                EntidadAfectada = r.EntidadAfectada,
                Detalle = r.Detalle,
                FechaRegistro = r.FechaRegistro
            }).ToList();
        }
    }
}
