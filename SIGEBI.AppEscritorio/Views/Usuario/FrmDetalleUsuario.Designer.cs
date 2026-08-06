namespace SIGEBI.AppEscritorio.Views.Usuario
{
    partial class FrmDetalleUsuario
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
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblValNombre = new System.Windows.Forms.Label();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.lblValCorreo = new System.Windows.Forms.Label();
            this.lblRol = new System.Windows.Forms.Label();
            this.lblValRol = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblValEstado = new System.Windows.Forms.Label();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEstado = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
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
            this.pnlTopBar.Size = new System.Drawing.Size(460, 50);
            this.pnlTopBar.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(188, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "👤 Detalles del Usuario";
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnClose.Location = new System.Drawing.Point(415, 10);
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
            this.lblIdentificacion.Size = new System.Drawing.Size(137, 15);
            this.lblIdentificacion.TabIndex = 1;
            this.lblIdentificacion.Text = "Identificación / Código:";
            // 
            // lblValIdentificacion
            // 
            this.lblValIdentificacion.AutoSize = true;
            this.lblValIdentificacion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValIdentificacion.ForeColor = System.Drawing.Color.White;
            this.lblValIdentificacion.Location = new System.Drawing.Point(180, 69);
            this.lblValIdentificacion.Name = "lblValIdentificacion";
            this.lblValIdentificacion.Size = new System.Drawing.Size(33, 17);
            this.lblValIdentificacion.TabIndex = 2;
            this.lblValIdentificacion.Text = "N/A";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblNombre.Location = new System.Drawing.Point(30, 105);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(112, 15);
            this.lblNombre.TabIndex = 3;
            this.lblNombre.Text = "Nombre Completo:";
            // 
            // lblValNombre
            // 
            this.lblValNombre.AutoSize = true;
            this.lblValNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValNombre.ForeColor = System.Drawing.Color.White;
            this.lblValNombre.Location = new System.Drawing.Point(180, 104);
            this.lblValNombre.Name = "lblValNombre";
            this.lblValNombre.Size = new System.Drawing.Size(33, 17);
            this.lblValNombre.TabIndex = 4;
            this.lblValNombre.Text = "N/A";
            // 
            // lblCorreo
            // 
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCorreo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCorreo.Location = new System.Drawing.Point(30, 140);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(113, 15);
            this.lblCorreo.TabIndex = 5;
            this.lblCorreo.Text = "Correo Electrónico:";
            // 
            // lblValCorreo
            // 
            this.lblValCorreo.AutoSize = true;
            this.lblValCorreo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValCorreo.ForeColor = System.Drawing.Color.White;
            this.lblValCorreo.Location = new System.Drawing.Point(180, 139);
            this.lblValCorreo.Name = "lblValCorreo";
            this.lblValCorreo.Size = new System.Drawing.Size(33, 17);
            this.lblValCorreo.TabIndex = 6;
            this.lblValCorreo.Text = "N/A";
            // 
            // lblRol
            // 
            this.lblRol.AutoSize = true;
            this.lblRol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblRol.Location = new System.Drawing.Point(30, 175);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(95, 15);
            this.lblRol.TabIndex = 7;
            this.lblRol.Text = "Rol del Sistema:";
            // 
            // lblValRol
            // 
            this.lblValRol.AutoSize = true;
            this.lblValRol.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValRol.ForeColor = System.Drawing.Color.White;
            this.lblValRol.Location = new System.Drawing.Point(180, 174);
            this.lblValRol.Name = "lblValRol";
            this.lblValRol.Size = new System.Drawing.Size(33, 17);
            this.lblValRol.TabIndex = 8;
            this.lblValRol.Text = "N/A";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblEstado.Location = new System.Drawing.Point(30, 210);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(86, 15);
            this.lblEstado.TabIndex = 9;
            this.lblEstado.Text = "Estado Actual:";
            // 
            // lblValEstado
            // 
            this.lblValEstado.AutoSize = true;
            this.lblValEstado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblValEstado.ForeColor = System.Drawing.Color.White;
            this.lblValEstado.Location = new System.Drawing.Point(180, 209);
            this.lblValEstado.Name = "lblValEstado";
            this.lblValEstado.Size = new System.Drawing.Size(33, 17);
            this.lblValEstado.TabIndex = 10;
            this.lblValEstado.Text = "N/A";
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(20, 255);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(95, 36);
            this.btnEditar.TabIndex = 11;
            this.btnEditar.Text = "✏️ Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEstado
            // 
            this.btnEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEstado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEstado.FlatAppearance.BorderSize = 0;
            this.btnEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEstado.ForeColor = System.Drawing.Color.White;
            this.btnEstado.Location = new System.Drawing.Point(123, 255);
            this.btnEstado.Name = "btnEstado";
            this.btnEstado.Size = new System.Drawing.Size(115, 36);
            this.btnEstado.TabIndex = 12;
            this.btnEstado.Text = "🚫 Desactivar";
            this.btnEstado.UseVisualStyleBackColor = false;
            this.btnEstado.Click += new System.EventHandler(this.btnEstado_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(246, 255);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(95, 36);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "🔑 Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(349, 255);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(90, 36);
            this.btnCerrar.TabIndex = 14;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // FrmDetalleUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(460, 310);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEstado);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.lblValEstado);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblValRol);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.lblValCorreo);
            this.Controls.Add(this.lblCorreo);
            this.Controls.Add(this.lblValNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblValIdentificacion);
            this.Controls.Add(this.lblIdentificacion);
            this.Controls.Add(this.pnlTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDetalleUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalles del Usuario";
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
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblValNombre;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.Label lblValCorreo;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.Label lblValRol;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblValEstado;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEstado;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCerrar;
    }
}