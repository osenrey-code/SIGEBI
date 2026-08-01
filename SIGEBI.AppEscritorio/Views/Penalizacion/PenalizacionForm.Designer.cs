namespace SIGEBI.AppEscritorio.Views.Penalizaciones
{
    partial class PenalizacionForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlContenedor = new System.Windows.Forms.Panel();
            this.dgvPenalizaciones = new System.Windows.Forms.DataGridView();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnResolver = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtUsuarioId = new System.Windows.Forms.TextBox();
            this.lblUsuarioId = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.pnlContenedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenalizaciones)).BeginInit();
            this.pnlFiltros.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContenedor
            // 
            this.pnlContenedor.Controls.Add(this.dgvPenalizaciones);
            this.pnlContenedor.Controls.Add(this.pnlFiltros);
            this.pnlContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenedor.Location = new System.Drawing.Point(15, 15);
            this.pnlContenedor.Name = "pnlContenedor";
            this.pnlContenedor.Padding = new System.Windows.Forms.Padding(10);
            this.pnlContenedor.Size = new System.Drawing.Size(920, 620);
            this.pnlContenedor.TabIndex = 0;
            // 
            // dgvPenalizaciones
            // 
            this.dgvPenalizaciones.AllowUserToAddRows = false;
            this.dgvPenalizaciones.AllowUserToDeleteRows = false;
            this.dgvPenalizaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPenalizaciones.Location = new System.Drawing.Point(10, 65);
            this.dgvPenalizaciones.Name = "dgvPenalizaciones";
            this.dgvPenalizaciones.ReadOnly = true;
            this.dgvPenalizaciones.RowTemplate.Height = 38;
            this.dgvPenalizaciones.Size = new System.Drawing.Size(900, 545);
            this.dgvPenalizaciones.TabIndex = 1;
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.Controls.Add(this.btnRefrescar);
            this.pnlFiltros.Controls.Add(this.btnResolver);
            this.pnlFiltros.Controls.Add(this.btnBuscar);
            this.pnlFiltros.Controls.Add(this.txtUsuarioId);
            this.pnlFiltros.Controls.Add(this.lblUsuarioId);
            this.pnlFiltros.Controls.Add(this.cmbEstado);
            this.pnlFiltros.Controls.Add(this.lblEstado);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(10, 10);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(900, 55);
            this.pnlFiltros.TabIndex = 0;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))); // 👈 Fijado a la izquierda para evitar que se desplace sobre Resolver
            this.btnRefrescar.Location = new System.Drawing.Point(670, 11);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(105, 32);
            this.btnRefrescar.TabIndex = 6;
            this.btnRefrescar.Text = "🔄 Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // btnResolver
            // 
            this.btnResolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResolver.Location = new System.Drawing.Point(505, 11);
            this.btnResolver.Name = "btnResolver";
            this.btnResolver.Size = new System.Drawing.Size(155, 32); // 👈 Ancho suficiente para "✅ Resolver Penalización"
            this.btnResolver.TabIndex = 5;
            this.btnResolver.Text = "✅  Resolver Penalización";
            this.btnResolver.UseVisualStyleBackColor = true;
            this.btnResolver.Click += new System.EventHandler(this.btnResolver_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(395, 11);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 32);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "🔍 Consultar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtUsuarioId
            // 
            this.txtUsuarioId.Location = new System.Drawing.Point(295, 15);
            this.txtUsuarioId.Name = "txtUsuarioId";
            this.txtUsuarioId.Size = new System.Drawing.Size(90, 23);
            this.txtUsuarioId.TabIndex = 3;
            // 
            // lblUsuarioId
            // 
            this.lblUsuarioId.AutoSize = true;
            this.lblUsuarioId.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUsuarioId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblUsuarioId.Location = new System.Drawing.Point(165, 18);
            this.lblUsuarioId.Name = "lblUsuarioId";
            this.lblUsuarioId.Size = new System.Drawing.Size(125, 17);
            this.lblUsuarioId.TabIndex = 2;
            this.lblUsuarioId.Text = "Lector / Matrícula:";
            // 
            // cmbEstado
            // 
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(65, 14);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(90, 25);
            this.cmbEstado.TabIndex = 1;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblEstado.Location = new System.Drawing.Point(10, 18);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(53, 17);
            this.lblEstado.TabIndex = 0;
            this.lblEstado.Text = "Estado:";
            // 
            // PenalizacionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(950, 650);
            this.Controls.Add(this.pnlContenedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PenalizacionForm";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.Text = "Gestión de Penalizaciones";
            this.Load += new System.EventHandler(this.PenalizacionForm_Load);
            this.pnlContenedor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenalizaciones)).EndInit();
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContenedor;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblUsuarioId;
        private System.Windows.Forms.TextBox txtUsuarioId;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnResolver;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.DataGridView dgvPenalizaciones;
    }
}