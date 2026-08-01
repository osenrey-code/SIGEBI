using SIGEBI.AppEscritorio.Dtos.Catalogo.Request;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Response;
using SIGEBI.AppEscritorio.Services.Categoria;
using SIGEBI.AppEscritorio.Services.Interfaces;
using SIGEBI.AppEscritorio.Session; 
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class GestionarRecursoForm : Form
    {
        private readonly ICatalogoService _catalogoService;
        private readonly ICategoriaService _categoriaService;
        private int _recursoIdActual = 0;
        private string _rutaImagenSeleccionada = string.Empty;
        private string _urlImagenActual = string.Empty;

        // 🎨 PALETA DE COLORES
        private readonly Color bgForm = Color.FromArgb(30, 41, 59);       // Fondo de la ventana
        private readonly Color bgInputs = Color.FromArgb(15, 23, 42);     // Fondo de los inputs
        private readonly Color textPrimary = Color.White;                 // Texto principal
        private readonly Color textMuted = Color.FromArgb(148, 163, 184); // Textos secundarios
        private readonly Color primaryColor = Color.FromArgb(59, 130, 246); // Azul
        private readonly Color borderColor = Color.FromArgb(71, 85, 105); // Color del borde

        public GestionarRecursoForm(ICatalogoService catalogoService, ICategoriaService categoriaService)
        {
            InitializeComponent();
            _catalogoService = catalogoService;
            _categoriaService = categoriaService;

            // Configuración inicial de la ventana
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = bgForm;
            this.Size = new Size(600, 540);

            this.Load += async (s, e) =>
            {
                ForzarLayoutPerfecto();
                await CargarCategoriasAsync();
            };

            picPortada.Paint += PicPortada_Paint;
        }

        #region Renderizado y Layout Perfecto

        private void ForzarLayoutPerfecto()
        {
            AplicarBordesRedondeadosForm();
            CrearBarraTituloCustom();

            // 1. FUENTES GLOBALES
            Font fontLabel = new Font("Segoe UI Semibold", 9.5F);
            Font fontInput = new Font("Segoe UI", 11F);

            // 2. POSICIONES ABSOLUTAS
            int colIzquierda = 30;
            int colDerecha = 330;
            int anchoInput = 260;

            lblISBN.Location = new Point(colIzquierda, 65);
            txtISBN.Location = new Point(colIzquierda, 90);
            txtISBN.Size = new Size(anchoInput, 30);

            lblTitulo.Location = new Point(colIzquierda, 145);
            txtTitulo.Location = new Point(colIzquierda, 170);
            txtTitulo.Size = new Size(anchoInput, 30);

            lblAutor.Location = new Point(colIzquierda, 225);
            txtAutor.Location = new Point(colIzquierda, 250);
            txtAutor.Size = new Size(anchoInput, 30);

            lblCategoria.Location = new Point(colIzquierda, 305);
            cmbCategoria.Location = new Point(colIzquierda, 330);
            cmbCategoria.Size = new Size(anchoInput, 30);

            lblAnio.Location = new Point(colIzquierda, 385);
            numAnio.Location = new Point(colIzquierda, 410);
            numAnio.Size = new Size(120, 30);

            lblEjemplares.Location = new Point(colIzquierda + 140, 385);
            numEjemplares.Location = new Point(colIzquierda + 140, 410);
            numEjemplares.Size = new Size(120, 30);

            picPortada.Location = new Point(colDerecha, 65);
            picPortada.Size = new Size(240, 295);
            picPortada.BackColor = bgInputs;

            btnSeleccionarFoto.Location = new Point(colDerecha, 375);
            btnSeleccionarFoto.Size = new Size(240, 40);

            btnCancelar.Location = new Point(350, 475);
            btnCancelar.Size = new Size(100, 40);

            btnGuardar.Location = new Point(470, 475);
            btnGuardar.Size = new Size(100, 40);

            // 3. APLICAR ESTILOS A LOS CONTROLES
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label lbl && lbl.Name != "lblTituloApp")
                {
                    lbl.ForeColor = textMuted;
                    lbl.Font = fontLabel;
                }
            }

            Control[] inputs = { txtISBN, txtTitulo, txtAutor, cmbCategoria, numAnio, numEjemplares };
            foreach (Control input in inputs)
            {
                input.BackColor = bgInputs;
                input.ForeColor = textPrimary;
                input.Font = fontInput;

                if (input is TextBox txt) txt.BorderStyle = BorderStyle.None;
                if (input is ComboBox cmb) cmb.FlatStyle = FlatStyle.Flat;
                if (input is NumericUpDown num)
                {
                    num.BorderStyle = BorderStyle.None;
                    num.Controls[0].BackColor = bgInputs;
                }

                CrearLineaUnderline(input);
            }

            EstilitarBotonPildora(btnGuardar, primaryColor, Color.White);
            EstilitarBotonPildora(btnCancelar, bgInputs, Color.White);
            EstilitarBotonPildora(btnSeleccionarFoto, bgInputs, Color.White);
        }

        private void CrearBarraTituloCustom()
        {
            Panel pnlHeader = new Panel { Height = 45, Dock = DockStyle.Top, BackColor = bgForm };
            Label lblTituloApp = new Label { Name = "lblTituloApp", Text = "Detalles del Recurso", ForeColor = Color.White, Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Location = new Point(15, 12) };
            Button btnCerrar = new Button { Text = "✕", ForeColor = textMuted, FlatStyle = FlatStyle.Flat, Size = new Size(45, 45), Location = new Point(this.Width - 45, 0), Cursor = Cursors.Hand, Font = new Font("Segoe UI", 12F, FontStyle.Bold) };

            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.MouseEnter += (s, e) => { btnCerrar.ForeColor = Color.White; btnCerrar.BackColor = Color.FromArgb(239, 68, 68); };
            btnCerrar.MouseLeave += (s, e) => { btnCerrar.ForeColor = textMuted; btnCerrar.BackColor = bgForm; };
            btnCerrar.Click += (s, e) => this.Close();

            pnlHeader.Controls.Add(lblTituloApp);
            pnlHeader.Controls.Add(btnCerrar);
            this.Controls.Add(pnlHeader);

            HabilitarArrastre(pnlHeader);
            HabilitarArrastre(lblTituloApp);
        }

        private void CrearLineaUnderline(Control ctrl)
        {
            Panel linea = new Panel { Height = 2, Width = ctrl.Width, Location = new Point(ctrl.Location.X, ctrl.Bottom + 2), BackColor = borderColor };
            this.Controls.Add(linea);

            ctrl.Enter += (s, e) => linea.BackColor = primaryColor;
            ctrl.Leave += (s, e) => linea.BackColor = borderColor;
        }

        private void PicPortada_Paint(object? sender, PaintEventArgs e)
        {
            if (picPortada.Image != null) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen penDashed = new Pen(textMuted, 2) { DashStyle = DashStyle.Dash })
            {
                e.Graphics.DrawRectangle(penDashed, 1, 1, picPortada.Width - 3, picPortada.Height - 3);
            }

            string texto = "Seleccione una\nportada para\nel recurso";
            using (Font font = new Font("Segoe UI", 10F, FontStyle.Italic))
            using (Brush brush = new SolidBrush(textMuted))
            {
                StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(texto, font, brush, picPortada.ClientRectangle, format);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(borderColor, 2))
            {
                e.Graphics.DrawPath(pen, CrearRutaRedondeada(new Rectangle(1, 1, this.Width - 2, this.Height - 2), 15));
            }
        }

        private void EstilitarBotonPildora(Button btn, Color bg, Color fg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = fg;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(bgForm);

                using (GraphicsPath path = CrearRutaRedondeada(new Rectangle(0, 0, btn.Width, btn.Height), 8))
                using (SolidBrush brush = new SolidBrush(btn.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btn.MouseEnter += (s, e) => { btn.BackColor = ControlPaint.Light(bg); btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { btn.BackColor = bg; btn.Invalidate(); };
            btn.MouseDown += (s, e) => { btn.BackColor = ControlPaint.Dark(bg); btn.Invalidate(); };
            btn.MouseUp += (s, e) => { btn.BackColor = bg; btn.Invalidate(); };
        }

        [DllImport("gdi32.dll")] private static extern IntPtr CreateRoundRectRgn(int nL, int nT, int nR, int nB, int nW, int nH);
        private void AplicarBordesRedondeadosForm() { this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 15, 15)); }

        private GraphicsPath CrearRutaRedondeada(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radio * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private void HabilitarArrastre(Control control) { control.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); } }; }

        #endregion

        // ------------------ LÓGICA DE NEGOCIO ------------------

        private async Task CargarCategoriasAsync()
        {
            try
            {
                var categorias = await _categoriaService.ConsultarCategoriasAsync();

                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "CategoriaId";
                cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargarDatosParaEdicion(RecursoResponse recurso)
        {
            _recursoIdActual = recurso.RecursoBibliograficoId;

            txtISBN.Text = recurso.ISBN;
            txtISBN.Enabled = false;

            txtTitulo.Text = recurso.Titulo;
            txtAutor.Text = recurso.Autor;

            cmbCategoria.SelectedValue = recurso.CategoriaId;

            numAnio.Value = recurso.AnioPublicado;
            numEjemplares.Value = recurso.TotalEjemplares;

            // 🔒 REGLA DE NEGOCIO: Habilitar modificación de ejemplares ÚNICAMENTE para Administradores
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            bool esAdministrador = rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            numEjemplares.Enabled = esAdministrador;

            if (esAdministrador)
            {
                // Se fija como valor mínimo la cantidad actual para asegurar que sólo se pueda AUMENTAR
                numEjemplares.Minimum = recurso.TotalEjemplares;
            }

            _urlImagenActual = recurso.ImagenUrl ?? string.Empty;
            btnGuardar.Text = "Actualizar";
        }

        private void btnSeleccionarFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png", Title = "Seleccionar Portada" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _rutaImagenSeleccionada = ofd.FileName;
                picPortada.Image = Image.FromFile(_rutaImagenSeleccionada);
                picPortada.SizeMode = PictureBoxSizeMode.Zoom;
                picPortada.Invalidate();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                btnGuardar.Enabled = false;

                if (_recursoIdActual == 0)
                {
                    var request = new RegistrarRecursoRequest
                    {
                        ISBN = txtISBN.Text.Trim(),
                        Titulo = txtTitulo.Text.Trim(),
                        Autor = txtAutor.Text.Trim(),
                        CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                        AnioPublicado = (int)numAnio.Value,
                        CantidadEjemplares = (int)numEjemplares.Value,
                        RutaImagenLocal = _rutaImagenSeleccionada
                    };
                    await _catalogoService.RegistrarRecursoAsync(request);
                }
                else
                {
                    var request = new ActualizarRecursoRequest
                    {
                        RecursoBibliograficoId = _recursoIdActual,
                        Titulo = txtTitulo.Text.Trim(),
                        Autor = txtAutor.Text.Trim(),
                        CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                        AnioPublicado = (int)numAnio.Value,
                        CantidadEjemplares = (int)numEjemplares.Value, // 👈 Envía la nueva cantidad de ejemplares al actualizar
                        ImagenUrlActual = _urlImagenActual,
                        RutaNuevaImagenLocal = _rutaImagenSeleccionada
                    };
                    await _catalogoService.ActualizarRecursoAsync(request);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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