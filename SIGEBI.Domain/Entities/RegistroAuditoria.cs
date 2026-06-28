using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class RegistroAuditoria
    {
        public Guid Id { get; private set; }
        public string Usuario { get; private set; } = string.Empty;
        public string Accion { get; private set; } = string.Empty;
        public string TablaAfectada { get; private set; } = string.Empty;
        public DateTime FechaRegistro { get; private set; }
        public string ValoresAnteriores { get; private set; } = string.Empty;
        public string ValoresNuevos { get; private set; } = string.Empty;

        private RegistroAuditoria() { }

        public RegistroAuditoria(string Usuario, string Accion, string TablaAfectada, string ValoresAnteriores,
            string ValoresNuevos)
        {
            Id = Guid.NewGuid();
            Usuario = string.IsNullOrWhiteSpace(Usuario) ? "Sitema/Anónimo" : Usuario;
            this.Accion = Accion;
            this.TablaAfectada = TablaAfectada;
            FechaRegistro = DateTime.Now;
            this.ValoresAnteriores = ValoresAnteriores;
            this.ValoresNuevos = ValoresNuevos;
        }
        
    }
}
