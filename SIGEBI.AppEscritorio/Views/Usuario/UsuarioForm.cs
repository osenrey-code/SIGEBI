using SIGEBI.AppEscritorio.Dtos.Usuarios;
using SIGEBI.AppEscritorio.Services.Usuario;
using SIGEBI.AppEscritorio.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // 👈 Necesario para el filtrado LINQ por rol
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Usuario
{
    public partial class UsuarioForm : Form
    {
        private readonly IUsuarioService _usuarioService;
        private UsuarioDto? _usuarioSeleccionado;

        // Inyección limpia por constructor
        public UsuarioForm(IUsuarioService usuarioService)
        {
            InitializeComponent();
            _usuarioService = usuarioService;

            ConfigurarDisenoTabla();
        }

        private async void UsuarioForm_Load(object sender, EventArgs e)
        {
            AplicarPermisosPorRol();

            if (cmbFiltroTipo.Items.Count > 0) cmbFiltroTipo.SelectedIndex = 0;
            if (cmbFiltroEstado.Items.Count > 0) cmbFiltroEstado.SelectedIndex = 0;

            await CargarUsuariosAsync();
        }

        #region Configuración Estética de la Tabla

        private void ConfigurarDisenoTabla()
        {
            // Propiedades base del DataGridView
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
            dgvUsuarios.RowTemplate.Height = 36;

            // Filas Alternadas (Efecto Zebra)
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 34, 49);
            dgvUsuarios.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);

            // Mapeo explicito de columnas y anchos proporcionales
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UsuarioId",
                HeaderText = "ID",
                Width = 60,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Identificacion",
                HeaderText = "Identificación",
                Width = 130
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 120
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
                HeaderText = "Tipo de Usuario",
                Width = 140
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Width = 110,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Evento para badges visuales en la columna Estado
            dgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting;
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

        #endregion

        private void AplicarPermisosPorRol()
        {
            string rol = UserSession.Instancia.TipoUsuario ?? string.Empty;
            bool esAdmin = rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            // Visibilidad de botones de acción
            btnNuevoUsuario.Visible = esAdmin;
            btnEditar.Visible = esAdmin;
            btnEstadoAccion.Visible = esAdmin;
            btnResetearPass.Visible = esAdmin;

            // 🔒 Si el usuario es Bibliotecario, limitar las opciones del ComboBox de filtro
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

                var filtro = new ConsultarUsuariosFiltroDto
                {
                    Nombre = string.IsNullOrWhiteSpace(txtBusqueda.Text) ? null : txtBusqueda.Text.Trim(),
                    TipoUsuario = cmbFiltroTipo.SelectedItem?.ToString() == "Todos" ? null : cmbFiltroTipo.SelectedItem?.ToString(),
                    Estado = cmbFiltroEstado.SelectedItem?.ToString() == "Todos" ? null : cmbFiltroEstado.SelectedItem?.ToString()
                };

                var usuarios = await _usuarioService.ConsultarUsuariosAsync(filtro);

                // 🔒 Restricción estricta de seguridad: Bibliotecarios solo ven Estudiantes y Docentes
                string rolActual = UserSession.Instancia.TipoUsuario ?? string.Empty;
                if (rolActual.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase) ||
                    rolActual.Equals("PersonalBibliotecario", StringComparison.OrdinalIgnoreCase))
                {
                    usuarios = usuarios
                        ?.Where(u => u.TipoUsuario.Equals("Estudiante", StringComparison.OrdinalIgnoreCase) ||
                                     u.TipoUsuario.Equals("Docente", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                dgvUsuarios.DataSource = usuarios;

                ActualizarEstadoBotonera();
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

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                _usuarioSeleccionado = dgvUsuarios.SelectedRows[0].DataBoundItem as UsuarioDto;
            }
            else
            {
                _usuarioSeleccionado = null;
            }

            ActualizarEstadoBotonera();
        }

        private void ActualizarEstadoBotonera()
        {
            bool seleccionado = _usuarioSeleccionado != null;
            bool esAdmin = UserSession.Instancia.TipoUsuario == "Administrador";

            btnEditar.Enabled = seleccionado && esAdmin;
            btnResetearPass.Enabled = seleccionado && esAdmin;
            btnEstadoAccion.Enabled = seleccionado && esAdmin;

            if (seleccionado)
            {
                if (_usuarioSeleccionado!.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase))
                {
                    btnEstadoAccion.Text = "🚫 Desactivar";
                    btnEstadoAccion.BackColor = Color.FromArgb(239, 68, 68);
                }
                else
                {
                    btnEstadoAccion.Text = "✅ Activar";
                    btnEstadoAccion.BackColor = Color.FromArgb(34, 197, 94);
                }
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
        }

        private async void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await CargarUsuariosAsync();
            }
        }

        private async void Filtros_Changed(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
        }

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

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null) return;

            using (var frm = new FrmEditarUsuario(
                _usuarioService,
                _usuarioSeleccionado.UsuarioId,
                _usuarioSeleccionado.NombreCompleto))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await CargarUsuariosAsync();
                }
            }
        }

        private async void btnEstadoAccion_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null) return;

            try
            {
                if (_usuarioSeleccionado.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase))
                {
                    using (var frm = new FrmDesactivarUsuario(
                        _usuarioService,
                        _usuarioSeleccionado.UsuarioId,
                        _usuarioSeleccionado.NombreCompleto))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            await CargarUsuariosAsync();
                        }
                    }
                }
                else
                {
                    var confirm = MessageBox.Show(
                        $"¿Desea reactivar al usuario {_usuarioSeleccionado.NombreCompleto}?",
                        "Confirmar Activación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        await _usuarioService.ActivarAsync(_usuarioSeleccionado.UsuarioId);
                        MessageBox.Show("Usuario activado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarUsuariosAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado: {ex.Message}", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnResetearPass_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null) return;

            using (var frm = new FrmResetearPasswordAdmin(
                _usuarioService,
                _usuarioSeleccionado.UsuarioId,
                _usuarioSeleccionado.NombreCompleto))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("La contraseña del usuario ha sido restablecida exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}