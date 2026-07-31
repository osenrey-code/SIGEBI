using SIGEBI.AppEscritorio.Dtos.Prestamos;
using SIGEBI.AppEscritorio.Services.Prestamo;
using SIGEBI.AppEscritorio.Session; // 👈 Manejo de sesión para validación de roles
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Prestamo
{
    public partial class PrestamoForm : Form
    {
        private readonly IPrestamoService _prestamoService;

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
        private Button btnAprobar = null!;
        private SolicitudDto? _solicitudSeleccionada;

        // Vista 2: Activos
        private Panel pnlVistaActivos = null!;
        private DataGridView dgvActivos = null!;

        // Vista 3: Historial
        private Panel pnlVistaHistorial = null!;
        private DataGridView dgvHistorial = null!;

        private string _pestanaActiva = "Solicitudes";

        public PrestamoForm(IPrestamoService prestamoService)
        {
            InitializeComponent();
            _prestamoService = prestamoService;
            ConfigurarDiseñoProfesional();
        }

        private async void PrestamoForm_Load(object sender, EventArgs e)
        {
            await CargarDatosPestanaActualAsync();
        }

        private void ValidarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            // 🔒 1. Caso Auditor: Solo consulta el Historial General
            if (rol == "Auditor")
            {
                btnTabSolicitudes.Visible = false;
                btnTabActivos.Visible = false;

                btnTabHistorial.Location = new Point(0, 0);
                _pestanaActiva = "Historial";
            }
            // 🔒 2. Caso Administrador: Consulta Préstamos Activos e Historial (no aprueba solicitudes)
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
            this.BackColor = Color.FromArgb(15, 23, 42); // Fondo general Dark Slate
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            // 1. Barra de Pestañas (Reemplazo del TabControl)
            pnlNavegacion = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(15, 23, 42)
            };

            btnTabSolicitudes = CrearBotonPestaña("📥 Solicitudes Pendientes", 0);
            btnTabActivos = CrearBotonPestaña("📚 Préstamos Activos", 205);
            btnTabHistorial = CrearBotonPestaña("📜 Historial General", 390);

            btnTabSolicitudes.Click += (s, e) => SeleccionarPestana("Solicitudes");
            btnTabActivos.Click += (s, e) => SeleccionarPestana("Activos");
            btnTabHistorial.Click += (s, e) => SeleccionarPestana("Historial");

            // 🔄 Botón de Refrescar alineado a la derecha
            btnRefrescar = new Button
            {
                Text = "🔄  Refrescar",
                Size = new Size(130, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlNavegacion.Width - 130, 3),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White
            };
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Click += async (s, e) => await CargarDatosPestanaActualAsync();

            pnlNavegacion.Controls.Add(btnTabSolicitudes);
            pnlNavegacion.Controls.Add(btnTabActivos);
            pnlNavegacion.Controls.Add(btnTabHistorial);
            pnlNavegacion.Controls.Add(btnRefrescar);

            // 2. Contenedor Dinámico para las Tablas
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

            // 3. Aplicar filtro de permisos por rol antes de cargar la pestaña
            ValidarPermisosPorRol();

            // Cargar la pestaña activa según el rol
            SeleccionarPestana(_pestanaActiva);
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

        private void SeleccionarPestana(string nombrePestana)
        {
            _pestanaActiva = nombrePestana;

            // Actualizar colores visuales de los botones
            EstilarBotonPestaña(btnTabSolicitudes, nombrePestana == "Solicitudes");
            EstilarBotonPestaña(btnTabActivos, nombrePestana == "Activos");
            EstilarBotonPestaña(btnTabHistorial, nombrePestana == "Historial");

            // Alternar visibilidad de paneles sin parpadeo
            pnlVistaSolicitudes.Visible = (nombrePestana == "Solicitudes");
            pnlVistaActivos.Visible = (nombrePestana == "Activos");
            pnlVistaHistorial.Visible = (nombrePestana == "Historial");

            _ = CargarDatosPestanaActualAsync();
        }

        private void EstilarBotonPestaña(Button btn, bool activo)
        {
            btn.BackColor = activo ? Color.FromArgb(37, 99, 235) : Color.FromArgb(30, 41, 59);
            btn.ForeColor = activo ? Color.White : Color.FromArgb(148, 163, 184);
        }

        #region Construcción de Vistas Profesionales

        private void ConstruirVistaSolicitudes()
        {
            pnlVistaSolicitudes = new Panel { Dock = DockStyle.Fill, Visible = false };

            // Panel superior de acciones
            var pnlAcciones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(15, 10, 15, 10)
            };

            btnAprobar = new Button
            {
                Text = "✅  Aprobar Solicitud",
                BackColor = Color.FromArgb(22, 163, 74), // Verde esmeralda
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Size = new Size(180, 38),
                Location = new Point(15, 10),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnAprobar.FlatAppearance.BorderSize = 0;
            btnAprobar.Click += BtnAprobar_Click;
            pnlAcciones.Controls.Add(btnAprobar);

            dgvSolicitudes = CrearDataGridView();
            ConfigurarColumnasSolicitudes();

            dgvSolicitudes.SelectionChanged += (s, e) =>
            {
                _solicitudSeleccionada = dgvSolicitudes.SelectedRows.Count > 0
                    ? dgvSolicitudes.SelectedRows[0].DataBoundItem as SolicitudDto
                    : null;
                btnAprobar.Enabled = _solicitudSeleccionada != null;
            };

            var pnlWrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
            pnlWrapper.Controls.Add(dgvSolicitudes);

            pnlVistaSolicitudes.Controls.Add(pnlWrapper);
            pnlVistaSolicitudes.Controls.Add(pnlAcciones);
            pnlContenedor.Controls.Add(pnlVistaSolicitudes);
        }

        private void ConstruirVistaActivos()
        {
            pnlVistaActivos = new Panel { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(15) };
            dgvActivos = CrearDataGridView();
            ConfigurarColumnasPrestamos(dgvActivos);
            pnlVistaActivos.Controls.Add(dgvActivos);
            pnlContenedor.Controls.Add(pnlVistaActivos);
        }

        private void ConstruirVistaHistorial()
        {
            pnlVistaHistorial = new Panel { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(15) };
            dgvHistorial = CrearDataGridView();
            ConfigurarColumnasPrestamos(dgvHistorial);
            pnlVistaHistorial.Controls.Add(dgvHistorial);
            pnlContenedor.Controls.Add(pnlVistaHistorial);
        }

        #endregion

        #region Formateo del DataGridView

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

            // Estilo del encabezado de la tabla
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersHeight = 42;

            // Estilo de las celdas y filas
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 38;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);

            return dgv;
        }

        private void ConfigurarColumnasSolicitudes()
        {
            dgvSolicitudes.Columns.Clear();

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SolicitudId",
                HeaderText = "ID",
                Width = 70,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

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
                Width = 140,
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
                Width = 120,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvSolicitudes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MotivoRechazo",
                HeaderText = "Motivo Rechazo",
                Width = 180
            });
        }

        private void ConfigurarColumnasPrestamos(DataGridView dgv)
        {
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrestamoId",
                HeaderText = "ID",
                Width = 70,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

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
                Width = 130,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaInicio",
                HeaderText = "Fecha Inicio",
                Width = 130,
                DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaLimite",
                HeaderText = "Fecha Límite",
                Width = 130,
                DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 110,
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
                }
                else if (_pestanaActiva == "Activos")
                {
                    var lista = await _prestamoService.ConsultarActivosAsync(new ConsultarPrestamosActivosRequest());
                    dgvActivos.DataSource = lista;
                }
                else if (_pestanaActiva == "Historial")
                {
                    var lista = await _prestamoService.ConsultarHistorialAsync(new ConsultarHistorialPrestamosRequest());
                    dgvHistorial.DataSource = lista;
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

        private async void BtnAprobar_Click(object? sender, EventArgs e)
        {
            if (_solicitudSeleccionada == null) return;

            var confirm = MessageBox.Show(
                $"¿Está seguro de aprobar la solicitud para el libro '{_solicitudSeleccionada.TituloRecurso}'?\nEsto generará el préstamo oficialmente.",
                "Confirmar Aprobación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    var request = new AprobarSolicitudRequest { SolicitudId = _solicitudSeleccionada.SolicitudId };
                    await _prestamoService.AprobarSolicitudAsync(request);

                    MessageBox.Show("Solicitud aprobada y préstamo generado con éxito.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await CargarDatosPestanaActualAsync();
                    btnAprobar.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al aprobar la solicitud: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
    }
}