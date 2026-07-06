using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Exceptions;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class ConsultarHistorialPrestamos
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;

        public ConsultarHistorialPrestamos(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
        }

        public async Task<IEnumerable<PrestamoResponse>> ConsultarHistorialAsync(
            ConsultarHistorialPrestamosRequest request)
        {

            int? usuarioId = null;

            if (!string.IsNullOrWhiteSpace(request.Identificacion))
            {
                var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(request.Identificacion);

                if (usuario == null) throw new BusinessException("Usuario no encontrado en el sistema.");

                usuarioId = usuario.UsuarioId;
            }

            var historialPrestamos = await _prestamos.ConsultarHistorialAsync(usuarioId, request.EjemplarId);

            return historialPrestamos.Select(p => new PrestamoResponse
            {
                PrestamoId = p.PrestamoId,
                TituloRecurso = p.Ejemplar?.RecursoBibliografico?.Titulo ?? "Titulo no disponible",
                IdentificadorEjemplar = p.Ejemplar?.Identificador ?? "N/A",
                FechaInicio = p.FechaInicio,
                FechaLimite = p.FechaLimite,
                Estado = p.Estado.ToString()
            });
        }
    }
}
