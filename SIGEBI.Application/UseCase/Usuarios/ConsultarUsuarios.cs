using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class ConsultarUsuarios
    {
        private readonly IUsuario _usuarios;

        public ConsultarUsuarios(IUsuario usuario)
        {
            _usuarios = usuario;
        }


        public async Task<IEnumerable<UsuarioResponse>> ConsultarUsuariosAsync(ConsultarUsuariosRequest filtros)
        {
            var listaUsuarios = await _usuarios.ConsultarPorFiltrosAsync(
                filtros.nombre,
                filtros.TipoUsuario,
                filtros.Estado
            );

            return listaUsuarios.Select(u => new UsuarioResponse
            {
                UsuarioId = u.UsuarioId,
                Identificacion = ObtenerIdentificacion(u), 
                NombreCompleto = u.NombreCompleto,
                Correo = u.Correo,
                TipoUsuario = u.GetType().Name,
                Estado = u.Estado.ToString()
            });
        }

        // Método privado para saber si debemos leer la Matrícula o el Código de Empleado
        private string ObtenerIdentificacion(Usuario usuario)
        {
            return usuario switch
            {
                Estudiante e => e.Matricula,
                Docente d => d.CodigoEmpleado,
                Administrador a => a.CodigoEmpleado,
                Bibliotecario b => b.CodigoEmpleado,
                Auditor au => au.CodigoEmpleado,
                _ => string.Empty //por si ocurre un error de casteo
            };
        }
    }
}
