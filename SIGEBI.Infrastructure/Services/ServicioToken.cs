using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Infrastructure.Services
{
    public class ServicioToken : IServicioToken
    {
        private readonly IConfiguration _configuration;

        public ServicioToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(int usuarioId, string nombreCompleto, string correo, string tipoUsuario)
        {
            var clave = _configuration["Jwt:Key"]
                ?? "SIGEBI_CLAVE_TEMPORAL_DESARROLLO_CAMBIAR_EN_PRODUCCION_2026";

            var issuer = _configuration["Jwt:Issuer"] ?? "SIGEBI.Api";
            var audience = _configuration["Jwt:Audience"] ?? "SIGEBI.Clients";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim(ClaimTypes.Name, nombreCompleto),
                new Claim(ClaimTypes.Email, correo),
                new Claim(ClaimTypes.Role, tipoUsuario)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(clave)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}