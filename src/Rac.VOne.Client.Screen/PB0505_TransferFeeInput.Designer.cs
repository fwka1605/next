namespace Rac.VOne.Client.Screen
{
    partial class PB0505
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            this.pnlCurrency = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.pnlCurrency.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Location = new System.Drawing.Point(71, 8);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(58, 16);
            this.lblCurrencyCode.TabIndex = 4;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(135, 6);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(115, 22);
            this.txtCurrencyCode.TabIndex = 5;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToResize = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(12, 47);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(381, 398);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 7;
            this.grid.Text = "vOneGridControl1";
            // 
            // btnCurrencyCode
            // 
            this.btnCurrencyCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrencyCode.Location = new System.Drawing.Point(256, 5);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 6;
            this.btnCurrencyCode.UseVisualStyleBackColor = false;
            // 
            // pnlCurrency
            // 
            this.pnlCurrency.Controls.Add(this.txtCurrencyCode);
            this.pnlCurrency.Controls.Add(this.lblCurrencyCode);
            this.pnlCurrency.Controls.Add(this.btnCurrencyCode);
            this.pnlCurrency.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCurrency.Location = new System.Drawing.Point(12, 12);
            this.pnlCurrency.Name = "pnlCurrency";
            this.pnlCurrency.Size = new System.Drawing.Size(381, 35);
            this.pnlCurrency.TabIndex = 8;
            // 
            // PB0505
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grid);
            this.Controls.Add(this.pnlCurrency);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PB0505";
            this.Size = new System.Drawing.Size(405, 457);
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.pnlCurrency.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrencyCode;
        private Common.Controls.VOneGridControl grid;
        private System.Windows.Forms.Panel pnlCurrency;
    }
}
