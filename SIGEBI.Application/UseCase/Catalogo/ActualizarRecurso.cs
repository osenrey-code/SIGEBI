using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ActualizarRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public ActualizarRecurso(IRepositorioRecurso recursos, IUsuario usuarios, IAuditoriaService auditoria)
        {
            _recursos = recursos;
            _usuarios = usuarios;
            _auditoria = auditoria;
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

            // Guardamos cómo estaba el recurso antes de modificarlo.
            // Esto sirve para que la auditoría pueda mostrar qué cambió.
            var valoresAnteriores =
                $"Identificador: {recurso.Identificador}; " +
                $"Título: {recurso.Titulo}; " +
                $"Autor: {recurso.Autor}; " +
                $"Categoría: {recurso.Categoria}; " +
                $"Número de ejemplares: {recurso.NumeroEjemplares}; " +
                $"Estado: {recurso.Estado}";

            // Aquí aplicamos el cambio real sobre la entidad del dominio.
            recurso.ActualizarInformacion(
                request.Identificador.Trim(),
                request.Titulo.Trim(),
                request.Autor.Trim(),
                request.Categoria.Trim(),
                request.NumeroEjemplares,
                request.ImagenUrl!
            );

            // Guardamos los cambios del recurso en la base de datos.
            await _recursos.ActualizarAsync(recurso);

            // Guardamos cómo quedó el recurso después de actualizarlo.
            var valoresNuevos =
                $"Identificador: {recurso.Identificador}; " +
                $"Título: {recurso.Titulo}; " +
                $"Autor: {recurso.Autor}; " +
                $"Categoría: {recurso.Categoria}; " +
                $"Número de ejemplares: {recurso.NumeroEjemplares}; " +
                $"Estado: {recurso.Estado}";

            // Registramos la acción en auditoría.
            // El usuario ejecutor es quien hizo la modificación en el sistema.
            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Actualizar recurso",
                "RecursoBibliografico",
                recurso.Id,
                "Exitoso",
                $"Se actualizó el recurso '{recurso.Titulo}'.",
                valoresAnteriores,
                valoresNuevos
            );

            var response = MapearRecurso(recurso);

            return ResultadoOperacionResponse<RecursoResponse>.Ok(
                "Recurso actualizado correctamente.",
                response
            );
        }

        private static string? ValidarDatosBasicos(ActualizarRecursoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ImagenUrl))
                return "La imagen del recurso es obligatoria.";

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
                NumeroEjemplares = recurso.NumeroEjemplares,
                ImagenUrl = recurso.ImagenUrl,
            };
        }
    }
}