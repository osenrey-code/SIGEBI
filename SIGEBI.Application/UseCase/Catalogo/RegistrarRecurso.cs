using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class RegistrarRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioCategoria _categorias;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public RegistrarRecurso(
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
            RegistrarRecursoRequest request,
            int usuarioEjecutorId)
        {
            if (usuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioEjecutorId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede registrar recursos.");

            if (string.IsNullOrWhiteSpace(request.ISBN))
                throw new BusinessException("El ISBN es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Titulo))
                throw new BusinessException("El título del recurso es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Autor))
                throw new BusinessException("El autor del recurso es obligatorio.");

            if (request.CategoriaId <= 0)
                throw new BusinessException("La categoría del recurso es obligatoria.");

            if (request.AnioPublicado <= 0)
                throw new BusinessException("El año de publicación es obligatorio.");

            if (request.CantidadEjemplares <= 0)
                throw new BusinessException("La cantidad de ejemplares debe ser mayor que cero.");

            var recursoExistente = await _recursos.BuscarPorIsbnAsync(request.ISBN.Trim());

            if (recursoExistente is not null)
                throw new BusinessException("Ya existe un recurso registrado con ese ISBN.");

            var categoria = await _categorias.ObtenerporIdAsync(request.CategoriaId);

            if (categoria is null)
                throw new BusinessException("La categoría indicada no existe.");

            var recurso = new RecursoBibliografico(
                request.ISBN.Trim(),
                request.Titulo.Trim(),
                request.Autor.Trim(),
                request.CategoriaId,
                request.AnioPublicado,
                request.ImagenUrl
            );

            for (int i = 1; i <= request.CantidadEjemplares; i++)
            {
                var identificadorEjemplar = $"{request.ISBN.Trim()}-{i:D3}";
                recurso.RegistrarNuevoEjemplar(identificadorEjemplar);
            }

            await _recursos.AgregarAsync(recurso);

            await _auditoria.RegistrarAsync(
                usuarioEjecutorId,
                "Registrar recurso",
                "RecursoBibliografico",
                $"Se registró el recurso '{recurso.Titulo}' con ISBN {recurso.ISBN}."
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