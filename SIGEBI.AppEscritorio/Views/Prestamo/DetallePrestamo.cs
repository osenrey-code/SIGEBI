using SIGEBI.AppEscritorio.Dtos.Devoluciones;
using SIGEBI.AppEscritorio.Dtos.Prestamos;
using SIGEBI.AppEscritorio.Services.Devolucion;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class DetallePrestamo : Form
    {
        private readonly IDevolucionService _devolucionService;
        private PrestamoDto? _prestamoActual;

        public DetallePrestamo(IDevolucionService devolucionService)
        {
            InitializeComponent();
            _devolucionService = devolucionService;

            HabilitarArrastre(pnlTopBar);
            AplicarBordesRedondeados();
            InicializarOpcionesCondicion();
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

        private void InicializarOpcionesCondicion()
        {
            cmbCondicion.Items.Clear();
            cmbCondicion.Items.AddRange(new object[] { "Excelente", "Bueno", "Regular", "Deteriorado", "Inservible / Perdido" });
            cmbCondicion.SelectedIndex = 1; // "Bueno" por defecto

            cmbCondicion.SelectedIndexChanged += CmbCondicion_SelectedIndexChanged;
        }

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);
            Color azulAccion = Color.FromArgb(37, 99, 235);
            Color verdeEsmeralda = Color.FromArgb(22, 163, 74);
            Color colorBotonGris = Color.FromArgb(51, 65, 85);

            this.BackColor = fondoDark;

            // Botón Devolución (Paso 1)
            btnDevolucion.BackColor = azulAccion;
            btnDevolucion.ForeColor = Color.White;
            btnDevolucion.FlatStyle = FlatStyle.Flat;
            btnDevolucion.FlatAppearance.BorderSize = 0;
            btnDevolucion.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnDevolucion.Cursor = Cursors.Hand;

            // Botón Confirmar Devolución (Paso 2)
            btnConfirmarDevolucion.BackColor = verdeEsmeralda;
            btnConfirmarDevolucion.ForeColor = Color.White;
            btnConfirmarDevolucion.FlatStyle = FlatStyle.Flat;
            btnConfirmarDevolucion.FlatAppearance.BorderSize = 0;
            btnConfirmarDevolucion.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnConfirmarDevolucion.Cursor = Cursors.Hand;

            // Botón Cancelar / Cerrar
            btnCancelar.BackColor = colorBotonGris;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCancelar.Cursor = Cursors.Hand;

            // Entradas de texto
            cmbCondicion.BackColor = Color.FromArgb(15, 23, 42);
            cmbCondicion.ForeColor = Color.White;

            txtObservacion.BackColor = Color.FromArgb(15, 23, 42);
            txtObservacion.ForeColor = Color.White;
            txtObservacion.BorderStyle = BorderStyle.FixedSingle;
        }

        public void CargarPrestamo(PrestamoDto prestamo, bool esSoloLectura = false)
        {
            _prestamoActual = prestamo;

            lblTitulo.Text = "🔄 Detalle del Préstamo";

            string nombreUsuario = string.IsNullOrWhiteSpace(prestamo.NombreUsuario) ? "Usuario Desconocido" : prestamo.NombreUsuario;
            string identificacion = string.IsNullOrWhiteSpace(prestamo.IdentificacionUsuario) ? "N/A" : prestamo.IdentificacionUsuario;
            string ejemplar = string.IsNullOrWhiteSpace(prestamo.IdentificadorEjemplar) ? "N/A" : prestamo.IdentificadorEjemplar;
            string estado = string.IsNullOrWhiteSpace(prestamo.Estado) ? "ACTIVO" : prestamo.Estado.ToUpper();

            lblDetalleInfo.Text = $"👤 Lector: {nombreUsuario} ({identificacion})";
            lblDetalleLibro.Text =
                "───────────────────────────────────────────────\n\n" +
                $"📌 Estado: {estado}\n\n" +
                $"📖 Recurso: {prestamo.TituloRecurso}\n\n" +
                $"🏷️ Ejemplar ID: {ejemplar}\n\n" +
                $"📅 Inicio: {prestamo.FechaInicio:dd/MM/yyyy}  |  📅 Límite: {prestamo.FechaLimite:dd/MM/yyyy}";

            // Resetear visibilidad de vistas
            lblDetalleLibro.Visible = true;
            lblCondicion.Visible = false;
            cmbCondicion.Visible = false;
            lblObservacion.Visible = false;
            txtObservacion.Visible = false;
            btnConfirmarDevolucion.Visible = false;

            this.ClientSize = new Size(520, 450);

            bool puedeDevolver = !esSoloLectura && (estado == "ACTIVO" || estado == "APROBADO" || estado == "VENCIDO");
            btnDevolucion.Visible = puedeDevolver;

            if (!puedeDevolver)
            {
                btnCancelar.Location = new Point(160, 14);
                btnCancelar.Size = new Size(200, 38);
                btnCancelar.Text = "Cerrar";
            }
            else
            {
                btnDevolucion.Location = new Point(20, 14);
                btnDevolucion.Size = new Size(225, 38);

                btnCancelar.Location = new Point(260, 14);
                btnCancelar.Size = new Size(225, 38);
                btnCancelar.Text = "Cerrar";
            }
        }

        private void btnDevolucion_Click(object sender, EventArgs e)
        {
            if (_prestamoActual == null) return;

            lblTitulo.Text = "📥 Procesar Devolución";
            lblDetalleInfo.Text = $"Recurso: {_prestamoActual.TituloRecurso}";
            lblDetalleInfo.ForeColor = Color.FromArgb(59, 130, 246);

            lblDetalleLibro.Visible = false;
            btnDevolucion.Visible = false;

            lblCondicion.Visible = true;
            cmbCondicion.Visible = true;
            lblObservacion.Visible = true;
            txtObservacion.Visible = true;
            btnConfirmarDevolucion.Visible = true;

            btnCancelar.Text = "Cancelar";

            this.ClientSize = new Size(520, 520);
            AplicarBordesRedondeados();

            btnConfirmarDevolucion.Location = new Point(20, 14);
            btnConfirmarDevolucion.Size = new Size(225, 38);

            btnCancelar.Location = new Point(260, 14);
            btnCancelar.Size = new Size(225, 38);

            cmbCondicion.Focus();
        }

        private void CmbCondicion_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool esRegularOBajo = cmbCondicion.SelectedIndex >= 2;

            if (esRegularOBajo)
            {
                lblObservacion.Text = "Observaciones (Obligatorio):";
                lblObservacion.ForeColor = Color.FromArgb(239, 68, 68);
            }
            else
            {
                lblObservacion.Text = "Observaciones (Opcional):";
                lblObservacion.ForeColor = Color.FromArgb(148, 163, 184);
            }
        }

        private async void btnConfirmarDevolucion_Click(object sender, EventArgs e)
        {
            if (_prestamoActual == null) return;

            if (cmbCondicion.SelectedItem == null)
            {
                MessageBox.Show("Seleccione la condición física del recurso.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string condicion = cmbCondicion.SelectedItem.ToString()!;
            string observacion = txtObservacion.Text.Trim();

            bool esRegularOBajo = cmbCondicion.SelectedIndex >= 2;

            if (esRegularOBajo && string.IsNullOrWhiteSpace(observacion))
            {
                MessageBox.Show(
                    $"Las observaciones son obligatorias cuando la condición física del recurso es '{condicion}'. Por favor especifique los detalles del estado.",
                    "Observación Requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtObservacion.Focus();
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnConfirmarDevolucion.Enabled = false;

                var request = new RegistrarDevolucionRequestDto
                {
                    PrestamoId = _prestamoActual.PrestamoId,
                    Condicion = condicion,
                    Observacion = string.IsNullOrWhiteSpace(observacion) ? null : observacion
                };

                var respuesta = await _devolucionService.RegistrarDevolucionAsync(request);

                if (respuesta != null)
                {
                    MessageBoxIcon icono = respuesta.PenalizacionGenerada ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                    MessageBox.Show(respuesta.Mensaje, "Devolución Procesada", MessageBoxButtons.OK, icono);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la devolución: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnConfirmarDevolucion.Enabled = true;
            }
        }

        public void CargarDevolucion(DevolucionResponseDto devolucion)
        {
            lblTitulo.Text = "📥 Detalle de la Devolución";

            string nombreUsuario = !string.IsNullOrWhiteSpace(devolucion.NombreUsuario) ? devolucion.NombreUsuario : "Usuario Desconocido";
            string identificacion = !string.IsNullOrWhiteSpace(devolucion.IdentificacionUsuario) ? devolucion.IdentificacionUsuario : "N/A";
            string condicion = !string.IsNullOrWhiteSpace(devolucion.Condicion) ? devolucion.Condicion : "N/A";

            string observaciones = !string.IsNullOrWhiteSpace(devolucion.Observacion)
                ? devolucion.Observacion
                : "Ninguna (Devuelto sin notas adicionales)";

            string detalleMensaje = !string.IsNullOrWhiteSpace(devolucion.Mensaje)
                ? devolucion.Mensaje
                : "N/A";

            lblDetalleInfo.Text = $"👤 Lector: {nombreUsuario} ({identificacion})";

            lblDetalleLibro.Text =
                "───────────────────────────────────────────────\n\n" +
                $"📌 Estado: {(devolucion.Mensaje?.Contains("tardía") == true ? "DEVUELTO CON RETRASO" : "DEVUELTO A TIEMPO")}\n\n" +
                $"🔍 Condición: {condicion}\n\n" +
                $"📖 Recurso: {devolucion.TituloRecurso}\n\n" +
                $"📅 Fecha de Devolución: {devolucion.FechaDevolucion:dd/MM/yyyy HH:mm}\n\n" +
                "───────────────────────────────────────────────\n\n" +
                $"💬 Observaciones:   {observaciones}\n\n" +
                $"ℹ️ Evaluación de Plazo:   {detalleMensaje}";

            lblDetalleLibro.Visible = true;
            lblCondicion.Visible = false;
            cmbCondicion.Visible = false;
            lblObservacion.Visible = false;
            txtObservacion.Visible = false;

            btnDevolucion.Visible = false;
            btnConfirmarDevolucion.Visible = false;

            this.ClientSize = new Size(520, 450);

            btnCancelar.Location = new Point(160, 14);
            btnCancelar.Size = new Size(200, 38);
            btnCancelar.Text = "Cerrar";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
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