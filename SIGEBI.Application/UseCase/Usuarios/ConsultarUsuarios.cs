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
