using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class ConsultarPrestamosActivos
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;

        public ConsultarPrestamosActivos(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
        }

        public async Task<IEnumerable<PrestamoResponse>> ConsultarPrestamosActivosAsync(
            ConsultarPrestamosActivosRequest request)
        {
            Guard.NotNull(request, "Los filtros de préstamos activos");

            int? usuarioId = null;

            if (!string.IsNullOrWhiteSpace(request.Identificacion))
            {
                string identificacion = request.Identificacion.Trim();

                var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                    identificacion
                );

                if (usuario is null)
                    throw new BusinessException("Usuario no encontrado en el sistema.");

                usuarioId = usuario.UsuarioId;
            }

            if (request.RecursoBibliograficoId.HasValue &&
                request.RecursoBibliograficoId.Value <= 0)
            {
                throw new BusinessException("El recurso bibliográfico debe ser mayor que cero.");
            }

            if (request.EjemplarId.HasValue &&
                request.EjemplarId.Value <= 0)
            {
                throw new BusinessException("El ejemplar debe ser mayor que cero.");
            }

            var prestamosActivos = await _prestamos.ConsultarActivosAsync(
                usuarioId,
                request.RecursoBibliograficoId,
                request.EjemplarId
            );

            return prestamosActivos
                .Select(p => new PrestamoResponse
                {
                    PrestamoId = p.PrestamoId,
                    TituloRecurso = p.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                    IdentificadorEjemplar = p.Ejemplar?.Identificador ?? "N/A",
                    FechaInicio = p.FechaInicio,
                    FechaLimite = p.FechaLimite,
                    Estado = p.Estado.ToString()
                })
                .ToList();
        }
    }
}