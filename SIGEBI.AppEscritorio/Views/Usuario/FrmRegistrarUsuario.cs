using SIGEBI.AppEscritorio.Dtos.Usuarios;
using SIGEBI.AppEscritorio.Services.Usuario;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Usuario
{
    public partial class FrmRegistrarUsuario : Form
    {
        private readonly IUsuarioService _usuarioService;

        public FrmRegistrarUsuario(IUsuarioService usuarioService)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            cmbTipo.SelectedIndex = 0;

            HabilitarArrastre(pnlTopBar);
            AplicarBordesRedondeados();
        }

        #region Arrastre y Bordes Redondeados
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void AplicarBordesRedondeados()
        {
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 14, 18));
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

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text) ||
                string.IsNullOrWhiteSpace(txtNombreCompleto.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnGuardar.Enabled = false;

                var dto = new RegistrarUsuarioDto
                {
                    Identificacion = txtIdentificacion.Text.Trim(),
                    NombreCompleto = txtNombreCompleto.Text.Trim(),
                    Correo = txtCorreo.Text.Trim(),
                    Tipo = cmbTipo.SelectedItem?.ToString() ?? "Estudiante"
                };

                await _usuarioService.RegistrarAsync(dto);
                MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnGuardar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}