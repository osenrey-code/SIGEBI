using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Auditory
{
    public class ConsultarLogAuditoria : ILogAuditoria
    {
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IUsuario _usuarios;

        public ConsultarLogAuditoria(IRepositorioAuditoria auditoria, IUsuario usuarios)
        {
            _auditoria = auditoria;
            _usuarios = usuarios;
        }

        public async Task<IEnumerable<LogAuditoriaResponse>> ConsultarAuditoriaLog(
            ConsultarLogAuditoriaRequest request, int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Administrador && usuarioEjecutor is not Auditor)
                throw new BusinessException("Solo un administrador o auditor puede consultar los registros de auditoría.");

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                throw new BusinessException("La fecha de inicio no puede ser mayor que la fecha final.");
            }

            var registros = (await _auditoria.ConsultarAsync(
                request.Identificacion,
                request.Accion,
                request.EntidadAfectada,
                request.FechaInicio,
                request.FechaFin

            )).ToList();

            
            var userIds = registros.Select(r => r.UsuarioId).Distinct().ToList();
            var usuariosDict = new Dictionary<int, (string NombreCompleto, string Identificacion)>();

            foreach (var id in userIds)
            {
                var user = await _usuarios.ObtenerporIdAsync(id);
                if (user != null)
                {
                    string identificacion = user switch
                    {
                        Estudiante est => est.Matricula,
                        Docente doc => doc.CodigoEmpleado,
                        Administrador admin => admin.CodigoEmpleado,
                        Bibliotecario biblio => biblio.CodigoEmpleado,
                        Auditor auditor => auditor.CodigoEmpleado,
                        _ => id.ToString()
                    };

                    usuariosDict[id] = (user.NombreCompleto, identificacion);
                }
                else
                {
                    usuariosDict[id] = ("Usuario Desconocido", "N/A");
                }
            }


            var resultado = registros.Select(registro => new LogAuditoriaResponse
            {
                AuditoriaId = registro.AuditoriaId,
                UsuarioId = registro.UsuarioId,
                NombreCompleto = usuariosDict.ContainsKey(registro.UsuarioId) ? usuariosDict[registro.UsuarioId].NombreCompleto : "Usuario Desconocido",
                Identificacion = usuariosDict.ContainsKey(registro.UsuarioId) ? usuariosDict[registro.UsuarioId].Identificacion : "N/A",
                Accion = registro.Accion,
                EntidadAfectada = registro.EntidadAfectada,
                Detalle = registro.Detalle,
                FechaRegistro = registro.FechaRegistro
            }).ToList();

 
            if (!string.IsNullOrWhiteSpace(request.Identificacion))
            {
                string filtro = request.Identificacion.Trim();
                resultado = resultado
                    .Where(r => r.Identificacion.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                                r.NombreCompleto.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return resultado;
        }
    }

}
