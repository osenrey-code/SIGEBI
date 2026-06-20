using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{

    public class PerfilLector
    {
        public Guid Id { get; private set; }
        public Guid UsuarioId { get; private set; }
        public int LimitePrestamos { get; private set; }
        public int DiasPrestamosPermitidos { get; private set; }

        public List<Prestamo> prestamos { get; private set; } = new();
        public List<Penalizacion> Penalizaciones { get; private set; } = new();

        private PerfilLector() { }

        public PerfilLector(Guid UsuarioId, int LimitePrestamos, int DiasPrestamoPermitidos)
        {
            Id = Guid.NewGuid();
            this.UsuarioId = UsuarioId;
            this.LimitePrestamos = LimitePrestamos;
            this.DiasPrestamosPermitidos = DiasPrestamoPermitidos;
        }

        public bool PuedeSolicitarPrestamo(int cantidadPerstamosActivos)
        {
            if (cantidadPerstamosActivos >= LimitePrestamos) { return false; }
            return true;
        }
    }
}
