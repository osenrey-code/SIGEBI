using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ActualizarRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioCategoria _categorias;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public ActualizarRecurso(
            IRepositorioRecurso recursos,
            IRepositorioCategoria categorias,
            IUsuario usuarios,
            IAuditoriaService auditoria)
        {
            _recursos = recursos;
            _categorias = categorias;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<RecursoResponse> EjecutarAsync(
            ActualizarRecursoRequest request)
        {
            Guard.NotNull(request, "Los datos del recurso");

            if (request.UsuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Titulo, "El título del recurso");
            Guard.NotNullOrWhiteSpace(request.Autor, "El autor del recurso");

            if (request.CategoriaId <= 0)
                throw new BusinessException("La categoría del recurso es obligatoria.");

            if (request.AnioPublicado <= 0)
                throw new BusinessException("El año de publicación es obligatorio.");

            string titulo = request.Titulo.Trim();
            string autor = request.Autor.Trim();
            string? imagenUrl = string.IsNullOrWhiteSpace(request.ImagenUrl)
                ? null
                : request.ImagenUrl.Trim();

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede actualizar recursos.");

            var recurso = await _recursos.ObtenerporIdAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var categoria = await _categorias.ObtenerporIdAsync(request.CategoriaId);

            if (categoria is null)
                throw new BusinessException("La categoría indicada no existe.");

            string tituloAnterior = recurso.Titulo;

            recurso.ActualizarInformacion(
                titulo,
                autor,
                request.CategoriaId,
                request.AnioPublicado,
                imagenUrl
            );

            await _recursos.ActualizarAsync(recurso);

            await _auditoria.RegistrarAsync(
                UsuarioId: request.UsuarioEjecutorId,
                Accion: "Actualizar Recurso",
                EntidadAfectada: "RecursosBibliograficos",
                detalles: $"Se actualizó el recurso ID {recurso.RecursoBibliograficoId}. Título anterior: '{tituloAnterior}', nuevo título: '{recurso.Titulo}'."
            );

            return MapearRecurso(recurso, categoria.Nombre);
        }

        private static RecursoResponse MapearRecurso(
            RecursoBibliografico recurso,
            string nombreCategoria)
        {
            return new RecursoResponse
            {
                RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                ISBN = recurso.ISBN,
                Titulo = recurso.Titulo,
                Autor = recurso.Autor,
                CategoriaId = recurso.CategoriaId,
                Categoria = nombreCategoria,
                AnioPublicado = recurso.AnioPublicado,
                ImagenUrl = recurso.ImagenUrl,
                TotalEjemplares = recurso.TotalEjemplares,
                CopiasDisponibles = recurso.CopiasDisponibles
            };
        }
    }
}