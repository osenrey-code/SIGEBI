using SIGEBI.AppEscritorio.Dtos.Auditorias;
using SIGEBI.AppEscritorio.Services.Auditoria;
using SIGEBI.AppEscritorio.Session;

namespace SIGEBI.AppEscritorio.Views.Auditoria
{
    public partial class AuditoriaForm : Form
    {
        private readonly IAuditoriaService _auditoriaService;

        public AuditoriaForm(IAuditoriaService auditoriaService)
        {
            InitializeComponent();
            _auditoriaService = auditoriaService;

            AplicarEstilosDarkSlate();
        }

        private async void AuditoriaForm_Load(object sender, EventArgs e)
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            // 🔒 REGLA DE NEGOCIO: Únicamente Administrador y Auditor pueden ver el log de auditoría
            if (!rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) &&
                !rol.Equals("Auditor", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No tiene los permisos requeridos para acceder al módulo de Auditoría.",
                                "Acceso Denegado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            // Rango por defecto: Último mes
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            dtpFechaFin.Value = DateTime.Now;

            await CargarDatosAsync();
        }

        #region Estilos y Configuración UI

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);      // #0F172A
            Color fondoPanel = Color.FromArgb(30, 41, 59);     // #1E293B
            Color textoGris = Color.FromArgb(148, 163, 184);  // #94A3B8
            Color azulPrimario = Color.FromArgb(37, 99, 235);  // #2563EB
            Color colorBotonGris = Color.FromArgb(51, 65, 85); // #334155

            this.BackColor = fondoDark;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            pnlContenedor.BackColor = fondoPanel;
            pnlFiltros.BackColor = fondoPanel;

            // Inputs
            txtUsuarioId.BackColor = fondoDark;
            txtUsuarioId.ForeColor = Color.White;
            txtUsuarioId.BorderStyle = BorderStyle.FixedSingle;

            txtEntidad.BackColor = fondoDark;
            txtEntidad.ForeColor = Color.White;
            txtEntidad.BorderStyle = BorderStyle.FixedSingle;

            dtpFechaInicio.CalendarMonthBackground = fondoDark;
            dtpFechaInicio.CalendarTitleBackColor = fondoDark;
            dtpFechaFin.CalendarMonthBackground = fondoDark;
            dtpFechaFin.CalendarTitleBackColor = fondoDark;

            // Botones
            btnConsultar.BackColor = azulPrimario;
            btnConsultar.ForeColor = Color.White;
            btnConsultar.FlatStyle = FlatStyle.Flat;
            btnConsultar.FlatAppearance.BorderSize = 0;
            btnConsultar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnConsultar.Cursor = Cursors.Hand;

            btnRefrescar.BackColor = colorBotonGris;
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnRefrescar.Cursor = Cursors.Hand;

            // DataGridView Configuración
            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.BackgroundColor = fondoPanel;
            dgvLogs.BorderStyle = BorderStyle.None;
            dgvLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLogs.GridColor = Color.FromArgb(51, 65, 85);
            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.MultiSelect = false;
            dgvLogs.ReadOnly = true;

            dgvLogs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = fondoDark;
            dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = textoGris;
            dgvLogs.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoDark;
            dgvLogs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvLogs.ColumnHeadersHeight = 40;

            dgvLogs.DefaultCellStyle.BackColor = fondoPanel;
            dgvLogs.DefaultCellStyle.ForeColor = Color.White;
            dgvLogs.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvLogs.DefaultCellStyle.SelectionBackColor = azulPrimario;
            dgvLogs.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLogs.RowTemplate.Height = 38;
            dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);

            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvLogs.Columns.Clear();

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AuditoriaId",
                HeaderText = "ID Log",
                Width = 70,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UsuarioId",
                HeaderText = "Usuario ID",
                Width = 90,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Accion",
                HeaderText = "Acción",
                Width = 160,
                DefaultCellStyle = { Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EntidadAfectada",
                HeaderText = "Entidad",
                Width = 130,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Detalle",
                HeaderText = "Detalles de la Operación",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaRegistro",
                HeaderText = "Fecha y Hora",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
        }

        #endregion

        #region Eventos y Lógica

        private void chkUsarFechas_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaInicio.Enabled = chkUsarFechas.Checked;
            dtpFechaFin.Enabled = chkUsarFechas.Checked;
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            txtUsuarioId.Clear();
            txtEntidad.Clear();
            chkUsarFechas.Checked = false;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var request = new ConsultarLogAuditoriaRequestDto();

                if (int.TryParse(txtUsuarioId.Text.Trim(), out int usuarioId) && usuarioId > 0)
                {
                    request.UsuarioId = usuarioId;
                }

                if (!string.IsNullOrWhiteSpace(txtEntidad.Text))
                {
                    request.EntidadAfectada = txtEntidad.Text.Trim();
                }

                if (chkUsarFechas.Checked)
                {
                    request.FechaInicio = dtpFechaInicio.Value.Date; // 00:00:00
                    request.FechaFin = dtpFechaFin.Value.Date.AddDays(1).AddTicks(-1); // 23:59:59
                }

                var logs = await _auditoriaService.ConsultarLogsAsync(request);

                dgvLogs.DataSource = logs;
                dgvLogs.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los logs de auditoría: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}