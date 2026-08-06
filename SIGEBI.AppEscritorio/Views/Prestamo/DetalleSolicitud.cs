using SIGEBI.AppEscritorio.Dtos.Prestamos;
using SIGEBI.AppEscritorio.Services.Prestamo;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class DetalleSolicitud : Form
    {
        private readonly IPrestamoService _prestamoService;
        private SolicitudDto? _solicitudActual;
        private bool _modoRechazoActivo = false; // Control de estado para el botón de rechazo

        public DetalleSolicitud(IPrestamoService prestamoService)
        {
            InitializeComponent();
            _prestamoService = prestamoService;

            AplicarEstilosDarkSlate();
        }

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);
            Color fondoPanel = Color.FromArgb(30, 41, 59);
            Color verdeEsmeralda = Color.FromArgb(22, 163, 74);
            Color rojoAlerta = Color.FromArgb(239, 68, 68);
            Color colorBotonGris = Color.FromArgb(51, 65, 85);

            this.BackColor = fondoDark;
            pnlContenedor.BackColor = fondoPanel;

            // Botón Aprobar
            btnAprobar.BackColor = verdeEsmeralda;
            btnAprobar.ForeColor = Color.White;
            btnAprobar.FlatStyle = FlatStyle.Flat;
            btnAprobar.FlatAppearance.BorderSize = 0;
            btnAprobar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnAprobar.Cursor = Cursors.Hand;

            // Botón Rechazar
            btnRechazar.BackColor = rojoAlerta;
            btnRechazar.ForeColor = Color.White;
            btnRechazar.FlatStyle = FlatStyle.Flat;
            btnRechazar.FlatAppearance.BorderSize = 0;
            btnRechazar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnRechazar.Cursor = Cursors.Hand;

            // Botón Cancelar / Cerrar
            btnCancelar.BackColor = colorBotonGris;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCancelar.Cursor = Cursors.Hand;
        }

        public void CargarSolicitud(SolicitudDto solicitud)
        {
            _solicitudActual = solicitud;

            // 🟢 TÍTULO LIMPIO Y FORMAL
            lblTitulo.Text = "✔ Detalles de la Solicitud";

            string nombreUsuario = string.IsNullOrWhiteSpace(solicitud.NombreUsuario) ? "Usuario Desconocido" : solicitud.NombreUsuario;
            string identificacion = string.IsNullOrWhiteSpace(solicitud.IdentificacionUsuario) ? "N/A" : solicitud.IdentificacionUsuario;
            string ejemplar = string.IsNullOrWhiteSpace(solicitud.IdentificadorEjemplar) ? "N/A" : solicitud.IdentificadorEjemplar;
            string estado = string.IsNullOrWhiteSpace(solicitud.Estado) ? "PENDIENTE" : solicitud.Estado.ToUpper();

            // 🟢 INFORMACIÓN ORGANIZADA DE FORMA VERTICAL Y PROFESIONAL
            lblDetalleInfo.Text = $"👤 Lector: {nombreUsuario} ({identificacion})";
            lblDetalleLibro.Text = $"📌 Estado: {estado}\n📖 Recurso: {solicitud.TituloRecurso}\n🏷️ Ejemplar ID: {ejemplar}  |  📅 Fecha: {solicitud.FechaSolicitud:dd/MM/yyyy HH:mm}";
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            if (_solicitudActual == null) return;

            var confirm = MessageBox.Show(
                $"¿Desea aprobar la solicitud de '{_solicitudActual.NombreUsuario}' para '{_solicitudActual.TituloRecurso}' y generar el préstamo?",
                "Confirmar Aprobación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                ProcesarAprobacionAsync();
            }
        }

        private async void ProcesarAprobacionAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var request = new AprobarSolicitudRequest { SolicitudId = _solicitudActual!.SolicitudId };
                await _prestamoService.AprobarSolicitudAsync(request);

                MessageBox.Show("Solicitud aprobada y préstamo generado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aprobar la solicitud: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void btnRechazar_Click(object sender, EventArgs e)
        {
            if (!_modoRechazoActivo)
            {
                // Paso 1: Activar visualmente la interfaz de motivo de rechazo
                _modoRechazoActivo = true;
                lblMotivoRechazo.Visible = true;
                txtMotivoRechazo.Visible = true;

                // 🟢 Ocultar el botón de aprobar en lugar de dejarlo pegado y deshabilitado
                btnAprobar.Visible = false;
                btnRechazar.Text = "✔ Confirmar";

                // Ajustar tamaño del formulario dinámicamente y posicionar con separación limpia
                this.ClientSize = new System.Drawing.Size(510, 485);
                pnlContenedor.Size = new System.Drawing.Size(480, 455);

                // Distribuir con espacio profesional los botones activos
                btnRechazar.Location = new System.Drawing.Point(15, 395);
                btnRechazar.Size = new System.Drawing.Size(220, 38); // Botón ancho y cómodo para confirmar
                btnCancelar.Location = new System.Drawing.Point(245, 395);
                btnCancelar.Size = new System.Drawing.Size(220, 38);

                txtMotivoRechazo.Focus();
            }
            else
            {
                // Paso 2: Validar el texto y enviar el rechazo a la API
                if (_solicitudActual == null) return;

                string motivo = txtMotivoRechazo.Text.Trim();
                if (string.IsNullOrWhiteSpace(motivo))
                {
                    MessageBox.Show("Debe especificar un motivo de rechazo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMotivoRechazo.Focus();
                    return;
                }

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    var request = new RechazarSolicitudRequest
                    {
                        SolicitudId = _solicitudActual.SolicitudId,
                        MotivoRechazo = motivo
                    };

                    await _prestamoService.RechazarSolicitudAsync(request);

                    MessageBox.Show("Solicitud rechazada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al rechazar la solicitud: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void btnConfirmarRechazo_Click(object sender, EventArgs e)
        {
            btnRechazar_Click(sender, e);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}