using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class GestionDevoluciones : IGestionDevolucionesUseCase
    {
        private const decimal MontoMoraPorDia = 25m;
        private const decimal MontoPenalizacionDano = 500m;

        private readonly IRepositorioPrestamo _prestamos;
        private readonly IEjemplarRepository _ejemplares;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly IRepositorioDevolucion _devoluciones;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioNotificacion _notificaciones;
        private readonly IApplicationDbContext _db;

        public GestionDevoluciones(
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            IRepositorioDevolucion devoluciones,
            IEjemplarRepository ejemplares,
            IAuditoriaService auditoria,
            IServicioNotificacion notificaciones, IApplicationDbContext db)
        {
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _devoluciones = devoluciones;
            _ejemplares = ejemplares;
            _auditoria = auditoria;
            _notificaciones = notificaciones;
            _db = db;
        }

        public async Task<DevolucionResponse> RegistrarDevolucionAsync(
            RegistrarDevolucionRequest request,
            int bibliotecarioId)
        {
            Guard.NotNull(request, "Los datos de la devolución");

            if (bibliotecarioId <= 0)
                throw new BusinessException("El bibliotecario responsable es obligatorio.");

            if (request.PrestamoId <= 0)
                throw new BusinessException("El préstamo es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Condicion, "La condición del recurso");

            string condicion = request.Condicion.Trim();

            string? observacion = string.IsNullOrWhiteSpace(request.Observacion)
                ? null
                : request.Observacion.Trim();

            var bibliotecario = await _usuarios.ObtenerporIdAsync(
                bibliotecarioId
            );

            if (bibliotecario is null)
                throw new BusinessException("El bibliotecario responsable no existe.");

            if (bibliotecario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El bibliotecario responsable no está activo.");

            if (bibliotecario is not Bibliotecario && bibliotecario is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede registrar devoluciones.");

            var prestamo = await _prestamos.ObtenerConDetallesAsync(
                request.PrestamoId
            );

            if (prestamo is null)
                throw new BusinessException("El préstamo especificado no existe.");

            if (prestamo.Estado != EstadoPrestamo.Activo)
                throw new BusinessException("Solo se puede registrar devolución de un préstamo activo.");

            if (prestamo.Ejemplar is null)
                throw new BusinessException("El ejemplar físico asociado al préstamo no existe.");

            var devolucionExistente = await _devoluciones.ObtenerPorPrestamoIdAsync(
                prestamo.PrestamoId
            );

            if (devolucionExistente is not null)
                throw new BusinessException("Ya existe una devolución registrada para este préstamo.");

            var nuevaDevolucion = new Devolucion(
                prestamoId: prestamo.PrestamoId,
                bibliotecarioId: bibliotecarioId,
                condicion: condicion,
                observacion: observacion
            );

            int diasRetraso = prestamo.CalcularDiasRetraso(
                nuevaDevolucion.FechaDevolucion
            );

            if (diasRetraso < 0)
            {
                diasRetraso = 0;
            }
            bool tieneRetraso = diasRetraso > 0;

            bool penalizaPorCondicion = condicion.Equals("Deteriorado", StringComparison.OrdinalIgnoreCase) ||
                                        condicion.Equals("Inservible / Perdido", StringComparison.OrdinalIgnoreCase);

            bool requiereRetiroDeServicio = nuevaDevolucion.RequiereRetiro();

            decimal montoPenalizacion = 0;
            bool penalizacionGenerada = false;
            string motivoPenalizacion = string.Empty;

            prestamo.MarcarComoDevuelto();

            prestamo.Ejemplar.RegistrarDevolucion(
                observacion
            );

            if (requiereRetiroDeServicio)
            {
                prestamo.Ejemplar.MarcarFueraDeServicio(
                    $"Retirado por condición: {condicion}. {observacion ?? "Sin observación adicional"}"
                );
            }

            if (tieneRetraso || penalizaPorCondicion)
            {
                if (tieneRetraso)
                {
                    montoPenalizacion += diasRetraso * MontoMoraPorDia;
                    motivoPenalizacion += $"Devolución tardía de {diasRetraso} día(s). ";
                }

                if (penalizaPorCondicion)
                {
                    montoPenalizacion += MontoPenalizacionDano;
                    motivoPenalizacion += $"Recurso devuelto en mala condición ({condicion}).";
                }

                var penalizacion = new Penalizacion(
                    usuarioId: prestamo.UsuarioId,
                    prestamoId: prestamo.PrestamoId,
                    diasRetraso: diasRetraso,
                    montoMora: montoPenalizacion,
                    motivo: motivoPenalizacion.Trim()
                );

                await _penalizaciones.AgregarAsync(penalizacion);

                await _notificaciones.EnviarNotificacionAsync(
                    penalizacion.UsuarioId,
                    $"Se ha generado una penalización por su préstamo del recurso #{prestamo.Ejemplar.RecursoBibliografico?.Titulo}." +
                    $" Motivo: {motivoPenalizacion.Trim()}",
                    TipoNotificacion.PenalizacionGenerada
                );

                penalizacionGenerada = true;
            }

            await _devoluciones.AgregarAsync(
                nuevaDevolucion
            );

            await _prestamos.ActualizarAsync(
                prestamo
            );

            await _ejemplares.ActualizarAsync(
                prestamo.Ejemplar
            );

            string tituloRecurso = prestamo.Ejemplar.RecursoBibliografico?.Titulo
                ?? "Recurso";

            await _auditoria.RegistrarAsync(
                UsuarioId: bibliotecarioId,
                Accion: "Registrar Devolución",
                EntidadAfectada: "Devoluciones",
                detalles: $"Se registró la devolución del libro '{prestamo.Ejemplar?.RecursoBibliografico?.Titulo}'" +
                $"devuelto por el usuario '{prestamo.Usuario?.NombreCompleto}'."
            );

            await _db.SaveChangesAsync();

            return new DevolucionResponse
            {
                PrestamoId = prestamo.PrestamoId,
                TituloRecurso = tituloRecurso,
                FechaDevolucion = nuevaDevolucion.FechaDevolucion,
                DiasRetraso = diasRetraso,
                Condicion = nuevaDevolucion.Condicion,
                PenalizacionGenerada = penalizacionGenerada,
                MontoPenalizacion = montoPenalizacion,
                Mensaje = penalizacionGenerada
                    ? $"Devolución registrada con penalización de RD$ {montoPenalizacion:N2}. Motivo: {motivoPenalizacion.Trim()}"
                    : "Devolución registrada exitosamente sin penalización."
            };
        }

        public async Task<IEnumerable<DevolucionResponse>> ConsultarHistorialAsync(
            ConsultarHistorialDevolucionesRequest request)
        {
            Guard.NotNull(request, "Los filtros del historial de devoluciones");

            if (request.UsuarioId.HasValue && request.UsuarioId.Value <= 0)
                throw new BusinessException("El usuario no existe.");

            if (request.RecursoBibliograficoId.HasValue && request.RecursoBibliograficoId.Value <= 0)
                throw new BusinessException("El recurso bibliográfico no existe.");

            if (request.EjemplarId.HasValue && request.EjemplarId.Value <= 0)
                throw new BusinessException("El ejemplar debe ser mayor que cero.");

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                throw new BusinessException("La fecha de inicio no puede ser mayor que la fecha final.");
            }

            var resultados = await _devoluciones.ConsultarHistorialAsync(
                request.UsuarioId,
                request.RecursoBibliograficoId,
                request.EjemplarId,
                request.FechaInicio,
                request.FechaFin
            );
            return resultados
                .Select(MapearDevolucion)
                .ToList();
        }

        private static DevolucionResponse MapearDevolucion(
            Devolucion devolucion)
        {
            int diasRetraso = devolucion.Prestamo is not null
                ? devolucion.Prestamo.CalcularDiasRetraso(devolucion.FechaDevolucion)
                : 0;

            bool penalizaPorCondicion = devolucion.Condicion.Equals("Deteriorado", StringComparison.OrdinalIgnoreCase) ||
                                         devolucion.Condicion.Equals("Inservible / Perdido", StringComparison.OrdinalIgnoreCase);

            decimal montoPenalizacion = 0;
            string mensaje = string.Empty;

            if (diasRetraso > 0)
            {
                montoPenalizacion += diasRetraso * MontoMoraPorDia;
                mensaje += $"Devolución tardía ({diasRetraso} días). ";
            }

            if (penalizaPorCondicion)
            {
                montoPenalizacion += MontoPenalizacionDano;
                mensaje += $"Recurso con daños ({devolucion.Condicion}). ";
            }

            if (diasRetraso == 0 && !penalizaPorCondicion)
            {
                mensaje = "Devolución realizada dentro del plazo y en buena condición.";
            }

            string tituloRecurso = devolucion.Prestamo?.Ejemplar?.RecursoBibliografico?.Titulo
                ?? "Recurso no especificado";

            string nombreUsuario = devolucion.Prestamo?.Usuario?.NombreCompleto ?? "Usuario Desconocido";

            string identificacion = "N/A";

            if (devolucion.Prestamo?.Usuario is Estudiante est)
            {
                identificacion = est.Matricula;
            }
            else if (devolucion.Prestamo?.Usuario is Docente doc)
            {
                identificacion = doc.CodigoEmpleado;
            }



            return new DevolucionResponse
            {
                PrestamoId = devolucion.PrestamoId,
                TituloRecurso = tituloRecurso,
                FechaDevolucion = devolucion.FechaDevolucion,
                NombreUsuario = nombreUsuario,
                IdentificacionUsuario = identificacion,
                DiasRetraso = diasRetraso,
                Observacion = devolucion.Observacion ?? string.Empty,
                Condicion = devolucion.Condicion,
                PenalizacionGenerada = diasRetraso > 0 || penalizaPorCondicion,
                MontoPenalizacion = montoPenalizacion,
                Mensaje = mensaje.Trim()
            };
        }
    }
}