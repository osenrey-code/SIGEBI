using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class GestionCategorias : IGestionCategorias
    {
        private readonly IRepositorioCategoria _categorias;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public GestionCategorias(
            IRepositorioCategoria categorias,
            IUsuario usuarios,
            IAuditoriaService auditoria)
        {
            _categorias = categorias;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<CategoriaResponse> RegistrarCategoriaAsync(
            CategoriaRequest request,
            int actorId)
        {
            Guard.NotNull(request, "Los datos de la categoría");

            if (actorId <= 0)
                throw new BusinessException("El usuario que realiza la acción es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Nombre, "El nombre de la categoría");

            string nombre = request.Nombre.Trim();

            string descripcion = string.IsNullOrWhiteSpace(request.Descripcion)
                ? string.Empty
                : request.Descripcion.Trim();

            var actor = await _usuarios.ObtenerporIdAsync(actorId);

            if (actor is null)
                throw new BusinessException("El usuario que realiza la acción no existe.");

            if (actor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario que realiza la acción no está activo.");

            if (actor is not Administrador && actor is not Bibliotecario)
                throw new BusinessException("Solo un administrador o bibliotecario puede registrar categorías.");

            var categoriaExistente = await _categorias.ObtenerPorNombreAsync(nombre);

            if (categoriaExistente is not null)
                throw new BusinessException("Ya existe una categoría con ese nombre.");

            var categoria = new Categoria(
                nombre,
                descripcion
            );

            await _categorias.AgregarAsync(categoria);

            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Registrar Categoría",
                EntidadAfectada: "Categorías",
                detalles: $"Se registró la categoría '{categoria.Nombre}'."
            );

            return MapearCategoria(categoria);
        }

        public async Task<IEnumerable<CategoriaResponse>> ConsultarCategoriasAsync()
        {
            var categorias = await _categorias.ObtenerTodosAsync();

            return categorias
                .OrderBy(c => c.Nombre)
                .Select(MapearCategoria)
                .ToList();
        }

        private static CategoriaResponse MapearCategoria(
            Categoria categoria)
        {
            return new CategoriaResponse
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };
        }
    }
}