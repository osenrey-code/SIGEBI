using SIGEBI.AppEscritorio.Dtos.Usuarios;
using SIGEBI.AppEscritorio.Services.Usuario;
using SIGEBI.AppEscritorio.Session;

namespace SIGEBI.AppEscritorio.Views.Usuario
{
    public partial class UsuarioForm : Form
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioForm(IUsuarioService usuarioService)
        {
            InitializeComponent();
            _usuarioService = usuarioService;

            ConfigurarDisenoTabla();
        }

        private async void UsuarioForm_Load(object sender, EventArgs e)
        {
            AplicarPermisosPorRol();

            cmbFiltroTipo.SelectedIndexChanged -= Filtros_Changed;
            cmbFiltroEstado.SelectedIndexChanged -= Filtros_Changed;

            if (cmbFiltroTipo.Items.Count > 0) cmbFiltroTipo.SelectedIndex = 0;
            if (cmbFiltroEstado.Items.Count > 0) cmbFiltroEstado.SelectedIndex = 0;

            // 🟢 Filtrado automático en tiempo real
            txtBusqueda.TextChanged -= Filtros_Changed;
            txtBusqueda.TextChanged += Filtros_Changed;

            cmbFiltroTipo.SelectedIndexChanged += Filtros_Changed;
            cmbFiltroEstado.SelectedIndexChanged += Filtros_Changed;

            await CargarUsuariosAsync();
        }

        #region Configuración Estética de la Tabla

        private void ConfigurarDisenoTabla()
        {
            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.GridColor = Color.FromArgb(51, 65, 85);

            // Estilo de Encabezados
            dgvUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dgvUsuarios.ColumnHeadersHeight = 40;

            // Estilo de Filas Principales
            dgvUsuarios.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgvUsuarios.DefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsuarios.RowTemplate.Height = 40;

            // Filas Alternadas
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);
            dgvUsuarios.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);

            dgvUsuarios.Columns.Clear();

            // Columnas de datos
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UsuarioId",
                HeaderText = "ID",
                Visible = false
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Identificacion",
                HeaderText = "Identificación",
                Width = 140
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 130
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Correo",
                HeaderText = "Correo Electrónico",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 140
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoUsuario",
                HeaderText = "Rol",
                Width = 130
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 110,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            // 🟢 Panel inferior para el Tip informativo
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            var lblHint = new Label
            {
                Text = "💡 Tip: Haz doble clic sobre un usuario para ver sus detalles y gestionarlo.",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(10, 10)
            };

            pnlBottom.Controls.Add(lblHint);

            // Añadir el panel al contenedor principal del Grid
            pnlGrid.Controls.Add(pnlBottom);
            pnlBottom.SendToBack(); // Mantiene pnlBottom abajo y dgvUsuarios tomando el espacio restante en Dock.Fill

            dgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting;
            dgvUsuarios.CellDoubleClick += DgvUsuarios_CellDoubleClick;
        }

        private void DgvUsuarios_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvUsuarios.Columns[e.ColumnIndex].DataPropertyName == "Estado" && e.Value != null)
            {
                string estado = e.Value.ToString() ?? "";
                if (estado.Equals("Activo", StringComparison.OrdinalIgnoreCase))
                {
                    e.Value = "● Activo";
                    e.CellStyle.ForeColor = Color.FromArgb(34, 197, 94);
                    e.CellStyle.SelectionForeColor = Color.FromArgb(134, 239, 172);
                }
                else
                {
                    e.Value = "● Inactivo";
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                    e.CellStyle.SelectionForeColor = Color.FromArgb(254, 202, 202);
                }
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            }
        }

        private async void DgvUsuarios_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvUsuarios.Rows[e.RowIndex].DataBoundItem is UsuarioDto usuario)
            {
                using (var modal = new FrmDetalleUsuario(_usuarioService, usuario))
                {
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        await CargarUsuariosAsync();
                    }
                }
            }
        }

        #endregion

        #region Permisos y Carga de Datos

        private void AplicarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            bool esAdmin = rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            btnNuevoUsuario.Visible = esAdmin;

            if (rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("PersonalBibliotecario", StringComparison.OrdinalIgnoreCase))
            {
                cmbFiltroTipo.Items.Clear();
                cmbFiltroTipo.Items.Add("Todos");
                cmbFiltroTipo.Items.Add("Estudiante");
                cmbFiltroTipo.Items.Add("Docente");
            }
        }

        public async Task CargarUsuariosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string? busqueda = string.IsNullOrWhiteSpace(txtBusqueda.Text) ? null : txtBusqueda.Text.Trim();

                var filtro = new ConsultarUsuariosFiltroDto
                {
                    TipoUsuario = cmbFiltroTipo.SelectedItem?.ToString() == "Todos" ? null : cmbFiltroTipo.SelectedItem?.ToString(),
                    Estado = cmbFiltroEstado.SelectedItem?.ToString() == "Todos" ? null : cmbFiltroEstado.SelectedItem?.ToString()
                };

                var usuarios = await _usuarioService.ConsultarUsuariosAsync(filtro);

                string rolActual = UserSession.Instancia.TipoUsuario ?? string.Empty;
                if (rolActual.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase) ||
                    rolActual.Equals("PersonalBibliotecario", StringComparison.OrdinalIgnoreCase))
                {
                    usuarios = usuarios
                        ?.Where(u => u.TipoUsuario.Equals("Estudiante", StringComparison.OrdinalIgnoreCase) ||
                                     u.TipoUsuario.Equals("Docente", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(busqueda) && usuarios != null)
                {
                    usuarios = usuarios.Where(u =>
                        (!string.IsNullOrEmpty(u.NombreCompleto) && u.NombreCompleto.Contains(busqueda, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(u.Identificacion) && u.Identificacion.Contains(busqueda, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(u.Correo) && u.Correo.Contains(busqueda, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                dgvUsuarios.DataSource = usuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Búsqueda y Filtros

        private async void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmRegistrarUsuario(_usuarioService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await CargarUsuariosAsync();
                }
            }
        }

        private async void Filtros_Changed(object? sender, EventArgs e)
        {
            await CargarUsuariosAsync();
        }

        #endregion
    }
}