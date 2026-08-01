using SIGEBI.AppEscritorio.Services.Categoria;
using SIGEBI.AppEscritorio.Session;


namespace SIGEBI.AppEscritorio.Views.Categorias
{
    public partial class CategoriaForm : Form
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaForm(ICategoriaService categoriaService)
        {
            InitializeComponent();
            _categoriaService = categoriaService;

            AplicarEstilosDarkSlate();
        }

        private async void CategoriaForm_Load(object sender, EventArgs e)
        {
            // Solo Administrador puede ver el botón de crear nueva categoría (según el backend [Authorize(Roles = "Administrador")])
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            btnNuevo.Visible = rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            await CargarDatosAsync();
        }

        #region Estilos y UI

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);
            Color fondoPanel = Color.FromArgb(30, 41, 59);
            Color textoGris = Color.FromArgb(148, 163, 184);
            Color azulPrimario = Color.FromArgb(37, 99, 235);
            Color verdeEsmeralda = Color.FromArgb(22, 163, 74);
            Color colorBotonGris = Color.FromArgb(51, 65, 85);

            this.BackColor = fondoDark;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            pnlContenedor.BackColor = fondoPanel;
            pnlFiltros.BackColor = fondoPanel;

            txtBuscar.BackColor = fondoDark;
            txtBuscar.ForeColor = Color.White;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;

            btnBuscar.BackColor = azulPrimario;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnBuscar.Cursor = Cursors.Hand;

            btnNuevo.BackColor = verdeEsmeralda;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnNuevo.Cursor = Cursors.Hand;

            btnRefrescar.BackColor = colorBotonGris;
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnRefrescar.Cursor = Cursors.Hand;

            // DataGridView Configuración
            dgvCategorias.AutoGenerateColumns = false;
            dgvCategorias.BackgroundColor = fondoPanel;
            dgvCategorias.BorderStyle = BorderStyle.None;
            dgvCategorias.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCategorias.GridColor = Color.FromArgb(51, 65, 85);
            dgvCategorias.EnableHeadersVisualStyles = false;
            dgvCategorias.RowHeadersVisible = false;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.MultiSelect = false;
            dgvCategorias.ReadOnly = true;

            dgvCategorias.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCategorias.ColumnHeadersDefaultCellStyle.BackColor = fondoDark;
            dgvCategorias.ColumnHeadersDefaultCellStyle.ForeColor = textoGris;
            dgvCategorias.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoDark;
            dgvCategorias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvCategorias.ColumnHeadersHeight = 40;

            dgvCategorias.DefaultCellStyle.BackColor = fondoPanel;
            dgvCategorias.DefaultCellStyle.ForeColor = Color.White;
            dgvCategorias.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvCategorias.DefaultCellStyle.SelectionBackColor = azulPrimario;
            dgvCategorias.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCategorias.RowTemplate.Height = 38;
            dgvCategorias.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);

            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvCategorias.Columns.Clear();

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoriaId",
                HeaderText = "ID",
                Width = 70,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre de Categoría",
                Width = 220,
                DefaultCellStyle = { Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                HeaderText = "Descripción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        #endregion

        #region Eventos y Lógica

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var lista = await _categoriaService.ConsultarCategoriasAsync();
                string filtro = txtBuscar.Text.Trim();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    lista = lista.Where(c =>
                        c.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                        c.Descripcion.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                dgvCategorias.DataSource = lista;
                dgvCategorias.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (var frmModal = new FrmRegistrarCategoria(_categoriaService))
            {
                if (frmModal.ShowDialog() == DialogResult.OK)
                {
                    _ = CargarDatosAsync();
                }
            }
        }

        #endregion
    }
}