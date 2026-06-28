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

        public async Task<ResultadoOperacionResponse<List<UsuarioResponse>>> EjecutarAsync(
            ConsultarUsuariosRequest request)
        {
            var usuarios = await _usuarios.ObtenerTodosAsync();

            //Buscar por nombre
            if (!string.IsNullOrWhiteSpace(request.Nombre))
            {
                usuarios = usuarios.Where(u => u.NombreCompleto.Contains(
                    request.Nombre, StringComparison.OrdinalIgnoreCase));
            }

            //Buscar por tipo de usuario
            if (!string.IsNullOrWhiteSpace(request.TipoUsuario))
            {
                if (!Enum.TryParse<TipoUsuario>(request.TipoUsuario, true, out var tipoUsuario))
                {
                    return ResultadoOperacionResponse<List<UsuarioResponse>>.Error("El tipo de usuario no es válido.");
                }

                usuarios = usuarios.Where(u => u.Tipo == tipoUsuario);
            }

            //Buscar por estado
            if (!string.IsNullOrWhiteSpace(request.Estado))
            {
                if (!Enum.TryParse<EstadoUsuario>(request.Estado, true, out var estadoUsuario))
                {
                    return ResultadoOperacionResponse<List<UsuarioResponse>>.Error("El estado de usuario no es válido.");
                }
                usuarios = usuarios.Where(u => u.Estado == estadoUsuario);
            }

            var response = usuarios.Select(MapearUsuario).ToList();
            return ResultadoOperacionResponse<List<UsuarioResponse>>.Ok("Usuarios encontrados", response);
        }

        private static UsuarioResponse MapearUsuario(Usuario usuario)
        {
            return new UsuarioResponse
            {
                UsuarioId = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                DocumentoIdentidad = usuario.Identificacion,
                Correo = usuario.Correo,
                TipoUsuario = usuario.Tipo.ToString(),
                Estado = usuario.Estado.ToString()
            };
        }
    }
}
