using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
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

        public async Task<RecursoResponse> EjecutarAsync(ActualizarRecursoRequest request)
        {
            if (request.UsuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(request.UsuarioEjecutorId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede actualizar recursos.");

            if (string.IsNullOrWhiteSpace(request.Titulo))
                throw new BusinessException("El título del recurso es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Autor))
                throw new BusinessException("El autor del recurso es obligatorio.");

            if (request.CategoriaId <= 0)
                throw new BusinessException("La categoría del recurso es obligatoria.");

            if (request.AnioPublicado <= 0)
                throw new BusinessException("El año de publicación es obligatorio.");

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoBibliograficoId);

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var categoria = await _categorias.ObtenerporIdAsync(request.CategoriaId);

            if (categoria is null)
                throw new BusinessException("La categoría indicada no existe.");

            recurso.ActualizarInformacion(
                request.Titulo.Trim(),
                request.Autor.Trim(),
                request.CategoriaId,
                request.AnioPublicado,
                request.ImagenUrl
            );

            await _recursos.ActualizarAsync(recurso);

            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Actualizar recurso",
                "RecursoBibliografico",
                $"Se actualizó el recurso ID {recurso.RecursoBibliograficoId}: {recurso.Titulo}."
            );

            return MapearRecurso(recurso, categoria.Nombre);
        }

        private static RecursoResponse MapearRecurso(RecursoBibliografico recurso, string nombreCategoria)
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