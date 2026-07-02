using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;
using System.Reflection.Metadata.Ecma335;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class RegistrarRecurso 
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioCategoria _categoria;

        public RegistrarRecurso(
            IRepositorioRecurso recursos,
            IUsuario usuarios,
            IAuditoriaService auditoria, IServicioCategoria categoria)
        {
            _recursos = recursos;
            _usuarios = usuarios;
            _auditoria = auditoria;
            _categoria = categoria;
        }

        public async Task RegistrarRecursoAsync(RegistrarRecursoRequest request, int UsuarioEjecutorId)
        {
           if (await _recursos.ObtenerporIdAsync(request.ISBN) != null)
                throw new BusinessException("Existe un libro registrado con este ISBN.");

            var categorias = await _categoria.ListarTodasAsync();

            if (!categorias.Any(c => c.CategoriaId == request.CategoriaId))
                throw new BusinessException("La categoria no existe.");

            var libro = new RecursoBibliografico(request.ISBN, request.Titulo, request.Autor, request.AnioPublicacion,
                request.CategoriaId, request.ImagenUrl)
            {
                ISBN = request.ISBN,
                Titulo = request.Titulo,
                Autor = request.Autor,
                AnioPublicado = request.AnioPublicacion,
                CategoriaId = request.CategoriaId,
                ImagenUrl = request.ImagenUrl
            };

            await _recursos.AgregarAsync(libro);
            await _auditoria.RegistrarAsync(
              UsuarioId: UsuarioEjecutorId,
              Accion: "Creación de libro",
              EntidadAfectada: "Recurso Bibliografico",
              detalles: $"Se registro el libro {libro.Titulo}"
             );
        }
       
    }
}