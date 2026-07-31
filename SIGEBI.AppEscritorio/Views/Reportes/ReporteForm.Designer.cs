namespace SIGEBI.AppEscritorio.Views.Reportes
{
    partial class ReporteForm
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
            this.pnlNavegacion = new System.Windows.Forms.Panel();
            this.btnTabCatalogo = new System.Windows.Forms.Button();
            this.btnTabPenalizaciones = new System.Windows.Forms.Button();
            this.btnTabPrestamos = new System.Windows.Forms.Button();
            this.btnTabInventario = new System.Windows.Forms.Button();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.lblDesde = new System.Windows.Forms.Label();
            this.pnlKpis = new System.Windows.Forms.Panel();
            this.pnlKpi4 = new System.Windows.Forms.Panel();
            this.lblKpi4Val = new System.Windows.Forms.Label();
            this.lblKpi4Title = new System.Windows.Forms.Label();
            this.pnlKpi3 = new System.Windows.Forms.Panel();
            this.lblKpi3Val = new System.Windows.Forms.Label();
            this.lblKpi3Title = new System.Windows.Forms.Label();
            this.pnlKpi2 = new System.Windows.Forms.Panel();
            this.lblKpi2Val = new System.Windows.Forms.Label();
            this.lblKpi2Title = new System.Windows.Forms.Label();
            this.pnlKpi1 = new System.Windows.Forms.Panel();
            this.lblKpi1Val = new System.Windows.Forms.Label();
            this.lblKpi1Title = new System.Windows.Forms.Label();
            this.pnlContenedorGrid = new System.Windows.Forms.Panel();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.pnlNavegacion.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.pnlKpis.SuspendLayout();
            this.pnlKpi4.SuspendLayout();
            this.pnlKpi3.SuspendLayout();
            this.pnlKpi2.SuspendLayout();
            this.pnlKpi1.SuspendLayout();
            this.pnlContenedorGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlNavegacion
            // 
            this.pnlNavegacion.Controls.Add(this.btnTabCatalogo);
            this.pnlNavegacion.Controls.Add(this.btnTabPenalizaciones);
            this.pnlNavegacion.Controls.Add(this.btnTabPrestamos);
            this.pnlNavegacion.Controls.Add(this.btnTabInventario);
            this.pnlNavegacion.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavegacion.Location = new System.Drawing.Point(15, 15);
            this.pnlNavegacion.Name = "pnlNavegacion";
            this.pnlNavegacion.Size = new System.Drawing.Size(920, 45);
            this.pnlNavegacion.TabIndex = 0;
            // 
            // btnTabCatalogo
            // 
            this.btnTabCatalogo.Location = new System.Drawing.Point(525, 0);
            this.btnTabCatalogo.Name = "btnTabCatalogo";
            this.btnTabCatalogo.Size = new System.Drawing.Size(165, 38);
            this.btnTabCatalogo.TabIndex = 3;
            this.btnTabCatalogo.Text = "📈 Usage Catálogo";
            this.btnTabCatalogo.UseVisualStyleBackColor = true;
            this.btnTabCatalogo.Click += new System.EventHandler(this.btnTabCatalogo_Click);
            // 
            // btnTabPenalizaciones
            // 
            this.btnTabPenalizaciones.Location = new System.Drawing.Point(350, 0);
            this.btnTabPenalizaciones.Name = "btnTabPenalizaciones";
            this.btnTabPenalizaciones.Size = new System.Drawing.Size(165, 38);
            this.btnTabPenalizaciones.TabIndex = 2;
            this.btnTabPenalizaciones.Text = "⚠️ Penalizaciones";
            this.btnTabPenalizaciones.UseVisualStyleBackColor = true;
            this.btnTabPenalizaciones.Click += new System.EventHandler(this.btnTabPenalizaciones_Click);
            // 
            // btnTabPrestamos
            // 
            this.btnTabPrestamos.Location = new System.Drawing.Point(175, 0);
            this.btnTabPrestamos.Name = "btnTabPrestamos";
            this.btnTabPrestamos.Size = new System.Drawing.Size(165, 38);
            this.btnTabPrestamos.TabIndex = 1;
            this.btnTabPrestamos.Text = "📚 Préstamos";
            this.btnTabPrestamos.UseVisualStyleBackColor = true;
            this.btnTabPrestamos.Click += new System.EventHandler(this.btnTabPrestamos_Click);
            // 
            // btnTabInventario
            // 
            this.btnTabInventario.Location = new System.Drawing.Point(0, 0);
            this.btnTabInventario.Name = "btnTabInventario";
            this.btnTabInventario.Size = new System.Drawing.Size(165, 38);
            this.btnTabInventario.TabIndex = 0;
            this.btnTabInventario.Text = "📦 Inventario";
            this.btnTabInventario.UseVisualStyleBackColor = true;
            this.btnTabInventario.Click += new System.EventHandler(this.btnTabInventario_Click);
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.Controls.Add(this.btnExportarPdf);
            this.pnlFiltros.Controls.Add(this.btnBuscar);
            this.pnlFiltros.Controls.Add(this.dtpFin);
            this.pnlFiltros.Controls.Add(this.lblHasta);
            this.pnlFiltros.Controls.Add(this.dtpInicio);
            this.pnlFiltros.Controls.Add(this.lblDesde);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(15, 60);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(920, 55);
            this.pnlFiltros.TabIndex = 1;
            // 
            // btnExportarPdf
            // 
            this.btnExportarPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarPdf.Location = new System.Drawing.Point(755, 11);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(155, 32);
            this.btnExportarPdf.TabIndex = 5;
            this.btnExportarPdf.Text = "📄 Exportar PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = true;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(375, 11);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 32);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "🔍 Consultar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dtpFin
            // 
            this.dtpFin.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFin.Location = new System.Drawing.Point(250, 14);
            this.dtpFin.Name = "dtpFin";
            this.dtpFin.Size = new System.Drawing.Size(110, 24);
            this.dtpFin.TabIndex = 3;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHasta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblHasta.Location = new System.Drawing.Point(195, 18);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(47, 17);
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Text = "Hasta:";
            // 
            // dtpInicio
            // 
            this.dtpInicio.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInicio.Location = new System.Drawing.Point(70, 14);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(110, 24);
            this.dtpInicio.TabIndex = 1;
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDesde.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblDesde.Location = new System.Drawing.Point(12, 18);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(50, 17);
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Text = "Desde:";
            // 
            // pnlKpis
            // 
            this.pnlKpis.Controls.Add(this.pnlKpi4);
            this.pnlKpis.Controls.Add(this.pnlKpi3);
            this.pnlKpis.Controls.Add(this.pnlKpi2);
            this.pnlKpis.Controls.Add(this.pnlKpi1);
            this.pnlKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpis.Location = new System.Drawing.Point(15, 115);
            this.pnlKpis.Name = "pnlKpis";
            this.pnlKpis.Size = new System.Drawing.Size(920, 85);
            this.pnlKpis.TabIndex = 2;
            // 
            // pnlKpi4
            // 
            this.pnlKpi4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlKpi4.Controls.Add(this.lblKpi4Val);
            this.pnlKpi4.Controls.Add(this.lblKpi4Title);
            this.pnlKpi4.Location = new System.Drawing.Point(615, 10);
            this.pnlKpi4.Name = "pnlKpi4";
            this.pnlKpi4.Size = new System.Drawing.Size(190, 65);
            this.pnlKpi4.TabIndex = 3;
            // 
            // lblKpi4Val
            // 
            this.lblKpi4Val.AutoSize = true;
            this.lblKpi4Val.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi4Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblKpi4Val.Location = new System.Drawing.Point(10, 28);
            this.lblKpi4Val.Name = "lblKpi4Val";
            this.lblKpi4Val.Size = new System.Drawing.Size(23, 25);
            this.lblKpi4Val.TabIndex = 1;
            this.lblKpi4Val.Text = "0";
            // 
            // lblKpi4Title
            // 
            this.lblKpi4Title.AutoSize = true;
            this.lblKpi4Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi4Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblKpi4Title.Location = new System.Drawing.Point(10, 8);
            this.lblKpi4Title.Name = "lblKpi4Title";
            this.lblKpi4Title.Size = new System.Drawing.Size(51, 15);
            this.lblKpi4Title.TabIndex = 0;
            this.lblKpi4Title.Text = "Métrica";
            // 
            // pnlKpi3
            // 
            this.pnlKpi3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlKpi3.Controls.Add(this.lblKpi3Val);
            this.pnlKpi3.Controls.Add(this.lblKpi3Title);
            this.pnlKpi3.Location = new System.Drawing.Point(410, 10);
            this.pnlKpi3.Name = "pnlKpi3";
            this.pnlKpi3.Size = new System.Drawing.Size(190, 65);
            this.pnlKpi3.TabIndex = 2;
            // 
            // lblKpi3Val
            // 
            this.lblKpi3Val.AutoSize = true;
            this.lblKpi3Val.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi3Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.lblKpi3Val.Location = new System.Drawing.Point(10, 28);
            this.lblKpi3Val.Name = "lblKpi3Val";
            this.lblKpi3Val.Size = new System.Drawing.Size(23, 25);
            this.lblKpi3Val.TabIndex = 1;
            this.lblKpi3Val.Text = "0";
            // 
            // lblKpi3Title
            // 
            this.lblKpi3Title.AutoSize = true;
            this.lblKpi3Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblKpi3Title.Location = new System.Drawing.Point(10, 8);
            this.lblKpi3Title.Name = "lblKpi3Title";
            this.lblKpi3Title.Size = new System.Drawing.Size(51, 15);
            this.lblKpi3Title.TabIndex = 0;
            this.lblKpi3Title.Text = "Métrica";
            // 
            // pnlKpi2
            // 
            this.pnlKpi2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlKpi2.Controls.Add(this.lblKpi2Val);
            this.pnlKpi2.Controls.Add(this.lblKpi2Title);
            this.pnlKpi2.Location = new System.Drawing.Point(205, 10);
            this.pnlKpi2.Name = "pnlKpi2";
            this.pnlKpi2.Size = new System.Drawing.Size(190, 65);
            this.pnlKpi2.TabIndex = 1;
            // 
            // lblKpi2Val
            // 
            this.lblKpi2Val.AutoSize = true;
            this.lblKpi2Val.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi2Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblKpi2Val.Location = new System.Drawing.Point(10, 28);
            this.lblKpi2Val.Name = "lblKpi2Val";
            this.lblKpi2Val.Size = new System.Drawing.Size(23, 25);
            this.lblKpi2Val.TabIndex = 1;
            this.lblKpi2Val.Text = "0";
            // 
            // lblKpi2Title
            // 
            this.lblKpi2Title.AutoSize = true;
            this.lblKpi2Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblKpi2Title.Location = new System.Drawing.Point(10, 8);
            this.lblKpi2Title.Name = "lblKpi2Title";
            this.lblKpi2Title.Size = new System.Drawing.Size(51, 15);
            this.lblKpi2Title.TabIndex = 0;
            this.lblKpi2Title.Text = "Métrica";
            // 
            // pnlKpi1
            // 
            this.pnlKpi1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlKpi1.Controls.Add(this.lblKpi1Val);
            this.pnlKpi1.Controls.Add(this.lblKpi1Title);
            this.pnlKpi1.Location = new System.Drawing.Point(0, 10);
            this.pnlKpi1.Name = "pnlKpi1";
            this.pnlKpi1.Size = new System.Drawing.Size(190, 65);
            this.pnlKpi1.TabIndex = 0;
            // 
            // lblKpi1Val
            // 
            this.lblKpi1Val.AutoSize = true;
            this.lblKpi1Val.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi1Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblKpi1Val.Location = new System.Drawing.Point(10, 28);
            this.lblKpi1Val.Name = "lblKpi1Val";
            this.lblKpi1Val.Size = new System.Drawing.Size(23, 25);
            this.lblKpi1Val.TabIndex = 1;
            this.lblKpi1Val.Text = "0";
            // 
            // lblKpi1Title
            // 
            this.lblKpi1Title.AutoSize = true;
            this.lblKpi1Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpi1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblKpi1Title.Location = new System.Drawing.Point(10, 8);
            this.lblKpi1Title.Name = "lblKpi1Title";
            this.lblKpi1Title.Size = new System.Drawing.Size(51, 15);
            this.lblKpi1Title.TabIndex = 0;
            this.lblKpi1Title.Text = "Métrica";
            // 
            // pnlContenedorGrid
            // 
            this.pnlContenedorGrid.Controls.Add(this.dgvDatos);
            this.pnlContenedorGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenedorGrid.Location = new System.Drawing.Point(15, 200);
            this.pnlContenedorGrid.Name = "pnlContenedorGrid";
            this.pnlContenedorGrid.Padding = new System.Windows.Forms.Padding(10);
            this.pnlContenedorGrid.Size = new System.Drawing.Size(920, 435);
            this.pnlContenedorGrid.TabIndex = 3;
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDatos.Location = new System.Drawing.Point(10, 10);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowTemplate.Height = 38;
            this.dgvDatos.Size = new System.Drawing.Size(900, 415);
            this.dgvDatos.TabIndex = 0;
            // 
            // ReporteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(950, 650);
            this.Controls.Add(this.pnlContenedorGrid);
            this.Controls.Add(this.pnlKpis);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlNavegacion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReporteForm";
            this.Text = "Auditoría y Reportes";
            this.Load += new System.EventHandler(this.ReporteForm_Load);
            this.pnlNavegacion.ResumeLayout(false);
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.pnlKpis.ResumeLayout(false);
            this.pnlKpi4.ResumeLayout(false);
            this.pnlKpi4.PerformLayout();
            this.pnlKpi3.ResumeLayout(false);
            this.pnlKpi3.PerformLayout();
            this.pnlKpi2.ResumeLayout(false);
            this.pnlKpi2.PerformLayout();
            this.pnlKpi1.ResumeLayout(false);
            this.pnlKpi1.PerformLayout();
            this.pnlContenedorGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNavegacion;
        private System.Windows.Forms.Button btnTabCatalogo;
        private System.Windows.Forms.Button btnTabPenalizaciones;
        private System.Windows.Forms.Button btnTabPrestamos;
        private System.Windows.Forms.Button btnTabInventario;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.Panel pnlKpis;
        private System.Windows.Forms.Panel pnlKpi4;
        private System.Windows.Forms.Label lblKpi4Val;
        private System.Windows.Forms.Label lblKpi4Title;
        private System.Windows.Forms.Panel pnlKpi3;
        private System.Windows.Forms.Label lblKpi3Val;
        private System.Windows.Forms.Label lblKpi3Title;
        private System.Windows.Forms.Panel pnlKpi2;
        private System.Windows.Forms.Label lblKpi2Val;
        private System.Windows.Forms.Label lblKpi2Title;
        private System.Windows.Forms.Panel pnlKpi1;
        private System.Windows.Forms.Label lblKpi1Val;
        private System.Windows.Forms.Label lblKpi1Title;
        private System.Windows.Forms.Panel pnlContenedorGrid;
        private System.Windows.Forms.DataGridView dgvDatos;
    }
}