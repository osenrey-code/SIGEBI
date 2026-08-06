using SIGEBI.AppEscritorio.Dtos.Penalizaciones;
using SIGEBI.AppEscritorio.Services.Penalizaciones;
using SIGEBI.AppEscritorio.Session;

namespace SIGEBI.AppEscritorio.Views.Penalizaciones
{
    public partial class PenalizacionForm : Form
    {
        private readonly IPenalizacionService _penalizacionService;
        private readonly IServiceProvider _serviceProvider;
        private PenalizacionDto? _penalizacionSeleccionada;

        public PenalizacionForm(IPenalizacionService penalizacionService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _penalizacionService = penalizacionService;
            _serviceProvider = serviceProvider;

            AplicarEstilosDarkSlate();
        }

        private async void PenalizacionForm_Load(object sender, EventArgs e)
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            if (rol.Equals("Auditor", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No tiene los permisos requeridos para acceder al módulo de Penalizaciones.",
                                "Acceso Denegado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                this.BeginInvoke(new Action(() => this.Close()));
                return;
            }

            cmbEstado.SelectedIndexChanged -= FiltroEnTiempoReal_Changed;

            cmbEstado.Items.Clear();
            cmbEstado.Items.AddRange(new object[] { "Todas", "Activa", "Resuelta" });
            cmbEstado.SelectedIndex = 1; // "Activa" por defecto

            cmbEstado.SelectedIndexChanged += FiltroEnTiempoReal_Changed;

            txtUsuarioId.TextChanged -= FiltroEnTiempoReal_Changed;
            txtUsuarioId.TextChanged += FiltroEnTiempoReal_Changed;

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
            pnlTotal.BackColor = fondoPanel;

            // Inputs
            txtUsuarioId.BackColor = fondoDark;
            txtUsuarioId.ForeColor = Color.White;
            txtUsuarioId.BorderStyle = BorderStyle.FixedSingle;

            cmbEstado.BackColor = fondoDark;
            cmbEstado.ForeColor = Color.White;

            btnRefrescar.BackColor = colorBotonGris;
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnRefrescar.Cursor = Cursors.Hand;

            // Grid
            dgvPenalizaciones.AutoGenerateColumns = false;
            dgvPenalizaciones.BackgroundColor = fondoPanel;
            dgvPenalizaciones.BorderStyle = BorderStyle.None;
            dgvPenalizaciones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPenalizaciones.GridColor = Color.FromArgb(51, 65, 85);
            dgvPenalizaciones.EnableHeadersVisualStyles = false;
            dgvPenalizaciones.RowHeadersVisible = false;
            dgvPenalizaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPenalizaciones.MultiSelect = false;
            dgvPenalizaciones.ReadOnly = true;

            dgvPenalizaciones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPenalizaciones.ColumnHeadersDefaultCellStyle.BackColor = fondoDark;
            dgvPenalizaciones.ColumnHeadersDefaultCellStyle.ForeColor = textoGris;
            dgvPenalizaciones.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoDark;
            dgvPenalizaciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvPenalizaciones.ColumnHeadersHeight = 40;

            dgvPenalizaciones.DefaultCellStyle.BackColor = fondoPanel;
            dgvPenalizaciones.DefaultCellStyle.ForeColor = Color.White;
            dgvPenalizaciones.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvPenalizaciones.DefaultCellStyle.SelectionBackColor = azulPrimario;
            dgvPenalizaciones.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPenalizaciones.RowTemplate.Height = 38;
            dgvPenalizaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);

            ConfigurarGridPenalizaciones();

            var lblHint = new Label
            {
                Text = "💡 Tip: Haz doble clic sobre una penalización para gestionarla o resolverla.",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(15, 16)
            };
            pnlTotal.Controls.Add(lblHint);
        }

