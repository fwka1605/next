namespace Rac.VOne.Client.Screen
{
    partial class PB1801
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
            this.lblBankCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBankName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBankKana = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBranchName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBranchKana = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtBranchCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblBranchCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtBankCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBankName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBankKana = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBranchName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBranchKana = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.gbxBankBranch = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchKana)).BeginInit();
            this.gbxBankBranch.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBankCode
            // 
            this.lblBankCode.Location = new System.Drawing.Point(55, 169);
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Size = new System.Drawing.Size(62, 16);
            this.lblBankCode.TabIndex = 0;
            this.lblBankCode.Text = "銀行コード";
            this.lblBankCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBankName
            // 
            this.lblBankName.Location = new System.Drawing.Point(55, 249);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new System.Drawing.Size(62, 16);
            this.lblBankName.TabIndex = 1;
            this.lblBankName.Text = "銀行名";
            this.lblBankName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBankKana
            // 
            this.lblBankKana.Location = new System.Drawing.Point(55, 283);
            this.lblBankKana.Name = "lblBankKana";
            this.lblBankKana.Size = new System.Drawing.Size(62, 16);
            this.lblBankKana.TabIndex = 2;
            this.lblBankKana.Text = "銀行名カナ";
            this.lblBankKana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBranchName
            // 
            this.lblBranchName.Location = new System.Drawing.Point(55, 329);
            this.lblBranchName.Name = "lblBranchName";
            this.lblBranchName.Size = new System.Drawing.Size(62, 16);
            this.lblBranchName.TabIndex = 3;
            this.lblBranchName.Text = "支店名";
            this.lblBranchName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBranchKana
            // 
            this.lblBranchKana.Location = new System.Drawing.Point(55, 363);
            this.lblBranchKana.Name = "lblBranchKana";
            this.lblBranchKana.Size = new System.Drawing.Size(62, 16);
            this.lblBranchKana.TabIndex = 4;
            this.lblBranchKana.Text = "支店名カナ";
            this.lblBranchKana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBranchCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBranchCode.DropDown.AllowDrop = false;
            this.txtBranchCode.Format = "9";
            this.txtBranchCode.HighlightText = true;
            this.txtBranchCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBranchCode.Location = new System.Drawing.Point(126, 200);
            this.txtBranchCode.Margin = new System.Windows.Forms.Padding(6, 6, 6, 12);
            this.txtBranchCode.MaxLength = 3;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Required = true;
            this.txtBranchCode.Size = new System.Drawing.Size(40, 22);
            this.txtBranchCode.TabIndex = 3;
            this.txtBranchCode.Validated += new System.EventHandler(this.txtBranchCode_Validated);
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.Location = new System.Drawing.Point(55, 201);
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Size = new System.Drawing.Size(62, 16);
            this.lblBranchCode.TabIndex = 7;
            this.lblBranchCode.Text = "支店コード";
            this.lblBranchCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBankCode
            // 
            this.txtBankCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBankCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBankCode.DropDown.AllowDrop = false;
            this.txtBankCode.Format = "9";
            this.txtBankCode.HighlightText = true;
            this.txtBankCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBankCode.Location = new System.Drawing.Point(126, 166);
            this.txtBankCode.Margin = new System.Windows.Forms.Padding(6);
            this.txtBankCode.MaxLength = 4;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Required = true;
            this.txtBankCode.Size = new System.Drawing.Size(40, 22);
            this.txtBankCode.TabIndex = 1;
            this.txtBankCode.Validated += new System.EventHandler(this.txtBankCode_Validated);
            // 
            // txtBankName
            // 
            this.txtBankName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBankName.DropDown.AllowDrop = false;
            this.txtBankName.HighlightText = true;
            this.txtBankName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtBankName.Location = new System.Drawing.Point(126, 246);
            this.txtBankName.Margin = new System.Windows.Forms.Padding(6, 12, 6, 6);
            this.txtBankName.MaxLength = 120;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Required = true;
            this.txtBankName.Size = new System.Drawing.Size(803, 22);
            this.txtBankName.TabIndex = 4;
            // 
            // txtBankKana
            // 
            this.txtBankKana.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBankKana.DropDown.AllowDrop = false;
            this.txtBankKana.Format = "9AN@";
            this.txtBankKana.HighlightText = true;
            this.txtBankKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtBankKana.Location = new System.Drawing.Point(126, 280);
            this.txtBankKana.Margin = new System.Windows.Forms.Padding(6, 6, 6, 12);
            this.txtBankKana.MaxLength = 120;
            this.txtBankKana.Name = "txtBankKana";
            this.txtBankKana.Required = false;
            this.txtBankKana.Size = new System.Drawing.Size(803, 22);
            this.txtBankKana.TabIndex = 5;
            // 
            // txtBranchName
            // 
            this.txtBranchName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBranchName.DropDown.AllowDrop = false;
            this.txtBranchName.HighlightText = true;
            this.txtBranchName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtBranchName.Location = new System.Drawing.Point(126, 326);
            this.txtBranchName.Margin = new System.Windows.Forms.Padding(6, 12, 6, 6);
            this.txtBranchName.MaxLength = 120;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Required = true;
            this.txtBranchName.Size = new System.Drawing.Size(803, 22);
            this.txtBranchName.TabIndex = 6;
            // 
            // txtBranchKana
            // 
            this.txtBranchKana.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBranchKana.DropDown.AllowDrop = false;
            this.txtBranchKana.Format = "9AN@";
            this.txtBranchKana.HighlightText = true;
            this.txtBranchKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtBranchKana.Location = new System.Drawing.Point(126, 360);
            this.txtBranchKana.Margin = new System.Windows.Forms.Padding(6);
            this.txtBranchKana.MaxLength = 120;
            this.txtBranchKana.Name = "txtBranchKana";
            this.txtBranchKana.Required = false;
            this.txtBranchKana.Size = new System.Drawing.Size(803, 22);
            this.txtBranchKana.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSearch.Location = new System.Drawing.Point(175, 165);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(24, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gbxBankBranch
            // 
            this.gbxBankBranch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbxBankBranch.Controls.Add(this.lblBranchName);
            this.gbxBankBranch.Controls.Add(this.lblBranchKana);
            this.gbxBankBranch.Controls.Add(this.lblBankName);
            this.gbxBankBranch.Controls.Add(this.txtBranchName);
            this.gbxBankBranch.Controls.Add(this.lblBankKana);
            this.gbxBankBranch.Controls.Add(this.txtBranchKana);
            this.gbxBankBranch.Controls.Add(this.lblBankCode);
            this.gbxBankBranch.Controls.Add(this.btnSearch);
            this.gbxBankBranch.Controls.Add(this.txtBankName);
            this.gbxBankBranch.Controls.Add(this.txtBankCode);
            this.gbxBankBranch.Controls.Add(this.txtBankKana);
            this.gbxBankBranch.Controls.Add(this.txtBranchCode);
            this.gbxBankBranch.Controls.Add(this.lblBranchCode);
            this.gbxBankBranch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxBankBranch.Location = new System.Drawing.Point(15, 12);
            this.gbxBankBranch.Name = "gbxBankBranch";
            this.gbxBankBranch.Size = new System.Drawing.Size(978, 591);
            this.gbxBankBranch.TabIndex = 1;
            this.gbxBankBranch.TabStop = false;
            // 
            // PB1801
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxBankBranch);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1801";
            this.Load += new System.EventHandler(this.PB1801_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchKana)).EndInit();
            this.gbxBankBranch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblBankCode;
        private Common.Controls.VOneLabelControl lblBankName;
        private Common.Controls.VOneLabelControl lblBankKana;
        private Common.Controls.VOneLabelControl lblBranchName;
        private Common.Controls.VOneLabelControl lblBranchKana;
        private Common.Controls.VOneTextControl txtBranchCode;
        private Common.Controls.VOneLabelControl lblBranchCode;
        private Common.Controls.VOneTextControl txtBankCode;
        private Common.Controls.VOneTextControl txtBankName;
        private Common.Controls.VOneTextControl txtBankKana;
        private Common.Controls.VOneTextControl txtBranchName;
        private Common.Controls.VOneTextControl txtBranchKana;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox gbxBankBranch;
    }
}