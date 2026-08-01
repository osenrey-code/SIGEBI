using SIGEBI.AppEscritorio.Dtos.Categoria;
using SIGEBI.AppEscritorio.Services.Categoria;

namespace SIGEBI.AppEscritorio.Views.Categorias
{
    public partial class FrmRegistrarCategoria : Form
    {
        private readonly ICategoriaService _categoriaService;

        public FrmRegistrarCategoria(ICategoriaService categoriaService)
        {
            InitializeComponent();
            _categoriaService = categoriaService;

            AplicarEstilosDarkSlate();
        }

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);
            Color fondoPanel = Color.FromArgb(30, 41, 59);
            Color azulPrimario = Color.FromArgb(37, 99, 235);
            Color colorBotonGris = Color.FromArgb(51, 65, 85);

            this.BackColor = fondoDark;
            pnlContenedor.BackColor = fondoPanel;

            txtNombre.BackColor = fondoDark;
            txtNombre.ForeColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;

            txtDescripcion.BackColor = fondoDark;
            txtDescripcion.ForeColor = Color.White;
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;

            btnGuardar.BackColor = azulPrimario;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnGuardar.Cursor = Cursors.Hand;

            btnCancelar.BackColor = colorBotonGris;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCancelar.Cursor = Cursors.Hand;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de la categoría es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnGuardar.Enabled = false;

                var request = new RegistrarCategoriaRequestDto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                var respuesta = await _categoriaService.RegistrarCategoriaAsync(request);

                MessageBox.Show(respuesta?.Mensaje ?? "Categoría registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la categoría: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnGuardar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}