using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class CambiarEstadoRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public CambiarEstadoRecurso(IRepositorioRecurso recursos, IUsuario usuarios,
            IAuditoriaService auditoria)
        {
            _recursos = recursos;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<RecursoResponse>> EjecutarAsync(
            CambiarEstadoRecursoRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            if (request.RecursoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El recurso es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(request.NuevoEstado))
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El nuevo estado del recurso es obligatorio."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Bibliotecario &&
                usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "Solo un bibliotecario o administrador puede cambiar el estado de un recurso."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El recurso no existe."
                );
            }

            if (!Enum.TryParse<EstadoRecurso>(
                    request.NuevoEstado,
                    ignoreCase: true,
                    out var nuevoEstado))
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El estado indicado no es válido. Estados permitidos: Disponible, Reservado, FueraDeServicio."
                );
            }

            // No permitimos poner un recurso como Prestado manualmente.
            if (nuevoEstado == EstadoRecurso.Prestado)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "No use este caso de uso para prestar un recurso. El estado Prestado debe asignarse desde el módulo de préstamos."
                );
            }

            // Si el recurso ya está prestado, tampoco permitimos cambiarlo manualmente.
            // Primero debe registrarse su devolución desde el módulo de devoluciones.
            if (recurso.Estado == EstadoRecurso.Prestado)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "Un recurso prestado no puede cambiarse manualmente desde catálogo. Primero debe registrarse la devolución."
                );
            }

            if (recurso.Estado == nuevoEstado)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El recurso ya tiene el estado indicado."
                );
            }

            // Guardamos el estado anterior para la auditoría.
            var valoresAnteriores =
                $"Estado anterior: {recurso.Estado}";

            recurso.CambiarEstado(nuevoEstado);
            await _recursos.ActualizarAsync(recurso);

            // Guardamos el nuevo estado para la auditoría.
            var valoresNuevos =
                $"Estado nuevo: {recurso.Estado}";

            // Registramos quién cambió el estado, sobre qué recurso y cuál fue el cambio.
            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Cambiar estado de recurso",
                "RecursoBibliografico",
                recurso.Id,
                "Exitoso",
                $"Se cambió manualmente el estado del recurso '{recurso.Titulo}'.",
                valoresAnteriores,
                valoresNuevos
            );

            var response = MapearRecurso(recurso);

            return ResultadoOperacionResponse<RecursoResponse>.Ok(
                "Estado del recurso actualizado correctamente.",
                response
            );
        }

        private static RecursoResponse MapearRecurso(RecursoBibliografico recurso)
        {
            return new RecursoResponse
            {
                Id = recurso.Id,
                Identificador = recurso.Identificador,
                Titulo = recurso.Titulo,
                Autor = recurso.Autor,
                Categoria = recurso.Categoria,
                Estado = recurso.Estado.ToString(),
                NumeroEjemplares = recurso.NumeroEjemplares
            };
        }
    }
}