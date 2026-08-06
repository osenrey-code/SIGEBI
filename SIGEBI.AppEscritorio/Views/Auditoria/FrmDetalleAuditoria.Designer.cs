namespace SIGEBI.AppEscritorio.Views.Auditoria
{
    partial class FrmDetalleAuditoria
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblIdentificacion = new System.Windows.Forms.Label();
            this.lblValIdentificacion = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblValUsuario = new System.Windows.Forms.Label();
            this.lblAccion = new System.Windows.Forms.Label();
            this.lblValAccion = new System.Windows.Forms.Label();
            this.lblEntidad = new System.Windows.Forms.Label();
            this.lblValEntidad = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblValFecha = new System.Windows.Forms.Label();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.txtValDetalle = new System.Windows.Forms.TextBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.pnlTopBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlTopBar.Controls.Add(this.lblTitle);
            this.pnlTopBar.Controls.Add(this.btnClose);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(520, 50);
            this.pnlTopBar.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(262, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🛡️ Detalle de Log de Auditoría";
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
            // lblIdentificacion
            // 
            this.lblIdentificacion.AutoSize = true;
            this.lblIdentificacion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblIdentificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblIdentificacion.Location = new System.Drawing.Point(30, 70);
            this.lblIdentificacion.Name = "lblIdentificacion";
            this.lblIdentificacion.Size = new System.Drawing.Size(87, 15);
            this.lblIdentificacion.TabIndex = 1;
            this.lblIdentificacion.Text = "Identificación:";
            // 
            // lblValIdentificacion
            // 
            this.lblValIdentificacion.AutoSize = true;
            this.lblValIdentificacion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValIdentificacion.ForeColor = System.Drawing.Color.White;
            this.lblValIdentificacion.Location = new System.Drawing.Point(140, 69);
            this.lblValIdentificacion.Name = "lblValIdentificacion";
            this.lblValIdentificacion.Size = new System.Drawing.Size(33, 17);
            this.lblValIdentificacion.TabIndex = 2;
            this.lblValIdentificacion.Text = "N/A";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblUsuario.Location = new System.Drawing.Point(30, 100);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(52, 15);
            this.lblUsuario.TabIndex = 3;
            this.lblUsuario.Text = "Usuario:";
            // 
            // lblValUsuario
            // 
            this.lblValUsuario.AutoSize = true;
            this.lblValUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValUsuario.ForeColor = System.Drawing.Color.White;
            this.lblValUsuario.Location = new System.Drawing.Point(140, 99);
            this.lblValUsuario.Name = "lblValUsuario";
            this.lblValUsuario.Size = new System.Drawing.Size(33, 17);
            this.lblValUsuario.TabIndex = 4;
            this.lblValUsuario.Text = "N/A";
            // 
            // lblAccion
            // 
            this.lblAccion.AutoSize = true;
            this.lblAccion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblAccion.Location = new System.Drawing.Point(30, 130);
            this.lblAccion.Name = "lblAccion";
            this.lblAccion.Size = new System.Drawing.Size(48, 15);
            this.lblAccion.TabIndex = 5;
            this.lblAccion.Text = "Acción:";
            // 
            // lblValAccion
            // 
            this.lblValAccion.AutoSize = true;
            this.lblValAccion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValAccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(197)))), ((int)(((byte)(253)))));
            this.lblValAccion.Location = new System.Drawing.Point(140, 129);
            this.lblValAccion.Name = "lblValAccion";
            this.lblValAccion.Size = new System.Drawing.Size(33, 17);
            this.lblValAccion.TabIndex = 6;
            this.lblValAccion.Text = "N/A";
            // 
            // lblEntidad
            // 
            this.lblEntidad.AutoSize = true;
            this.lblEntidad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEntidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblEntidad.Location = new System.Drawing.Point(30, 160);
            this.lblEntidad.Name = "lblEntidad";
            this.lblEntidad.Size = new System.Drawing.Size(51, 15);
            this.lblEntidad.TabIndex = 7;
            this.lblEntidad.Text = "Entidad:";
            // 
            // lblValEntidad
            // 
            this.lblValEntidad.AutoSize = true;
            this.lblValEntidad.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValEntidad.ForeColor = System.Drawing.Color.White;
            this.lblValEntidad.Location = new System.Drawing.Point(140, 159);
            this.lblValEntidad.Name = "lblValEntidad";
            this.lblValEntidad.Size = new System.Drawing.Size(33, 17);
            this.lblValEntidad.TabIndex = 8;
            this.lblValEntidad.Text = "N/A";
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFecha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblFecha.Location = new System.Drawing.Point(30, 190);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(93, 15);
            this.lblFecha.TabIndex = 9;
            this.lblFecha.Text = "Fecha / Hora:";
            // 
            // lblValFecha
            // 
            this.lblValFecha.AutoSize = true;
            this.lblValFecha.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValFecha.ForeColor = System.Drawing.Color.White;
            this.lblValFecha.Location = new System.Drawing.Point(140, 189);
            this.lblValFecha.Name = "lblValFecha";
            this.lblValFecha.Size = new System.Drawing.Size(33, 17);
            this.lblValFecha.TabIndex = 10;
            this.lblValFecha.Text = "N/A";
            // 
            // lblDetalle
            // 
            this.lblDetalle.AutoSize = true;
            this.lblDetalle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblDetalle.Location = new System.Drawing.Point(30, 225);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Size = new System.Drawing.Size(107, 15);
            this.lblDetalle.TabIndex = 11;
            this.lblDetalle.Text = "Detalle Completo:";
            // 
            // txtValDetalle
            // 
            this.txtValDetalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtValDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValDetalle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtValDetalle.ForeColor = System.Drawing.Color.White;
            this.txtValDetalle.Location = new System.Drawing.Point(33, 248);
            this.txtValDetalle.Multiline = true;
            this.txtValDetalle.Name = "txtValDetalle";
            this.txtValDetalle.ReadOnly = true;
            this.txtValDetalle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtValDetalle.Size = new System.Drawing.Size(454, 90);
            this.txtValDetalle.TabIndex = 12;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(387, 355);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(100, 36);
            this.btnCerrar.TabIndex = 13;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // FrmDetalleAuditoria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(520, 410);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.txtValDetalle);
            this.Controls.Add(this.lblDetalle);
            this.Controls.Add(this.lblValFecha);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.lblValEntidad);
            this.Controls.Add(this.lblEntidad);
            this.Controls.Add(this.lblValAccion);
            this.Controls.Add(this.lblAccion);
            this.Controls.Add(this.lblValUsuario);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.lblValIdentificacion);
            this.Controls.Add(this.lblIdentificacion);
            this.Controls.Add(this.pnlTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDetalleAuditoria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalle de Log de Auditoría";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblIdentificacion;
        private System.Windows.Forms.Label lblValIdentificacion;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblValUsuario;
        private System.Windows.Forms.Label lblAccion;
        private System.Windows.Forms.Label lblValAccion;
        private System.Windows.Forms.Label lblEntidad;
        private System.Windows.Forms.Label lblValEntidad;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblValFecha;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.TextBox txtValDetalle;
        private System.Windows.Forms.Button btnCerrar;
    }
}