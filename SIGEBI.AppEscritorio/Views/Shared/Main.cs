using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Session;
using SIGEBI.AppEscritorio.Views.Devolucion;
using SIGEBI.AppEscritorio.Views.Penalizaciones; 
using SIGEBI.AppEscritorio.Views.Prestamo;
using SIGEBI.AppEscritorio.Views.Reportes;
using SIGEBI.AppEscritorio.Views.Usuario;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class Main : Form
    {
        private static readonly Color ColorHoverBotonSidebar = Color.FromArgb(51, 65, 85);   // #334155
        private static readonly Color ColorActiveSidebar = Color.FromArgb(37, 99, 235);     // #2563EB
        private static readonly Color ColorInactiveSidebar = Color.FromArgb(30, 41, 59);   // #1E293B
        private static readonly Color ColorAccentBar = Color.FromArgb(59, 130, 246);        // #3B82F6

        private Form? _formularioActivo = null;
        private Button? _botonMenuSeleccionado = null;

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

            // Renderizado vectorial de la botonera del título
            btnMinimize.Paint += BtnControlTitulo_Paint;
            btnMaximize.Paint += BtnControlTitulo_Paint;
            btnClose.Paint += BtnControlTitulo_Paint;

            // Arrastre sin barra de título
            HabilitarArrastre(pnlTopBar);
            HabilitarArrastre(pnlBrand);
            HabilitarArrastre(lblBrandTitle);

            // Hover dinámico en controles de ventana
            btnClose.MouseEnter += (s, e) => btnClose.Invalidate();
            btnClose.MouseLeave += (s, e) => btnClose.Invalidate();
            btnMaximize.MouseEnter += (s, e) => btnMaximize.Invalidate();
            btnMaximize.MouseLeave += (s, e) => btnMaximize.Invalidate();
            btnMinimize.MouseEnter += (s, e) => btnMinimize.Invalidate();
            btnMinimize.MouseLeave += (s, e) => btnMinimize.Invalidate();

            // Configuración visual de la barra de navegación lateral y botón de salida
            ConfigurarEstilosSidebar();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!GestionPermisos())
            {
                this.BeginInvoke(new Action(() =>
                {
                    UserSession.Instancia.CerrarSesion();
                    this.Close();
                }));
                return;
            }

            CargarDatosUsuario();

            // Seleccionar por defecto la vista principal del Dashboard
            SeleccionarBotonMenu(btnMenuDashboard, "Panel Principal");
        }

        private void CargarDatosUsuario()
        {
            var sesion = UserSession.Instancia;
            string nombre = string.IsNullOrWhiteSpace(sesion.NombreCompleto) ? "Usuario" : sesion.NombreCompleto;
            string rol = string.IsNullOrWhiteSpace(sesion.TipoUsuario) ? "Sin Rol" : sesion.TipoUsuario;

            // Insignia de usuario superior estilizada
            lblUsuarioLogueado.Text = $"👤  {nombre}    │    Rol: {rol}";
            this.Text = $"SIGEBI - Panel Principal | Usuario: {nombre} ({rol})";

            ConfigurarDashboardProfesional(nombre, rol);
        }

        private void ConfigurarDashboardProfesional(string nombreUsuario, string rol)
        {
            lblWelcomeHeader.Text = $"¡Bienvenido de nuevo, {nombreUsuario}! 👋";
            lblWelcomeSub.Text = "Panel de control central de SIGEBI. Seleccione un módulo lateral o un acceso rápido.";

            // 1. Tarjeta 1: Acceso Directo a Catálogo
            lblCard1Title.Text = "📚 Gestión de Catálogo";
            lblCard1Val.Text = "Explorar ➔";
            lblCard1Val.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblCard1Val.ForeColor = Color.FromArgb(59, 130, 246);
            ConfigurarTarjetaComoBoton(pnlCard1, btnMenuCatalogo_Click);

            // 2. Tarjeta 2: Acceso Directo a Préstamos
            bool esBibliotecario = (rol == "Bibliotecario" || rol == "PersonalBibliotecario");
            lblCard2Title.Text = esBibliotecario ? "🔄 Préstamos y Solicitudes" : "🔄 Préstamos";
            lblCard2Val.Text = "Gestionar ➔";
            lblCard2Val.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblCard2Val.ForeColor = Color.FromArgb(34, 197, 94);
            ConfigurarTarjetaComoBoton(pnlCard2, btnMenuPrestamos_Click);

            // 3. Tarjeta 3: Información de Rol
            lblCard3Title.Text = "👤 Rol Asignado";
            lblCard3Val.Text = rol;
            lblCard3Val.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblCard3Val.ForeColor = Color.FromArgb(168, 85, 247);

            // 4. Tarjeta 4: Estado del Sistema
            lblCard4Title.Text = "⚡ Estado del Servicio";
            lblCard4Val.Text = "● En Línea";
            lblCard4Val.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblCard4Val.ForeColor = Color.FromArgb(34, 197, 94);
        }

        private void ConfigurarTarjetaComoBoton(Panel tarjeta, EventHandler accionClic)
        {
            tarjeta.Cursor = Cursors.Hand;
            tarjeta.Click += accionClic;

            foreach (Control control in tarjeta.Controls)
            {
                control.Cursor = Cursors.Hand;
                control.Click += accionClic;
            }

            tarjeta.MouseEnter += (s, e) => tarjeta.BackColor = Color.FromArgb(51, 65, 85);
            tarjeta.MouseLeave += (s, e) => tarjeta.BackColor = Color.FromArgb(30, 41, 59);
        }

        #region Estilizado del Menú Lateral y Botón de Cierre de Sesión

        private void ConfigurarEstilosSidebar()
        {
            Button[] botonesMenu = { btnMenuDashboard, btnMenuCatalogo, btnMenuPrestamos, btnMenuDevoluciones, btnMenuPenalizaciones, btnMenuUsuarios, btnMenuAuditoria };

            foreach (var btn in botonesMenu)
            {
                if (btn != null)
                {
                    btn.Paint += BotonSidebar_Paint;
                }
            }

            EstilarBotonCerrarSesion();
        }

        private void BotonSidebar_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is Button btn && btn == _botonMenuSeleccionado)
            {
                // Dibujar indicador lateral azul de 4px cuando el botón está activo
                using var brushBarra = new SolidBrush(ColorAccentBar);
                e.Graphics.FillRectangle(brushBarra, 0, 0, 4, btn.Height);
            }
        }

        private void EstilarBotonCerrarSesion()
        {
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.BackColor = Color.FromArgb(24, 30, 45);
            btnCerrarSesion.ForeColor = Color.FromArgb(239, 68, 68);
            btnCerrarSesion.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnCerrarSesion.Text = "🚪    Cerrar Sesión";
            btnCerrarSesion.Cursor = Cursors.Hand;

            // Animación Hover al pasar el cursor sobre Cerrar Sesión
            btnCerrarSesion.MouseEnter += (s, e) =>
            {
                btnCerrarSesion.BackColor = Color.FromArgb(69, 26, 26);
                btnCerrarSesion.ForeColor = Color.FromArgb(248, 113, 113);
            };

            btnCerrarSesion.MouseLeave += (s, e) =>
            {
                btnCerrarSesion.BackColor = Color.FromArgb(24, 30, 45);
                btnCerrarSesion.ForeColor = Color.FromArgb(239, 68, 68);
            };
        }

        private void SeleccionarBotonMenu(Button botonActivo, string tituloSeccion)
        {
            lblPageTitle.Text = tituloSeccion;

            ResetearBotonMenu(btnMenuDashboard);
            ResetearBotonMenu(btnMenuCatalogo);
            ResetearBotonMenu(btnMenuPrestamos);
            ResetearBotonMenu(btnMenuDevoluciones);
            ResetearBotonMenu(btnMenuPenalizaciones);
            ResetearBotonMenu(btnMenuUsuarios);
            ResetearBotonMenu(btnMenuAuditoria);

            _botonMenuSeleccionado = botonActivo;
            botonActivo.BackColor = ColorActiveSidebar;
            botonActivo.ForeColor = Color.White;
            botonActivo.Invalidate(); // Forzar redibujado de la barra acentuada
        }

        private void ResetearBotonMenu(Button? btn)
        {
            if (btn == null) return;
            btn.BackColor = ColorInactiveSidebar;
            btn.ForeColor = Color.FromArgb(148, 163, 184);
            btn.Invalidate();
        }

        #endregion

        private bool GestionPermisos()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            switch (rol)
            {
                case "Administrador":
                    btnMenuDashboard.Visible = true;
                    btnMenuCatalogo.Visible = true;
                    btnMenuPrestamos.Visible = true;
                    btnMenuPrestamos.Text = "🔄  Préstamos";
                    btnMenuDevoluciones.Visible = true;
                    btnMenuDevoluciones.Text = "↩️  Devoluciones";
                    btnMenuPenalizaciones.Visible = true; // 👈 Visible
                    btnMenuPenalizaciones.Text = "⚠️  Penalizaciones";
                    btnMenuUsuarios.Visible = true;
                    btnMenuAuditoria.Visible = true;
                    return true;

                case "Bibliotecario" or "PersonalBibliotecario":
                    btnMenuDashboard.Visible = true;
                    btnMenuCatalogo.Visible = true;
                    btnMenuPrestamos.Visible = true;
                    btnMenuPrestamos.Text = "🔄  Préstamos y Solicitudes";
                    btnMenuDevoluciones.Visible = true;
                    btnMenuDevoluciones.Text = "↩️  Devoluciones";
                    btnMenuPenalizaciones.Visible = true; // 👈 Visible
                    btnMenuPenalizaciones.Text = "⚠️  Penalizaciones";
                    btnMenuUsuarios.Visible = true;
                    btnMenuAuditoria.Visible = false;
                    return true;

                case "Auditor":
                    btnMenuDashboard.Visible = true;
                    btnMenuCatalogo.Visible = true;
                    btnMenuPrestamos.Visible = true;
                    btnMenuPrestamos.Text = "🔄  Préstamos";
                    btnMenuDevoluciones.Visible = true;
                    btnMenuDevoluciones.Text = "↩️  Devoluciones";
                    btnMenuPenalizaciones.Visible = false; // 🔒 Oculto para Auditor
                    btnMenuUsuarios.Visible = false;
                    btnMenuAuditoria.Visible = true;
                    return true;

                default:
                    MessageBox.Show("No posee un rol válido para acceder a las funciones del sistema de escritorio.",
                                    "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
            }
        }

        #region Funciones para abrir Formularios dentro del Panel Principal

        private void AbrirFormularioEnPanel(Form formularioHijo)
        {
            if (_formularioActivo != null)
            {
                _formularioActivo.Close();
            }

            _formularioActivo = formularioHijo;

            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            CambiarVisibilidadDashboard(false);

            pnlContent.Controls.Add(formularioHijo);
            pnlContent.Tag = formularioHijo;
            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        private void MostrarDashboard()
        {
            if (_formularioActivo != null)
            {
                _formularioActivo.Close();
                _formularioActivo = null;
            }

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

        #region Renderizado Vectorial Alinear Botones de Título

        private void BtnControlTitulo_Paint(object? sender, PaintEventArgs e)
        {
            var btn = (Button)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

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
            int cy = btn.Height / 2;

            using (var pen = new Pen(colorIcono, 1.5f))
            {
                if (btn == btnMinimize)
                {
                    e.Graphics.DrawLine(pen, cx - 5, cy, cx + 5, cy);
                }
                else if (btn == btnMaximize)
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        e.Graphics.DrawRectangle(pen, cx - 5, cy - 5, 10, 10);
                    }
                    else
                    {
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

        #region Eventos de Botones de Navegación Lateral y Cambio de Pantalla

        private void btnMenuDashboard_Click(object? sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuDashboard, "Panel Principal");
            MostrarDashboard();
        }

        private void btnMenuUsuarios_Click(object? sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuUsuarios, "Gestión de Usuarios");
            var frmUsuarios = Program.ServiceProvider.GetRequiredService<UsuarioForm>();
            AbrirFormularioEnPanel(frmUsuarios);
        }

        private void btnMenuCatalogo_Click(object? sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuCatalogo, "Catálogo de Libros");
            var catalogoForm = Program.ServiceProvider.GetRequiredService<CatalogoForm>();
            AbrirFormularioEnPanel(catalogoForm);
        }

        private void btnMenuPrestamos_Click(object? sender, EventArgs e)
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            bool esBibliotecario = (rol == "Bibliotecario" || rol == "PersonalBibliotecario");
            string tituloSeccion = esBibliotecario ? "Préstamos y Solicitudes" : "Préstamos";

            SeleccionarBotonMenu(btnMenuPrestamos, tituloSeccion);
            var prestamoForm = Program.ServiceProvider.GetRequiredService<PrestamoForm>();
            AbrirFormularioEnPanel(prestamoForm);
        }

        private void btnMenuDevoluciones_Click(object? sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuDevoluciones, "Historial de Devoluciones");
            var devolucionForm = Program.ServiceProvider.GetRequiredService<DevolucionForm>();
            AbrirFormularioEnPanel(devolucionForm);
        }

        // 🚀 Evento para el módulo de Penalizaciones
        private void btnMenuPenalizaciones_Click(object? sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuPenalizaciones, "Gestión de Penalizaciones");
            var penalizacionForm = Program.ServiceProvider.GetRequiredService<PenalizacionForm>();
            AbrirFormularioEnPanel(penalizacionForm);
        }

        private void btnMenuAuditoria_Click(object? sender, EventArgs e)
        {
            SeleccionarBotonMenu(btnMenuAuditoria, "Auditoría y Reportes");
            var reporteForm = Program.ServiceProvider.GetRequiredService<ReporteForm>();
            AbrirFormularioEnPanel(reporteForm);
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

        private void btnClose_Click(object sender, EventArgs e) => Application.Exit();

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