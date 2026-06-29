using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ActualizarRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;

        public ActualizarRecurso(
            IRepositorioRecurso recursos,
            IUsuario usuarios)
        {
            _recursos = recursos;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<RecursoResponse>> EjecutarAsync(
            ActualizarRecursoRequest request)
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
                    "Solo un bibliotecario o administrador puede actualizar recursos."
                );
            }

            var errorValidacion = ValidarDatosBasicos(request);

            if (errorValidacion is not null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    errorValidacion
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El recurso no existe."
                );
            }

            var recursoConMismoIdentificador = await _recursos.ObtenerPorIdentificadorAsync(
                request.Identificador.Trim()
            );

            if (recursoConMismoIdentificador is not null &&
                recursoConMismoIdentificador.Id != recurso.Id)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "Ya existe otro recurso con ese identificador."
                );
            }

            recurso.ActualizarInformacion(
                request.Identificador.Trim(),
                request.Titulo.Trim(),
                request.Autor.Trim(),
                request.Categoria.Trim(),
                request.NumeroEjemplares
            );

            await _recursos.ActualizarAsync(recurso);

            var response = MapearRecurso(recurso);

            return ResultadoOperacionResponse<RecursoResponse>.Ok(
                "Recurso actualizado correctamente.",
                response
            );
        }

        private static string? ValidarDatosBasicos(ActualizarRecursoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Identificador))
                return "El identificador del recurso es obligatorio.";

            if (string.IsNullOrWhiteSpace(request.Titulo))
                return "El título del recurso es obligatorio.";

            if (string.IsNullOrWhiteSpace(request.Autor))
                return "El autor del recurso es obligatorio.";

            if (string.IsNullOrWhiteSpace(request.Categoria))
                return "La categoría del recurso es obligatoria.";

            if (request.NumeroEjemplares <= 0)
                return "El número de ejemplares debe ser mayor que cero.";

            return null;
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