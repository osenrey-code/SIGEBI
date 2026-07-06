using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class RechazarPrestamo
    {

        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly ISolicitudRepository _solicitud;

        public RechazarPrestamo(IUsuario usuarios, IAuditoriaService auditoria,
            ISolicitudRepository solicitud)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
            _solicitud = solicitud;
        }

        public async Task<SolicitudResponse> RechazarPrestamoAsync(RechazarSolicitudRequest request, string Identificacion)
        {
            var solicitud = await _solicitud.ObtenerConDetallesAsync(request.SolicitudId);

            if (solicitud == null) throw new BusinessException("La solicitud especificada no existe.");

            solicitud.Rechazar(request.Motivo);

            await _solicitud.ActualizarAsync(solicitud);

            string nombreUsuario = solicitud.Usuario?.NombreCompleto ?? "Usuario";
            string correoUsuario = solicitud.Usuario?.Correo ?? string.Empty;
            string tituloLibro = solicitud.Ejemplar?.RecursoBibliografico?.Titulo ?? "Recurso desconocido";
            string identificadorEjemplar = solicitud.Ejemplar?.Identificador ?? "N/A";


            return new SolicitudResponse
            {
                SolicitudId = solicitud.SolicitudId,
                TituloRecurso = tituloLibro,
                IdentificadorEjemplar = identificadorEjemplar,
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado.ToString(),
                MotivoRechazo = solicitud.MotivoRechazo

            };
            


        }

      
    }
}
