using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class RegistrarUsuario
    {
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioPassword _password;

        public RegistrarUsuario(IUsuario usuarios, IAuditoriaService auditoria, IServicioPassword password)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
            _password = password;
        }

        public async Task<UsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            Guard.NotNull(request, "Los datos del usuario");

            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación del usuario");
            Guard.NotNullOrWhiteSpace(request.NombreCompleto, "El nombre completo del usuario");
            Guard.NotNullOrWhiteSpace(request.Correo, "El correo del usuario");
            Guard.NotNullOrWhiteSpace(request.Tipo, "El tipo de usuario");

            string identificacion = request.Identificacion.Trim();
            string nombreCompleto = request.NombreCompleto.Trim();
            string correo = request.Correo.Trim();
            string tipo = request.Tipo.Trim().ToLower();


            var usuarioExistente = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                identificacion
            );

            if (usuarioExistente is not null)
                throw new BusinessException("El usuario ya está registrado.");

            bool correoOcupado = await _usuarios.ExisteCorreoAsync(correo);

            if (correoOcupado)
                throw new BusinessException("Ya existe un usuario registrado con este correo.");

            string passwordInicial = _password.GenerarHash(identificacion);

            Usuario usuario = tipo switch
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

            usuario.NombreCompleto = nombreCompleto;
            usuario.Correo = correo;
            usuario.Estado = EstadoUsuario.Activo;

            await _usuarios.AgregarAsync(usuario);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Registrar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se agregó el usuario {usuario.GetType().Name} con identificación {identificacion}."
            );

            return MapearUsuario(usuario);
        }

        private static UsuarioResponse MapearUsuario(Usuario usuario)
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

        private static string ObtenerIdentificacion(Usuario usuario)
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


