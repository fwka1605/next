namespace Rac.VOne.Client.Screen
{
    partial class PH1001
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
            this.gbxColumnNameList = new System.Windows.Forms.GroupBox();
            this.grdColumnName = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxColumnNameInput = new System.Windows.Forms.GroupBox();
            this.lblType = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOriginal = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblTableName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtAlias = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblAlias = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOriginalName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.gbxColumnNameList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdColumnName)).BeginInit();
            this.gbxColumnNameInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblTableName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOriginalName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxColumnNameList
            // 
            this.gbxColumnNameList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxColumnNameList.Controls.Add(this.grdColumnName);
            this.gbxColumnNameList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxColumnNameList.Location = new System.Drawing.Point(251, 24);
            this.gbxColumnNameList.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.gbxColumnNameList.Name = "gbxColumnNameList";
            this.gbxColumnNameList.Size = new System.Drawing.Size(506, 441);
            this.gbxColumnNameList.TabIndex = 1;
            this.gbxColumnNameList.TabStop = false;
            this.gbxColumnNameList.Text = "□　登録済みの項目名称";
            // 
            // grdColumnName
            // 
            this.grdColumnName.AllowAutoExtend = true;
            this.grdColumnName.AllowUserToAddRows = false;
            this.grdColumnName.AllowUserToShiftSelect = true;
            this.grdColumnName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdColumnName.Location = new System.Drawing.Point(18, 25);
            this.grdColumnName.Margin = new System.Windows.Forms.Padding(15, 6, 15, 12);
            this.grdColumnName.Name = "grdColumnName";
            this.grdColumnName.Size = new System.Drawing.Size(470, 401);
            this.grdColumnName.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdColumnName.TabIndex = 2;
            this.grdColumnName.Text = "vOneGridControl1";
            this.grdColumnName.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdColumnName_CellDoubleClick);
            // 
            // gbxColumnNameInput
            // 
            this.gbxColumnNameInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxColumnNameInput.Controls.Add(this.lblType);
            this.gbxColumnNameInput.Controls.Add(this.lblOriginal);
            this.gbxColumnNameInput.Controls.Add(this.lblTableName);
            this.gbxColumnNameInput.Controls.Add(this.txtAlias);
            this.gbxColumnNameInput.Controls.Add(this.lblAlias);
            this.gbxColumnNameInput.Controls.Add(this.lblOriginalName);
            this.gbxColumnNameInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxColumnNameInput.Location = new System.Drawing.Point(251, 471);
            this.gbxColumnNameInput.Name = "gbxColumnNameInput";
            this.gbxColumnNameInput.Size = new System.Drawing.Size(506, 58);
            this.gbxColumnNameInput.TabIndex = 3;
            this.gbxColumnNameInput.TabStop = false;
            // 
            // lblType
            // 
            this.lblType.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(12, 24);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(32, 16);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "種別";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOriginal
            // 
            this.lblOriginal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOriginal.Location = new System.Drawing.Point(113, 24);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(44, 16);
            this.lblOriginal.TabIndex = 0;
            this.lblOriginal.Text = "項目名";
            this.lblOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTableName
            // 
            this.lblTableName.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTableName.DropDown.AllowDrop = false;
            this.lblTableName.Enabled = false;
            this.lblTableName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableName.HighlightText = true;
            this.lblTableName.Location = new System.Drawing.Point(50, 22);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.ReadOnly = true;
            this.lblTableName.Required = false;
            this.lblTableName.Size = new System.Drawing.Size(57, 22);
            this.lblTableName.TabIndex = 0;
            // 
            // txtAlias
            // 
            this.txtAlias.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAlias.DropDown.AllowDrop = false;
            this.txtAlias.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlias.HighlightText = true;
            this.txtAlias.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtAlias.Location = new System.Drawing.Point(370, 22);
            this.txtAlias.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.txtAlias.MaxLength = 10;
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Required = false;
            this.txtAlias.Size = new System.Drawing.Size(127, 22);
            this.txtAlias.TabIndex = 5;
            // 
            // lblAlias
            // 
            this.lblAlias.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlias.Location = new System.Drawing.Point(296, 24);
            this.lblAlias.Name = "lblAlias";
            this.lblAlias.Size = new System.Drawing.Size(68, 16);
            this.lblAlias.TabIndex = 0;
            this.lblAlias.Text = "変更後名称";
            this.lblAlias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOriginalName
            // 
            this.lblOriginalName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOriginalName.DropDown.AllowDrop = false;
            this.lblOriginalName.Enabled = false;
            this.lblOriginalName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOriginalName.HighlightText = true;
            this.lblOriginalName.Location = new System.Drawing.Point(163, 22);
            this.lblOriginalName.Name = "lblOriginalName";
            this.lblOriginalName.ReadOnly = true;
            this.lblOriginalName.Required = false;
            this.lblOriginalName.Size = new System.Drawing.Size(127, 22);
            this.lblOriginalName.TabIndex = 0;
            // 
            // PH1001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxColumnNameList);
            this.Controls.Add(this.gbxColumnNameInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH1001";
            this.Load += new System.EventHandler(this.PH1001_Load);
            this.gbxColumnNameList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdColumnName)).EndInit();
            this.gbxColumnNameInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblTableName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOriginalName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxColumnNameInput;
        private Common.Controls.VOneLabelControl lblType;
        private Common.Controls.VOneDispLabelControl lblTableName;
        private Common.Controls.VOneLabelControl lblOriginal;
        private Common.Controls.VOneDispLabelControl lblOriginalName;
        private Common.Controls.VOneLabelControl lblAlias;
        private Common.Controls.VOneTextControl txtAlias;
        private System.Windows.Forms.GroupBox gbxColumnNameList;
        private Common.Controls.VOneGridControl grdColumnName;
    }
}
