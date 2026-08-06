using SIGEBI.AppEscritorio.Dtos.Usuarios;
using SIGEBI.AppEscritorio.Services.Usuario;
using SIGEBI.AppEscritorio.Session;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Usuario
{
    public partial class FrmDetalleUsuario : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly UsuarioDto _usuario;

        public FrmDetalleUsuario(IUsuarioService usuarioService, UsuarioDto usuario)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            _usuario = usuario;

            HabilitarArrastre(pnlTopBar);
            AplicarBordesRedondeados();

            ConfigurarPermisos();
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

        private void ConfigurarPermisos()
        {
            bool esAdmin = UserSession.Instancia.TipoUsuario == "Administrador";

            btnEditar.Visible = esAdmin;
            btnEstado.Visible = esAdmin;
            btnReset.Visible = esAdmin;

            bool esActivo = _usuario.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase);
            btnEstado.Text = esActivo ? "🚫 Desactivar" : "✅ Activar";
            btnEstado.BackColor = esActivo ? Color.FromArgb(239, 68, 68) : Color.FromArgb(34, 197, 94);
        }

        private void CargarDatos()
        {
            lblValIdentificacion.Text = string.IsNullOrWhiteSpace(_usuario.Identificacion) ? "N/A" : _usuario.Identificacion;
            lblValNombre.Text = _usuario.NombreCompleto;
            lblValCorreo.Text = _usuario.Correo;
            lblValRol.Text = _usuario.TipoUsuario;

            bool esActivo = _usuario.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase);
            lblValEstado.Text = esActivo ? "● Activo" : "● Inactivo";
            lblValEstado.ForeColor = esActivo ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmEditarUsuario(_usuarioService, _usuario.UsuarioId, _usuario.NombreCompleto))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private async void btnEstado_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuario.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase))
                {
                    using (var frm = new FrmDesactivarUsuario(_usuarioService, _usuario.UsuarioId, _usuario.NombreCompleto))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                else
                {
                    var confirm = MessageBox.Show($"¿Desea reactivar al usuario {_usuario.NombreCompleto}?", "Confirmar Activación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        await _usuarioService.ActivarAsync(_usuario.UsuarioId);
                        MessageBox.Show("Usuario activado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmResetearPasswordAdmin(_usuarioService, _usuario.UsuarioId, _usuario.NombreCompleto))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("La contraseña del usuario ha sido restablecida exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => this.Close();
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}