using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Dtos.Prestamos;
using SIGEBI.AppEscritorio.Services.Devolucion;
using SIGEBI.AppEscritorio.Services.Prestamo;
using SIGEBI.AppEscritorio.Session;
using SIGEBI.AppEscritorio.Views.Shared;

namespace SIGEBI.AppEscritorio.Views.Prestamo
{
    public partial class PrestamoForm : Form
    {
        private readonly IPrestamoService _prestamoService;
        private readonly IDevolucionService _devolucionService;
        private readonly IServiceProvider _serviceProvider;

        // Barra de Navegación Superior (Tabs Personalizados)
        private Panel pnlNavegacion = null!;
        private Button btnTabSolicitudes = null!;
        private Button btnTabActivos = null!;
        private Button btnTabHistorial = null!;
        private Button btnRefrescar = null!;

        // Contenedor principal de vistas
        private Panel pnlContenedor = null!;

        // Vista 1: Solicitudes
        private Panel pnlVistaSolicitudes = null!;
        private DataGridView dgvSolicitudes = null!;

        // Vista 2: Activos
        private Panel pnlVistaActivos = null!;
        private DataGridView dgvActivos = null!;

        // Vista 3: Historial
        private Panel pnlVistaHistorial = null!;
        private DataGridView dgvHistorial = null!;

        private string _pestanaActiva = "Solicitudes";

        public PrestamoForm(
            IPrestamoService prestamoService,
            IDevolucionService devolucionService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _prestamoService = prestamoService;
            _devolucionService = devolucionService;
            _serviceProvider = serviceProvider;
            ConfigurarDiseñoProfesional();
        }

        private async void PrestamoForm_Load(object sender, EventArgs e)
        {
            await CargarDatosPestanaActualAsync();
        }

        private void ValidarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            if (rol == "Auditor")
            {
                btnTabSolicitudes.Visible = false;
                btnTabActivos.Visible = false;

                btnTabHistorial.Location = new Point(0, 0);
                _pestanaActiva = "Historial";
            }
            else if (rol == "Administrador")
            {
                btnTabSolicitudes.Visible = false;

                btnTabActivos.Location = new Point(0, 0);
                btnTabHistorial.Location = new Point(205, 0);
                _pestanaActiva = "Activos";
            }
        }

        private void ConfigurarDiseñoProfesional()
        {
            this.BackColor = Color.FromArgb(15, 23, 42);
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            pnlNavegacion = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(15, 23, 42)
            };

            btnTabSolicitudes = CrearBotonPestaña("📥 Solicitudes Pendientes", 0);
            btnTabActivos = CrearBotonPestaña("📚 Préstamos Activos", 205);
            btnTabHistorial = CrearBotonPestaña("📜 Historial General", 390);

            btnTabSolicitudes.Click += async (s, e) => await SeleccionarPestanaAsync("Solicitudes");
            btnTabActivos.Click += async (s, e) => await SeleccionarPestanaAsync("Activos");
            btnTabHistorial.Click += async (s, e) => await SeleccionarPestanaAsync("Historial");

            var pnlDerechoRefrescar = new Panel
            {
                Dock = DockStyle.Right,
                Width = 580,
                BackColor = Color.Transparent
            };

