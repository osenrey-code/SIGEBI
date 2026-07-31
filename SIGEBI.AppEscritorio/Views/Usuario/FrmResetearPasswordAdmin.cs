using SIGEBI.AppEscritorio.Dtos.Usuarios;
using SIGEBI.AppEscritorio.Services.Usuario;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Usuario
{
    public partial class FrmResetearPasswordAdmin : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly int _usuarioId;

        public FrmResetearPasswordAdmin(IUsuarioService usuarioService, int usuarioId, string nombreUsuario)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            _usuarioId = usuarioId;

            lblTitle.Text = $"🔑 Resetear Pass - {nombreUsuario}";

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

        private async void btnResetear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNuevaPassword.Text))
            {
                MessageBox.Show("Debe ingresar la nueva contraseña.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnResetear.Enabled = false;

                var dto = new ResetearPasswordAdminDto
                {
                    NuevaPassword = txtNuevaPassword.Text.Trim()
                };

                await _usuarioService.ResetPasswordAdminAsync(_usuarioId, dto);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restablecer contraseña: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnResetear.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}