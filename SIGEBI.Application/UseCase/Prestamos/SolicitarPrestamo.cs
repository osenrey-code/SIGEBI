using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class SolicitarPrestamo
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly ISolicitudRepository _solicitudes;
        private readonly IAuditoriaService _auditoria;
        private readonly IEjemplarRepository _ejemplares;

        public SolicitarPrestamo(
            IUsuario usuarios,
            IEjemplarRepository ejemplares,
            ISolicitudRepository solicitudes,
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _ejemplares = ejemplares;
            _solicitudes = solicitudes;
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
            _auditoria = auditoria;
        }

        public async Task<SolicitudResponse> SolicitarPrestamoAsync(
            RegistrarSolicitudRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los datos de la solicitud");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario solicitante es obligatorio.");

            if (request.EjemplarId <= 0)
                throw new BusinessException("El ejemplar solicitado es obligatorio.");

            var usuario = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuario is null)
                throw new BusinessException("El usuario solicitante no existe.");

            if (usuario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario solicitante no está activo.");

            int limitePermitido = usuario switch
            {
                Estudiante estudiante => estudiante.LimitePrestamos,
                Docente docente => docente.LimitePrestamo,
                _ => throw new BusinessException("Solo estudiantes y docentes pueden solicitar préstamos.")
            };

            bool tienePenalizacion = await _penalizaciones.TienePenalizacionActivaAsync(
                usuario.UsuarioId
            );

            if (tienePenalizacion)
                throw new BusinessException("El usuario tiene una penalización activa y no puede solicitar recursos.");

            int prestamosActivos = await _prestamos.ContarActivosPorUsuarioAsync(
                usuario.UsuarioId
            );

            if (prestamosActivos >= limitePermitido)
            {
                throw new BusinessException(
                    $"Solicitud rechazada. El usuario tiene {prestamosActivos} préstamos activos y su límite permitido es {limitePermitido}."
                );
            }

            var ejemplar = await _ejemplares.ObtenerPorIdAsync(request.EjemplarId);

            if (ejemplar is null)
                throw new BusinessException("El ejemplar físico solicitado no existe.");

            if (ejemplar.Estado != EstadoEjemplar.Disponible)
            {
                throw new BusinessException(
                    $"El ejemplar seleccionado no está disponible. Estado actual: {ejemplar.Estado}."
                );
            }

            var nuevaSolicitud = new Solicitud(
                usuario.UsuarioId,
                request.EjemplarId
            );

            await _solicitudes.AgregarAsync(nuevaSolicitud);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Solicitar Préstamo",
                EntidadAfectada: "Solicitudes",
                detalles: $"El usuario '{usuario.NombreCompleto}' solicitó el ejemplar ID {ejemplar.EjemplarId}. Préstamos activos actuales: {prestamosActivos}. Límite permitido: {limitePermitido}."
            );

            return new SolicitudResponse
            {
                SolicitudId = nuevaSolicitud.SolicitudId,
                TituloRecurso = ejemplar.RecursoBibliografico?.Titulo ?? "Título no disponible",
                IdentificadorEjemplar = ejemplar.Identificador,
                FechaSolicitud = nuevaSolicitud.FechaSolicitud,
                Estado = nuevaSolicitud.Estado.ToString()
            };
        }
    }
}