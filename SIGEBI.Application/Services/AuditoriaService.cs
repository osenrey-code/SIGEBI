using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IUsuario _usuarios;

        public AuditoriaService(
            IRepositorioAuditoria auditoria,
            IUsuario usuarios)
        {
            _auditoria = auditoria;
            _usuarios = usuarios;
        }

        public async Task RegistrarAsync(
            Guid? usuarioId,
            string accion,
            string entidadAfectada,
            Guid? entidadId,
            string resultado,
            string detalle,
            string valoresAnteriores = "",
            string valoresNuevos = "")
        {
            string nombreUsuario = "Sistema/Anónimo";

            if (usuarioId.HasValue && usuarioId.Value != Guid.Empty)
            {
                var usuario = await _usuarios.ObtenerPorIdAsync(
                    usuarioId.Value
                );

                if (usuario is not null)
                {
                    nombreUsuario = usuario.NombreCompleto;
                }
            }

            var registro = new RegistroAuditoria(
                usuarioId,
                nombreUsuario,
                accion,
                entidadAfectada,
                entidadId,
                resultado,
                detalle,
                valoresAnteriores,
                valoresNuevos
            );

            await _auditoria.AgregarAsync(registro);
        }
    }
}
