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
    public class GestionCatalogo : IGestionCatalogo
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioCategoria _categorias;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioAuditoria _auditoriaRepositorio;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IApplicationDbContext _db;

        public GestionCatalogo(
            IRepositorioRecurso recursos,
            IRepositorioCategoria categorias,
            IRepositorioPrestamo prestamos,
            IRepositorioAuditoria auditoriaRepositorio,
            IUsuario usuarios,
            IAuditoriaService auditoria,
            IApplicationDbContext db)
        {
            _recursos = recursos;
            _categorias = categorias;
            _prestamos = prestamos;
            _auditoriaRepositorio = auditoriaRepositorio;
            _usuarios = usuarios;
            _auditoria = auditoria;
            _db = db;
        }

        public async Task<RecursoResponse> RegistrarRecursoAsync(
            RegistrarRecursoRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los datos del recurso");

            if (usuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.ISBN, "El ISBN");
            Guard.NotNullOrWhiteSpace(request.Titulo, "El título del recurso");
            Guard.NotNullOrWhiteSpace(request.Autor, "El autor del recurso");

            if (request.CategoriaId <= 0)
                throw new BusinessException("La categoría del recurso es obligatoria.");

            if (request.AnioPublicado <= 0)
                throw new BusinessException("El año de publicación es obligatorio.");

            if (request.CantidadEjemplares <= 0)
                throw new BusinessException("La cantidad de ejemplares debe ser mayor que cero.");

            string isbn = request.ISBN.Trim();
            string titulo = request.Titulo.Trim();
            string autor = request.Autor.Trim();

            string? imagenUrl = string.IsNullOrWhiteSpace(request.ImagenUrl)
                ? null
                : request.ImagenUrl.Trim();

            await ValidarBibliotecarioOAdministradorAsync(
                usuarioEjecutorId,
                "Solo un bibliotecario o administrador puede registrar recursos."
            );

            var recursoExistente = await _recursos.BuscarPorIsbnAsync(isbn);

            if (recursoExistente is not null)
                throw new BusinessException("Ya existe un recurso registrado con ese ISBN.");

            var categoria = await _categorias.ObtenerporIdAsync(request.CategoriaId);

            if (categoria is null)
                throw new BusinessException("La categoría indicada no existe.");

            var recurso = new RecursoBibliografico(
                isbn,
                titulo,
                autor,
                request.CategoriaId,
                request.AnioPublicado,
                imagenUrl,
                request.Descripcion
            );

            for (int i = 1; i <= request.CantidadEjemplares; i++)
            {
                string identificadorEjemplar = $"{isbn}-{i:D3}";
                recurso.RegistrarNuevoEjemplar(identificadorEjemplar);
            }

            await _recursos.AgregarAsync(recurso);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioEjecutorId,
                Accion: "Registrar Recurso",
                EntidadAfectada: "RecursosBibliograficos",
                detalles: $"Se registró el recurso '{recurso.Titulo}' con ISBN {recurso.ISBN} y {request.CantidadEjemplares} ejemplares."
            );

            await _db.SaveChangesAsync();

            return MapearRecurso(recurso, categoria.Nombre);
        }

        public async Task<RecursoResponse> ActualizarRecursoAsync(
            ActualizarRecursoRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los datos del recurso");

            if (usuarioId <= 0)
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

            await ValidarBibliotecarioOAdministradorAsync(
                usuarioId,
                "Solo un bibliotecario o administrador puede actualizar recursos."
            );

            var recurso = await _recursos.BuscarConCategoriaAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var categoria = await _categorias.ObtenerporIdAsync(
                request.CategoriaId
            );

            if (categoria is null)
                throw new BusinessException("La categoría indicada no existe.");

            string tituloAnterior = recurso.Titulo;
            int cantidadEjemplaresAnterior = recurso.TotalEjemplares;

            recurso.ActualizarInformacion(
                titulo,
                autor,
                request.CategoriaId,
                request.AnioPublicado,
                imagenUrl,
                request.Descripcion
            );

            if (request.CantidadEjemplares > recurso.TotalEjemplares)
            {
                int ejemplaresNuevos = request.CantidadEjemplares - recurso.TotalEjemplares;
                recurso.AgregarEjemplares(ejemplaresNuevos);
            }

            await _recursos.ActualizarAsync(recurso);

            string detalleAuditoria = $"Se actualizó el libro '{recurso.Titulo}'. Título anterior: '{tituloAnterior}'.";
            if (recurso.TotalEjemplares > cantidadEjemplaresAnterior)
            {
                detalleAuditoria += $" Se incrementó la cantidad de ejemplares de {cantidadEjemplaresAnterior} a {recurso.TotalEjemplares}.";
            }

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Actualizar Recurso",
                EntidadAfectada: "RecursosBibliograficos",
                detalles: detalleAuditoria
            );

            await _db.SaveChangesAsync();
            return MapearRecurso(recurso, categoria.Nombre);
        }

        public async Task<RecursoResponse> CambiarEstadoRecursoAsync(
            CambiarEstadoRecursoRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los datos del cambio de estado");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            if (request.EjemplarId <= 0)
                throw new BusinessException("El ejemplar es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.NuevoEstado, "El nuevo estado del ejemplar");

            await ValidarBibliotecarioOAdministradorAsync(
                usuarioId,
                "Solo un bibliotecario o administrador puede cambiar estados de ejemplares."
            );

            var recurso = await _recursos.BuscarConCategoriaAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var ejemplar = recurso.Ejemplares.FirstOrDefault(
                e => e.EjemplarId == request.EjemplarId
            );

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
                    ejemplar.MarcarFueraDeServicio(
                        request.Motivo ?? "No especificado"
                    );
                    break;

                default:
                    throw new BusinessException("El estado indicado no es válido.");
            }

            await _recursos.ActualizarAsync(recurso);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Cambiar estado de ejemplar",
                EntidadAfectada: "RecursosBibliograficos",
                detalles: $"Se cambió el ejemplar ID {ejemplar.EjemplarId} del recurso ID {recurso.RecursoBibliograficoId} de {estadoAnterior} a {ejemplar.Estado}."
            );

            await _db.SaveChangesAsync();

            return MapearRecurso(recurso);
        }

        public async Task<IEnumerable<RecursoResponse>> ConsultarCatalogoAsync(
            ConsultarCatalogoRequest request)
        {
            Guard.NotNull(request, "Los filtros de consulta");

            string? titulo = string.IsNullOrWhiteSpace(request.Titulo)
                ? null
                : request.Titulo.Trim();

            string? autor = string.IsNullOrWhiteSpace(request.Autor)
                ? null
                : request.Autor.Trim();

            string? categoria = string.IsNullOrWhiteSpace(request.Categoria)
                ? null
                : request.Categoria.Trim();

            var recursos = await _recursos.ConsultarCatalogoAsync(
                titulo,
                autor,
                categoria,
                request.SoloDisponibles
            );

            return recursos
                .Select(MapearRecurso)
                .ToList();
        }

        public async Task<IEnumerable<RecursoResponse>> ConsultarTodosAsync()
        {
            var recursos = await _recursos.ObtenerTodosAsync();

            return recursos
                .Select(MapearRecurso)
                .ToList();
        }

        public async Task<RecursoResponse> ConsultarDetalleRecursoAsync(
            ConsultarDetalleRecursoRequest request)
        {
            Guard.NotNull(request, "Los datos de consulta del recurso");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            var recurso = await _recursos.BuscarConCategoriaAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            return MapearRecurso(recurso);
        }

        public async Task<IEnumerable<HistorialRecursoResponse>> ConsultarHistorialRecursoAsync(
            ConsultarHistorialRecursoRequest request)
        {
            Guard.NotNull(request, "Los datos de consulta del historial");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            var recurso = await _recursos.ObtenerporIdAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var registros = await _auditoriaRepositorio.ObtenerPorEntidadAsync(
                "RecursosBibliograficos"
            );

            string filtroId = $"ID {request.RecursoBibliograficoId}";

            var historial = registros
                .Where(r =>
                    !string.IsNullOrWhiteSpace(r.Detalle) &&
                    r.Detalle.Contains(filtroId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.FechaRegistro)
                .Select(r => new HistorialRecursoResponse
                {
                    AuditoriaId = r.AuditoriaId,
                    RecursoBibliograficoId = request.RecursoBibliograficoId,
                    TipoCambio = r.Accion,
                    Detalle = r.Detalle,
                    FechaRegistro = r.FechaRegistro,
                    UsuarioResponsableId = r.UsuarioId
                })
                .ToList();

            return historial;
        }

        public async Task EliminarRecursoAsync(
            EliminarRecursoRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los datos de eliminación del recurso");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            string motivo = string.IsNullOrWhiteSpace(request.Motivo)
                ? "No especificado"
                : request.Motivo.Trim();

            await ValidarBibliotecarioOAdministradorAsync(
                usuarioId,
                "Solo un bibliotecario o administrador puede desactivar recursos."
            );

            var recurso = await _recursos.BuscarConCategoriaAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            bool tienePrestamosActivos =
                await _prestamos.ExistePrestamoActivoPorRecursoAsync(
                    request.RecursoBibliograficoId
                );

            if (tienePrestamosActivos)
            {
                throw new BusinessException(
                    "No se puede desactivar el recurso porque tiene préstamos activos."
                );
            }

            recurso.Desactivar();
            if (recurso.Ejemplares != null)
            {
                foreach (var ejemplar in recurso.Ejemplares)
                {
                    ejemplar.MarcarFueraDeServicio($"Recurso desactivado. Motivo: '{motivo}'");
                }
            }

            await _recursos.ActualizarAsync(recurso);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Desactivar Recurso",
                EntidadAfectada: "RecursosBibliograficos",
                detalles: $"Se desactivó el recurso '{recurso.Titulo}' con ISBN {recurso.ISBN}. Motivo: '{motivo}'."
            );

            await _db.SaveChangesAsync();
        }

        private async Task<Usuario> ValidarBibliotecarioOAdministradorAsync(
            int usuarioId,
            string mensajeNoAutorizado)
        {
            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(
                usuarioId
            );

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException(mensajeNoAutorizado);

            return usuarioEjecutor;
        }

        private static RecursoResponse MapearRecurso(
            RecursoBibliografico recurso)
        {
            var primerDisponible = recurso.Ejemplares?
             .FirstOrDefault(e => e.Estado == EstadoEjemplar.Disponible);

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
                CopiasDisponibles = recurso.CopiasDisponibles,
                EjemplarDisponibleId = primerDisponible?.EjemplarId,
                Descripcion = recurso?.Descripcion
            };
        }

        private static RecursoResponse MapearRecurso(
            RecursoBibliografico recurso,
            string nombreCategoria)
        {
            var primerDisponible = recurso.Ejemplares?
            .FirstOrDefault(e => e.Estado == EstadoEjemplar.Disponible);
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
                CopiasDisponibles = recurso.CopiasDisponibles,
                EjemplarDisponibleId = primerDisponible?.EjemplarId,
                Descripcion = recurso.Descripcion
            };
        }

        public async Task<int?> ObtenerPrimerEjemplarDisponibleIdAsync(int recursoId)
        {
            
            var recurso = await _recursos.BuscarConCategoriaAsync(recursoId);

            if (recurso == null || recurso.Ejemplares == null)
                return null;

            // Busca la primera copia física cuyo estado sea Disponible
            var ejemplarDisponible = recurso.Ejemplares
                .FirstOrDefault(e => e.Estado == EstadoEjemplar.Disponible);

            return ejemplarDisponible?.EjemplarId; 
        }
    }
}