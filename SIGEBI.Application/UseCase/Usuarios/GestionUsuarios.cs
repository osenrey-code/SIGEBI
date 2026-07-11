using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class GestionUsuarios : IGestionUsuariosUseCase
    {
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioPassword _password;
        private readonly IServicioToken _token;

        public GestionUsuarios(
            IUsuario usuarios,
            IAuditoriaService auditoria,
            IServicioPassword password,
            IServicioToken token)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
            _password = password;
            _token = token;
        }

        public async Task<UsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request, int usuarioId)
        {
            Guard.NotNull(request, "Los datos del usuario");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario que realiza la acción es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación del usuario");
            Guard.NotNullOrWhiteSpace(request.NombreCompleto, "El nombre completo del usuario");
            Guard.NotNullOrWhiteSpace(request.Correo, "El correo del usuario");
            Guard.NotNullOrWhiteSpace(request.Tipo, "El tipo de usuario");

            string identificacion = request.Identificacion.Trim();
            string nombreCompleto = request.NombreCompleto.Trim();
            string correo = request.Correo.Trim();
            string tipo = request.Tipo.Trim().ToLower();

            await ValidarActorAdministradorAsync(
                usuarioId,
                "Solo un administrador puede registrar usuarios."
            );

            var usuarioExistente = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                identificacion
            );

            if (usuarioExistente is not null)
                throw new BusinessException("El usuario ya está registrado.");

            bool correoOcupado = await _usuarios.ExisteCorreoAsync(correo);

            if (correoOcupado)
                throw new BusinessException("Ya existe un usuario registrado con este correo.");

            string passwordInicial = _password.GenerarHash(identificacion);

            Usuario usuario = CrearUsuarioPorTipo(
                tipo,
                identificacion,
                passwordInicial
            );

            usuario.NombreCompleto = nombreCompleto;
            usuario.Correo = correo;
            usuario.Estado = EstadoUsuario.Activo;

            await _usuarios.AgregarAsync(usuario);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Registrar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se agregó el usuario {usuario.GetType().Name} con identificación {identificacion}."
            );

            return MapearUsuario(usuario);
        }

        public async Task<UsuarioResponse> ActualizarUsuarioAsync(
            ActualizarUsuarioRequest request,
            int actorId)
        {
            Guard.NotNull(request, "Los datos del usuario");

            if (actorId <= 0)
                throw new BusinessException("El usuario que realiza la acción es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación del usuario");
            Guard.NotNullOrWhiteSpace(request.NombreCompleto, "El nombre completo del usuario");

            string identificacion = request.Identificacion.Trim();
            string nombreCompleto = request.NombreCompleto.Trim();

            await ValidarActorAdministradorAsync(
                actorId,
                "Solo un administrador puede actualizar usuarios."
            );

            var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                identificacion
            );

            if (usuario is null)
                throw new BusinessException("No existe un usuario registrado con esta identificación.");

            string nombreAnterior = usuario.NombreCompleto;

            usuario.Actualizar(nombreCompleto);

            await _usuarios.ActualizarAsync(usuario);

            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Actualizar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se actualizó el usuario ({identificacion}). Nombre anterior: '{nombreAnterior}', nuevo nombre: '{usuario.NombreCompleto}'."
            );

            return MapearUsuario(usuario);
        }

        public async Task DesactivarUsuarioAsync(
            DesactivarUsuarioRequest request,
            int actorId)
        {
            Guard.NotNull(request, "Los datos de desactivación");

            if (actorId <= 0)
                throw new BusinessException("El usuario que realiza la acción es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación del usuario");
            Guard.NotNullOrWhiteSpace(request.Motivo, "El motivo de desactivación");

            string identificacion = request.Identificacion.Trim();
            string motivo = request.Motivo.Trim();

            await ValidarActorAdministradorAsync(
                actorId,
                "Solo un administrador puede desactivar usuarios."
            );

            var usuario = await _usuarios.ObtenerUsuarioConDetallesAsync(
                identificacion
            );

            if (usuario is null)
                throw new BusinessException("No existe un usuario registrado con esta identificación.");

            if (usuario.UsuarioId == actorId)
                throw new BusinessException("Un administrador no puede desactivarse a sí mismo.");

            if (usuario.Estado == EstadoUsuario.Inactivo)
                throw new BusinessException("El usuario ya se encuentra inactivo.");

            bool tienePrestamosActivos = usuario.Prestamos is not null &&
                usuario.Prestamos.Any(p => p.Estado == EstadoPrestamo.Activo);

            if (tienePrestamosActivos)
                throw new BusinessException("No se puede desactivar al usuario porque tiene préstamos activos pendientes de devolución.");

            usuario.Estado = EstadoUsuario.Inactivo;

            await _usuarios.ActualizarAsync(usuario);

            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Desactivar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se desactivó el usuario con identificación {identificacion}. Motivo: '{motivo}'."
            );
        }

        public async Task<IEnumerable<UsuarioResponse>> ConsultarUsuariosAsync(
            ConsultarUsuariosRequest filtros)
        {
            if (filtros is null)
                throw new BusinessException("Los filtros de consulta son obligatorios.");

            string? nombre = string.IsNullOrWhiteSpace(filtros.nombre)
                ? null
                : filtros.nombre.Trim();

            string? tipoUsuario = string.IsNullOrWhiteSpace(filtros.TipoUsuario)
                ? null
                : filtros.TipoUsuario.Trim();

            string? estado = string.IsNullOrWhiteSpace(filtros.Estado)
                ? null
                : filtros.Estado.Trim();

            var listaUsuarios = await _usuarios.ConsultarPorFiltrosAsync(
                nombre,
                tipoUsuario,
                estado
            );

            if (listaUsuarios is null)
                return Enumerable.Empty<UsuarioResponse>();

            var usuariosValidos = listaUsuarios
                .Where(u => u is not null)
                .Cast<Usuario>();

            return usuariosValidos
                .Select(MapearUsuario)
                .ToList();
        }

        public async Task<LoginResponse> AutenticarUsuarioAsync(
            LoginRequest request)
        {
            Guard.NotNull(request, "Los datos de inicio de sesión");
            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación");
            Guard.NotNullOrWhiteSpace(request.Password, "La contraseña");

            string identificacion = request.Identificacion.Trim();
            string password = request.Password.Trim();

            var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                identificacion
            );

            if (usuario is null)
                throw new BusinessException("Credenciales inválidas.");

            if (usuario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("La cuenta de usuario no está activa.");

            bool passwordValido = _password.VerificarPassword(
                password,
                usuario.PassWord
            );

            if (!passwordValido)
            {
                await _auditoria.RegistrarAsync(
                    UsuarioId: usuario.UsuarioId,
                    Accion: "Inicio de sesión fallido",
                    EntidadAfectada: "Usuarios",
                    detalles: $"Intento fallido con identificación: {identificacion}."
                );

                throw new BusinessException("Credenciales inválidas.");
            }

            string tipoUsuarioReal = usuario.GetType().Name;

            string token = _token.GenerarToken(
                usuario.UsuarioId,
                usuario.NombreCompleto,
                usuario.Correo,
                tipoUsuarioReal
            );

            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Inicio de sesión",
                EntidadAfectada: "Usuarios",
                detalles: $"El usuario '{usuario.NombreCompleto}' inició sesión correctamente."
            );

            return new LoginResponse
            {
                UsuarioId = usuario.UsuarioId,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = tipoUsuarioReal,
                Token = token
            };
        }

        private async Task<Usuario> ValidarActorAdministradorAsync(
            int actorId,
            string mensajeNoAutorizado)
        {
            var actor = await _usuarios.ObtenerporIdAsync(actorId);

            if (actor is null)
                throw new BusinessException("El usuario que realiza la acción no existe.");

            if (actor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario que realiza la acción no está activo.");

            if (actor is not Administrador)
                throw new BusinessException(mensajeNoAutorizado);

            return actor;
        }

        private static Usuario CrearUsuarioPorTipo(
            string tipo,
            string identificacion,
            string passwordInicial)
        {
            return tipo switch
            {
                "estudiante" => new Estudiante
                {
                    Matricula = identificacion,
                    PassWord = passwordInicial
                },

                "docente" => new Docente
                {
                    CodigoEmpleado = identificacion,
                    PassWord = passwordInicial
                },

                "administrador" => new Administrador
                {
                    CodigoEmpleado = identificacion,
                    PassWord = passwordInicial
                },

                "bibliotecario" => new Bibliotecario
                {
                    CodigoEmpleado = identificacion,
                    PassWord = passwordInicial
                },

                "auditor" => new Auditor
                {
                    CodigoEmpleado = identificacion,
                    PassWord = passwordInicial
                },

                _ => throw new BusinessException("Tipo de usuario inválido.")
            };
        }

        private static UsuarioResponse MapearUsuario(
            Usuario usuario)
        {
            return new UsuarioResponse
            {
                UsuarioId = usuario.UsuarioId,
                Identificacion = ObtenerIdentificacion(usuario),
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = usuario.GetType().Name,
                Estado = usuario.Estado.ToString()
            };
        }

        private static string ObtenerIdentificacion(
            Usuario usuario)
        {
            return usuario switch
            {
                Estudiante estudiante => estudiante.Matricula,
                Docente docente => docente.CodigoEmpleado,
                Administrador administrador => administrador.CodigoEmpleado,
                Bibliotecario bibliotecario => bibliotecario.CodigoEmpleado,
                Auditor auditor => auditor.CodigoEmpleado,
                _ => string.Empty
            };
        }
    }
}