            btnRefrescar = new Button
            {
                Text = "🔄  Refrescar",
                Size = new Size(130, 38),
                Location = new Point(5, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White
            };
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Click += async (s, e) => await CargarDatosPestanaActualAsync();

            pnlDerechoRefrescar.Controls.Add(btnRefrescar);

            pnlNavegacion.Controls.Add(btnTabSolicitudes);
            pnlNavegacion.Controls.Add(btnTabActivos);
            pnlNavegacion.Controls.Add(btnTabHistorial);
            pnlNavegacion.Controls.Add(pnlDerechoRefrescar);

            pnlContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            ConstruirVistaSolicitudes();
            ConstruirVistaActivos();
            ConstruirVistaHistorial();

            this.Controls.Add(pnlContenedor);
            this.Controls.Add(pnlNavegacion);

            ValidarPermisosPorRol();
            EstablecerEstadoPestanaVisual(_pestanaActiva);

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(30, 41, 59)
            };
            var lblHint = new Label
            {
                Text = "💡 Tip: Haz doble clic sobre un registro para gestionarlo.",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(15, 11)
            };
            pnlBottom.Controls.Add(lblHint);
            pnlContenedor.Controls.Add(pnlBottom);
        }

        private Button CrearBotonPestaña(string texto, int posicionX)
        {
            var btn = new Button
            {
                Text = texto,
                Size = new Size(195, 38),
                Location = new Point(posicionX, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private async Task SeleccionarPestanaAsync(string nombrePestana)
        {
            if (_pestanaActiva == nombrePestana) return;

            EstablecerEstadoPestanaVisual(nombrePestana);
            await CargarDatosPestanaActualAsync();
        }

        private void EstablecerEstadoPestanaVisual(string nombrePestana)
        {
            _pestanaActiva = nombrePestana;

            EstilarBotonPestaña(btnTabSolicitudes, nombrePestana == "Solicitudes");
            EstilarBotonPestaña(btnTabActivos, nombrePestana == "Activos");
            EstilarBotonPestaña(btnTabHistorial, nombrePestana == "Historial");

            pnlVistaSolicitudes.Visible = (nombrePestana == "Solicitudes");
            pnlVistaActivos.Visible = (nombrePestana == "Activos");
            pnlVistaHistorial.Visible = (nombrePestana == "Historial");
        }

        private void EstilarBotonPestaña(Button btn, bool activo)
        {
            btn.BackColor = activo ? Color.FromArgb(37, 99, 235) : Color.FromArgb(30, 41, 59);
            btn.ForeColor = activo ? Color.White : Color.FromArgb(148, 163, 184);
        }

        #region Construcción de Vistas

        private void ConstruirVistaSolicitudes()
        {
            pnlVistaSolicitudes = new Panel { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(15) };

            dgvSolicitudes = CrearDataGridView();
            ConfigurarColumnasSolicitudes();

            dgvSolicitudes.CellDoubleClick += DgvSolicitudes_CellDoubleClick;

            pnlVistaSolicitudes.Controls.Add(dgvSolicitudes);
            pnlContenedor.Controls.Add(pnlVistaSolicitudes);
        }

        private void DgvSolicitudes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvSolicitudes.Rows[e.RowIndex].DataBoundItem is SolicitudDto solicitud)
            {
                AbrirModalSolicitud(solicitud);
            }
        }

        private void AbrirModalSolicitud(SolicitudDto? solicitud)
        {
            if (solicitud == null) return;

            var modal = _serviceProvider.GetRequiredService<DetalleSolicitud>();
            modal.CargarSolicitud(solicitud);

            if (modal.ShowDialog() == DialogResult.OK)
            {
                _ = CargarDatosPestanaActualAsync();
            }
        }

        private void ConstruirVistaActivos()
        {
            pnlVistaActivos = new Panel { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(15) };

            var pnlFiltrosActivos = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            var lblFiltroId = new Label
            {
                Text = "Identificación:",
                Location = new Point(0, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184)
            };

            var txtFiltroId = new TextBox
            {
                Name = "txtFiltroIdentificacionActivos",
                Location = new Point(95, 9),
                Size = new Size(110, 23),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtFiltroId.TextChanged += TxtFiltroIdActivos_TextChanged;

            pnlFiltrosActivos.Controls.Add(lblFiltroId);
            pnlFiltrosActivos.Controls.Add(txtFiltroId);

            dgvActivos = CrearDataGridView();
            ConfigurarColumnasPrestamos(dgvActivos);

            dgvActivos.CellDoubleClick += DgvActivos_CellDoubleClick;

            pnlVistaActivos.Controls.Add(dgvActivos);
            pnlVistaActivos.Controls.Add(pnlFiltrosActivos);
            pnlContenedor.Controls.Add(pnlVistaActivos);
        }

        private void DgvActivos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvActivos.Rows[e.RowIndex].DataBoundItem is PrestamoDto prestamo)
            {
                AbrirModalPrestamo(prestamo, esSoloLectura: false);
            }
        }

        private void ConstruirVistaHistorial()
        {
            pnlVistaHistorial = new Panel { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(15) };

            var pnlFiltrosHistorial = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(30, 41, 59)
            };

            var lblFiltroIdHistorial = new Label
            {
                Text = "Identificación:",
                Location = new Point(0, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184)
            };

            var txtFiltroIdHistorial = new TextBox
            {
                Name = "txtFiltroIdentificacionHistorial",
                Location = new Point(95, 9),
                Size = new Size(110, 23),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtFiltroIdHistorial.TextChanged += TxtFiltroIdHistorial_TextChanged;

            pnlFiltrosHistorial.Controls.Add(lblFiltroIdHistorial);
            pnlFiltrosHistorial.Controls.Add(txtFiltroIdHistorial);

            dgvHistorial = CrearDataGridView();
            ConfigurarColumnasPrestamos(dgvHistorial);

            dgvHistorial.CellDoubleClick += DgvHistorial_CellDoubleClick;

            pnlVistaHistorial.Controls.Add(dgvHistorial);
            pnlVistaHistorial.Controls.Add(pnlFiltrosHistorial);
            pnlContenedor.Controls.Add(pnlVistaHistorial);
        }

        private void DgvHistorial_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistorial.Rows[e.RowIndex].DataBoundItem is PrestamoDto prestamo)
            {
                AbrirModalPrestamo(prestamo, esSoloLectura: true);
            }
        }

        private void AbrirModalPrestamo(PrestamoDto? prestamo, bool esSoloLectura = false)
        {
            if (prestamo == null) return;

            var modal = _serviceProvider.GetRequiredService<DetallePrestamo>();
            modal.CargarPrestamo(prestamo, esSoloLectura);

            if (modal.ShowDialog() == DialogResult.OK)
            {
                _ = CargarDatosPestanaActualAsync();
            }
        }

        #endregion

        #region Formateo y Estilo de DataGridView

        private DataGridView CrearDataGridView()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                BackgroundColor = Color.FromArgb(30, 41, 59),
                GridColor = Color.FromArgb(51, 65, 85)
            };

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersHeight = 42;

            dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 38;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);

            dgv.CellFormatting += Dgv_CellFormatting;

            return dgv;
        }

        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is DataGridView dgv && dgv.Columns[e.ColumnIndex].DataPropertyName == "Estado" && e.Value != null)
            {
                string estado = e.Value.ToString()?.ToUpperInvariant() ?? string.Empty;
                switch (estado)
                {
                    case "PENDIENTE":
                        e.CellStyle.ForeColor = Color.FromArgb(251, 191, 36);
                        break;
                    case "APROBADO":
                    case "ACTIVO":
                        e.CellStyle.ForeColor = Color.FromArgb(74, 222, 128);
                        break;
                    case "RECHAZADO":
                    case "VENCIDO":
                        e.CellStyle.ForeColor = Color.FromArgb(248, 113, 113);
                        break;
                    case "DEVUELTO":
                    case "FINALIZADO":
                        e.CellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                        break;
                }
            }
        }

        private void ConfigurarColumnasSolicitudes()
        {
            dgvSolicitudes.Columns.Clear();

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TituloRecurso",
                HeaderText = "Recurso / Libro",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 160
            });

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdentificadorEjemplar",
                HeaderText = "Ejemplar ID",
                Width = 160,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaSolicitud",
                HeaderText = "Fecha Solicitud",
                Width = 160,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 130,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MotivoRechazo",
                HeaderText = "Motivo Rechazo",
                Width = 200
            });
        }

        private void ConfigurarColumnasPrestamos(DataGridView dgv)
        {
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TituloRecurso",
                HeaderText = "Recurso / Libro",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 160
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdentificadorEjemplar",
                HeaderText = "Ejemplar ID",
                Width = 160,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaInicio",
                HeaderText = "Fecha Inicio",
                Width = 140,
                DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaLimite",
                HeaderText = "Fecha Límite",
                Width = 140,
                DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 130,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });
        }

        #endregion

        private async Task CargarDatosPestanaActualAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (_pestanaActiva == "Solicitudes")
                {
                    var lista = await _prestamoService.ConsultarSolicitudesPendientesAsync();
                    dgvSolicitudes.DataSource = lista;
                    dgvSolicitudes.ClearSelection();
                }
                else if (_pestanaActiva == "Activos")
                {
                    await CargarPrestamosActivosAsync();
                }
                else if (_pestanaActiva == "Historial")
                {
                    await CargarHistorialGeneralAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public async void IrAPrestamosActivos()
        {
            await SeleccionarPestanaAsync("Activos");
        }

        private async void TxtFiltroIdActivos_TextChanged(object? sender, EventArgs e)
        {
            if (_pestanaActiva == "Activos")
            {
                await CargarPrestamosActivosAsync();
            }
        }

        private async void TxtFiltroIdHistorial_TextChanged(object? sender, EventArgs e)
        {
            if (_pestanaActiva == "Historial")
            {
                await CargarHistorialGeneralAsync();
            }
        }

        private async Task CargarPrestamosActivosAsync()
        {
            try
            {
                string? identificacionFiltro = null;

                var txtFiltro = pnlVistaActivos.Controls.Find("txtFiltroIdentificacionActivos", true).FirstOrDefault() as TextBox;
                if (txtFiltro != null && !string.IsNullOrWhiteSpace(txtFiltro.Text))
                {
                    identificacionFiltro = txtFiltro.Text.Trim();
                }

                var request = new ConsultarPrestamosActivosRequest
                {
                    Identificacion = identificacionFiltro
                };

                var lista = await _prestamoService.ConsultarActivosAsync(request);
                dgvActivos.DataSource = lista;
                dgvActivos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar préstamos activos: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarHistorialGeneralAsync()
        {
            try
            {
                string? identificacionFiltro = null;

                var txtFiltro = pnlVistaHistorial.Controls.Find("txtFiltroIdentificacionHistorial", true).FirstOrDefault() as TextBox;
                if (txtFiltro != null && !string.IsNullOrWhiteSpace(txtFiltro.Text))
                {
                    identificacionFiltro = txtFiltro.Text.Trim();
                }

                var request = new ConsultarHistorialPrestamosRequest
                {
                    Identificacion = identificacionFiltro
                };

                var lista = await _prestamoService.ConsultarHistorialAsync(request);
                dgvHistorial.DataSource = lista;
                dgvHistorial.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar historial general: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}