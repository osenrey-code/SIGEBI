using SIGEBI.AppEscritorio.Services.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class LoginForm : Form
    {
        private readonly IAuthService _auth;

        private static readonly Color ColorLineaInactiva = Color.FromArgb(51, 65, 85);
        private static readonly Color ColorLineaActiva = Color.FromArgb(59, 130, 246);
        private static readonly Color ColorHoverBoton = Color.FromArgb(51, 65, 85);

        public LoginForm(IAuthService auth)
        {
            InitializeComponent();
            _auth = auth;

            // Bordes redondeados para la ventana principal
            this.Load += (s, e) => AplicarBordesRedondeados();
            this.Resize += (s, e) => AplicarBordesRedondeados();

            // Desactivar bordes por defecto de WinForms que causan líneas blancas
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatAppearance.BorderColor = Color.FromArgb(15, 23, 42);
            btnIngresar.FlatAppearance.MouseDownBackColor = Color.FromArgb(15, 23, 42);
            btnIngresar.FlatAppearance.MouseOverBackColor = Color.FromArgb(15, 23, 42);

            // Pintado personalizado del botón e icono del libro
            btnIngresar.Paint += BtnIngresar_Paint;
            picLogo.Paint += PicLogo_Paint;

            // La ventana ya no tiene barra de título de Windows: la hacemos arrastrable
            HabilitarArrastre(pnlBranding);
            HabilitarArrastre(lblLoginHeader);
            HabilitarArrastre(lblLoginSubtext);

            // Línea de acento bajo cada campo: se ilumina al recibir foco
            txtIdentificacion.Enter += (s, e) => pnlIdentificacionLine.BackColor = ColorLineaActiva;
            txtIdentificacion.Leave += (s, e) => pnlIdentificacionLine.BackColor = ColorLineaInactiva;
            txtPassword.Enter += (s, e) => pnlPasswordLine.BackColor = ColorLineaActiva;
            txtPassword.Leave += (s, e) => pnlPasswordLine.BackColor = ColorLineaInactiva;

            // Efecto hover para los botones de la barra de título propia
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            btnMinimize.MouseEnter += btnMinimize_MouseEnter;
            btnMinimize.MouseLeave += btnMinimize_MouseLeave;

            this.AcceptButton = btnIngresar;
        }

        #region Bordes redondeados de la ventana

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void AplicarBordesRedondeados()
        {
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 18, 18));
        }

        #endregion

        #region Renderizado del Logo del Libro en Vector (GDI+)

        private void PicLogo_Paint(object? sender, PaintEventArgs e)
        {
            var pic = (PictureBox)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Dibujar Contenedor Azul Redondeado estilo 'App Badge'
            var rectBadge = new Rectangle(0, 0, pic.Width - 1, pic.Height - 1);
            using (var pathBadge = CrearRutaRedondeada(rectBadge, 14))
            using (var brushBadge = new LinearGradientBrush(rectBadge, Color.FromArgb(59, 130, 246), Color.FromArgb(37, 99, 235), 45f))
            {
                e.Graphics.FillPath(brushBadge, pathBadge);
            }

            // 2. Dibujar el Icono del Libro Blanco
            using (var penBook = new Pen(Color.White, 3f))
            {
                penBook.StartCap = LineCap.Round;
                penBook.EndCap = LineCap.Round;
                penBook.LineJoin = LineJoin.Round;

                int cx = pic.Width / 2;
                int cy = pic.Height / 2;

                // Lomo Central
                e.Graphics.DrawLine(penBook, cx, cy - 10, cx, cy + 12);

                // Hoja Izquierda
                e.Graphics.DrawArc(penBook, cx - 16, cy - 12, 16, 8, 180, 180);
                e.Graphics.DrawLine(penBook, cx - 16, cy - 8, cx - 16, cy + 10);
                e.Graphics.DrawArc(penBook, cx - 16, cy + 6, 16, 8, 0, 180);

                // Hoja Derecha
                e.Graphics.DrawArc(penBook, cx, cy - 12, 16, 8, 180, 180);
                e.Graphics.DrawLine(penBook, cx + 16, cy - 8, cx + 16, cy + 10);
                e.Graphics.DrawArc(penBook, cx, cy + 6, 16, 8, 0, 180);
            }
        }

        #endregion

        #region Renderizado Sin Bordes del Botón Ingresar

        private void BtnIngresar_Paint(object? sender, PaintEventArgs e)
        {
            var btn = (Button)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 1. Limpiar completamente el área del botón con el color de fondo exacto del formulario
            e.Graphics.Clear(this.BackColor);

            // 2. Definir un rectángulo interno inset (con margen de 1px) para evitar artefactos del marco
            var rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);
            int radio = 10;

            using (var path = CrearRutaRedondeada(rect, radio))
            {
                Point mousePos = btn.PointToClient(Cursor.Position);
                bool estaHover = btn.ClientRectangle.Contains(mousePos);

                Color colorBoton = !btn.Enabled
                    ? Color.FromArgb(51, 65, 85)
                    : (estaHover ? Color.FromArgb(29, 78, 216) : Color.FromArgb(37, 99, 235));

                using (var brush = new SolidBrush(colorBoton))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }

            // 3. Dibujar el texto centrado
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath CrearRutaRedondeada(Rectangle rect, int radio)
        {
            var path = new GraphicsPath();
            int d = radio * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region Arrastrar ventana sin barra de título

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        private void HabilitarArrastre(Control control)
        {
            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
        }

        #endregion

        #region Botones de la barra de título propia

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_MouseEnter(object? sender, EventArgs e)
        {
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.FromArgb(220, 38, 38);
        }

        private void btnClose_MouseLeave(object? sender, EventArgs e)
        {
            btnClose.ForeColor = Color.FromArgb(148, 163, 184);
            btnClose.BackColor = Color.Transparent;
        }

        private void btnMinimize_MouseEnter(object? sender, EventArgs e)
        {
            btnMinimize.ForeColor = Color.White;
            btnMinimize.BackColor = ColorHoverBoton;
        }

        private void btnMinimize_MouseLeave(object? sender, EventArgs e)
        {
            btnMinimize.ForeColor = Color.FromArgb(148, 163, 184);
            btnMinimize.BackColor = Color.Transparent;
        }

        #endregion

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            await ProcesarLoginAsync();
        }

        private async Task ProcesarLoginAsync()
        {
            SetCargando(true);

            var resultado = await _auth.IniciarSesionAsync(
                txtIdentificacion.Text.Trim(),
                txtPassword.Text.Trim()
            );

            SetCargando(false);

            if (!resultado.Exitoso)
            {
                MessageBox.Show(resultado.MensajeError, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NavegarAFormularioPrincipal(resultado.TipoUsuario!);
        }

        private void NavegarAFormularioPrincipal(string tipoUsuario)
        {
            this.Hide();

            Form mainForm = new Main();

            mainForm.ShowDialog();
            this.Close();
        }

        private void SetCargando(bool cargando)
        {
            btnIngresar.Enabled = !cargando;
            txtIdentificacion.Enabled = !cargando;
            txtPassword.Enabled = !cargando;
            btnIngresar.Invalidate(); // Fuerza el redibujado instantáneo del botón
            this.Cursor = cargando ? Cursors.WaitCursor : Cursors.Default;
        }

        private async void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await ProcesarLoginAsync();
            }
        }
    }
}