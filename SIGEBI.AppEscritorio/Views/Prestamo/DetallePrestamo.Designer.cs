namespace SIGEBI.AppEscritorio.Views.Shared
{
    partial class DetallePrestamo
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
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlContenedor = new System.Windows.Forms.Panel();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.cmbCondicion = new System.Windows.Forms.ComboBox();
            this.lblCondicion = new System.Windows.Forms.Label();
            this.lblDetalleLibro = new System.Windows.Forms.Label();
            this.lblDetalleInfo = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnDevolucion = new System.Windows.Forms.Button();
            this.btnConfirmarDevolucion = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pnlTopBar.SuspendLayout();
            this.pnlContenedor.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlTopBar.Controls.Add(this.lblTitulo);
            this.pnlTopBar.Controls.Add(this.btnClose);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(520, 50);
            this.pnlTopBar.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(210, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "🔄 Detalle de Préstamo";
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnClose.Location = new System.Drawing.Point(475, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlContenedor
            // 
            this.pnlContenedor.Controls.Add(this.txtObservacion);
            this.pnlContenedor.Controls.Add(this.lblObservacion);
            this.pnlContenedor.Controls.Add(this.cmbCondicion);
            this.pnlContenedor.Controls.Add(this.lblCondicion);
            this.pnlContenedor.Controls.Add(this.lblDetalleLibro);
            this.pnlContenedor.Controls.Add(this.lblDetalleInfo);
            this.pnlContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenedor.Location = new System.Drawing.Point(0, 50);
            this.pnlContenedor.Name = "pnlContenedor";
            this.pnlContenedor.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContenedor.Size = new System.Drawing.Size(520, 335);
            this.pnlContenedor.TabIndex = 1;
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtObservacion.Location = new System.Drawing.Point(20, 132);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(475, 140);
            this.txtObservacion.TabIndex = 5;
            this.txtObservacion.Visible = false;
            // 
            // lblObservacion
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblObservacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblObservacion.Location = new System.Drawing.Point(20, 110);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(150, 15);
            this.lblObservacion.TabIndex = 4;
            this.lblObservacion.Text = "Observaciones (Opcional):";
            this.lblObservacion.Visible = false;
            // 
            // cmbCondicion
            // 
            this.cmbCondicion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCondicion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbCondicion.FormattingEnabled = true;
            this.cmbCondicion.Location = new System.Drawing.Point(20, 72);
            this.cmbCondicion.Name = "cmbCondicion";
            this.cmbCondicion.Size = new System.Drawing.Size(475, 25);
            this.cmbCondicion.TabIndex = 3;
            this.cmbCondicion.Visible = false;
            // 
            // lblCondicion
            // 
            this.lblCondicion.AutoSize = true;
            this.lblCondicion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCondicion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCondicion.Location = new System.Drawing.Point(20, 50);
            this.lblCondicion.Name = "lblCondicion";
            this.lblCondicion.Size = new System.Drawing.Size(167, 15);
            this.lblCondicion.TabIndex = 2;
            this.lblCondicion.Text = "Condición Física del Recurso:";
            this.lblCondicion.Visible = false;
            // 
            // lblDetalleLibro
            // 
            this.lblDetalleLibro.AutoSize = true;
            this.lblDetalleLibro.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDetalleLibro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblDetalleLibro.Location = new System.Drawing.Point(20, 45);
            this.lblDetalleLibro.Name = "lblDetalleLibro";
            this.lblDetalleLibro.Size = new System.Drawing.Size(220, 100);
            this.lblDetalleLibro.TabIndex = 1;
            this.lblDetalleLibro.Text = "📖 Recurso: ---\r\n🏷️ Ejemplar ID: ---\r\n📅 Fecha Inicio: ---\r\n📅 Fecha Límite: ---";
            // 
            // lblDetalleInfo
            // 
            this.lblDetalleInfo.AutoSize = true;
            this.lblDetalleInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetalleInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblDetalleInfo.Location = new System.Drawing.Point(20, 15);
            this.lblDetalleInfo.Name = "lblDetalleInfo";
            this.lblDetalleInfo.Size = new System.Drawing.Size(230, 19);
            this.lblDetalleInfo.TabIndex = 0;
            this.lblDetalleInfo.Text = "👤 Lector: ---  |  📌 Estado: ACTIVO";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlFooter.Controls.Add(this.btnDevolucion);
            this.pnlFooter.Controls.Add(this.btnConfirmarDevolucion);
            this.pnlFooter.Controls.Add(this.btnCancelar);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 385);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(520, 65);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnDevolucion
            // 
            this.btnDevolucion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnDevolucion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDevolucion.FlatAppearance.BorderSize = 0;
            this.btnDevolucion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevolucion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDevolucion.ForeColor = System.Drawing.Color.White;
            this.btnDevolucion.Location = new System.Drawing.Point(20, 14);
            this.btnDevolucion.Name = "btnDevolucion";
            this.btnDevolucion.Size = new System.Drawing.Size(225, 38);
            this.btnDevolucion.TabIndex = 0;
            this.btnDevolucion.Text = "🔄 Registrar Devolución";
            this.btnDevolucion.UseVisualStyleBackColor = false;
            this.btnDevolucion.Click += new System.EventHandler(this.btnDevolucion_Click);
            // 
            // btnConfirmarDevolucion
            // 
            this.btnConfirmarDevolucion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnConfirmarDevolucion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmarDevolucion.FlatAppearance.BorderSize = 0;
            this.btnConfirmarDevolucion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarDevolucion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnConfirmarDevolucion.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarDevolucion.Location = new System.Drawing.Point(20, 14);
            this.btnConfirmarDevolucion.Name = "btnConfirmarDevolucion";
            this.btnConfirmarDevolucion.Size = new System.Drawing.Size(225, 38);
            this.btnConfirmarDevolucion.TabIndex = 1;
            this.btnConfirmarDevolucion.Text = "✅ Confirmar Devolución";
            this.btnConfirmarDevolucion.UseVisualStyleBackColor = false;
            this.btnConfirmarDevolucion.Visible = false;
            this.btnConfirmarDevolucion.Click += new System.EventHandler(this.btnConfirmarDevolucion_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(260, 14);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(225, 38);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cerrar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // DetallePrestamo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(520, 450);
            this.Controls.Add(this.pnlContenedor);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DetallePrestamo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalles de Préstamo";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlContenedor.ResumeLayout(false);
            this.pnlContenedor.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlContenedor;
        private System.Windows.Forms.Label lblDetalleInfo;
        private System.Windows.Forms.Label lblDetalleLibro;
        private System.Windows.Forms.Label lblCondicion;
        private System.Windows.Forms.ComboBox cmbCondicion;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnDevolucion;
        private System.Windows.Forms.Button btnConfirmarDevolucion;
        private System.Windows.Forms.Button btnCancelar;
    }
}