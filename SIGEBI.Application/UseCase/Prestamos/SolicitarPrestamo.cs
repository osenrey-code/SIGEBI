using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Domain.Exceptions;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class SolicitarPrestamo
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly ISolicitudRepository _solicitudes;
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;
        private readonly IEjemplarRepository _ejemplares;

        public SolicitarPrestamoUseCase(
            IUsuario usuarios,
            IEjemplarRepository ejemplares,
            ISolicitudRepository solicitudes,
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones)
        {
            _usuarios = usuarios;
            _ejemplares = ejemplares;
            _solicitudes = solicitudes;
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
        }

        public async Task<SolicitudResponse> EjecutarAsync(RegistrarSolicitudRequest request, int usuarioId)
        {
            // 1. Validar que el usuario exista
            var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId);
            if (usuario == null)
                throw new BusinessException("Usuario no encontrado."); // Idealmente usarías una custom BusinessException

            // 2. Validar Penalizaciones Activas
            bool tienePenalizacion = await _penalizaciones.TienePenalizacionActivaAsync(usuarioId);
            if (tienePenalizacion)
                throw new BusinessException("El usuario tiene una penalización activa y no puede solicitar recursos.");

            // 3. Validar Límite de Préstamos
            int prestamosActivos = await _prestamos.ContarActivosPorUsuarioAsync(usuarioId);

            if (prestamosActivos >= usuario.LimitePrestamos)
                throw new Exception($"Límite excedido. Su rol permite un máximo de {usuario.LimitePrestamos} préstamos simultáneos.");

            // 4. Validar Existencia y Disponibilidad del Ejemplar
            var ejemplar = await _ejemplares.ObtenerPorIdAsync(request.EjemplarId);
            if (ejemplar == null)
                throw new BusinessException("El ejemplar físico solicitado no existe.");

            if (ejemplar.Estado != EstadoEjemplar.Disponible)
                throw new BusinessException($"El ejemplar seleccionado no está disponible. Estado actual: {ejemplar.Estado}");


            var nuevaSolicitud = new Solicitud(usuarioId, request.EjemplarId);
            await _solicitudes.AgregarAsync(nuevaSolicitud);

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

