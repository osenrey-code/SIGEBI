using SIGEBI.AppEscritorio.Dtos.Penalizaciones;
using SIGEBI.AppEscritorio.Services.Penalizaciones;
using SIGEBI.AppEscritorio.Views.Penalizaciones;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Penalizacion
{
    public partial class DetallePenalizacion : Form
    {
        private readonly IPenalizacionService _penalizacionService;
        private PenalizacionDto? _penalizacionActual;

        public DetallePenalizacion(IPenalizacionService penalizacionService)
        {
            InitializeComponent();
            _penalizacionService = penalizacionService;

            HabilitarArrastre(pnlTopBar);
            AplicarBordesRedondeados();
            AplicarEstilosDarkSlate();
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

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);      // #0F172A
            Color verdeEsmeralda = Color.FromArgb(22, 163, 74); // #16A34A
            Color colorBotonGris = Color.FromArgb(51, 65, 85); // #334155

            this.BackColor = fondoDark;

            // Botón Resolver Penalización
            btnResolver.BackColor = verdeEsmeralda;
            btnResolver.ForeColor = Color.White;
            btnResolver.FlatStyle = FlatStyle.Flat;
            btnResolver.FlatAppearance.BorderSize = 0;
            btnResolver.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnResolver.Cursor = Cursors.Hand;

            // Botón Cancelar / Cerrar
            btnCerrar.BackColor = colorBotonGris;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCerrar.Cursor = Cursors.Hand;
        }

        public void CargarPenalizacion(PenalizacionDto penalizacion)
        {
            _penalizacionActual = penalizacion;

            lblTitulo.Text = "⚠️ Detalle de la Penalización";

            string nombreUsuario = !string.IsNullOrWhiteSpace(penalizacion.NombreUsuario) ? penalizacion.NombreUsuario : "Usuario Desconocido";
            string identificacion = !string.IsNullOrWhiteSpace(penalizacion.IdentificacionUsuario) ? penalizacion.IdentificacionUsuario : "N/A";
            string motivo = !string.IsNullOrWhiteSpace(penalizacion.Motivo) ? penalizacion.Motivo : "Sin motivo especificado";
            string estado = !string.IsNullOrWhiteSpace(penalizacion.Estado) ? penalizacion.Estado.ToUpper() : "ACTIVA";

            // Encabezado con datos del Lector
            lblDetalleInfo.Text = $"👤 Lector: {nombreUsuario}  ({identificacion})";

            // Texto de resolución si aplica
            string infoResolucion = penalizacion.FechaResolucion.HasValue
                ? $"\n───────────────────────────────────────────────\n" +
                  $"✅ Resuelta el: {penalizacion.FechaResolucion.Value:dd/MM/yyyy HH:mm}\n" +
                  $"💬 Motivo Resolución: {penalizacion.MotivoResolucion ?? "Atendida por administración"}"
                : string.Empty;

            // Estructura limpia por bloques
            lblDetallePenalizacion.Text =
                "───────────────────────────────────────────────\n\n" +
                $"📌 Estado: {estado}     💰 Monto Mora: RD$ {penalizacion.MontoMora:N2}\n\n" +
                $"📑 Préstamo: #{penalizacion.PrestamoId}        ⏱️ Días Retraso: {penalizacion.DiasRetraso}\n\n" +
                $"📅 Fecha Generación: {penalizacion.FechaGeneracion:dd/MM/yyyy HH:mm}\n\n" +
                "───────────────────────────────────────────────\n\n" +
                $"💬 Motivo de Penalización:\n   {motivo}" +
                infoResolucion;

            // Mostrar u ocultar el botón 'Resolver' según el estado
            bool esActiva = estado == "ACTIVA" || estado == "PENDIENTE";
            btnResolver.Visible = esActiva;

            if (esActiva)
            {
                btnResolver.Location = new Point(25, 14);
                btnResolver.Size = new Size(220, 38);

                btnCerrar.Location = new Point(265, 14);
                btnCerrar.Size = new Size(220, 38);
                btnCerrar.Text = "Cancelar";
            }
            else
            {
                btnCerrar.Location = new Point(155, 14);
                btnCerrar.Size = new Size(200, 38);
                btnCerrar.Text = "Cerrar";
            }
        }

        private void btnResolver_Click(object sender, EventArgs e)
        {
            if (_penalizacionActual == null) return;

            using (var frmResolver = new FrmResolverPenalizacion(_penalizacionService, _penalizacionActual))
            {
                if (frmResolver.ShowDialog() == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}