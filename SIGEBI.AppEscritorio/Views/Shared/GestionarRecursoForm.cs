using Microsoft.Extensions.Configuration;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Request;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Response;
using SIGEBI.AppEscritorio.Services.Categoria;
using SIGEBI.AppEscritorio.Services.Interfaces;
using SIGEBI.AppEscritorio.Session;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class GestionarRecursoForm : Form
    {
        private readonly ICatalogoService _catalogoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IConfiguration _configuration;

        private int _recursoIdActual = 0;
        private int _categoriaIdGuardada = 0;
        private string _rutaImagenSeleccionada = string.Empty;
        private string _urlImagenActual = string.Empty;

        public GestionarRecursoForm(
            ICatalogoService catalogoService,
            ICategoriaService categoriaService,
            IConfiguration configuration)
        {
            InitializeComponent();
            _catalogoService = catalogoService;
            _categoriaService = categoriaService;
            _configuration = configuration;

            HabilitarArrastre(pnlTopBar);
            AplicarBordesRedondeados();

            this.Load += async (s, e) =>
            {
                await CargarCategoriasAsync();

                if (_recursoIdActual == 0)
                {
                    ActivarModoEdicion(esNuevo: true);
                }
                else
                {
                    if (_categoriaIdGuardada > 0) cmbCategoria.SelectedValue = _categoriaIdGuardada;
                }
            };

            picPortada.Paint += PicPortada_Paint;
            cmbCategoria.DrawItem += CmbCategoria_DrawItem;
        }

        #region Arrastre y Bordes Redondeados
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void AplicarBordesRedondeados()
        {
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 14, 18));
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void HabilitarArrastre(Control control)
        {
            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 0x2, 0);
                }
            };
        }
        #endregion

        #region Renderizado y Estilos ComboBox / PictureBox

        private void CmbCategoria_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color backColor = isSelected ? Color.FromArgb(59, 130, 246) : Color.FromArgb(30, 41, 59);
            Color textColor = Color.White;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            var item = cmbCategoria.Items[e.Index];
            string text = item != null ? cmbCategoria.GetItemText(item) ?? string.Empty : string.Empty;
            Font fontToUse = e.Font ?? cmbCategoria.Font;

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(text, fontToUse, textBrush, e.Bounds.X + 4, e.Bounds.Y + 2);
            }

            e.DrawFocusRectangle();
        }

        private void PicPortada_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen penBorder = new Pen(Color.FromArgb(71, 85, 105), 1))
            {
                e.Graphics.DrawRectangle(penBorder, 0, 0, picPortada.Width - 1, picPortada.Height - 1);
            }

            if (picPortada.Image != null) return;

            using (Pen penDashed = new Pen(Color.FromArgb(148, 163, 184), 2) { DashStyle = DashStyle.Dash })
            {
                e.Graphics.DrawRectangle(penDashed, 4, 4, picPortada.Width - 9, picPortada.Height - 9);
            }

            string texto = "Seleccione una\nportada para\nel recurso";
            using (Font font = new Font("Segoe UI", 10F, FontStyle.Italic))
            using (Brush brush = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(texto, font, brush, picPortada.ClientRectangle, format);
            }
        }

        #endregion

        #region Lógica de Negocio y Estados de Vista

        private void ActivarModoVista()
        {
            lblHeaderTitle.Text = "📖 Detalle del Recurso";

            txtISBN.Enabled = false;
            txtTitulo.Enabled = false;
            txtAutor.Enabled = false;
            cmbCategoria.Enabled = false;
            numAnio.Enabled = false;
            numEjemplares.Enabled = false;
            txtDescripcion.Enabled = false;

            btnSeleccionarFoto.Visible = false;
            btnGuardar.Visible = false;

            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            bool esAdministrador = rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
            bool esAuditor = rol.Equals("Auditor", StringComparison.OrdinalIgnoreCase);

            btnEditar.Visible = !esAuditor;
            btnDesactivar.Visible = esAdministrador;

            btnCancelar.Text = "Cerrar";
        }

        private void ActivarModoEdicion(bool esNuevo)
        {
            lblHeaderTitle.Text = esNuevo ? "➕ Nuevo Recurso" : "✏️ Editar Recurso";

            txtISBN.Enabled = esNuevo;
            txtTitulo.Enabled = true;
            txtAutor.Enabled = true;
            cmbCategoria.Enabled = true;
            numAnio.Enabled = true;
            txtDescripcion.Enabled = true;

            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            bool esAdministrador = rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            numEjemplares.Enabled = esAdministrador || esNuevo;

            btnSeleccionarFoto.Visible = true;
            btnGuardar.Visible = true;
            btnGuardar.Text = esNuevo ? "Guardar" : "Actualizar";

            btnEditar.Visible = false;
            btnDesactivar.Visible = !esNuevo && esAdministrador;
            btnCancelar.Text = "Cancelar";
        }

        private async Task CargarCategoriasAsync()
        {
            try
            {
                var categorias = await _categoriaService.ConsultarCategoriasAsync();

                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "CategoriaId";
                cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbCategoria.DrawMode = DrawMode.OwnerDrawFixed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarImagenRecursoAsync(string rutaRelativa)
        {
            picPortada.Image = null;

            if (string.IsNullOrWhiteSpace(rutaRelativa))
            {
                picPortada.Invalidate();
                return;
            }

            try
            {
                string fullUrl = rutaRelativa.Replace("\\", "/");

                if (!fullUrl.StartsWith("http://") && !fullUrl.StartsWith("https://"))
                {
                    string baseUrl = _configuration?["ApiSettings:BaseUrl"] ?? "https://localhost:54538/";
                    fullUrl = $"{baseUrl.TrimEnd('/')}/{fullUrl.TrimStart('/')}";
                }

                using var handler = new System.Net.Http.HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var client = new System.Net.Http.HttpClient(handler);
                var response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var tempImg = Image.FromStream(ms))
                        {
                            picPortada.Image = new Bitmap(tempImg);
                        }
                    }
                    picPortada.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch
            {
                picPortada.Image = null;
            }

            picPortada.Invalidate();
        }

        public async Task CargarDatosParaEdicionAsync(int recursoId, bool iniciarEnModoEdicion = false)
        {
            _recursoIdActual = recursoId;

            try
            {
                var recurso = await _catalogoService.ConsultarDetalleAsync(recursoId);

                if (recurso != null)
                {
                    _categoriaIdGuardada = recurso.CategoriaId;

                    txtISBN.Text = recurso.ISBN;
                    txtTitulo.Text = recurso.Titulo;
                    txtAutor.Text = recurso.Autor;
                    numAnio.Value = recurso.AnioPublicado;
                    numEjemplares.Value = recurso.TotalEjemplares;
                    txtDescripcion.Text = recurso.Descripcion ?? string.Empty;

                    _urlImagenActual = recurso.ImagenUrl ?? string.Empty;

                    await CargarImagenRecursoAsync(_urlImagenActual);

                    if (iniciarEnModoEdicion)
                        ActivarModoEdicion(esNuevo: false);
                    else
                        ActivarModoVista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar el detalle: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ActivarModoEdicion(esNuevo: false);
        }

        private void btnSeleccionarFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png", Title = "Seleccionar Portada" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _rutaImagenSeleccionada = ofd.FileName;
                picPortada.Image = Image.FromFile(_rutaImagenSeleccionada);
                picPortada.SizeMode = PictureBoxSizeMode.Zoom;
                picPortada.Invalidate();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                btnGuardar.Enabled = false;

                if (_recursoIdActual == 0)
                {
                    var request = new RegistrarRecursoRequest
                    {
                        ISBN = txtISBN.Text.Trim(),
                        Titulo = txtTitulo.Text.Trim(),
                        Autor = txtAutor.Text.Trim(),
                        CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                        AnioPublicado = (int)numAnio.Value,
                        CantidadEjemplares = (int)numEjemplares.Value,
                        Descripcion = txtDescripcion.Text.Trim(),
                        RutaImagenLocal = _rutaImagenSeleccionada
                    };
                    await _catalogoService.RegistrarRecursoAsync(request);

                    MessageBox.Show("Recurso registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var request = new ActualizarRecursoRequest
                    {
                        RecursoBibliograficoId = _recursoIdActual,
                        Titulo = txtTitulo.Text.Trim(),
                        Autor = txtAutor.Text.Trim(),
                        CategoriaId = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : 0,
                        AnioPublicado = (int)numAnio.Value,
                        CantidadEjemplares = (int)numEjemplares.Value,
                        Descripcion = txtDescripcion.Text.Trim(),
                        ImagenUrlActual = _urlImagenActual,
                        RutaNuevaImagenLocal = _rutaImagenSeleccionada
                    };
                    await _catalogoService.ActualizarRecursoAsync(request);

                    MessageBox.Show("Recurso actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
            }
        }

        private async void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (_recursoIdActual <= 0) return;

            var confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea desactivar el recurso '{txtTitulo.Text}'?\n\nEl recurso y sus ejemplares quedarán fuera de servicio.",
                "Confirmar Desactivación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnDesactivar.Enabled = false;

                    await _catalogoService.EliminarRecursoAsync(_recursoIdActual, "Desactivado desde el detalle del recurso");

                    MessageBox.Show("Recurso desactivado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo desactivar el recurso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    btnDesactivar.Enabled = true;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}