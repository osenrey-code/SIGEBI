using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.UseCase
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IRepositorioAuditoria _auditoria;

        public AuditoriaService(IRepositorioAuditoria auditoria)
        {
            _auditoria = auditoria;
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
            int? usuarioIdConvertido = null;

            if (!string.IsNullOrWhiteSpace(UsuarioId))
            {
                if (int.TryParse(UsuarioId, out int id))
                    usuarioIdConvertido = id;
            }

            var registros = await _auditoria.ConsultarAsync(
                usuarioIdConvertido,
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
