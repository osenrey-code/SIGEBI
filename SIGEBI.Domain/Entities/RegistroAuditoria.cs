using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class RegistroAuditoria
    {
        public Guid Id { get; private set; }

        public Guid? UsuarioId { get; private set; }

        public string Usuario { get; private set; } = string.Empty;

        public string Accion { get; private set; } = string.Empty;

        public string EntidadAfectada { get; private set; } = string.Empty;

        public Guid? EntidadId { get; private set; }

        public string Resultado { get; private set; } = string.Empty;

        public string Detalle { get; private set; } = string.Empty;

        public DateTime FechaRegistro { get; private set; }

        public string ValoresAnteriores { get; private set; } = string.Empty;

        public string ValoresNuevos { get; private set; } = string.Empty;

        private RegistroAuditoria() { }

        public RegistroAuditoria(
            Guid? usuarioId,
            string usuario,
            string accion,
            string entidadAfectada,
            Guid? entidadId,
            string resultado,
            string detalle,
            string valoresAnteriores,
            string valoresNuevos)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            Usuario = string.IsNullOrWhiteSpace(usuario) ? "Sistema/Anónimo" : usuario.Trim();
            Accion = string.IsNullOrWhiteSpace(accion) ? "Acción no especificada" : accion.Trim();
            EntidadAfectada = string.IsNullOrWhiteSpace(entidadAfectada) ? "No especificada" : entidadAfectada.Trim();
            EntidadId = entidadId;
            Resultado = string.IsNullOrWhiteSpace(resultado) ? "No especificado" : resultado.Trim();
            Detalle = string.IsNullOrWhiteSpace(detalle) ? string.Empty : detalle.Trim();
            FechaRegistro = DateTime.Now;
            ValoresAnteriores = valoresAnteriores ?? string.Empty;
            ValoresNuevos = valoresNuevos ?? string.Empty;
        }

        // Constructor viejo para no romper código que ya use RegistroAuditoria.
        public RegistroAuditoria(
            string usuario,
            string accion,
            string tablaAfectada,
            string valoresAnteriores,
            string valoresNuevos)
            : this(
                null,
                usuario,
                accion,
                tablaAfectada,
                null,
                "Exitoso",
                string.Empty,
                valoresAnteriores,
                valoresNuevos)
        {
        }
    }
}