        private void ConfigurarGridPenalizaciones()
        {
            dgvPenalizaciones.Columns.Clear();
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PenalizacionId", HeaderText = "ID", Visible = false });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrestamoId", HeaderText = "Préstamo ID", Visible = false });

            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdentificacionUsuario", HeaderText = "Identificación", Width = 140, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiasRetraso", HeaderText = "Días Retraso", Width = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MontoMora", HeaderText = "Mora (RD$)", Width = 110, DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) } });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Motivo", HeaderText = "Motivo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", HeaderText = "Estado", Width = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) } });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaGeneracion", HeaderText = "Fecha Generación", Width = 140, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvPenalizaciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MotivoResolucion", HeaderText = "Motivo Resolución", Width = 180 });

            dgvPenalizaciones.CellFormatting += DgvPenalizaciones_CellFormatting;
            dgvPenalizaciones.SelectionChanged += DgvPenalizaciones_SelectionChanged;
            dgvPenalizaciones.CellClick += DgvPenalizaciones_CellClick;
            dgvPenalizaciones.CellDoubleClick += DgvPenalizaciones_CellDoubleClick;
        }

        private void DgvPenalizaciones_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvPenalizaciones.Rows[e.RowIndex].DataBoundItem is PenalizacionDto pen)
            {
                if (dgvPenalizaciones.Columns[e.ColumnIndex].DataPropertyName == "Estado")
                {
                    if (pen.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68); // Rojo
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(34, 197, 94); // Verde
                    }
                }
            }
        }

        private void DgvPenalizaciones_SelectionChanged(object? sender, EventArgs e)
        {
            ActualizarSeleccionPenalizacion();
        }

        private void DgvPenalizaciones_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ActualizarSeleccionPenalizacion();
            }
        }

        private async void DgvPenalizaciones_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvPenalizaciones.Rows[e.RowIndex].DataBoundItem is PenalizacionDto penalizacionSeleccionada)
            {
                using (var modal = new Views.Penalizacion.DetallePenalizacion(_penalizacionService))
                {
                    modal.CargarPenalizacion(penalizacionSeleccionada);

                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        await CargarDatosAsync();
                    }
                }
            }
        }

        private void ActualizarSeleccionPenalizacion()
        {
            if (dgvPenalizaciones.CurrentRow?.DataBoundItem is PenalizacionDto penalizacion)
            {
                _penalizacionSeleccionada = penalizacion;
            }
            else
            {
                _penalizacionSeleccionada = null;
            }
        }

        #endregion

        #region Lógica de Negocio y Eventos

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            txtUsuarioId.Clear();
            cmbEstado.SelectedIndex = 0; // "Todas"
            await CargarDatosAsync();
        }
        private async void FiltroEnTiempoReal_Changed(object? sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var request = new ConsultarPenalizacionesRequestDto();

                if (cmbEstado.SelectedItem != null && cmbEstado.SelectedItem.ToString() != "Todas")
                {
                    request.Estado = cmbEstado.SelectedItem.ToString();
                }

                string filtroTexto = txtUsuarioId.Text.Trim();

                if (int.TryParse(filtroTexto, out int usuarioId) && usuarioId > 0)
                {
                    request.UsuarioId = usuarioId;
                }

                var resultadoApi = await _penalizacionService.ConsultarPenalizacionesAsync(request);
                var lista = resultadoApi?.ToList() ?? new List<PenalizacionDto>();

                if (!string.IsNullOrWhiteSpace(filtroTexto))
                {
                    lista = lista.Where(p =>
                        (!string.IsNullOrEmpty(p.IdentificacionUsuario) && p.IdentificacionUsuario.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase)) ||
                        p.UsuarioId.ToString() == filtroTexto ||
                        p.PrestamoId.ToString() == filtroTexto
                    ).ToList();
                }

                dgvPenalizaciones.DataSource = lista;
                ActualizarSeleccionPenalizacion();
                CalcularTotalMora(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar penalizaciones: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void CalcularTotalMora(IEnumerable<PenalizacionDto> lista)
        {
            decimal totalMora = lista != null ? lista.Sum(p => p.MontoMora) : 0m;

            if (lblTotalMoraValor != null)
            {
                lblTotalMoraValor.Text = $"RD$ {totalMora:N2}";
            }
        }

        #endregion
    }
}