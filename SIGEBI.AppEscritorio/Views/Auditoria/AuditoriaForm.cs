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

            lblUsuarioId.Text = "Identificación:";
            lblUsuarioId.Location = new Point(10, 16);
            lblUsuarioId.AutoSize = true;

            txtUsuarioId.Location = new Point(102, 13);
            txtUsuarioId.Size = new Size(85, 23);

            lblEntidad.Location = new Point(198, 16);
            lblEntidad.AutoSize = true;

            txtEntidad.Location = new Point(250, 13);
            txtEntidad.Size = new Size(80, 23);

            chkUsarFechas.Location = new Point(340, 15);
            chkUsarFechas.AutoSize = true;

            lblFechaInicio.Location = new Point(435, 16);
            lblFechaInicio.AutoSize = true;

            dtpFechaInicio.Location = new Point(478, 13);
            dtpFechaInicio.Size = new Size(90, 23);

            lblFechaFin.Location = new Point(575, 16);
            lblFechaFin.AutoSize = true;

            dtpFechaFin.Location = new Point(618, 13);
            dtpFechaFin.Size = new Size(90, 23);

            btnConsultar.Location = new Point(715, 10);
            btnConsultar.Size = new Size(88, 30);

            btnRefrescar.Location = new Point(808, 10);
            btnRefrescar.Size = new Size(88, 30);

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

            txtUsuarioId.TextChanged -= FiltroEnTiempoReal_TextChanged;
            txtUsuarioId.TextChanged += FiltroEnTiempoReal_TextChanged;

            txtEntidad.TextChanged -= FiltroEnTiempoReal_TextChanged;
            txtEntidad.TextChanged += FiltroEnTiempoReal_TextChanged;

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

            // 🟢 Panel inferior Tip
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = fondoPanel
            };

            var lblHint = new Label
            {
                Text = "💡 Tip: Haz doble clic sobre un registro para ver la información completa de auditoría.",
                ForeColor = textoGris,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(10, 8)
            };

            pnlBottom.Controls.Add(lblHint);
            pnlContenedor.Controls.Add(pnlBottom);
            pnlBottom.SendToBack();

            ConfigurarGrid();
        }

        private async void FiltroEnTiempoReal_TextChanged(object? sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private void ConfigurarGrid()
        {
            dgvLogs.Columns.Clear();

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AuditoriaId",
                HeaderText = "ID Log",
                Width = 70,
                Visible = false
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Identificacion",
                HeaderText = "Identificación",
                Width = 110,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Usuario",
                Width = 180,
                DefaultCellStyle = { Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UsuarioId",
                HeaderText = "Usuario ID",
                Width = 90,
                Visible = false
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
                HeaderText = "Detalles",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaRegistro",
                HeaderText = "Fecha",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 🟢 Asignación del evento de doble clic
            dgvLogs.CellDoubleClick += DgvLogs_CellDoubleClick;
        }

        private void DgvLogs_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvLogs.Rows[e.RowIndex].DataBoundItem is LogAuditoriaResponseDto log)
            {
                using (var modal = new FrmDetalleAuditoria(log))
                {
                    modal.ShowDialog();
                }
            }
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

                if (!string.IsNullOrWhiteSpace(txtUsuarioId.Text))
                {
                    request.Identificacion = txtUsuarioId.Text.Trim();
                }

                if (!string.IsNullOrWhiteSpace(txtEntidad.Text))
                {
                    request.EntidadAfectada = txtEntidad.Text.Trim();
                }

                if (chkUsarFechas.Checked)
                {
                    request.FechaInicio = dtpFechaInicio.Value.Date;
                    request.FechaFin = dtpFechaFin.Value.Date.AddDays(1).AddTicks(-1);
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