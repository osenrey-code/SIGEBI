using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Response;
using SIGEBI.AppEscritorio.Services.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            // Aplicamos el diseño moderno inmediatamente al cargar
            AplicarDisenoModerno();
        }

        #region Estilos y Diseño UI

        private void AplicarDisenoModerno()
        {
            // Colores base extraídos de tu menú lateral
            Color fondoPrincipal = Color.FromArgb(15, 23, 42);       // Fondo oscuro
            Color fondoPaneles = Color.FromArgb(30, 41, 59);         // Oscuro un poco más claro
            Color textoSecundario = Color.FromArgb(148, 163, 184);   // Gris claro
            Color colorPrimario = Color.FromArgb(37, 99, 235);       // Azul vibrante
            Color colorPeligro = Color.FromArgb(239, 68, 68);        // Rojo
            Color colorAdvertencia = Color.FromArgb(245, 158, 11);   // Naranja/Amarillo

            // 1. Fondos Generales
            this.BackColor = fondoPrincipal;
            pnlBotones.BackColor = fondoPrincipal;
            pnlBotones.Padding = new Padding(10, 10, 10, 10); // Un poco de espacio

            // 2. Estilos de Botones
            ConfigurarBoton(btnNuevo, colorPrimario, Color.White);
            ConfigurarBoton(btnEditar, colorAdvertencia, Color.White);
            ConfigurarBoton(btnEliminar, colorPeligro, Color.White);
            ConfigurarBoton(btnRecargar, fondoPaneles, Color.White);

            // 3. Modernización del DataGridView
            dgvCatalogo.BackgroundColor = fondoPrincipal;
            dgvCatalogo.BorderStyle = BorderStyle.None;
            dgvCatalogo.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Solo líneas horizontales
            dgvCatalogo.GridColor = Color.FromArgb(51, 65, 85); // Color de las líneas
            dgvCatalogo.EnableHeadersVisualStyles = false; // Importante para poder pintar cabeceras

            // Ocultar columna izquierda vacía por defecto
            dgvCatalogo.RowHeadersVisible = false;

            // Configurar Cabeceras de columnas
            dgvCatalogo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCatalogo.ColumnHeadersDefaultCellStyle.BackColor = fondoPaneles;
            dgvCatalogo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCatalogo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCatalogo.ColumnHeadersDefaultCellStyle.SelectionBackColor = fondoPaneles; // Evitar que cambie al hacer clic
            dgvCatalogo.ColumnHeadersHeight = 45;

            // Configurar Celdas
            dgvCatalogo.DefaultCellStyle.BackColor = fondoPrincipal;
            dgvCatalogo.DefaultCellStyle.ForeColor = textoSecundario;
            dgvCatalogo.DefaultCellStyle.SelectionBackColor = colorPrimario;
            dgvCatalogo.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCatalogo.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvCatalogo.RowTemplate.Height = 40; // Filas más altas para mejor lectura

            // Margen para que la tabla no choque con los bordes
            dgvCatalogo.Margin = new Padding(10);
        }

        private void ConfigurarBoton(Button btn, Color bg, Color fg)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = fg;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Height = 38;
        }

        #endregion

        private async void CatalogoForm_Load(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var recursos = await _catalogoService.ConsultarTodosAsync();
                dgvCatalogo.DataSource = recursos?.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Por favor, seleccione un recurso de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow?.DataBoundItem is RecursoResponse recursoSeleccionado)
            {
                var confirmacion = MessageBox.Show($"¿Desea eliminar '{recursoSeleccionado.Titulo}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    try
                    {
                        await _catalogoService.EliminarRecursoAsync(recursoSeleccionado.RecursoBibliograficoId, "Eliminado desde sistema de escritorio");
                        MessageBox.Show("Recurso eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarDatosAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnRecargar_Click(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }
    }
}