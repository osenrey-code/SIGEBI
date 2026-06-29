using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class RegistrarRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;

        public RegistrarRecurso(
            IRepositorioRecurso recursos,
            IUsuario usuarios)
        {
            _recursos = recursos;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<RecursoResponse>> EjecutarAsync(
            RegistrarRecursoRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El usuario ejecutor es obligatorio."
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
                    "Solo un bibliotecario o administrador puede registrar recursos."
                );
            }

            var errorValidacion = ValidarDatosBasicos(request);

            if (errorValidacion is not null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    errorValidacion
                );
            }

            var recursoExistente = await _recursos.ObtenerPorIdentificadorAsync(
                request.Identificador.Trim()
            );

            if (recursoExistente is not null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "Ya existe un recurso con ese identificador."
                );
            }

            var recurso = new RecursoBibliografico(
                request.Identificador.Trim(),
                request.Titulo.Trim(),
                request.Autor.Trim(),
                request.Categoria.Trim(),
                request.NumeroEjemplares
            );

            await _recursos.AgregarAsync(recurso);

            var response = MapearRecurso(recurso);

            return ResultadoOperacionResponse<RecursoResponse>.Ok(
                "Recurso registrado correctamente.",
                response
            );
        }

        private static string? ValidarDatosBasicos(RegistrarRecursoRequest request)
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