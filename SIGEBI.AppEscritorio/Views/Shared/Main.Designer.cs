namespace SIGEBI.AppEscritorio.Views.Shared
{
    partial class Main
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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.btnMenuAuditoria = new System.Windows.Forms.Button();
            this.btnMenuReportes = new System.Windows.Forms.Button(); // 👈 Declaración de Reportes
            this.btnMenuUsuarios = new System.Windows.Forms.Button();
            this.btnMenuPenalizaciones = new System.Windows.Forms.Button();
            this.btnMenuDevoluciones = new System.Windows.Forms.Button();
            this.btnMenuPrestamos = new System.Windows.Forms.Button();
            this.btnMenuCategorias = new System.Windows.Forms.Button();
            this.btnMenuCatalogo = new System.Windows.Forms.Button();
            this.btnMenuDashboard = new System.Windows.Forms.Button();
            this.pnlBrand = new System.Windows.Forms.Panel();
            this.picSidebarLogo = new System.Windows.Forms.PictureBox();
            this.lblBrandSub = new System.Windows.Forms.Label();
            this.lblBrandTitle = new System.Windows.Forms.Label();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblUsuarioLogueado = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlCard4 = new System.Windows.Forms.Panel();
            this.lblCard4Val = new System.Windows.Forms.Label();
            this.lblCard4Title = new System.Windows.Forms.Label();
            this.pnlCard3 = new System.Windows.Forms.Panel();
            this.lblCard3Val = new System.Windows.Forms.Label();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.pnlCard2 = new System.Windows.Forms.Panel();
            this.lblCard2Val = new System.Windows.Forms.Label();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.pnlCard1 = new System.Windows.Forms.Panel();
            this.lblCard1Val = new System.Windows.Forms.Label();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.lblWelcomeSub = new System.Windows.Forms.Label();
            this.lblWelcomeHeader = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            this.pnlBrand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSidebarLogo)).BeginInit();
            this.pnlTopBar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlCard4.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlSidebar.Controls.Add(this.btnCerrarSesion);
            this.pnlSidebar.Controls.Add(this.btnMenuAuditoria);
            this.pnlSidebar.Controls.Add(this.btnMenuReportes); // 👈 Añadido a pnlSidebar
            this.pnlSidebar.Controls.Add(this.btnMenuUsuarios);
            this.pnlSidebar.Controls.Add(this.btnMenuPenalizaciones);
            this.pnlSidebar.Controls.Add(this.btnMenuDevoluciones);
            this.pnlSidebar.Controls.Add(this.btnMenuPrestamos);
            this.pnlSidebar.Controls.Add(this.btnMenuCategorias);
            this.pnlSidebar.Controls.Add(this.btnMenuCatalogo);
            this.pnlSidebar.Controls.Add(this.btnMenuDashboard);
            this.pnlSidebar.Controls.Add(this.pnlBrand);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(240, 680);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnCerrarSesion.Location = new System.Drawing.Point(0, 630);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCerrarSesion.Size = new System.Drawing.Size(240, 50);
            this.btnCerrarSesion.TabIndex = 10;
            this.btnCerrarSesion.Text = "🚪  Cerrar Sesión";
            this.btnCerrarSesion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // btnMenuAuditoria
            // 
            this.btnMenuAuditoria.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuAuditoria.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuAuditoria.FlatAppearance.BorderSize = 0;
            this.btnMenuAuditoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuAuditoria.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuAuditoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuAuditoria.Location = new System.Drawing.Point(0, 449);
            this.btnMenuAuditoria.Name = "btnMenuAuditoria";
            this.btnMenuAuditoria.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuAuditoria.Size = new System.Drawing.Size(240, 48);
            this.btnMenuAuditoria.TabIndex = 9;
            this.btnMenuAuditoria.Text = "🛡️  Log de Auditoría";
            this.btnMenuAuditoria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuAuditoria.UseVisualStyleBackColor = true;
            this.btnMenuAuditoria.Click += new System.EventHandler(this.btnMenuAuditoria_Click);
            // 
            // btnMenuReportes
            // 
            this.btnMenuReportes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuReportes.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuReportes.FlatAppearance.BorderSize = 0;
            this.btnMenuReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuReportes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuReportes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuReportes.Location = new System.Drawing.Point(0, 401);
            this.btnMenuReportes.Name = "btnMenuReportes";
            this.btnMenuReportes.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuReportes.Size = new System.Drawing.Size(240, 48);
            this.btnMenuReportes.TabIndex = 8;
            this.btnMenuReportes.Text = "📈  Reportes y KPIs";
            this.btnMenuReportes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuReportes.UseVisualStyleBackColor = true;
            this.btnMenuReportes.Click += new System.EventHandler(this.btnMenuReportes_Click);
            // 
            // btnMenuUsuarios
            // 
            this.btnMenuUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuUsuarios.FlatAppearance.BorderSize = 0;
            this.btnMenuUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuUsuarios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuUsuarios.Location = new System.Drawing.Point(0, 353);
            this.btnMenuUsuarios.Name = "btnMenuUsuarios";
            this.btnMenuUsuarios.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuUsuarios.Size = new System.Drawing.Size(240, 48);
            this.btnMenuUsuarios.TabIndex = 7;
            this.btnMenuUsuarios.Text = "👥  Gestión de Usuarios";
            this.btnMenuUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuUsuarios.UseVisualStyleBackColor = true;
            this.btnMenuUsuarios.Click += new System.EventHandler(this.btnMenuUsuarios_Click);
            // 
            // btnMenuPenalizaciones
            // 
            this.btnMenuPenalizaciones.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuPenalizaciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuPenalizaciones.FlatAppearance.BorderSize = 0;
            this.btnMenuPenalizaciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuPenalizaciones.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuPenalizaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuPenalizaciones.Location = new System.Drawing.Point(0, 305);
            this.btnMenuPenalizaciones.Name = "btnMenuPenalizaciones";
            this.btnMenuPenalizaciones.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuPenalizaciones.Size = new System.Drawing.Size(240, 48);
            this.btnMenuPenalizaciones.TabIndex = 6;
            this.btnMenuPenalizaciones.Text = "⚠️  Penalizaciones";
            this.btnMenuPenalizaciones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuPenalizaciones.UseVisualStyleBackColor = true;
            this.btnMenuPenalizaciones.Click += new System.EventHandler(this.btnMenuPenalizaciones_Click);
            // 
            // btnMenuDevoluciones
            // 
            this.btnMenuDevoluciones.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuDevoluciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuDevoluciones.FlatAppearance.BorderSize = 0;
            this.btnMenuDevoluciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuDevoluciones.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuDevoluciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuDevoluciones.Location = new System.Drawing.Point(0, 257);
            this.btnMenuDevoluciones.Name = "btnMenuDevoluciones";
            this.btnMenuDevoluciones.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuDevoluciones.Size = new System.Drawing.Size(240, 48);
            this.btnMenuDevoluciones.TabIndex = 5;
            this.btnMenuDevoluciones.Text = "↩️  Devoluciones";
            this.btnMenuDevoluciones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuDevoluciones.UseVisualStyleBackColor = true;
            this.btnMenuDevoluciones.Click += new System.EventHandler(this.btnMenuDevoluciones_Click);
            // 
            // btnMenuPrestamos
            // 
            this.btnMenuPrestamos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuPrestamos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuPrestamos.FlatAppearance.BorderSize = 0;
            this.btnMenuPrestamos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuPrestamos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuPrestamos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuPrestamos.Location = new System.Drawing.Point(0, 209);
            this.btnMenuPrestamos.Name = "btnMenuPrestamos";
            this.btnMenuPrestamos.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuPrestamos.Size = new System.Drawing.Size(240, 48);
            this.btnMenuPrestamos.TabIndex = 4;
            this.btnMenuPrestamos.Text = "🔄  Préstamos y Solicitudes";
            this.btnMenuPrestamos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuPrestamos.UseVisualStyleBackColor = true;
            this.btnMenuPrestamos.Click += new System.EventHandler(this.btnMenuPrestamos_Click);
            // 
            // btnMenuCategorias
            // 
            this.btnMenuCategorias.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuCategorias.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuCategorias.FlatAppearance.BorderSize = 0;
            this.btnMenuCategorias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuCategorias.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuCategorias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuCategorias.Location = new System.Drawing.Point(0, 161);
            this.btnMenuCategorias.Name = "btnMenuCategorias";
            this.btnMenuCategorias.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuCategorias.Size = new System.Drawing.Size(240, 48);
            this.btnMenuCategorias.TabIndex = 3;
            this.btnMenuCategorias.Text = "🏷️  Categorías";
            this.btnMenuCategorias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuCategorias.UseVisualStyleBackColor = true;
            this.btnMenuCategorias.Click += new System.EventHandler(this.btnMenuCategorias_Click);
            // 
            // btnMenuCatalogo
            // 
            this.btnMenuCatalogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuCatalogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuCatalogo.FlatAppearance.BorderSize = 0;
            this.btnMenuCatalogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuCatalogo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuCatalogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnMenuCatalogo.Location = new System.Drawing.Point(0, 113);
            this.btnMenuCatalogo.Name = "btnMenuCatalogo";
            this.btnMenuCatalogo.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuCatalogo.Size = new System.Drawing.Size(240, 48);
            this.btnMenuCatalogo.TabIndex = 2;
            this.btnMenuCatalogo.Text = "📚  Gestión de Catálogo";
            this.btnMenuCatalogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuCatalogo.UseVisualStyleBackColor = true;
            this.btnMenuCatalogo.Click += new System.EventHandler(this.btnMenuCatalogo_Click);
            // 
            // btnMenuDashboard
            // 
            this.btnMenuDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnMenuDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenuDashboard.FlatAppearance.BorderSize = 0;
            this.btnMenuDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuDashboard.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuDashboard.ForeColor = System.Drawing.Color.White;
            this.btnMenuDashboard.Location = new System.Drawing.Point(0, 65);
            this.btnMenuDashboard.Name = "btnMenuDashboard";
            this.btnMenuDashboard.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenuDashboard.Size = new System.Drawing.Size(240, 48);
            this.btnMenuDashboard.TabIndex = 1;
            this.btnMenuDashboard.Text = "📊  Inicio / Dashboard";
            this.btnMenuDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenuDashboard.UseVisualStyleBackColor = false;
            this.btnMenuDashboard.Click += new System.EventHandler(this.btnMenuDashboard_Click);
            // 
            // pnlBrand
            // 
            this.pnlBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlBrand.Controls.Add(this.picSidebarLogo);
            this.pnlBrand.Controls.Add(this.lblBrandSub);
            this.pnlBrand.Controls.Add(this.lblBrandTitle);
            this.pnlBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBrand.Location = new System.Drawing.Point(0, 0);
            this.pnlBrand.Name = "pnlBrand";
            this.pnlBrand.Size = new System.Drawing.Size(240, 65);
            this.pnlBrand.TabIndex = 0;
            // 
            // picSidebarLogo
            // 
            this.picSidebarLogo.Location = new System.Drawing.Point(15, 13);
            this.picSidebarLogo.Name = "picSidebarLogo";
            this.picSidebarLogo.Size = new System.Drawing.Size(38, 38);
            this.picSidebarLogo.TabIndex = 2;
            this.picSidebarLogo.TabStop = false;
            // 
            // lblBrandSub
            // 
            this.lblBrandSub.AutoSize = true;
            this.lblBrandSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblBrandSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(197)))), ((int)(((byte)(253)))));
            this.lblBrandSub.Location = new System.Drawing.Point(61, 36);
            this.lblBrandSub.Name = "lblBrandSub";
            this.lblBrandSub.Size = new System.Drawing.Size(112, 13);
            this.lblBrandSub.TabIndex = 1;
            this.lblBrandSub.Text = "Gestión Bibliotecaria";
            // 
            // lblBrandTitle
            // 
            this.lblBrandTitle.AutoSize = true;
            this.lblBrandTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblBrandTitle.ForeColor = System.Drawing.Color.White;
            this.lblBrandTitle.Location = new System.Drawing.Point(59, 10);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new System.Drawing.Size(79, 28);
            this.lblBrandTitle.TabIndex = 0;
            this.lblBrandTitle.Text = "SIGEBI";
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlTopBar.Controls.Add(this.lblPageTitle);
            this.pnlTopBar.Controls.Add(this.lblUsuarioLogueado);
            this.pnlTopBar.Controls.Add(this.btnMinimize);
            this.pnlTopBar.Controls.Add(this.btnMaximize);
            this.pnlTopBar.Controls.Add(this.btnClose);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(240, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(860, 65);
            this.pnlTopBar.TabIndex = 1;
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.White;
            this.lblPageTitle.Location = new System.Drawing.Point(25, 18);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(150, 28);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Panel Principal";
            // 
            // lblUsuarioLogueado
            // 
            this.lblUsuarioLogueado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsuarioLogueado.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblUsuarioLogueado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblUsuarioLogueado.Location = new System.Drawing.Point(370, 22);
            this.lblUsuarioLogueado.Name = "lblUsuarioLogueado";
            this.lblUsuarioLogueado.Size = new System.Drawing.Size(350, 20);
            this.lblUsuarioLogueado.TabIndex = 1;
            this.lblUsuarioLogueado.Text = "Usuario: Cargando... | Rol: --";
            this.lblUsuarioLogueado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(738, 12);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(35, 30);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(775, 12);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(35, 30);
            this.btnMaximize.TabIndex = 3;
            this.btnMaximize.UseVisualStyleBackColor = true;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(812, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlContent.Controls.Add(this.pnlCard4);
            this.pnlContent.Controls.Add(this.pnlCard3);
            this.pnlContent.Controls.Add(this.pnlCard2);
            this.pnlContent.Controls.Add(this.pnlCard1);
            this.pnlContent.Controls.Add(this.lblWelcomeSub);
            this.pnlContent.Controls.Add(this.lblWelcomeHeader);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(240, 65);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(860, 615);
            this.pnlContent.TabIndex = 2;
            // 
            // pnlCard4
            // 
            this.pnlCard4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlCard4.Controls.Add(this.lblCard4Val);
            this.pnlCard4.Controls.Add(this.lblCard4Title);
            this.pnlCard4.Location = new System.Drawing.Point(642, 110);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.Size = new System.Drawing.Size(188, 100);
            this.pnlCard4.TabIndex = 5;
            // 
            // lblCard4Val
            // 
            this.lblCard4Val.AutoSize = true;
            this.lblCard4Val.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCard4Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.lblCard4Val.Location = new System.Drawing.Point(20, 45);
            this.lblCard4Val.Name = "lblCard4Val";
            this.lblCard4Val.Size = new System.Drawing.Size(32, 37);
            this.lblCard4Val.TabIndex = 1;
            this.lblCard4Val.Text = "3";
            // 
            // lblCard4Title
            // 
            this.lblCard4Title.AutoSize = true;
            this.lblCard4Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCard4Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCard4Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard4Title.Name = "lblCard4Title";
            this.lblCard4Title.Size = new System.Drawing.Size(130, 17);
            this.lblCard4Title.TabIndex = 0;
            this.lblCard4Title.Text = "Préstamos Vencidos";
            // 
            // pnlCard3
            // 
            this.pnlCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlCard3.Controls.Add(this.lblCard3Val);
            this.pnlCard3.Controls.Add(this.lblCard3Title);
            this.pnlCard3.Location = new System.Drawing.Point(438, 110);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.Size = new System.Drawing.Size(188, 100);
            this.pnlCard3.TabIndex = 4;
            // 
            // lblCard3Val
            // 
            this.lblCard3Val.AutoSize = true;
            this.lblCard3Val.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCard3Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(85)))), ((int)(((byte)(247)))));
            this.lblCard3Val.Location = new System.Drawing.Point(20, 45);
            this.lblCard3Val.Name = "lblCard3Val";
            this.lblCard3Val.Size = new System.Drawing.Size(65, 37);
            this.lblCard3Val.TabIndex = 1;
            this.lblCard3Val.Text = "142";
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCard3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCard3Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(126, 17);
            this.lblCard3Title.TabIndex = 0;
            this.lblCard3Title.Text = "Usuarios Registrados";
            // 
            // pnlCard2
            // 
            this.pnlCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlCard2.Controls.Add(this.lblCard2Val);
            this.pnlCard2.Controls.Add(this.lblCard2Title);
            this.pnlCard2.Location = new System.Drawing.Point(234, 110);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.Size = new System.Drawing.Size(188, 100);
            this.pnlCard2.TabIndex = 3;
            // 
            // lblCard2Val
            // 
            this.lblCard2Val.AutoSize = true;
            this.lblCard2Val.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCard2Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblCard2Val.Location = new System.Drawing.Point(20, 45);
            this.lblCard2Val.Name = "lblCard2Val";
            this.lblCard2Val.Size = new System.Drawing.Size(48, 37);
            this.lblCard2Val.TabIndex = 1;
            this.lblCard2Val.Text = "18";
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCard2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCard2Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(111, 17);
            this.lblCard2Title.TabIndex = 0;
            this.lblCard2Title.Text = "Préstamos Activos";
            // 
            // pnlCard1
            // 
            this.pnlCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlCard1.Controls.Add(this.lblCard1Val);
            this.pnlCard1.Controls.Add(this.lblCard1Title);
            this.pnlCard1.Location = new System.Drawing.Point(30, 110);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.Size = new System.Drawing.Size(188, 100);
            this.pnlCard1.TabIndex = 2;
            // 
            // lblCard1Val
            // 
            this.lblCard1Val.AutoSize = true;
            this.lblCard1Val.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCard1Val.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblCard1Val.Location = new System.Drawing.Point(20, 45);
            this.lblCard1Val.Name = "lblCard1Val";
            this.lblCard1Val.Size = new System.Drawing.Size(81, 37);
            this.lblCard1Val.TabIndex = 1;
            this.lblCard1Val.Text = "1,250";
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCard1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCard1Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(102, 17);
            this.lblCard1Title.TabIndex = 0;
            this.lblCard1Title.Text = "Total de Libros";
            // 
            // lblWelcomeSub
            // 
            this.lblWelcomeSub.AutoSize = true;
            this.lblWelcomeSub.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWelcomeSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblWelcomeSub.Location = new System.Drawing.Point(28, 62);
            this.lblWelcomeSub.Name = "lblWelcomeSub";
            this.lblWelcomeSub.Size = new System.Drawing.Size(434, 19);
            this.lblWelcomeSub.TabIndex = 1;
            this.lblWelcomeSub.Text = "Seleccione una opción del menú lateral para gestionar la biblioteca.";
            // 
            // lblWelcomeHeader
            // 
            this.lblWelcomeHeader.AutoSize = true;
            this.lblWelcomeHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblWelcomeHeader.ForeColor = System.Drawing.Color.White;
            this.lblWelcomeHeader.Location = new System.Drawing.Point(26, 25);
            this.lblWelcomeHeader.Name = "lblWelcomeHeader";
            this.lblWelcomeHeader.Size = new System.Drawing.Size(387, 32);
            this.lblWelcomeHeader.TabIndex = 0;
            this.lblWelcomeHeader.Text = "¡Bienvenido al Sistema SIGEBI!";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(1100, 680);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SIGEBI - Panel Principal";
            this.Load += new System.EventHandler(this.Main_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlBrand.ResumeLayout(false);
            this.pnlBrand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSidebarLogo)).EndInit();
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlCard4.ResumeLayout(false);
            this.pnlCard4.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlBrand;
        private System.Windows.Forms.Label lblBrandTitle;
        private System.Windows.Forms.Label lblBrandSub;
        private System.Windows.Forms.PictureBox picSidebarLogo;
        private System.Windows.Forms.Button btnMenuDashboard;
        private System.Windows.Forms.Button btnMenuCatalogo;
        private System.Windows.Forms.Button btnMenuCategorias;
        private System.Windows.Forms.Button btnMenuPrestamos;
        private System.Windows.Forms.Button btnMenuDevoluciones;
        private System.Windows.Forms.Button btnMenuPenalizaciones;
        private System.Windows.Forms.Button btnMenuUsuarios;
        private System.Windows.Forms.Button btnMenuReportes; // 👈 Campo privado para Reportes
        private System.Windows.Forms.Button btnMenuAuditoria;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblUsuarioLogueado;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblWelcomeHeader;
        private System.Windows.Forms.Label lblWelcomeSub;
        private System.Windows.Forms.Panel pnlCard1;
        private System.Windows.Forms.Label lblCard1Title;
        private System.Windows.Forms.Label lblCard1Val;
        private System.Windows.Forms.Panel pnlCard2;
        private System.Windows.Forms.Label lblCard2Title;
        private System.Windows.Forms.Label lblCard2Val;
        private System.Windows.Forms.Panel pnlCard3;
        private System.Windows.Forms.Label lblCard3Title;
        private System.Windows.Forms.Label lblCard3Val;
        private System.Windows.Forms.Panel pnlCard4;
        private System.Windows.Forms.Label lblCard4Title;
        private System.Windows.Forms.Label lblCard4Val;
    }
}