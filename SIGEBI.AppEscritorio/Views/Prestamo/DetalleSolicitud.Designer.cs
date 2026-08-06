namespace SIGEBI.AppEscritorio.Views.Shared
{
    partial class DetalleSolicitud
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnRechazar = new System.Windows.Forms.Button();
            this.btnAprobar = new System.Windows.Forms.Button();
            this.txtMotivoRechazo = new System.Windows.Forms.TextBox();
            this.lblMotivoRechazo = new System.Windows.Forms.Label();
            this.lblDetalleLibro = new System.Windows.Forms.Label();
            this.lblDetalleInfo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlContenedor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContenedor
            // 
            this.pnlContenedor.Controls.Add(this.btnCancelar);
            this.pnlContenedor.Controls.Add(this.btnRechazar);
            this.pnlContenedor.Controls.Add(this.btnAprobar);
            this.pnlContenedor.Controls.Add(this.txtMotivoRechazo);
            this.pnlContenedor.Controls.Add(this.lblMotivoRechazo);
            this.pnlContenedor.Controls.Add(this.lblDetalleLibro);
            this.pnlContenedor.Controls.Add(this.lblDetalleInfo);
            this.pnlContenedor.Controls.Add(this.lblTitulo);
            this.pnlContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenedor.Location = new System.Drawing.Point(15, 15);
            this.pnlContenedor.Name = "pnlContenedor";
            this.pnlContenedor.Size = new System.Drawing.Size(480, 365);
            this.pnlContenedor.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(345, 305);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 38);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cerrar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnRechazar
            // 
            this.btnRechazar.Location = new System.Drawing.Point(195, 305);
            this.btnRechazar.Name = "btnRechazar";
            this.btnRechazar.Size = new System.Drawing.Size(140, 38);
            this.btnRechazar.TabIndex = 6;
            this.btnRechazar.Text = "✖ Rechazar...";
            this.btnRechazar.UseVisualStyleBackColor = true;
            this.btnRechazar.Click += new System.EventHandler(this.btnRechazar_Click);
            // 
            // btnAprobar
            // 
            this.btnAprobar.Location = new System.Drawing.Point(15, 305);
            this.btnAprobar.Name = "btnAprobar";
            this.btnAprobar.Size = new System.Drawing.Size(170, 38);
            this.btnAprobar.TabIndex = 5;
            this.btnAprobar.Text = "✔ Aprobar Solicitud";
            this.btnAprobar.UseVisualStyleBackColor = true;
            this.btnAprobar.Click += new System.EventHandler(this.btnAprobar_Click);
            // 
            // txtMotivoRechazo
            // 
            this.txtMotivoRechazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtMotivoRechazo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMotivoRechazo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMotivoRechazo.ForeColor = System.Drawing.Color.White;
            this.txtMotivoRechazo.Location = new System.Drawing.Point(15, 175);
            this.txtMotivoRechazo.Multiline = true;
            this.txtMotivoRechazo.Name = "txtMotivoRechazo";
            this.txtMotivoRechazo.Size = new System.Drawing.Size(450, 110);
            this.txtMotivoRechazo.TabIndex = 4;
            this.txtMotivoRechazo.Visible = false;
            // 
            // lblMotivoRechazo
            // 
            this.lblMotivoRechazo.AutoSize = true;
            this.lblMotivoRechazo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMotivoRechazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblMotivoRechazo.Location = new System.Drawing.Point(15, 150);
            this.lblMotivoRechazo.Name = "lblMotivoRechazo";
            this.lblMotivoRechazo.Size = new System.Drawing.Size(217, 17);
            this.lblMotivoRechazo.TabIndex = 3;
            this.lblMotivoRechazo.Text = "Motivo del Rechazo (Obligatorio):";
            this.lblMotivoRechazo.Visible = false;
            // 
            // lblDetalleLibro
            // 
            this.lblDetalleLibro.AutoSize = true;
            this.lblDetalleLibro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDetalleLibro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblDetalleLibro.Location = new System.Drawing.Point(15, 80);
            this.lblDetalleLibro.Name = "lblDetalleLibro";
            this.lblDetalleLibro.Size = new System.Drawing.Size(209, 19);
            this.lblDetalleLibro.TabIndex = 2;
            this.lblDetalleLibro.Text = "Recurso: Cien Años de Soledad";
            // 
            // lblDetalleInfo
            // 
            this.lblDetalleInfo.AutoSize = true;
            this.lblDetalleInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDetalleInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblDetalleInfo.Location = new System.Drawing.Point(15, 48);
            this.lblDetalleInfo.Name = "lblDetalleInfo";
            this.lblDetalleInfo.Size = new System.Drawing.Size(250, 17);
            this.lblDetalleInfo.TabIndex = 1;
            this.lblDetalleInfo.Text = "Ejemplar ID: --- | Estado: PENDIENTE";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(148, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "✔ Detalle Solicitud";
            // 
            // DetalleSolicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(510, 395);
            this.Controls.Add(this.pnlContenedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetalleSolicitud";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalles de Solicitud";
            this.pnlContenedor.ResumeLayout(false);
            this.pnlContenedor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContenedor;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDetalleInfo;
        private System.Windows.Forms.Label lblDetalleLibro;
        private System.Windows.Forms.Label lblMotivoRechazo;
        private System.Windows.Forms.TextBox txtMotivoRechazo;
        private System.Windows.Forms.Button btnAprobar;
        private System.Windows.Forms.Button btnRechazar;
        private System.Windows.Forms.Button btnCancelar;
    }
}