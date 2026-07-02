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

        public async Task RegistrarUsuarioAsync(RegistrarUsuarioRequest request, int actorId)
        {
            var existe = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(request.Identificacion);

            if (existe != null)
                throw new BusinessException("El usuario ya esta registrado.");

            bool CorreoOcupado = await _usuarios.ExisteCorreoAsync(request.Correo);

            if (CorreoOcupado) throw new BusinessException("Ya existe un usuario registrado con este correo.");


            Usuario usuario = request.Tipo.ToLower() switch
            {
                "estudiante" => new Estudiante { Matricula = request.Identificacion, PassWord = _password.GenerarHash(request.Identificacion) },
                "docente" => new Docente { CodigoEmpleado = request.Identificacion, PassWord = _password.GenerarHash(request.Identificacion) },
                "administrador" => new Administrador { CodigoEmpleado = request.Identificacion, PassWord = _password.GenerarHash(request.Identificacion) },
                "bibliotecario" => new Bibliotecario { CodigoEmpleado = request.Identificacion , PassWord = _password.GenerarHash(request.Identificacion) },
                "auditor" => new Auditor { CodigoEmpleado = request.Identificacion , PassWord = _password.GenerarHash(request.Identificacion) },
                _ => throw new BusinessException("Usuario Inválido.")
            };

            usuario.NombreCompleto = request.NombreCompleto;
            usuario.Correo = request.Correo;
            usuario.Estado = EstadoUsuario.Activo;

            await _usuarios.AgregarAsync(usuario);
            await _auditoria.RegistrarAsync(
               UsuarioId: actorId,
               Accion: "Registrar Usuario",
               EntidadAfectada: "Usuarios",
               detalles: $"Se agregó el usuario {usuario.GetType().Name} con identificación {request.Identificacion}"
             );
        }
    }
} 

