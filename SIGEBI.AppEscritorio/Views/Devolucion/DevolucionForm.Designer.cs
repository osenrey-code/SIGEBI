namespace SIGEBI.AppEscritorio.Views.Devolucion
{
    partial class DevolucionForm
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
            this.pnlHistorialContainer = new System.Windows.Forms.Panel();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.pnlFiltrosHistorial = new System.Windows.Forms.Panel();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnBuscarHistorial = new System.Windows.Forms.Button();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.lblDesde = new System.Windows.Forms.Label();
            this.pnlHistorialContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.pnlFiltrosHistorial.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHistorialContainer
            // 
            this.pnlHistorialContainer.Controls.Add(this.dgvHistorial);
            this.pnlHistorialContainer.Controls.Add(this.pnlFiltrosHistorial);
            this.pnlHistorialContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHistorialContainer.Location = new System.Drawing.Point(15, 15);
            this.pnlHistorialContainer.Name = "pnlHistorialContainer";
            this.pnlHistorialContainer.Padding = new System.Windows.Forms.Padding(10);
            this.pnlHistorialContainer.Size = new System.Drawing.Size(920, 620);
            this.pnlHistorialContainer.TabIndex = 0;
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistorial.Location = new System.Drawing.Point(10, 65);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowTemplate.Height = 38;
            this.dgvHistorial.Size = new System.Drawing.Size(900, 545);
            this.dgvHistorial.TabIndex = 1;
            // 
            // pnlFiltrosHistorial
            // 
            this.pnlFiltrosHistorial.Controls.Add(this.btnRefrescar);
            this.pnlFiltrosHistorial.Controls.Add(this.btnBuscarHistorial);
            this.pnlFiltrosHistorial.Controls.Add(this.dtpFin);
            this.pnlFiltrosHistorial.Controls.Add(this.lblHasta);
            this.pnlFiltrosHistorial.Controls.Add(this.dtpInicio);
            this.pnlFiltrosHistorial.Controls.Add(this.lblDesde);
            this.pnlFiltrosHistorial.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltrosHistorial.Location = new System.Drawing.Point(10, 10);
            this.pnlFiltrosHistorial.Name = "pnlFiltrosHistorial";
            this.pnlFiltrosHistorial.Size = new System.Drawing.Size(900, 55);
            this.pnlFiltrosHistorial.TabIndex = 0;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefrescar.Location = new System.Drawing.Point(770, 11);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(120, 32);
            this.btnRefrescar.TabIndex = 5;
            this.btnRefrescar.Text = "🔄 Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // btnBuscarHistorial
            // 
            this.btnBuscarHistorial.Location = new System.Drawing.Point(375, 11);
            this.btnBuscarHistorial.Name = "btnBuscarHistorial";
            this.btnBuscarHistorial.Size = new System.Drawing.Size(120, 32);
            this.btnBuscarHistorial.TabIndex = 4;
            this.btnBuscarHistorial.Text = "🔍 Consultar";
            this.btnBuscarHistorial.UseVisualStyleBackColor = true;
            this.btnBuscarHistorial.Click += new System.EventHandler(this.btnBuscarHistorial_Click);
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
            // DevolucionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(950, 650);
            this.Controls.Add(this.pnlHistorialContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DevolucionForm";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.Text = "Historial de Devoluciones";
            this.Load += new System.EventHandler(this.DevolucionForm_Load);
            this.pnlHistorialContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.pnlFiltrosHistorial.ResumeLayout(false);
            this.pnlFiltrosHistorial.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHistorialContainer;
        private System.Windows.Forms.Panel pnlFiltrosHistorial;
        private System.Windows.Forms.Button btnBuscarHistorial;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DataGridView dgvHistorial;
    }
}