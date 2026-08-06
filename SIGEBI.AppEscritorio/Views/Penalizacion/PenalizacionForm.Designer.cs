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
            this.pnlTotal = new System.Windows.Forms.Panel();
            this.lblTotalMoraTexto = new System.Windows.Forms.Label();
            this.lblTotalMoraValor = new System.Windows.Forms.Label();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.txtUsuarioId = new System.Windows.Forms.TextBox();
            this.lblUsuarioId = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.pnlContenedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPenalizaciones)).BeginInit();
            this.pnlTotal.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContenedor
            // 
            this.pnlContenedor.Controls.Add(this.dgvPenalizaciones);
            this.pnlContenedor.Controls.Add(this.pnlTotal);
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
            this.dgvPenalizaciones.Size = new System.Drawing.Size(900, 495);
            this.dgvPenalizaciones.TabIndex = 1;
            // 
            // pnlTotal
            // 
            this.pnlTotal.Controls.Add(this.lblTotalMoraValor);
            this.pnlTotal.Controls.Add(this.lblTotalMoraTexto);
            this.pnlTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTotal.Location = new System.Drawing.Point(10, 560);
            this.pnlTotal.Name = "pnlTotal";
            this.pnlTotal.Size = new System.Drawing.Size(900, 50);
            this.pnlTotal.TabIndex = 2;
            // 
            // lblTotalMoraTexto
            // 
            this.lblTotalMoraTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalMoraTexto.AutoSize = true;
            this.lblTotalMoraTexto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalMoraTexto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblTotalMoraTexto.Location = new System.Drawing.Point(580, 15);
            this.lblTotalMoraTexto.Name = "lblTotalMoraTexto";
            this.lblTotalMoraTexto.Size = new System.Drawing.Size(143, 19);
            this.lblTotalMoraTexto.TabIndex = 0;
            this.lblTotalMoraTexto.Text = "TOTAL ACUMULADO:";
            // 
            // lblTotalMoraValor
            // 
            this.lblTotalMoraValor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalMoraValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalMoraValor.ForeColor = System.Drawing.Color.White;
            this.lblTotalMoraValor.Location = new System.Drawing.Point(756, 12);
            this.lblTotalMoraValor.Name = "lblTotalMoraValor";
            this.lblTotalMoraValor.Size = new System.Drawing.Size(140, 25);
            this.lblTotalMoraValor.TabIndex = 1;
            this.lblTotalMoraValor.Text = "RD$ 0.00";
            this.lblTotalMoraValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.Controls.Add(this.btnRefrescar);
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
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefrescar.Location = new System.Drawing.Point(395, 11);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(105, 32);
            this.btnRefrescar.TabIndex = 4;
            this.btnRefrescar.Text = "🔄 Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
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
            this.lblUsuarioId.Text = "Identificación:";
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
            this.pnlTotal.ResumeLayout(false);
            this.pnlTotal.PerformLayout();
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
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.DataGridView dgvPenalizaciones;
        private System.Windows.Forms.Panel pnlTotal;
        private System.Windows.Forms.Label lblTotalMoraTexto;
        private System.Windows.Forms.Label lblTotalMoraValor;
    }
}