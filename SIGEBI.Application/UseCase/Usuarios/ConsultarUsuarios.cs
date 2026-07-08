using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

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
            if (filtros is null)
                throw new BusinessException("Los filtros de consulta son obligatorios.");

            var listaUsuarios = await _usuarios.ConsultarPorFiltrosAsync(
                filtros.nombre,
                filtros.TipoUsuario,
                filtros.Estado
            );

            if (listaUsuarios is null)
                return Enumerable.Empty<UsuarioResponse>();

            var usuariosValidos = listaUsuarios
                .Where(u => u is not null)
                .Cast<Usuario>();

            return usuariosValidos.Select(usuario => new UsuarioResponse
            {
                UsuarioId = usuario.UsuarioId,
                Identificacion = ObtenerIdentificacion(usuario),
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = usuario.GetType().Name,
                Estado = usuario.Estado.ToString()
            }).ToList();
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
