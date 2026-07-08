using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class AprobarPrestamo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IEjemplarRepository _ejemplares;
        private readonly ISolicitudRepository _solicitud;

        public AprobarPrestamo(IRepositorioPrestamo prestamos,IUsuario usuarios,
            INotificador notificador, IAuditoriaService auditoria, IEjemplarRepository ejemplares, 
            IRepositorioPenalizacion penalizaciones, ISolicitudRepository solicitud)
        {
            _prestamos = prestamos;
            _ejemplares = ejemplares;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _notificador = notificador;
            _auditoria = auditoria;
            _solicitud = solicitud;
        }

        public async Task<PrestamoResponse> AprobarPrestamoAsync(AprobarSolicitudRequest request, string Identificacion)
        {
            var solicitud = await _solicitud.ObtenerConDetallesAsync(request.SolicitudId);
            if (solicitud == null) throw new BusinessException("La solicitud especificada no existe.");

            var usuario = await _usuarios.ObtenerporIdAsync(solicitud.UsuarioId);
            if (usuario == null) throw new BusinessException("El usuario asociado a esta solicitud no existe.");

            if (usuario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("Aprobación denegada: El usuario se encuentra inactivo en el sistema.");

            bool tienePenalizaciones = await _penalizaciones.TienePenalizacionActivaAsync(usuario.UsuarioId);
            if (tienePenalizaciones)
            {
                solicitud.Rechazar("El usuario posee una penalización activa.");
                await _solicitud.ActualizarAsync(solicitud);
                throw new BusinessException("Aprobación denegada: El usuario tiene una penalización activa. La solicitud ha sido rechazada automáticamente.");
            }

            int prestamosActivos = await _prestamos.ContarActivosPorUsuarioAsync(usuario.UsuarioId);

            var (limiteCantidad, diasPrestamo) = usuario switch
            {
                Estudiante estudiante => (estudiante.LimitePrestamos, 7),
                Docente docente => (docente.LimitePrestamo, 14),
                _ => throw new BusinessException("Rol de usuario no válido para préstamos.")
            };

            if (prestamosActivos >= limiteCantidad) throw new BusinessException($"El usuario ya alcanzó su límite máximo de {limiteCantidad} préstamos.");
            if (solicitud.Ejemplar == null) throw new BusinessException("El ejemplar físico no está disponible.");

            solicitud.Aprobar();
            solicitud.Ejemplar.MarcarComoPrestado();

            var nuevoPrestamo = new Prestamo(
                solicitudId: solicitud.SolicitudId,
                usuarioId: usuario.UsuarioId,
                ejemplarId: solicitud.EjemplarId,
                diasPermitidos: diasPrestamo
            );

            await _solicitud.ActualizarAsync(solicitud);
            await _ejemplares.ActualizarAsync(solicitud.Ejemplar);
            await _prestamos.AgregarAsync(nuevoPrestamo);

            //Implementacion para notificacion y auditoria
            string tituloLibro = solicitud.Ejemplar.RecursoBibliografico?.Titulo ?? "Recurso Solicitado";

            return new PrestamoResponse
            {
                PrestamoId = nuevoPrestamo.PrestamoId,
                TituloRecurso = tituloLibro,
                IdentificadorEjemplar = solicitud.Ejemplar.Identificador,
                FechaInicio = nuevoPrestamo.FechaInicio,
                FechaLimite = nuevoPrestamo.FechaLimite,
                Estado = nuevoPrestamo.Estado.ToString()
            };
        }   
    }
}
