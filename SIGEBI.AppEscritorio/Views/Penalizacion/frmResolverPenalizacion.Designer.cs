namespace SIGEBI.AppEscritorio.Views.Penalizaciones
{
    partial class FrmResolverPenalizacion
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.lblMotivoResolucion = new System.Windows.Forms.Label();
            this.txtMotivoResolucion = new System.Windows.Forms.TextBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pnlContenedor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContenedor
            // 
            this.pnlContenedor.Controls.Add(this.btnCancelar);
            this.pnlContenedor.Controls.Add(this.btnConfirmar);
            this.pnlContenedor.Controls.Add(this.txtMotivoResolucion);
            this.pnlContenedor.Controls.Add(this.lblMotivoResolucion);
            this.pnlContenedor.Controls.Add(this.lblDetalle);
            this.pnlContenedor.Controls.Add(this.lblTitulo);
            this.pnlContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenedor.Location = new System.Drawing.Point(15, 15);
            this.pnlContenedor.Name = "pnlContenedor";
            this.pnlContenedor.Size = new System.Drawing.Size(450, 310);
            this.pnlContenedor.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "✅ Resolver Penalización";
            // 
            // lblDetalle
            // 
            this.lblDetalle.AutoSize = true;
            this.lblDetalle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblDetalle.Location = new System.Drawing.Point(15, 48);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Size = new System.Drawing.Size(235, 17);
            this.lblDetalle.TabIndex = 1;
            this.lblDetalle.Text = "Lector: --- | Préstamo ID: --- | Mora: ---";
            // 
            // lblMotivoResolucion
            // 
            this.lblMotivoResolucion.AutoSize = true;
            this.lblMotivoResolucion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMotivoResolucion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblMotivoResolucion.Location = new System.Drawing.Point(15, 85);
            this.lblMotivoResolucion.Name = "lblMotivoResolucion";
            this.lblMotivoResolucion.Size = new System.Drawing.Size(296, 17);
            this.lblMotivoResolucion.TabIndex = 2;
            this.lblMotivoResolucion.Text = "Motivo de la Resolución / Comprobante de Pago:";
            // 
            // txtMotivoResolucion
            // 
            this.txtMotivoResolucion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtMotivoResolucion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMotivoResolucion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMotivoResolucion.ForeColor = System.Drawing.Color.White;
            this.txtMotivoResolucion.Location = new System.Drawing.Point(15, 110);
            this.txtMotivoResolucion.Multiline = true;
            this.txtMotivoResolucion.Name = "txtMotivoResolucion";
            this.txtMotivoResolucion.Size = new System.Drawing.Size(420, 110);
            this.txtMotivoResolucion.TabIndex = 3;
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(15, 245);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(240, 38);
            this.btnConfirmar.TabIndex = 4;
            this.btnConfirmar.Text = "✅ Confirmar y Liberar Lector";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(265, 245);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(170, 38);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmResolverPenalizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(480, 340);
            this.Controls.Add(this.pnlContenedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResolverPenalizacion";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resolver Penalización";
            this.pnlContenedor.ResumeLayout(false);
            this.pnlContenedor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContenedor;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.Label lblMotivoResolucion;
        private System.Windows.Forms.TextBox txtMotivoResolucion;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnCancelar;
    }
}