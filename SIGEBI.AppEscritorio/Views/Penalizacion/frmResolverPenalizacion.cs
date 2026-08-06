using SIGEBI.AppEscritorio.Dtos.Penalizaciones;
using SIGEBI.AppEscritorio.Services.Penalizaciones;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Penalizaciones
{
    public partial class FrmResolverPenalizacion : Form
    {
        private readonly IPenalizacionService _penalizacionService;
        private readonly PenalizacionDto _penalizacion;

        public FrmResolverPenalizacion(IPenalizacionService penalizacionService, PenalizacionDto penalizacion)
        {
            InitializeComponent();
            _penalizacionService = penalizacionService;
            _penalizacion = penalizacion;

            AplicarEstilosDarkSlate();
            CargarDatos();
        }

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);
            Color fondoPanel = Color.FromArgb(30, 41, 59);
            Color verdeEsmeralda = Color.FromArgb(22, 163, 74);
            Color colorBotonGris = Color.FromArgb(51, 65, 85);

            this.BackColor = fondoDark;
            pnlContenedor.BackColor = fondoPanel;

            btnConfirmar.BackColor = verdeEsmeralda;
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnConfirmar.Cursor = Cursors.Hand;

            btnCancelar.BackColor = colorBotonGris;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCancelar.Cursor = Cursors.Hand;
        }

        private void CargarDatos()
        {
            lblTitulo.Text = $"✅ Resolver Penalización #{_penalizacion.PenalizacionId}";

            string lector = !string.IsNullOrWhiteSpace(_penalizacion.NombreUsuario)
                ? _penalizacion.NombreUsuario
                : _penalizacion.IdentificacionUsuario;

            lblDetalle.Text = $"Lector: {lector} ({_penalizacion.IdentificacionUsuario})  |  Préstamo ID: #{_penalizacion.PrestamoId}  |  Mora: RD$ {_penalizacion.MontoMora:N2}";
        }

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMotivoResolucion.Text))
            {
                MessageBox.Show("Por favor, ingrese el motivo de la resolución (ejemplo: 'Mora saldada en caja con recibo #1024').", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivoResolucion.Focus();
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnConfirmar.Enabled = false;

                var request = new ResolverPenalizacionRequestDto
                {
                    PenalizacionId = _penalizacion.PenalizacionId,
                    MotivoResolucion = txtMotivoResolucion.Text.Trim()
                };

                await _penalizacionService.ResolverPenalizacionAsync(request);

                MessageBox.Show("La penalización ha sido resuelta exitosamente. El usuario ha sido habilitado nuevamente para solicitar préstamos.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo resolver la penalización: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnConfirmar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}