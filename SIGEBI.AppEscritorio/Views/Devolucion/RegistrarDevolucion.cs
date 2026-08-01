using SIGEBI.AppEscritorio.Dtos.Devoluciones;
using SIGEBI.AppEscritorio.Services.Devolucion;
using System;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Devolucion
{
    public partial class RegistrarDevolucion : Form
    {
        private readonly IDevolucionService _devolucionService;
        private readonly int _prestamoId;
        private readonly string _tituloRecurso;

        public RegistrarDevolucion(IDevolucionService devolucionService, int prestamoId, string tituloRecurso)
        {
            InitializeComponent();
            _devolucionService = devolucionService;
            _prestamoId = prestamoId;
            _tituloRecurso = tituloRecurso;
        }

        private void RegistrarDevolucion_Load(object sender, EventArgs e)
        {
            lblInfoPrestamo.Text = $"Préstamo #{_prestamoId}: {_tituloRecurso}";

            cmbCondicion.Items.Clear();
            cmbCondicion.Items.AddRange(new object[] { "Excelente", "Bueno", "Regular", "Deteriorado", "Inservible / Perdido" });
            cmbCondicion.SelectedIndex = 1; // "Bueno" por defecto
        }

        private async void btnProcesar_Click(object sender, EventArgs e)
        {
            if (cmbCondicion.SelectedItem == null)
            {
                MessageBox.Show("Seleccione la condición física del recurso.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnProcesar.Enabled = false;

                var request = new RegistrarDevolucionRequestDto
                {
                    PrestamoId = _prestamoId,
                    Condicion = cmbCondicion.SelectedItem.ToString()!,
                    Observacion = string.IsNullOrWhiteSpace(txtObservacion.Text) ? null : txtObservacion.Text.Trim()
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
                btnProcesar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}