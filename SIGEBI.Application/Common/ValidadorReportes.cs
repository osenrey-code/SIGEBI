using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.Common
{
    public class ValidadorReportes
    {
        private readonly IUsuario _usuarios;

        public ValidadorReportes(IUsuario usuarios)
        {
            _usuarios = usuarios;
        }

        public async Task ValidarAdministradorOAuditorAsync(int usuarioEjecutorId)
        {
            var usuario = await ObtenerUsuarioActivoAsync(usuarioEjecutorId);

            if (!usuario.EsAdministradorOAuditor())
                throw new BusinessException("Solo un administrador o auditor puede generar este reporte.");
        }

        public async Task ValidarAccesoReporteInventarioAsync(int usuarioEjecutorId)
        {
            var usuario = await ObtenerUsuarioActivoAsync(usuarioEjecutorId);

            if (!usuario.PuedeGenerarReporteInventario())
                throw new BusinessException("Solo un bibliotecario, administrador o auditor puede generar el reporte de inventario.");
        }

        public static void ValidarRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio == default)
                throw new BusinessException("La fecha de inicio es obligatoria.");

            if (fechaFin == default)
                throw new BusinessException("La fecha final es obligatoria.");

            if (fechaInicio > fechaFin)
                throw new BusinessException("La fecha de inicio no puede ser mayor que la fecha final.");
        }

        private async Task<Domain.Entities.Usuario> ObtenerUsuarioActivoAsync(int usuarioEjecutorId)
        {
            if (usuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuario = await _usuarios.ObtenerporIdAsync(usuarioEjecutorId);

            if (usuario is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            return usuario;
        }
    }
}