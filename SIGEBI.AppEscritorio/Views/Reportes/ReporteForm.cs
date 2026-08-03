using SIGEBI.AppEscritorio.Dtos.Reporte;
using SIGEBI.AppEscritorio.Services.Reporte;
using SIGEBI.AppEscritorio.Session;
using System.Runtime.InteropServices; 


namespace SIGEBI.AppEscritorio.Views.Reportes
{
    public partial class ReporteForm : Form
    {
        private readonly IReporteService _reporteService;
        private string _pestanaActiva = "Inventario";

        // 🛠️ Función Win32 para desactivar el tema nativo en los DateTimePickers y permitir fondo oscuro
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public ReporteForm(IReporteService reporteService)
        {
            InitializeComponent();
            _reporteService = reporteService;

            AplicarEstilosDarkSlate();
        }

        private async void ReporteForm_Load(object sender, EventArgs e)
        {
            ValidarPermisosPorRol();

            // Rango de fechas por defecto (último mes)
            dtpInicio.Value = DateTime.Now.AddDays(-30);
            dtpFin.Value = DateTime.Now;

            SeleccionarPestana("Inventario");
        }

        private void ValidarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;

            if (rol == "Bibliotecario" || rol == "PersonalBibliotecario")
            {
                btnTabPrestamos.Visible = false;
                btnTabPenalizaciones.Visible = false;
                btnTabCatalogo.Visible = false;
            }
        }

        #region Estilizado de Interfaz Dark Slate

        private void AplicarEstilosDarkSlate()
        {
            Color fondoDark = Color.FromArgb(15, 23, 42);      // #0F172A
            Color fondoPanel = Color.FromArgb(30, 41, 59);     // #1E293B
            Color textoGris = Color.FromArgb(148, 163, 184);  // #94A3B8
            Color azulPrimario = Color.FromArgb(37, 99, 235);  // #2563EB
            Color verdeEsmeralda = Color.FromArgb(22, 163, 74);// #16A34A

            this.BackColor = fondoDark;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(15);

            // Contenedores
            pnlNavegacion.BackColor = fondoDark;
            pnlFiltros.BackColor = fondoPanel;
            pnlKpis.BackColor = fondoDark;
            pnlContenedorGrid.BackColor = fondoPanel;

            // 📅 1. Configuración de DateTimePickers en tono oscuro con texto blanco
            ConfigurarDateTimePickerOscuro(dtpInicio, fondoDark, Color.White);
            ConfigurarDateTimePickerOscuro(dtpFin, fondoDark, Color.White);

            // Botones
            btnBuscar.BackColor = azulPrimario;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnBuscar.Cursor = Cursors.Hand;

            btnExportarPdf.BackColor = verdeEsmeralda;
            btnExportarPdf.ForeColor = Color.White;
            btnExportarPdf.FlatStyle = FlatStyle.Flat;
            btnExportarPdf.FlatAppearance.BorderSize = 0;
            btnExportarPdf.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnExportarPdf.Cursor = Cursors.Hand;

            // 📊 2. Formateo de DataGridView (Sin resaltado azul en Encabezados)
            dgvDatos.AutoGenerateColumns = false;
            dgvDatos.BackgroundColor = fondoPanel;
            dgvDatos.BorderStyle = BorderStyle.None;
            dgvDatos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDatos.GridColor = Color.FromArgb(51, 65, 85);
            dgvDatos.EnableHeadersVisualStyles = false;
            dgvDatos.RowHeadersVisible = false;
            dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDatos.MultiSelect = false;
            dgvDatos.ReadOnly = true;

            // Encabezados Grid
            dgvDatos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvDatos.ColumnHeadersDefaultCellStyle.BackColor = fondoDark;
            dgvDatos.ColumnHeadersDefaultCellStyle.ForeColor = textoGris;
            dgvDatos.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoDark; // 👈 Evita selección azul en headers
            dgvDatos.ColumnHeadersDefaultCellStyle.SelectionForeColor = textoGris;
            dgvDatos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvDatos.ColumnHeadersHeight = 40;

            // Celdas Grid
            dgvDatos.DefaultCellStyle.BackColor = fondoPanel;
            dgvDatos.DefaultCellStyle.ForeColor = Color.White;
            dgvDatos.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvDatos.DefaultCellStyle.SelectionBackColor = azulPrimario;
            dgvDatos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvDatos.RowTemplate.Height = 38;
            dgvDatos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);

