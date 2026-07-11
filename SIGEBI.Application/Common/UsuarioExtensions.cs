using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Common
{
    public static class UsuarioExtensions
    {
        public static string ObtenerTipoUsuario(this Usuario? usuario)
        {
            return usuario switch
            {
                Estudiante => "Estudiante",
                Docente => "Docente",
                Bibliotecario => "Bibliotecario",
                Administrador => "Administrador",
                Auditor => "Auditor",
                _ => "Desconocido"
            };
        }

        public static bool EsAdministradorOAuditor(this Usuario? usuario)
        {
            return usuario is Administrador || usuario is Auditor;
        }

        public static bool PuedeGenerarReporteInventario(this Usuario? usuario)
        {
            return usuario is Bibliotecario ||
                   usuario is Administrador ||
                   usuario is Auditor;
        }
    }
}