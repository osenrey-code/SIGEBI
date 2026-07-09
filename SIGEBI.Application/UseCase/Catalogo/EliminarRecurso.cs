using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class EliminarRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public EliminarRecurso(
            IRepositorioRecurso recursos,
            IRepositorioPrestamo prestamos,
            IUsuario usuarios,
            IAuditoriaService auditoria)
        {
            _recursos = recursos;
            _prestamos = prestamos;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task EjecutarAsync(EliminarRecursoRequest request, int usuarioId)
        {
            Guard.NotNull(request, "Los datos de eliminación del recurso");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            string motivo = string.IsNullOrWhiteSpace(request.Motivo)
                ? "No especificado"
                : request.Motivo.Trim();

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(
                usuarioId
            );

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede eliminar recursos.");

            var recurso = await _recursos.ObtenerporIdAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            bool tienePrestamosActivos =
                await _prestamos.ExistePrestamoActivoPorRecursoAsync(
                    request.RecursoBibliograficoId
                );

            if (tienePrestamosActivos)
            {
                throw new BusinessException(
                    "No se puede eliminar el recurso porque tiene préstamos activos."
                );
            }

            string titulo = recurso.Titulo;
            string isbn = recurso.ISBN;

            await _recursos.EliminarAsync(recurso);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Eliminar Recurso",
                EntidadAfectada: "RecursosBibliograficos",
                detalles: $"Se eliminó el recurso '{titulo}' con ISBN {isbn}. Motivo: '{motivo}'."
            );
        }
    }
}