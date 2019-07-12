using Rac.VOne.Client.Common.Controls;

namespace Rac.VOne.Client.Screen
{
    partial class PA0101
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnMinimizeWindow = new System.Windows.Forms.Button();
            this.btnSearchCompany = new System.Windows.Forms.Button();
            this.btnSearchUser = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblAssemblyVersion = new System.Windows.Forms.Label();
            this.txtPassword = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtUserCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCompanyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtUserName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCompanyName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.btnExitApplication = new Rac.VOne.Client.Common.Controls.CircleNumButton2();
            this.btnChangePassword = new Rac.VOne.Client.Common.Controls.CircleNumButton2();
            this.btnLogin = new Rac.VOne.Client.Common.Controls.CircleNumButton2();
            this.pnlMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyName)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMinimizeWindow
            // 
            this.btnMinimizeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizeWindow.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeWindow.FlatAppearance.BorderSize = 0;
            this.btnMinimizeWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizeWindow.Image = global::Rac.VOne.Client.Screen.Properties.Resources.icon_minimize;
            this.btnMinimizeWindow.Location = new System.Drawing.Point(711, 2);
            this.btnMinimizeWindow.Name = "btnMinimizeWindow";
            this.btnMinimizeWindow.Size = new System.Drawing.Size(55, 23);
            this.btnMinimizeWindow.TabIndex = 0;
            this.btnMinimizeWindow.UseVisualStyleBackColor = false;
            this.btnMinimizeWindow.Click += new System.EventHandler(this.btnMinimizeWindow_Click);
            // 
            // btnSearchCompany
            // 
            this.btnSearchCompany.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearchCompany.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearchCompany.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchCompany.BackgroundImage = global::Rac.VOne.Client.Screen.Properties.Resources.icon_search;
            this.btnSearchCompany.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSearchCompany.FlatAppearance.BorderSize = 0;
            this.btnSearchCompany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchCompany.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSearchCompany.ForeColor = System.Drawing.Color.White;
            this.btnSearchCompany.Location = new System.Drawing.Point(618, 193);
            this.btnSearchCompany.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearchCompany.Name = "btnSearchCompany";
            this.btnSearchCompany.Size = new System.Drawing.Size(57, 57);
            this.btnSearchCompany.TabIndex = 1;
            this.btnSearchCompany.UseVisualStyleBackColor = false;
            this.btnSearchCompany.Click += new System.EventHandler(this.btnSearchCompany_Click);
            this.btnSearchCompany.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSearchUser_MouseDown);
            this.btnSearchCompany.MouseEnter += new System.EventHandler(this.btnSearchUser_MouseEnter);
            this.btnSearchCompany.MouseLeave += new System.EventHandler(this.btnSearchUser_MouseLeave);
            // 
            // btnSearchUser
            // 
            this.btnSearchUser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearchUser.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearchUser.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchUser.BackgroundImage = global::Rac.VOne.Client.Screen.Properties.Resources.icon_search;
            this.btnSearchUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSearchUser.FlatAppearance.BorderSize = 0;
            this.btnSearchUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchUser.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSearchUser.ForeColor = System.Drawing.Color.White;
            this.btnSearchUser.Location = new System.Drawing.Point(618, 260);
            this.btnSearchUser.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearchUser.Name = "btnSearchUser";
            this.btnSearchUser.Size = new System.Drawing.Size(57, 57);
            this.btnSearchUser.TabIndex = 3;
            this.btnSearchUser.UseVisualStyleBackColor = false;
            this.btnSearchUser.Click += new System.EventHandler(this.btnSearchUser_Click);
            this.btnSearchUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSearchUser_MouseDown);
            this.btnSearchUser.MouseEnter += new System.EventHandler(this.btnSearchUser_MouseEnter);
            this.btnSearchUser.MouseLeave += new System.EventHandler(this.btnSearchUser_MouseLeave);
            // 
            // picLogo
            // 
            this.picLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Enabled = false;
            this.picLogo.Image = global::Rac.VOne.Client.Screen.Properties.Resources.logo1;
            this.picLogo.Location = new System.Drawing.Point(97, 73);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(578, 104);
            this.picLogo.TabIndex = 10;
            this.picLogo.TabStop = false;
            // 
            // lblAssemblyVersion
            // 
            this.lblAssemblyVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAssemblyVersion.AutoSize = true;
            this.lblAssemblyVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAssemblyVersion.Font = new System.Drawing.Font("Meiryo UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAssemblyVersion.ForeColor = System.Drawing.Color.White;
            this.lblAssemblyVersion.Location = new System.Drawing.Point(13, 493);
            this.lblAssemblyVersion.Name = "lblAssemblyVersion";
            this.lblAssemblyVersion.Size = new System.Drawing.Size(87, 18);
            this.lblAssemblyVersion.TabIndex = 11;
            this.lblAssemblyVersion.Text = "Ver 1.2.3.4";
            // 
            // txtPassword
            // 
            this.txtPassword.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.txtPassword.AlternateText.DisplayNull.Text = "パスワード";
            this.txtPassword.AlternateText.Null.ForeColor = System.Drawing.Color.LightGray;
            this.txtPassword.AlternateText.Null.Text = "パスワード";
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPassword.DropDown.AllowDrop = false;
            this.txtPassword.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtPassword.HighlightText = true;
            this.txtPassword.Location = new System.Drawing.Point(97, 327);
            this.txtPassword.MaxLength = 15;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Required = false;
            this.gcShortcut1.SetShortcuts(this.txtPassword, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Return)))}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl"}));
            this.txtPassword.Size = new System.Drawing.Size(359, 57);
            this.txtPassword.TabIndex = 4;
            // 
            // txtUserCode
            // 
            this.txtUserCode.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.txtUserCode.AlternateText.DisplayNull.Text = "担当者コード";
            this.txtUserCode.AlternateText.Null.ForeColor = System.Drawing.Color.LightGray;
            this.txtUserCode.AlternateText.Null.Text = "担当者コード";
            this.txtUserCode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtUserCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtUserCode.DropDown.AllowDrop = false;
            this.txtUserCode.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtUserCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtUserCode.Format = "9A";
            this.txtUserCode.HighlightText = true;
            this.txtUserCode.Location = new System.Drawing.Point(97, 260);
            this.txtUserCode.MaxLength = 10;
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.txtUserCode.Required = false;
            this.gcShortcut1.SetShortcuts(this.txtUserCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Return)))}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl"}));
            this.txtUserCode.Size = new System.Drawing.Size(165, 57);
            this.txtUserCode.TabIndex = 2;
            this.txtUserCode.Validated += new System.EventHandler(this.txtUserCode_Validated);
            // 
            // txtCompanyCode
            // 
            this.txtCompanyCode.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.txtCompanyCode.AlternateText.DisplayNull.Text = "会社コード";
            this.txtCompanyCode.AlternateText.Null.ForeColor = System.Drawing.Color.LightGray;
            this.txtCompanyCode.AlternateText.Null.Text = "会社コード";
            this.txtCompanyCode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtCompanyCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCompanyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCompanyCode.DropDown.AllowDrop = false;
            this.txtCompanyCode.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtCompanyCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtCompanyCode.Format = "9A";
            this.txtCompanyCode.HighlightText = true;
            this.txtCompanyCode.Location = new System.Drawing.Point(97, 193);
            this.txtCompanyCode.MaxLength = 4;
            this.txtCompanyCode.Name = "txtCompanyCode";
            this.txtCompanyCode.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.txtCompanyCode.Required = false;
            this.gcShortcut1.SetShortcuts(this.txtCompanyCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Return)))}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl"}));
            this.txtCompanyCode.Size = new System.Drawing.Size(165, 57);
            this.txtCompanyCode.TabIndex = 0;
            this.txtCompanyCode.Validated += new System.EventHandler(this.txtCompanyCode_Validated);
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.txtUserName.DisabledBackColor = System.Drawing.SystemColors.Window;
            this.txtUserName.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtUserName.DropDown.AllowDrop = false;
            this.txtUserName.Enabled = false;
            this.txtUserName.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtUserName.HighlightText = true;
            this.txtUserName.Location = new System.Drawing.Point(262, 260);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Required = false;
            this.txtUserName.Size = new System.Drawing.Size(346, 57);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.TabStop = false;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCompanyName.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.txtCompanyName.DisabledBackColor = System.Drawing.SystemColors.Window;
            this.txtCompanyName.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtCompanyName.DropDown.AllowDrop = false;
            this.txtCompanyName.Enabled = false;
            this.txtCompanyName.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtCompanyName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.txtCompanyName.HighlightText = true;
            this.txtCompanyName.Location = new System.Drawing.Point(262, 193);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.Required = false;
            this.txtCompanyName.Size = new System.Drawing.Size(346, 57);
            this.txtCompanyName.TabIndex = 0;
            this.txtCompanyName.TabStop = false;
            // 
            // btnExitApplication
            // 
            this.btnExitApplication.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExitApplication.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExitApplication.BorderColor = System.Drawing.Color.White;
            this.btnExitApplication.BorderVisible = true;
            this.btnExitApplication.ButtonAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExitApplication.ButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(136)))), ((int)(((byte)(198)))));
            this.btnExitApplication.ButtonFont = new System.Drawing.Font("Meiryo UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnExitApplication.ButtonForeColor = System.Drawing.Color.White;
            this.btnExitApplication.ButtonSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(122)))), ((int)(((byte)(178)))));
            this.btnExitApplication.ButtonText = "終 了";
            this.btnExitApplication.CircleBackCokor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(136)))), ((int)(((byte)(198)))));
            this.btnExitApplication.CircleFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExitApplication.CircleForeColor = System.Drawing.Color.White;
            this.btnExitApplication.CirclePadding = new System.Windows.Forms.Padding(1, 2, 0, 0);
            this.btnExitApplication.CircleSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(122)))), ((int)(((byte)(178)))));
            this.btnExitApplication.CircleSize = 37;
            this.btnExitApplication.CircleSpace = 57;
            this.btnExitApplication.CircleText = "F10";
            this.btnExitApplication.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnExitApplication.Location = new System.Drawing.Point(467, 394);
            this.btnExitApplication.Margin = new System.Windows.Forms.Padding(0);
            this.btnExitApplication.Name = "btnExitApplication";
            this.btnExitApplication.Size = new System.Drawing.Size(208, 59);
            this.btnExitApplication.TabIndex = 7;
            this.btnExitApplication.Click += new System.EventHandler(this.btnExitApplication_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.AllowDrop = true;
            this.btnChangePassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChangePassword.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnChangePassword.BorderColor = System.Drawing.Color.White;
            this.btnChangePassword.BorderVisible = true;
            this.btnChangePassword.ButtonAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnChangePassword.ButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(136)))), ((int)(((byte)(198)))));
            this.btnChangePassword.ButtonFont = new System.Drawing.Font("Meiryo UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnChangePassword.ButtonForeColor = System.Drawing.Color.White;
            this.btnChangePassword.ButtonSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(122)))), ((int)(((byte)(178)))));
            this.btnChangePassword.ButtonText = "パスワード変更";
            this.btnChangePassword.CircleBackCokor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(136)))), ((int)(((byte)(198)))));
            this.btnChangePassword.CircleFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePassword.CircleForeColor = System.Drawing.Color.White;
            this.btnChangePassword.CirclePadding = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.btnChangePassword.CircleSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(122)))), ((int)(((byte)(178)))));
            this.btnChangePassword.CircleSize = 37;
            this.btnChangePassword.CircleSpace = 57;
            this.btnChangePassword.CircleText = "F7";
            this.btnChangePassword.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnChangePassword.Location = new System.Drawing.Point(467, 327);
            this.btnChangePassword.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(208, 57);
            this.btnChangePassword.TabIndex = 6;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLogin.BorderColor = System.Drawing.Color.White;
            this.btnLogin.BorderVisible = true;
            this.btnLogin.ButtonAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLogin.ButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(178)))), ((int)(((byte)(42)))));
            this.btnLogin.ButtonFont = new System.Drawing.Font("Meiryo UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLogin.ButtonForeColor = System.Drawing.Color.White;
            this.btnLogin.ButtonSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(213)))), ((int)(((byte)(50)))));
            this.btnLogin.ButtonText = "ログイン";
            this.btnLogin.CircleBackCokor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(178)))), ((int)(((byte)(42)))));
            this.btnLogin.CircleFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.CircleForeColor = System.Drawing.Color.White;
            this.btnLogin.CirclePadding = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.btnLogin.CircleSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(213)))), ((int)(((byte)(50)))));
            this.btnLogin.CircleSize = 37;
            this.btnLogin.CircleSpace = 57;
            this.btnLogin.CircleText = "F1";
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnLogin.ForeColor = System.Drawing.Color.Black;
            this.btnLogin.Location = new System.Drawing.Point(97, 394);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(359, 59);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlMain.BackgroundImage = global::Rac.VOne.Client.Screen.Properties.Resources.bg_login;
            this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlMain.Controls.Add(this.btnMinimizeWindow);
            this.pnlMain.Controls.Add(this.btnSearchCompany);
            this.pnlMain.Controls.Add(this.btnSearchUser);
            this.pnlMain.Controls.Add(this.btnLogin);
            this.pnlMain.Controls.Add(this.btnChangePassword);
            this.pnlMain.Controls.Add(this.btnExitApplication);
            this.pnlMain.Controls.Add(this.picLogo);
            this.pnlMain.Controls.Add(this.lblAssemblyVersion);
            this.pnlMain.Controls.Add(this.txtCompanyName);
            this.pnlMain.Controls.Add(this.txtUserName);
            this.pnlMain.Controls.Add(this.txtCompanyCode);
            this.pnlMain.Controls.Add(this.txtUserCode);
            this.pnlMain.Controls.Add(this.txtPassword);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(762, 527);
            this.pnlMain.TabIndex = 0;
            // 
            // PA0101
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(762, 527);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::Rac.VOne.Client.Screen.Properties.Resources.app_icon;
            this.Name = "PA0101";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Victory ONE G4";
            this.Load += new System.EventHandler(this.PA0101_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyName)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMinimizeWindow;
        private System.Windows.Forms.Button btnSearchCompany;
        private System.Windows.Forms.Button btnSearchUser;
        private CircleNumButton2 btnLogin;
        private CircleNumButton2 btnChangePassword;
        private CircleNumButton2 btnExitApplication;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblAssemblyVersion;
        private Common.Controls.VOneTextControl txtCompanyName;
        private Common.Controls.VOneTextControl txtUserName;
        private Common.Controls.VOneTextControl txtCompanyCode;
        private Common.Controls.VOneTextControl txtUserCode;
        private Common.Controls.VOneTextControl txtPassword;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnlMain;
    }
}