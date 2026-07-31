using SIGEBI.AppEscritorio.Session;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class Main : Form
    {
        private static readonly Color ColorHoverBotonSidebar = Color.FromArgb(51, 65, 85);
        private static readonly Color ColorActiveSidebar = Color.FromArgb(37, 99, 235);
        private static readonly Color ColorInactiveSidebar = Color.FromArgb(30, 41, 59);

        // Variable para rastrear qué formulario está abierto actualmente en el panel
        private Form _formularioActivo = null;

        public Main()
        {
            InitializeComponent();

            // Bordes redondeados del Formulario
            this.Load += (s, e) => AplicarBordesRedondeados();
            this.Resize += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Normal)
                    AplicarBordesRedondeados();
            };

            // Renderizado del logo del libro en la barra lateral
            picSidebarLogo.Paint += PicSidebarLogo_Paint;

            // Renderizado vectorial milimétricamente centrado para la botonera del título
            btnMinimize.Paint += BtnControlTitulo_Paint;
            btnMaximize.Paint += BtnControlTitulo_Paint;
            btnClose.Paint += BtnControlTitulo_Paint;

            // Arrastre sin barra de título
            HabilitarArrastre(pnlTopBar);
            HabilitarArrastre(pnlBrand);
            HabilitarArrastre(lblBrandTitle);

            // Involucrar redibujado dinámico al pasar el mouse (hover)
            btnClose.MouseEnter += (s, e) => btnClose.Invalidate();
            btnClose.MouseLeave += (s, e) => btnClose.Invalidate();
            btnMaximize.MouseEnter += (s, e) => btnMaximize.Invalidate();
            btnMaximize.MouseLeave += (s, e) => btnMaximize.Invalidate();
            btnMinimize.MouseEnter += (s, e) => btnMinimize.Invalidate();
            btnMinimize.MouseLeave += (s, e) => btnMinimize.Invalidate();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            CargarDatosUsuario();
            GestionPermisos();
        }

        private void CargarDatosUsuario()
        {
            var sesion = UserSession.Instancia;
            string nombre = string.IsNullOrWhiteSpace(sesion.NombreCompleto) ? "Usuario Desconocido" : sesion.NombreCompleto;
            string rol = string.IsNullOrWhiteSpace(sesion.TipoUsuario) ? "Sin Rol" : sesion.TipoUsuario;

            lblUsuarioLogueado.Text = $"Usuario: {nombre} | Rol: {rol}";
            this.Text = $"SIGEBI - Panel Principal | Usuario: {nombre} ({rol})";
        }

        private void GestionPermisos()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            switch (rol)
            {
                case "Administrador":
                    btnMenuDashboard.Visible = true;
                    btnMenuCatalogo.Visible = true;
                    btnMenuPrestamos.Visible = true;
                    btnMenuUsuarios.Visible = true;
                    btnMenuAuditoria.Visible = true;
                    break;

                case "Bibliotecario" or "PersonalBibliotecario":
                    btnMenuDashboard.Visible = true;
                    btnMenuCatalogo.Visible = true;
                    btnMenuPrestamos.Visible = true;
                    btnMenuUsuarios.Visible = false;
                    btnMenuAuditoria.Visible = false;
                    break;

                case "Auditor":
                    btnMenuDashboard.Visible = true;
                    btnMenuCatalogo.Visible = true;
                    btnMenuPrestamos.Visible = false;
                    btnMenuUsuarios.Visible = false;
                    btnMenuAuditoria.Visible = true;
                    break;

                default:
                    MessageBox.Show("No posee un rol válido para acceder a las funciones del sistema.",
                                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        #region Funciones para abrir Formularios dentro del Panel Principal

        private void AbrirFormularioEnPanel(Form formularioHijo)
        {
            // Cerramos el formulario anterior si hay alguno abierto
            if (_formularioActivo != null)
            {
                _formularioActivo.Close();
            }

            _formularioActivo = formularioHijo;

            // Configuramos el formulario para que se comporte como un control
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            // Ocultamos las tarjetas del dashboard
            CambiarVisibilidadDashboard(false);

            // Añadimos y mostramos
            pnlContent.Controls.Add(formularioHijo);
            pnlContent.Tag = formularioHijo;
            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        private void MostrarDashboard()
        {
            // Cerramos cualquier formulario hijo que esté abierto
            if (_formularioActivo != null)
            {
                _formularioActivo.Close();
                _formularioActivo = null;
            }

            // Volvemos a mostrar las tarjetas
            CambiarVisibilidadDashboard(true);
        }

        private void CambiarVisibilidadDashboard(bool visible)
        {
            lblWelcomeHeader.Visible = visible;
            lblWelcomeSub.Visible = visible;
            pnlCard1.Visible = visible;
            pnlCard2.Visible = visible;
            pnlCard3.Visible = visible;
            pnlCard4.Visible = visible;
        }

        #endregion

        #region Renderizado del Logo del Libro en Vector (GDI+)

        private void PicSidebarLogo_Paint(object? sender, PaintEventArgs e)
        {
            var pic = (PictureBox)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rectBadge = new Rectangle(0, 0, pic.Width - 1, pic.Height - 1);
            using (var pathBadge = CrearRutaRedondeada(rectBadge, 8))
            using (var brushBadge = new LinearGradientBrush(rectBadge, Color.FromArgb(59, 130, 246), Color.FromArgb(37, 99, 235), 45f))
            {
                e.Graphics.FillPath(brushBadge, pathBadge);
            }

            using (var penBook = new Pen(Color.White, 2.0f))
            {
                penBook.StartCap = LineCap.Round;
                penBook.EndCap = LineCap.Round;
                penBook.LineJoin = LineJoin.Round;

                int cx = pic.Width / 2;
                int cy = pic.Height / 2;

                e.Graphics.DrawLine(penBook, cx, cy - 6, cx, cy + 7);

                e.Graphics.DrawArc(penBook, cx - 9, cy - 7, 9, 5, 180, 180);
                e.Graphics.DrawLine(penBook, cx - 9, cy - 4, cx - 9, cy + 6);
                e.Graphics.DrawArc(penBook, cx - 9, cy + 4, 9, 5, 0, 180);

                e.Graphics.DrawArc(penBook, cx, cy - 7, 9, 5, 180, 180);
                e.Graphics.DrawLine(penBook, cx + 9, cy - 4, cx + 9, cy + 6);
                e.Graphics.DrawArc(penBook, cx, cy + 5, 9, 5, 0, 180);
            }
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

        #region Renderizado Vectorial Alinear Botones de Título (Minimizar, Maximizar, Cerrar)

        private void BtnControlTitulo_Paint(object? sender, PaintEventArgs e)
        {
            var btn = (Button)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Detectar estado Hover
            Point mousePos = btn.PointToClient(Cursor.Position);
            bool estaHover = btn.ClientRectangle.Contains(mousePos);

            Color colorFondo = estaHover
                ? (btn == btnClose ? Color.FromArgb(220, 38, 38) : ColorHoverBotonSidebar)
                : Color.Transparent;

            Color colorIcono = (estaHover && btn == btnClose) ? Color.White : Color.FromArgb(148, 163, 184);

            using (var bgBrush = new SolidBrush(colorFondo))
            {
                e.Graphics.FillRectangle(bgBrush, btn.ClientRectangle);
            }

            int cx = btn.Width / 2;
            int cy = btn.Height / 2; // Exacto punto Y central

            using (var pen = new Pen(colorIcono, 1.5f))
            {
                if (btn == btnMinimize)
                {
                    // Guión vectorizado exacto en el eje Y central
                    e.Graphics.DrawLine(pen, cx - 5, cy, cx + 5, cy);
                }
                else if (btn == btnMaximize)
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        // Cuadrado alineado en (cx, cy)
                        e.Graphics.DrawRectangle(pen, cx - 5, cy - 5, 10, 10);
                    }
                    else
                    {
                        // Cuadrados dobles (Restaurar)
                        e.Graphics.DrawRectangle(pen, cx - 3, cy - 6, 7, 7);
                        using (var fillFront = new SolidBrush(colorFondo == Color.Transparent ? Color.FromArgb(30, 41, 59) : colorFondo))
                        {
                            e.Graphics.FillRectangle(fillFront, cx - 6, cy - 3, 7, 7);
                        }
                        e.Graphics.DrawRectangle(pen, cx - 6, cy - 3, 7, 7);
                    }
                }
                else if (btn == btnClose)
                {
                    // Cruz 'X' centrada
                    e.Graphics.DrawLine(pen, cx - 5, cy - 5, cx + 5, cy + 5);
                    e.Graphics.DrawLine(pen, cx - 5, cy + 5, cx + 5, cy - 5);
                }
            }
        }

        #endregion

        #region Bordes Redondeados y Arrastre

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void AplicarBordesRedondeados()
        {
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 18, 18));
        }

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
                if (e.Button == MouseButtons.Left && this.WindowState == FormWindowState.Normal)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
        }

        #endregion

        #region Eventos de Botones de Navegación Lateral

        private void SeleccionarBotonMenu(Button botonActivo, string tituloSeccion)
        {
            lblPageTitle.Text = tituloSeccion;

            btnMenuDashboard.BackColor = ColorInactiveSidebar;
            btnMenuDashboard.ForeColor = Color.FromArgb(148, 163, 184);

            btnMenuCatalogo.BackColor = ColorInactiveSidebar;
            btnMenuCatalogo.ForeColor = Color.FromArgb(148, 163, 184);

            btnMenuPrestamos.BackColor = ColorInactiveSidebar;
            btnMenuPrestamos.ForeColor = Color.FromArgb(148, 163, 184);

            btnMenuUsuarios.BackColor = ColorInactiveSidebar;
            btnMenuUsuarios.ForeColor = Color.FromArgb(148, 163, 184);

            btnMenuAuditoria.BackColor = ColorInactiveSidebar;
            btnMenuAuditoria.ForeColor = Color.FromArgb(148, 163, 184);

            botonActivo.BackColor = ColorActiveSidebar;
            botonActivo.ForeColor = Color.White;
        }

        private void btnMenuDashboard_Click(object sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuDashboard, "Panel Principal");
            MostrarDashboard(); // Volvemos a mostrar las tarjetas
        }

        private void btnMenuCatalogo_Click(object sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuCatalogo, "Catálogo de Libros");

            // Le pedimos al Inyector de Dependencias que nos entregue un CatalogoForm con todos sus servicios listos
            var catalogoForm = Program.ServiceProvider.GetRequiredService<CatalogoForm>();
            AbrirFormularioEnPanel(catalogoForm);
        }

        private void btnMenuPrestamos_Click(object sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuPrestamos, "Préstamos y Devoluciones");
        }

        private void btnMenuUsuarios_Click(object sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuUsuarios, "Gestión de Usuarios");
        }

        private void btnMenuAuditoria_Click(object sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuAuditoria, "Auditoría y Reportes");
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            var confirmacion = MessageBox.Show("¿Está seguro de que desea cerrar la sesión actual?",
                                               "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                UserSession.Instancia.CerrarSesion();
                this.Hide();

                var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
                loginForm.ShowDialog();

                this.Close();
            }
        }

        #endregion

        #region Botones de Titulo: Minimizar, Maximizar/Restaurar y Cerrar

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
                this.Region = null;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                AplicarBordesRedondeados();
            }

            btnMaximize.Invalidate();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion
    }
}