            // Estilar pestañas
            EstilarBotonTab(btnTabInventario);
            EstilarBotonTab(btnTabPrestamos);
            EstilarBotonTab(btnTabPenalizaciones);
            EstilarBotonTab(btnTabCatalogo);
        }

        private void ConfigurarDateTimePickerOscuro(DateTimePicker dtp, Color fondoDark, Color textoClaro)
        {
            // Remover el tema nativo para permitir color de fondo personalizado
            if (dtp.IsHandleCreated)
            {
                SetWindowTheme(dtp.Handle, "", "");
            }
            else
            {
                dtp.HandleCreated += (s, e) => SetWindowTheme(dtp.Handle, "", "");
            }

            dtp.BackColor = fondoDark;
            dtp.ForeColor = textoClaro;

            // Ajuste del menú desplegable del calendario
            dtp.CalendarMonthBackground = fondoDark;
            dtp.CalendarTitleBackColor = Color.FromArgb(30, 41, 59);
            dtp.CalendarTitleForeColor = textoClaro;
            dtp.CalendarForeColor = textoClaro;
            dtp.CalendarTrailingForeColor = Color.FromArgb(148, 163, 184);
        }

        private void EstilarBotonTab(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        #endregion

        #region Control de Pestañas y Eventos

        private void SeleccionarPestana(string pestana)
        {
            _pestanaActiva = pestana;

            ActualizarEstadoTab(btnTabInventario, pestana == "Inventario");
            ActualizarEstadoTab(btnTabPrestamos, pestana == "Prestamos");
            ActualizarEstadoTab(btnTabPenalizaciones, pestana == "Penalizaciones");
            ActualizarEstadoTab(btnTabCatalogo, pestana == "Catalogo");

            bool requiereFechas = pestana != "Inventario";
            dtpInicio.Enabled = requiereFechas;
            dtpFin.Enabled = requiereFechas;
            btnBuscar.Enabled = requiereFechas;

            _ = CargarReporteActualAsync();
        }

        private void ActualizarEstadoTab(Button btn, bool activo)
        {
            btn.BackColor = activo ? Color.FromArgb(37, 99, 235) : Color.FromArgb(30, 41, 59);
            btn.ForeColor = activo ? Color.White : Color.FromArgb(148, 163, 184);
        }

        private void btnTabInventario_Click(object sender, EventArgs e) => SeleccionarPestana("Inventario");
        private void btnTabPrestamos_Click(object sender, EventArgs e) => SeleccionarPestana("Prestamos");
        private void btnTabPenalizaciones_Click(object sender, EventArgs e) => SeleccionarPestana("Penalizaciones");
        private void btnTabCatalogo_Click(object sender, EventArgs e) => SeleccionarPestana("Catalogo");

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CargarReporteActualAsync();
        }

        #endregion

        #region Carga de Datos y Configuración Dinámica de Grilla

        private async Task CargarReporteActualAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var request = new ReporteRangoFRequestDto
                {
                    FechaInicio = dtpInicio.Value.Date,
                    FechaFin = dtpFin.Value.Date
                };

                switch (_pestanaActiva)
                {
                    case "Inventario":
                        var inv = await _reporteService.ObtenerReporteInventarioAsync();
                        if (inv != null)
                        {
                            ActualizarTarjetasKpi(
                                "Total Libros", inv.TotalRecursos.ToString("N0"),
                                "Total Copias", inv.TotalEjemplares.ToString("N0"),
                                "Disponibles", inv.EjemplaresDisponibles.ToString("N0"),
                                "Prestados", inv.EjemplaresPrestados.ToString("N0")
                            );
                            ConfigurarGridInventario();
                            dgvDatos.DataSource = inv.Recursos;
                        }
                        break;

                    case "Prestamos":
                        var pres = await _reporteService.ObtenerReportePrestamosAsync(request);
                        if (pres != null)
                        {
                            ActualizarTarjetasKpi(
                                "Total Préstamos", pres.TotalPrestamos.ToString("N0"),
                                "Devueltos a Tiempo", pres.PrestamosDevueltosATiempo.ToString("N0"),
                                "Préstamos Vencidos", pres.PrestamosVencidos.ToString("N0"),
                                "% Devolución Puntual", $"{pres.TasaDevolucionPuntual:F1}%"
                            );
                            ConfigurarGridPrestamos();
                            dgvDatos.DataSource = pres.Prestamos;
                        }
                        break;

                    case "Penalizaciones":
                        var pen = await _reporteService.ObtenerReportePenalizacionesAsync(request);
                        if (pen != null)
                        {
                            ActualizarTarjetasKpi(
                                "Total Sanciones", pen.TotalPenalizaciones.ToString("N0"),
                                "Sanciones Activas", pen.PenalizacionesActivas.ToString("N0"),
                                "Mora Total", $"RD$ {pen.MontoTotalMora:N2}",
                                "Mora Activa", $"RD$ {pen.MontoMoraActiva:N2}"
                            );
                            ConfigurarGridPenalizaciones();
                            dgvDatos.DataSource = pen.Detalles;
                        }
                        break;

                    case "Catalogo":
                        var cat = await _reporteService.ObtenerReporteUsoCatalogoAsync(request);
                        if (cat != null)
                        {
                            ActualizarTarjetasKpi(
                                "Total Solicitudes", cat.TotalSolicitudes.ToString("N0"),
                                "Disponibilidad Prom.", $"{cat.DisponibilidadPromedio:F1}%",
                                "Top Solicitados", cat.RecursosMasSolicitados.Count.ToString(),
                                "Categorías con Demanda", cat.DemandaPorCategoria.Count.ToString()
                            );
                            ConfigurarGridUsoCatalogo();
                            dgvDatos.DataSource = cat.RecursosMasSolicitados;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar el reporte: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ActualizarTarjetasKpi(string t1, string v1, string t2, string v2, string t3, string v3, string t4, string v4)
        {
            lblKpi1Title.Text = t1; lblKpi1Val.Text = v1;
            lblKpi2Title.Text = t2; lblKpi2Val.Text = v2;
            lblKpi3Title.Text = t3; lblKpi3Val.Text = v3;
            lblKpi4Title.Text = t4; lblKpi4Val.Text = v4;
        }

        #region Mapeo de Columnas para DataGridView

        private void ConfigurarGridInventario()
        {
            dgvDatos.Columns.Clear();
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RecursoBibliograficoId", HeaderText = "ID", Width = 65, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ISBN", HeaderText = "ISBN", Width = 130, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", HeaderText = "Título", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Categoria", HeaderText = "Categoría", Width = 140 });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalEjemplares", HeaderText = "Total Copias", Width = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Disponibles", HeaderText = "Disponibles", Width = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Prestados", HeaderText = "Prestados", Width = 95, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Reservados", HeaderText = "Reservados", Width = 95, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FueraDeServicio", HeaderText = "Fuera Serv.", Width = 95, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
        }

        private void ConfigurarGridPrestamos()
        {
            dgvDatos.Columns.Clear();
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrestamoId", HeaderText = "ID", Width = 65, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TituloRecurso", HeaderText = "Libro", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdentificadorEjemplar", HeaderText = "Ejemplar ID", Width = 130, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaPrestamo", HeaderText = "Fecha Préstamo", Width = 130, DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaLimite", HeaderText = "Fecha Límite", Width = 130, DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaDevolucion", HeaderText = "Fecha Devolución", Width = 130, DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", HeaderText = "Estado", Width = 110, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) } });
        }

        private void ConfigurarGridPenalizaciones()
        {
            dgvDatos.Columns.Clear();
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PenalizacionId", HeaderText = "ID", Width = 65, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UsuarioId", HeaderText = "Usuario ID", Width = 90, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TipoUsuario", HeaderText = "Rol Usuario", Width = 120, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Motivo", HeaderText = "Motivo Penalización", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiasRetraso", HeaderText = "Días Retraso", Width = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MontoMora", HeaderText = "Mora (RD$)", Width = 110, DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", HeaderText = "Estado", Width = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) } });
        }

        private void ConfigurarGridUsoCatalogo()
        {
            dgvDatos.Columns.Clear();
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RecursoBibliograficoId", HeaderText = "Recurso ID", Width = 95, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", HeaderText = "Título del Recurso", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CantidadSolicitudes", HeaderText = "Total Solicitudes Recibidas", Width = 180, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) } });
        }

        #endregion

        #endregion

        #region Exportación a PDF

        private async void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                using var sfd = new SaveFileDialog
                {
                    Filter = "Archivo PDF (*.pdf)|*.pdf",
                    FileName = $"Reporte_SIGEBI_{_pestanaActiva}_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                    Title = "Guardar Reporte en Formato PDF"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    var request = new ReporteRangoFRequestDto
                    {
                        FechaInicio = dtpInicio.Value.Date,
                        FechaFin = dtpFin.Value.Date
                    };

                    byte[]? pdfBytes = null;

                    switch (_pestanaActiva)
                    {
                        case "Inventario":
                            pdfBytes = await _reporteService.DescargarInventarioPdfAsync();
                            break;
                        case "Prestamos":
                            pdfBytes = await _reporteService.DescargarPrestamosPdfAsync(request);
                            break;
                        case "Penalizaciones":
                            pdfBytes = await _reporteService.DescargarPenalizacionesPdfAsync(request);
                            break;
                        case "Catalogo":
                            pdfBytes = await _reporteService.DescargarUsoCatalogoPdfAsync(request);
                            break;
                    }

                    if (pdfBytes != null && pdfBytes.Length > 0)
                    {
                        await File.WriteAllBytesAsync(sfd.FileName, pdfBytes);
                        MessageBox.Show("El reporte en PDF ha sido generado y guardado correctamente.", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se obtuvieron datos de archivo PDF desde el servidor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar el archivo PDF: {ex.Message}", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}