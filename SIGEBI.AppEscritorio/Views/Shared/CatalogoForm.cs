using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Response;
using SIGEBI.AppEscritorio.Services.Interfaces;
using SIGEBI.AppEscritorio.Session;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class CatalogoForm : Form
    {
        private readonly ICatalogoService _catalogoService;
        private readonly IServiceProvider _serviceProvider;

        public CatalogoForm(ICatalogoService catalogoService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _catalogoService = catalogoService;
            _serviceProvider = serviceProvider;

            AplicarDisenoModerno();
        }

        #region Estilos y Diseño UI

        private void AplicarDisenoModerno()
        {
            Color fondoPrincipal = Color.FromArgb(15, 23, 42);      // #0F172A
            Color fondoPaneles = Color.FromArgb(30, 41, 59);        // #1E293B
            Color textoSecundario = Color.FromArgb(148, 163, 184);  // Gris claro
            Color colorPrimario = Color.FromArgb(37, 99, 235);      // Azul
            Color colorRefrescar = Color.FromArgb(51, 65, 85);      // Gris pizarra

            // 1. Configuración de la Ventana
            this.BackColor = fondoPrincipal;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            // 2. Panel Superior de Botones (Limpio, solo acciones globales)
            pnlBotones.BackColor = fondoPaneles;
            pnlBotones.Height = 60;
            pnlBotones.Padding = new Padding(15, 11, 15, 11);

            ConfigurarBoton(btnNuevo, "➕  Nuevo Recurso", colorPrimario, Color.White, 160);
            ConfigurarBoton(btnRecargar, "🔄  Refrescar", colorRefrescar, Color.White, 120);

            // 3. Formateo del DataGridView
            dgvCatalogo.AutoGenerateColumns = false;
            dgvCatalogo.BackgroundColor = fondoPaneles;
            dgvCatalogo.BorderStyle = BorderStyle.None;
            dgvCatalogo.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCatalogo.GridColor = Color.FromArgb(51, 65, 85);
            dgvCatalogo.EnableHeadersVisualStyles = false;
            dgvCatalogo.RowHeadersVisible = false;
            dgvCatalogo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCatalogo.MultiSelect = false;
            dgvCatalogo.ReadOnly = true;

            // Cabeceras
            dgvCatalogo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCatalogo.ColumnHeadersDefaultCellStyle.BackColor = fondoPrincipal;
            dgvCatalogo.ColumnHeadersDefaultCellStyle.ForeColor = textoSecundario;
            dgvCatalogo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvCatalogo.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoPrincipal;
            dgvCatalogo.ColumnHeadersHeight = 42;

            // Filas y Celdas
            dgvCatalogo.DefaultCellStyle.BackColor = fondoPaneles;
            dgvCatalogo.DefaultCellStyle.ForeColor = Color.White;
            dgvCatalogo.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvCatalogo.DefaultCellStyle.SelectionBackColor = colorPrimario;
            dgvCatalogo.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCatalogo.RowTemplate.Height = 38;

            dgvCatalogo.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);
            dgvCatalogo.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvCatalogo.AlternatingRowsDefaultCellStyle.SelectionBackColor = colorPrimario;

            dgvCatalogo.CellDoubleClick += DgvCatalogo_CellDoubleClick;

            ConfigurarColumnasGrid();

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = fondoPaneles
            };
            var lblHint = new Label
            {
                Text = "💡 Tip: Haz doble clic sobre cualquier recurso para editarlo o gestionarlo.",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(15, 11)
            };
            pnlBottom.Controls.Add(lblHint);
            this.Controls.Add(pnlBottom);
        }

        private void ConfigurarBoton(Button btn, string texto, Color bg, Color fg, int ancho)
        {
            btn.Text = texto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = fg;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Size = new Size(ancho, 38);
        }

        private void ConfigurarColumnasGrid()
        {
            dgvCatalogo.Columns.Clear();

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ISBN",
                HeaderText = "ISBN",
                Width = 140,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Titulo",
                HeaderText = "Título del Recurso",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 180
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Autor",
                HeaderText = "Autor",
                Width = 150
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Categoria",
                HeaderText = "Categoría",
                Width = 140,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AnioPublicado",
                HeaderText = "Año",
                Width = 80,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalEjemplares",
                HeaderText = "Ejemplares",
                Width = 95,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CopiasDisponibles",
                HeaderText = "Disponibles",
                Width = 100,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });
        }

        #endregion

        private async void CatalogoForm_Load(object sender, EventArgs e)
        {
            ValidarPermisosPorRol();
            await CargarDatosAsync();
        }

        private void ValidarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            if (rol == "Auditor")
            {
                btnNuevo.Visible = false;
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var recursos = await _catalogoService.ConsultarTodosAsync();
                dgvCatalogo.DataSource = recursos?.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el catálogo: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void DgvCatalogo_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvCatalogo.Rows[e.RowIndex].DataBoundItem is RecursoResponse recursoSeleccionado)
            {
                await AbrirModalDetallesAsync(recursoSeleccionado.RecursoBibliograficoId);
            }
        }

        private async Task AbrirModalDetallesAsync(int recursoId)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var modal = _serviceProvider.GetRequiredService<GestionarRecursoForm>();
                // Carga la data asíncronamente desde el endpoint de detalle para traer la imagen y la descripción
                await modal.CargarDatosParaEdicionAsync(recursoId);

                this.Cursor = Cursors.Default;

                if (modal.ShowDialog() == DialogResult.OK)
                {
                    await CargarDatosAsync();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Ocurrió un error al intentar abrir los detalles del recurso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            var modal = _serviceProvider.GetRequiredService<GestionarRecursoForm>();

            // ID = 0 por defecto indicará al formulario que inicie en "Modo Edición" para registrar
            if (modal.ShowDialog() == DialogResult.OK)
            {
                _ = CargarDatosAsync();
            }
        }

        private async void btnRecargar_Click(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }
    }
}