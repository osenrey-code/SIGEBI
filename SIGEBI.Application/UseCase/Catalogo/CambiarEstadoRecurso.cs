using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class CambiarEstadoRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public CambiarEstadoRecurso(
            IRepositorioRecurso recursos,
            IUsuario usuarios,
            IAuditoriaService auditoria)
        {
            _recursos = recursos;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<RecursoResponse> EjecutarAsync(CambiarEstadoRecursoRequest request, int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            if (request.EjemplarId <= 0)
                throw new BusinessException("El ejemplar es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.NuevoEstado))
                throw new BusinessException("El nuevo estado del ejemplar es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede cambiar estados de ejemplares.");

            var recurso = await _recursos.BuscarConCategoriaAsync(request.RecursoBibliograficoId);

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var ejemplar = recurso.Ejemplares.FirstOrDefault(e => e.EjemplarId == request.EjemplarId);

            if (ejemplar is null)
                throw new BusinessException("El ejemplar indicado no pertenece al recurso.");

            var estadoAnterior = ejemplar.Estado;

            if (!Enum.TryParse<EstadoEjemplar>(request.NuevoEstado, true, out var nuevoEstado))
                throw new BusinessException("El estado indicado no es válido.");

            switch (nuevoEstado)
            {
                case EstadoEjemplar.Disponible:
                    ejemplar.MarcarDisponible();
                    break;

                case EstadoEjemplar.Prestado:
                    ejemplar.MarcarComoPrestado();
                    break;

                case EstadoEjemplar.Reservado:
                    ejemplar.MarcarComoReservado();
                    break;

                case EstadoEjemplar.FueraDeServicio:
                    ejemplar.MarcarFueraDeServicio(request.Motivo ?? "No especificado");
                    break;

                default:
                    throw new BusinessException("El estado indicado no es válido.");
            }

            await _recursos.ActualizarAsync(recurso);

            await _auditoria.RegistrarAsync(
                usuarioId,
                "Cambiar estado de ejemplar",
                "RecursoBibliografico",
                $"Se cambió el ejemplar ID {ejemplar.EjemplarId} del recurso ID {recurso.RecursoBibliograficoId} de {estadoAnterior} a {ejemplar.Estado}."
            );

            return MapearRecurso(recurso);
        }

        private static RecursoResponse MapearRecurso(RecursoBibliografico recurso)
        {
            return new RecursoResponse
            {
                RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                ISBN = recurso.ISBN,
                Titulo = recurso.Titulo,
                Autor = recurso.Autor,
                CategoriaId = recurso.CategoriaId,
                Categoria = recurso.Categoria?.Nombre ?? "N/A",
                AnioPublicado = recurso.AnioPublicado,
                ImagenUrl = recurso.ImagenUrl,
                TotalEjemplares = recurso.TotalEjemplares,
                CopiasDisponibles = recurso.CopiasDisponibles
            };
        }
    }
}