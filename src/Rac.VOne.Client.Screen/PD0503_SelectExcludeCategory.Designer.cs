﻿namespace Rac.VOne.Client.Screen
{
    partial class PD0503
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
            this.cmbCategoryFlag = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblCaption = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCategoryFlag)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCategoryFlag
            // 
            this.cmbCategoryFlag.DisplayMember = "";
            this.cmbCategoryFlag.DropDown.AllowResize = false;
            this.cmbCategoryFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFlag.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbCategoryFlag.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbCategoryFlag.ListHeaderPane.Height = 22;
            this.cmbCategoryFlag.ListHeaderPane.Visible = false;
            this.cmbCategoryFlag.Location = new System.Drawing.Point(17, 56);
            this.cmbCategoryFlag.Name = "cmbCategoryFlag";
            this.cmbCategoryFlag.Required = false;
            this.cmbCategoryFlag.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbCategoryFlag.Size = new System.Drawing.Size(294, 22);
            this.cmbCategoryFlag.TabIndex = 3;
            this.cmbCategoryFlag.ValueMember = "";
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblCaption
            // 
            this.lblCaption.Location = new System.Drawing.Point(17, 16);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(222, 16);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "対象外区分を選択してください";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PD0503
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.cmbCategoryFlag);
            this.Controls.Add(this.lblCaption);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PD0503";
            this.Size = new System.Drawing.Size(340, 130);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCategoryFlag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneComboControl cmbCategoryFlag;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblCaption;
    }
}
