using SIGEBI.AppEscritorio.Dtos.Auditorias;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Auditoria
{
    public partial class FrmDetalleAuditoria : Form
    {
        private readonly LogAuditoriaResponseDto _log;

        public FrmDetalleAuditoria(LogAuditoriaResponseDto log)
        {
            InitializeComponent();
            _log = log;

            HabilitarArrastre(pnlTopBar);
            AplicarBordesRedondeados();

            CargarDatos();
        }

        #region Arrastre y Bordes Redondeados
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void AplicarBordesRedondeados()
        {
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 14, 18));
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void HabilitarArrastre(Control control)
        {
            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 0x2, 0);
                }
            };
        }
        #endregion

        private void CargarDatos()
        {
            lblValIdentificacion.Text = string.IsNullOrWhiteSpace(_log.Identificacion) ? "N/A" : _log.Identificacion;
            lblValUsuario.Text = string.IsNullOrWhiteSpace(_log.NombreCompleto) ? "N/A" : _log.NombreCompleto;
            lblValAccion.Text = string.IsNullOrWhiteSpace(_log.Accion) ? "N/A" : _log.Accion;
            lblValEntidad.Text = string.IsNullOrWhiteSpace(_log.EntidadAfectada) ? "N/A" : _log.EntidadAfectada;
            lblValFecha.Text = _log.FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss");
            txtValDetalle.Text = string.IsNullOrWhiteSpace(_log.Detalle) ? "Sin detalles adicionales." : _log.Detalle;
        }

        private void btnCerrar_Click(object sender, EventArgs e) => this.Close();
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}