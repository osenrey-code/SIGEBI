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
            // Paleta de Colores Dark Slate
            Color fondoPrincipal = Color.FromArgb(15, 23, 42);      // #0F172A
            Color fondoPaneles = Color.FromArgb(30, 41, 59);        // #1E293B
            Color textoSecundario = Color.FromArgb(148, 163, 184);  // Gris claro
            Color colorPrimario = Color.FromArgb(37, 99, 235);      // Azul
            Color colorPeligro = Color.FromArgb(239, 68, 68);       // Rojo
            Color colorAdvertencia = Color.FromArgb(245, 158, 11);  // Naranja/Amarillo
            Color colorRefrescar = Color.FromArgb(51, 65, 85);      // Gris pizarra

            // 1. Configuración de la Ventana
            this.BackColor = fondoPrincipal;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            // 2. Panel Superior de Botones
            pnlBotones.BackColor = fondoPaneles;
            pnlBotones.Height = 60;
            pnlBotones.Padding = new Padding(15, 11, 15, 11);

            // Estilos de la Botonera
            ConfigurarBoton(btnNuevo, "➕  Nuevo Recurso", colorPrimario, Color.White, 160);
            ConfigurarBoton(btnEditar, "✏️  Editar", colorAdvertencia, Color.White, 110);
            ConfigurarBoton(btnEliminar, "🗑️  Desactivar", colorPeligro, Color.White, 120);
            ConfigurarBoton(btnRecargar, "🔄  Refrescar", colorRefrescar, Color.White, 120);

            // 🟢 Habilitado para bibliotecarios/administradores (se controla visibilidad por rol en ValidarPermisosPorRol)
            btnEliminar.Visible = true;

            // 3. Formateo Profesional del DataGridView
            dgvCatalogo.AutoGenerateColumns = false; // 👈 Evita columnas automáticas
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

            ConfigurarColumnasGrid();
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
                DataPropertyName = "RecursoBibliograficoId",
                HeaderText = "ID",
                Width = 65,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

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
                Width = 130,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AnioPublicado",
                HeaderText = "Año",
                Width = 75,
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
            ValidarPermisosPorRol(); // 👈 Oculta los botones de gestión si el usuario es Auditor
            await CargarDatosAsync();
        }

        private void ValidarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            // 🔒 El Auditor solo tiene permisos de lectura/consulta en el catálogo
            if (rol == "Auditor")
            {
                btnNuevo.Visible = false;
                btnEditar.Visible = false;
                btnEliminar.Visible = false;
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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            var modal = _serviceProvider.GetRequiredService<GestionarRecursoForm>();

            if (modal.ShowDialog() == DialogResult.OK)
            {
                _ = CargarDatosAsync();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow?.DataBoundItem is RecursoResponse recursoSeleccionado)
            {
                var modal = _serviceProvider.GetRequiredService<GestionarRecursoForm>();
                modal.CargarDatosParaEdicion(recursoSeleccionado);

                if (modal.ShowDialog() == DialogResult.OK)
                {
                    _ = CargarDatosAsync();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un recurso de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow?.DataBoundItem is RecursoResponse recursoSeleccionado)
            {
                var confirmacion = MessageBox.Show(
                    $"¿Está seguro de que desea desactivar el recurso '{recursoSeleccionado.Titulo}'?\n\nEl recurso y sus ejemplares quedarán fuera de servicio y se ocultarán del catálogo activo.",
                    "Confirmar Desactivación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        await _catalogoService.EliminarRecursoAsync(recursoSeleccionado.RecursoBibliograficoId, "Desactivado desde el panel de escritorio");

                        MessageBox.Show("Recurso y ejemplares desactivados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo desactivar el recurso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un recurso de la lista para desactivar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnRecargar_Click(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }
    }
}