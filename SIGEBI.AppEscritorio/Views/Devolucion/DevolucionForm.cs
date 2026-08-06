using SIGEBI.AppEscritorio.Dtos.Devoluciones;
using SIGEBI.AppEscritorio.Services.Devolucion;
using SIGEBI.AppEscritorio.Session;
using SIGEBI.AppEscritorio.Views.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;


namespace SIGEBI.AppEscritorio.Views.Devolucion
{
    public partial class DevolucionForm : Form
    {
        private readonly IDevolucionService _devolucionService;
        private readonly IServiceProvider _serviceProvider;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public DevolucionForm(IDevolucionService devolucionService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _devolucionService = devolucionService;
            _serviceProvider = serviceProvider;

            AplicarEstilosDarkSlate();
        }

        private async void DevolucionForm_Load(object sender, EventArgs e)
        {
            dtpInicio.Value = DateTime.Now.Date.AddDays(-30);
            dtpFin.Value = DateTime.Now.Date;

            await CargarHistorialAsync();
        }

        #region Configuración Estética y Estilos

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);      // #0F172A
            Color fondoPanel = Color.FromArgb(30, 41, 59);     // #1E293B
            Color textoGris = Color.FromArgb(148, 163, 184);   // #94A3B8
            Color azulPrimario = Color.FromArgb(37, 99, 235);  // #2563EB
            Color colorBotonGris = Color.FromArgb(51, 65, 85); // #334155

            this.BackColor = fondoDark;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            // Ajustar el contenedor de historial para que ocupe todo el formulario
            pnlHistorialContainer.BackColor = fondoPanel;
            pnlHistorialContainer.Visible = true;
            pnlHistorialContainer.Dock = DockStyle.Fill;
            pnlHistorialContainer.BringToFront();

            pnlFiltrosHistorial.BackColor = fondoPanel;

            // DateTimePickers oscuros
            ConfigurarDateTimePickerOscuro(dtpInicio, fondoDark, Color.White);
            ConfigurarDateTimePickerOscuro(dtpFin, fondoDark, Color.White);

            // Botón Buscar / Consultar
            btnBuscarHistorial.BackColor = azulPrimario;
            btnBuscarHistorial.ForeColor = Color.White;
            btnBuscarHistorial.FlatStyle = FlatStyle.Flat;
            btnBuscarHistorial.FlatAppearance.BorderSize = 0;
            btnBuscarHistorial.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnBuscarHistorial.Cursor = Cursors.Hand;

            // Botón Refrescar
            btnRefrescar.BackColor = colorBotonGris;
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnRefrescar.Cursor = Cursors.Hand;

            // Configuración del DataGridView
            dgvHistorial.AutoGenerateColumns = false;
            dgvHistorial.BackgroundColor = fondoPanel;
            dgvHistorial.BorderStyle = BorderStyle.None;
            dgvHistorial.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistorial.GridColor = Color.FromArgb(51, 65, 85);
            dgvHistorial.EnableHeadersVisualStyles = false;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.ReadOnly = true;

            dgvHistorial.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = fondoDark;
            dgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = textoGris;
            dgvHistorial.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoDark;
            dgvHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvHistorial.ColumnHeadersHeight = 40;

            dgvHistorial.DefaultCellStyle.BackColor = fondoPanel;
            dgvHistorial.DefaultCellStyle.ForeColor = Color.White;
            dgvHistorial.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvHistorial.DefaultCellStyle.SelectionBackColor = azulPrimario;
            dgvHistorial.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvHistorial.RowTemplate.Height = 38;
            dgvHistorial.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);

            ConfigurarGridHistorial();
            dgvHistorial.CellDoubleClick += DgvHistorial_CellDoubleClick;

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = fondoPanel
            };
            var lblHint = new Label
            {
                Text = "💡 Tip: Haz doble clic sobre una devolución para ver sus detalles completos.",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(15, 11)
            };
            pnlBottom.Controls.Add(lblHint);
            pnlHistorialContainer.Controls.Add(pnlBottom);
        }

        private void ConfigurarDateTimePickerOscuro(DateTimePicker dtp, Color fondoDark, Color textoClaro)
        {
            if (dtp.IsHandleCreated) SetWindowTheme(dtp.Handle, "", "");
            else dtp.HandleCreated += (s, e) => SetWindowTheme(dtp.Handle, "", "");

            dtp.BackColor = fondoDark;
            dtp.ForeColor = textoClaro;
            dtp.CalendarMonthBackground = fondoDark;
            dtp.CalendarTitleBackColor = Color.FromArgb(30, 41, 59);
            dtp.CalendarTitleForeColor = textoClaro;
            dtp.CalendarForeColor = textoClaro;
        }

        #endregion

        #region Consulta de Historial

        private async void btnBuscarHistorial_Click(object sender, EventArgs e)
        {
            await CargarHistorialAsync();
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            await CargarHistorialAsync();
        }

        private async Task CargarHistorialAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var request = new ConsultarHistorialDevolucionesRequestDto
                {
                    FechaInicio = dtpInicio.Value.Date,
                    FechaFin = dtpFin.Value.Date.AddDays(1)
                };

                var lista = await _devolucionService.ConsultarHistorialAsync(request);
                dgvHistorial.DataSource = lista;
                dgvHistorial.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar historial: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ConfigurarGridHistorial()
        {
            dgvHistorial.Columns.Clear();
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TituloRecurso", HeaderText = "Libro", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 160 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaDevolucion", HeaderText = "Fecha Devolución", Width = 150, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Condicion", HeaderText = "Condición", Width = 130, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiasRetraso", HeaderText = "Días Retraso", Width = 110, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MontoPenalizacion", HeaderText = "Mora (RD$)", Width = 120, DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Mensaje", HeaderText = "Detalle", Width = 250 });

            dgvHistorial.CellFormatting += DgvHistorial_CellFormatting;
        }

        private void DgvHistorial_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistorial.Rows[e.RowIndex].DataBoundItem is DevolucionResponseDto dev)
            {
                if (dev.PenalizacionGenerada)
                {
                    if (dgvHistorial.Columns[e.ColumnIndex].DataPropertyName == "MontoPenalizacion")
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                        e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                    }
                }
            }
        }

        private void DgvHistorial_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistorial.Rows[e.RowIndex].DataBoundItem is DevolucionResponseDto devolucionSeleccionada)
            {
                AbrirModalDetalleDevolucion(devolucionSeleccionada);
            }
        }

        private void AbrirModalDetalleDevolucion(DevolucionResponseDto devolucion)
        {
            try
            {
                var modal = _serviceProvider.GetRequiredService<DetallePrestamo>();
                modal.CargarDevolucion(devolucion);

                modal.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir los detalles: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}