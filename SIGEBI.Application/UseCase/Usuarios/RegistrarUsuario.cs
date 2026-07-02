using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Domain.Exceptions;
using System.Net.Http.Headers;

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

        public async Task RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            var existe = await _usuarios.ObtenerUsuarioPorIdentificacion(request.Identifiacion);

            if (existe != null)
                throw new BusinessException("El usuario ya esta registrado.");

            var existeCorreo = await _usuarios.ListarTodosAsync();

            if (existeCorreo != null)
                throw new BusinessException("Ya existe un usuario registrado con este correo electrónico.");

            Usuario usuario = request.Tipo.ToLower() switch
            {
                "estudiante" => new Estudiante { Matricula = request.Matricula },
                "docente" => new Docente { CodigoEmpleado = request.CodigoEmpleado },
                "administrador" => new Administrador { CodigoEmpleado = request.CodigoEmpleado },
                "bibliotecario" => new Bibliotecario { CodigoEmpleado = request.CodigoEmpleado },
                "auditor" => new Auditor { CodigoEmpleado = request.CodigoEmpleado },
                _ => throw new BusinessException("Usuario Inválido.")
            };

            usuario.UsuarioId = request.Identifiacion;
            usuario.NombreCompleto = request.NombreCompleto;
            usuario.Correo = request.Correo;
            usuario.Estado = EstadoUsuario.Activo;
            usuario.PassWord = request.Identifiacion;

            await _usuarios.AgregarAsync(usuario);
            await _auditoria.RegistrarAsync(
               UsuarioId: request.Identificacion,
               Accion: "Registar Usuario",
               EntidadAfectada: "Usuarios",
               detalles: $"Se agrego el usuario {usuario}"
             );
        }
    }
} 

