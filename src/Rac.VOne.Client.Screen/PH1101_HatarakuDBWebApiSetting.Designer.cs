namespace Rac.VOne.Client.Screen
{
    partial class PH1101
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
            this.gbxBaseSettings = new System.Windows.Forms.GroupBox();
            this.lblApiVersion = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtAccessToken = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtApiVersion = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBaseUri = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblBaseUri = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAccessToken = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxExtractSettings = new System.Windows.Forms.GroupBox();
            this.txtExtractListId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtExtractSearchId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtExtractDbSchemaId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblImporterPatternName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblImporterPattern = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtImporterPatternCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnImporterPattern = new System.Windows.Forms.Button();
            this.lblExtractListId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractSearchId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractListId2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractSearchId2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractDbSchemaId2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractDbSchemaId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxOutputSettings = new System.Windows.Forms.GroupBox();
            this.txtOutputImportId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtOutputDbSchemaId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblOutputDbSchemaId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputDbSchemaId2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputImportId2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputImportId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtResponse = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gbxResponse = new System.Windows.Forms.GroupBox();
            this.gbxBaseSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccessToken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApiVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseUri)).BeginInit();
            this.gbxExtractSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtractListId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtractSearchId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtractDbSchemaId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblImporterPatternName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImporterPatternCode)).BeginInit();
            this.gbxOutputSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputImportId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputDbSchemaId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResponse)).BeginInit();
            this.gbxResponse.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxBaseSettings
            // 
            this.gbxBaseSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxBaseSettings.Controls.Add(this.lblApiVersion);
            this.gbxBaseSettings.Controls.Add(this.txtAccessToken);
            this.gbxBaseSettings.Controls.Add(this.txtApiVersion);
            this.gbxBaseSettings.Controls.Add(this.txtBaseUri);
            this.gbxBaseSettings.Controls.Add(this.lblBaseUri);
            this.gbxBaseSettings.Controls.Add(this.lblAccessToken);
            this.gbxBaseSettings.Location = new System.Drawing.Point(15, 15);
            this.gbxBaseSettings.Name = "gbxBaseSettings";
            this.gbxBaseSettings.Size = new System.Drawing.Size(967, 83);
            this.gbxBaseSettings.TabIndex = 0;
            this.gbxBaseSettings.TabStop = false;
            this.gbxBaseSettings.Text = "基本設定";
            // 
            // lblApiVersion
            // 
            this.lblApiVersion.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblApiVersion.Location = new System.Drawing.Point(711, 50);
            this.lblApiVersion.Name = "lblApiVersion";
            this.lblApiVersion.Size = new System.Drawing.Size(83, 22);
            this.lblApiVersion.TabIndex = 0;
            this.lblApiVersion.Text = "APIバージョン";
            this.lblApiVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtAccessToken.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAccessToken.DropDown.AllowDrop = false;
            this.txtAccessToken.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtAccessToken.Format = "Aa9";
            this.txtAccessToken.HighlightText = true;
            this.txtAccessToken.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtAccessToken.Location = new System.Drawing.Point(240, 22);
            this.txtAccessToken.MaxLength = 100;
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.Required = true;
            this.txtAccessToken.Size = new System.Drawing.Size(721, 22);
            this.txtAccessToken.TabIndex = 0;
            // 
            // txtApiVersion
            // 
            this.txtApiVersion.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtApiVersion.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtApiVersion.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtApiVersion.DropDown.AllowDrop = false;
            this.txtApiVersion.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtApiVersion.Format = "a9";
            this.txtApiVersion.HighlightText = true;
            this.txtApiVersion.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtApiVersion.Location = new System.Drawing.Point(800, 50);
            this.txtApiVersion.MaxLength = 5;
            this.txtApiVersion.Name = "txtApiVersion";
            this.txtApiVersion.Required = true;
            this.txtApiVersion.Size = new System.Drawing.Size(161, 22);
            this.txtApiVersion.TabIndex = 2;
            this.txtApiVersion.TabStop = false;
            this.txtApiVersion.Text = "v1";
            // 
            // txtBaseUri
            // 
            this.txtBaseUri.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBaseUri.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtBaseUri.AlternateText.DisplayNull.Text = "https://xx.htdb.jp/xxxxxxx/ まで入力してください。";
            this.txtBaseUri.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBaseUri.DropDown.AllowDrop = false;
            this.txtBaseUri.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBaseUri.Format = "Aa@9";
            this.txtBaseUri.HighlightText = true;
            this.txtBaseUri.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBaseUri.Location = new System.Drawing.Point(240, 50);
            this.txtBaseUri.MaxLength = 100;
            this.txtBaseUri.Name = "txtBaseUri";
            this.txtBaseUri.Required = true;
            this.txtBaseUri.Size = new System.Drawing.Size(438, 22);
            this.txtBaseUri.TabIndex = 1;
            // 
            // lblBaseUri
            // 
            this.lblBaseUri.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBaseUri.Location = new System.Drawing.Point(15, 50);
            this.lblBaseUri.Name = "lblBaseUri";
            this.lblBaseUri.Size = new System.Drawing.Size(130, 22);
            this.lblBaseUri.TabIndex = 0;
            this.lblBaseUri.Text = "働くDB URL 設定";
            this.lblBaseUri.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAccessToken
            // 
            this.lblAccessToken.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAccessToken.Location = new System.Drawing.Point(15, 22);
            this.lblAccessToken.Name = "lblAccessToken";
            this.lblAccessToken.Size = new System.Drawing.Size(130, 22);
            this.lblAccessToken.TabIndex = 0;
            this.lblAccessToken.Text = "アクセストークン";
            this.lblAccessToken.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxExtractSettings
            // 
            this.gbxExtractSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxExtractSettings.Controls.Add(this.txtExtractListId);
            this.gbxExtractSettings.Controls.Add(this.txtExtractSearchId);
            this.gbxExtractSettings.Controls.Add(this.txtExtractDbSchemaId);
            this.gbxExtractSettings.Controls.Add(this.lblImporterPatternName);
            this.gbxExtractSettings.Controls.Add(this.lblImporterPattern);
            this.gbxExtractSettings.Controls.Add(this.txtImporterPatternCode);
            this.gbxExtractSettings.Controls.Add(this.btnImporterPattern);
            this.gbxExtractSettings.Controls.Add(this.lblExtractListId);
            this.gbxExtractSettings.Controls.Add(this.lblExtractSearchId);
            this.gbxExtractSettings.Controls.Add(this.lblExtractListId2);
            this.gbxExtractSettings.Controls.Add(this.lblExtractSearchId2);
            this.gbxExtractSettings.Controls.Add(this.lblExtractDbSchemaId2);
            this.gbxExtractSettings.Controls.Add(this.lblExtractDbSchemaId);
            this.gbxExtractSettings.Location = new System.Drawing.Point(15, 104);
            this.gbxExtractSettings.Name = "gbxExtractSettings";
            this.gbxExtractSettings.Size = new System.Drawing.Size(554, 136);
            this.gbxExtractSettings.TabIndex = 1;
            this.gbxExtractSettings.TabStop = false;
            this.gbxExtractSettings.Text = "請求データ取込設定";
            // 
            // txtExtractListId
            // 
            this.txtExtractListId.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtExtractListId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtExtractListId.DropDown.AllowDrop = false;
            this.txtExtractListId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtExtractListId.Format = "9";
            this.txtExtractListId.HighlightText = true;
            this.txtExtractListId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtExtractListId.Location = new System.Drawing.Point(240, 78);
            this.txtExtractListId.MaxLength = 10;
            this.txtExtractListId.Name = "txtExtractListId";
            this.txtExtractListId.Required = false;
            this.txtExtractListId.Size = new System.Drawing.Size(158, 22);
            this.txtExtractListId.TabIndex = 2;
            // 
            // txtExtractSearchId
            // 
            this.txtExtractSearchId.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtExtractSearchId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtExtractSearchId.DropDown.AllowDrop = false;
            this.txtExtractSearchId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtExtractSearchId.Format = "9";
            this.txtExtractSearchId.HighlightText = true;
            this.txtExtractSearchId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtExtractSearchId.Location = new System.Drawing.Point(240, 50);
            this.txtExtractSearchId.MaxLength = 10;
            this.txtExtractSearchId.Name = "txtExtractSearchId";
            this.txtExtractSearchId.Required = false;
            this.txtExtractSearchId.Size = new System.Drawing.Size(158, 22);
            this.txtExtractSearchId.TabIndex = 1;
            // 
            // txtExtractDbSchemaId
            // 
            this.txtExtractDbSchemaId.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtExtractDbSchemaId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtExtractDbSchemaId.DropDown.AllowDrop = false;
            this.txtExtractDbSchemaId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtExtractDbSchemaId.Format = "9";
            this.txtExtractDbSchemaId.HighlightText = true;
            this.txtExtractDbSchemaId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtExtractDbSchemaId.Location = new System.Drawing.Point(240, 22);
            this.txtExtractDbSchemaId.MaxLength = 10;
            this.txtExtractDbSchemaId.Name = "txtExtractDbSchemaId";
            this.txtExtractDbSchemaId.Required = true;
            this.txtExtractDbSchemaId.Size = new System.Drawing.Size(158, 22);
            this.txtExtractDbSchemaId.TabIndex = 0;
            // 
            // lblImporterPatternName
            // 
            this.lblImporterPatternName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblImporterPatternName.DropDown.AllowDrop = false;
            this.lblImporterPatternName.Enabled = false;
            this.lblImporterPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImporterPatternName.HighlightText = true;
            this.lblImporterPatternName.Location = new System.Drawing.Point(306, 106);
            this.lblImporterPatternName.Name = "lblImporterPatternName";
            this.lblImporterPatternName.ReadOnly = true;
            this.lblImporterPatternName.Required = false;
            this.lblImporterPatternName.Size = new System.Drawing.Size(242, 22);
            this.lblImporterPatternName.TabIndex = 6;
            // 
            // lblImporterPattern
            // 
            this.lblImporterPattern.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImporterPattern.Location = new System.Drawing.Point(15, 105);
            this.lblImporterPattern.Name = "lblImporterPattern";
            this.lblImporterPattern.Size = new System.Drawing.Size(130, 22);
            this.lblImporterPattern.TabIndex = 0;
            this.lblImporterPattern.Text = "請求パターン";
            this.lblImporterPattern.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtImporterPatternCode
            // 
            this.txtImporterPatternCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtImporterPatternCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtImporterPatternCode.DropDown.AllowDrop = false;
            this.txtImporterPatternCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtImporterPatternCode.Format = "9";
            this.txtImporterPatternCode.HighlightText = true;
            this.txtImporterPatternCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtImporterPatternCode.Location = new System.Drawing.Point(240, 106);
            this.txtImporterPatternCode.MaxLength = 2;
            this.txtImporterPatternCode.Name = "txtImporterPatternCode";
            this.txtImporterPatternCode.Required = true;
            this.txtImporterPatternCode.Size = new System.Drawing.Size(30, 22);
            this.txtImporterPatternCode.TabIndex = 4;
            // 
            // btnImporterPattern
            // 
            this.btnImporterPattern.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnImporterPattern.Location = new System.Drawing.Point(276, 105);
            this.btnImporterPattern.Name = "btnImporterPattern";
            this.btnImporterPattern.Size = new System.Drawing.Size(24, 24);
            this.btnImporterPattern.TabIndex = 5;
            this.btnImporterPattern.UseVisualStyleBackColor = true;
            // 
            // lblExtractListId
            // 
            this.lblExtractListId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractListId.Location = new System.Drawing.Point(15, 78);
            this.lblExtractListId.Name = "lblExtractListId";
            this.lblExtractListId.Size = new System.Drawing.Size(130, 22);
            this.lblExtractListId.TabIndex = 0;
            this.lblExtractListId.Text = "レコード一覧画面設定ID";
            this.lblExtractListId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractSearchId
            // 
            this.lblExtractSearchId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractSearchId.Location = new System.Drawing.Point(15, 49);
            this.lblExtractSearchId.Name = "lblExtractSearchId";
            this.lblExtractSearchId.Size = new System.Drawing.Size(130, 22);
            this.lblExtractSearchId.TabIndex = 0;
            this.lblExtractSearchId.Text = "絞込みID";
            this.lblExtractSearchId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractListId2
            // 
            this.lblExtractListId2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractListId2.Location = new System.Drawing.Point(151, 78);
            this.lblExtractListId2.Name = "lblExtractListId2";
            this.lblExtractListId2.Size = new System.Drawing.Size(83, 22);
            this.lblExtractListId2.TabIndex = 0;
            this.lblExtractListId2.Text = "listId";
            this.lblExtractListId2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractSearchId2
            // 
            this.lblExtractSearchId2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractSearchId2.Location = new System.Drawing.Point(151, 51);
            this.lblExtractSearchId2.Name = "lblExtractSearchId2";
            this.lblExtractSearchId2.Size = new System.Drawing.Size(83, 22);
            this.lblExtractSearchId2.TabIndex = 0;
            this.lblExtractSearchId2.Text = "searchId";
            this.lblExtractSearchId2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractDbSchemaId2
            // 
            this.lblExtractDbSchemaId2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractDbSchemaId2.Location = new System.Drawing.Point(151, 22);
            this.lblExtractDbSchemaId2.Name = "lblExtractDbSchemaId2";
            this.lblExtractDbSchemaId2.Size = new System.Drawing.Size(83, 22);
            this.lblExtractDbSchemaId2.TabIndex = 0;
            this.lblExtractDbSchemaId2.Text = "dbSchemaId";
            this.lblExtractDbSchemaId2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractDbSchemaId
            // 
            this.lblExtractDbSchemaId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractDbSchemaId.Location = new System.Drawing.Point(15, 22);
            this.lblExtractDbSchemaId.Name = "lblExtractDbSchemaId";
            this.lblExtractDbSchemaId.Size = new System.Drawing.Size(130, 22);
            this.lblExtractDbSchemaId.TabIndex = 0;
            this.lblExtractDbSchemaId.Text = "データベースID";
            this.lblExtractDbSchemaId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxOutputSettings
            // 
            this.gbxOutputSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxOutputSettings.Controls.Add(this.txtOutputImportId);
            this.gbxOutputSettings.Controls.Add(this.txtOutputDbSchemaId);
            this.gbxOutputSettings.Controls.Add(this.lblOutputDbSchemaId);
            this.gbxOutputSettings.Controls.Add(this.lblOutputDbSchemaId2);
            this.gbxOutputSettings.Controls.Add(this.lblOutputImportId2);
            this.gbxOutputSettings.Controls.Add(this.lblOutputImportId);
            this.gbxOutputSettings.Location = new System.Drawing.Point(575, 104);
            this.gbxOutputSettings.Name = "gbxOutputSettings";
            this.gbxOutputSettings.Size = new System.Drawing.Size(407, 136);
            this.gbxOutputSettings.TabIndex = 2;
            this.gbxOutputSettings.TabStop = false;
            this.gbxOutputSettings.Text = "消込データ連携設定";
            // 
            // txtOutputImportId
            // 
            this.txtOutputImportId.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtOutputImportId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtOutputImportId.DropDown.AllowDrop = false;
            this.txtOutputImportId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtOutputImportId.Format = "9";
            this.txtOutputImportId.HighlightText = true;
            this.txtOutputImportId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtOutputImportId.Location = new System.Drawing.Point(240, 50);
            this.txtOutputImportId.MaxLength = 10;
            this.txtOutputImportId.Name = "txtOutputImportId";
            this.txtOutputImportId.Required = true;
            this.txtOutputImportId.Size = new System.Drawing.Size(158, 22);
            this.txtOutputImportId.TabIndex = 1;
            // 
            // txtOutputDbSchemaId
            // 
            this.txtOutputDbSchemaId.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtOutputDbSchemaId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtOutputDbSchemaId.DropDown.AllowDrop = false;
            this.txtOutputDbSchemaId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtOutputDbSchemaId.Format = "9";
            this.txtOutputDbSchemaId.HighlightText = true;
            this.txtOutputDbSchemaId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtOutputDbSchemaId.Location = new System.Drawing.Point(240, 22);
            this.txtOutputDbSchemaId.MaxLength = 10;
            this.txtOutputDbSchemaId.Name = "txtOutputDbSchemaId";
            this.txtOutputDbSchemaId.Required = true;
            this.txtOutputDbSchemaId.Size = new System.Drawing.Size(158, 22);
            this.txtOutputDbSchemaId.TabIndex = 0;
            // 
            // lblOutputDbSchemaId
            // 
            this.lblOutputDbSchemaId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOutputDbSchemaId.Location = new System.Drawing.Point(15, 22);
            this.lblOutputDbSchemaId.Name = "lblOutputDbSchemaId";
            this.lblOutputDbSchemaId.Size = new System.Drawing.Size(130, 22);
            this.lblOutputDbSchemaId.TabIndex = 0;
            this.lblOutputDbSchemaId.Text = "データベースID";
            this.lblOutputDbSchemaId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputDbSchemaId2
            // 
            this.lblOutputDbSchemaId2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOutputDbSchemaId2.Location = new System.Drawing.Point(151, 22);
            this.lblOutputDbSchemaId2.Name = "lblOutputDbSchemaId2";
            this.lblOutputDbSchemaId2.Size = new System.Drawing.Size(83, 22);
            this.lblOutputDbSchemaId2.TabIndex = 0;
            this.lblOutputDbSchemaId2.Text = "dbSchemaId";
            this.lblOutputDbSchemaId2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputImportId2
            // 
            this.lblOutputImportId2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOutputImportId2.Location = new System.Drawing.Point(151, 51);
            this.lblOutputImportId2.Name = "lblOutputImportId2";
            this.lblOutputImportId2.Size = new System.Drawing.Size(83, 22);
            this.lblOutputImportId2.TabIndex = 0;
            this.lblOutputImportId2.Text = "importId";
            this.lblOutputImportId2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputImportId
            // 
            this.lblOutputImportId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOutputImportId.Location = new System.Drawing.Point(15, 49);
            this.lblOutputImportId.Name = "lblOutputImportId";
            this.lblOutputImportId.Size = new System.Drawing.Size(130, 22);
            this.lblOutputImportId.TabIndex = 0;
            this.lblOutputImportId.Text = "インポート設定ID";
            this.lblOutputImportId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtResponse
            // 
            this.txtResponse.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtResponse.AlternateText.DisplayNull.Text = "請求データ取込設定でテスト接続を行います。詳細は働くDB のステータスコードを参照してください";
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponse.DropDown.AllowDrop = false;
            this.txtResponse.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtResponse.HighlightText = true;
            this.txtResponse.IgnoreFocusChange = true;
            this.txtResponse.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtResponse.Location = new System.Drawing.Point(3, 19);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.Required = false;
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponse.Size = new System.Drawing.Size(961, 338);
            this.txtResponse.TabIndex = 3;
            // 
            // gbxResponse
            // 
            this.gbxResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResponse.Controls.Add(this.txtResponse);
            this.gbxResponse.Location = new System.Drawing.Point(15, 246);
            this.gbxResponse.Name = "gbxResponse";
            this.gbxResponse.Size = new System.Drawing.Size(967, 360);
            this.gbxResponse.TabIndex = 4;
            this.gbxResponse.TabStop = false;
            this.gbxResponse.Text = "テスト接続結果";
            // 
            // PH1101
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxResponse);
            this.Controls.Add(this.gbxOutputSettings);
            this.Controls.Add(this.gbxExtractSettings);
            this.Controls.Add(this.gbxBaseSettings);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PH1101";
            this.gbxBaseSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtAccessToken)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApiVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseUri)).EndInit();
            this.gbxExtractSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtExtractListId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtractSearchId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtractDbSchemaId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblImporterPatternName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImporterPatternCode)).EndInit();
            this.gbxOutputSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputImportId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputDbSchemaId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResponse)).EndInit();
            this.gbxResponse.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxBaseSettings;
        private System.Windows.Forms.GroupBox gbxExtractSettings;
        private System.Windows.Forms.GroupBox gbxOutputSettings;
        private Common.Controls.VOneTextControl txtAccessToken;
        private Common.Controls.VOneTextControl txtBaseUri;
        private Common.Controls.VOneLabelControl lblBaseUri;
        private Common.Controls.VOneLabelControl lblAccessToken;
        private Common.Controls.VOneTextControl txtExtractListId;
        private Common.Controls.VOneTextControl txtExtractSearchId;
        private Common.Controls.VOneTextControl txtExtractDbSchemaId;
        private Common.Controls.VOneDispLabelControl lblImporterPatternName;
        private Common.Controls.VOneLabelControl lblImporterPattern;
        private Common.Controls.VOneTextControl txtImporterPatternCode;
        private System.Windows.Forms.Button btnImporterPattern;
        private Common.Controls.VOneLabelControl lblExtractListId;
        private Common.Controls.VOneLabelControl lblExtractSearchId;
        private Common.Controls.VOneLabelControl lblExtractDbSchemaId;
        private Common.Controls.VOneTextControl txtOutputImportId;
        private Common.Controls.VOneTextControl txtOutputDbSchemaId;
        private Common.Controls.VOneLabelControl lblExtractListId2;
        private Common.Controls.VOneLabelControl lblExtractSearchId2;
        private Common.Controls.VOneLabelControl lblExtractDbSchemaId2;
        private Common.Controls.VOneLabelControl lblOutputDbSchemaId;
        private Common.Controls.VOneLabelControl lblOutputDbSchemaId2;
        private Common.Controls.VOneLabelControl lblOutputImportId2;
        private Common.Controls.VOneLabelControl lblOutputImportId;
        private Common.Controls.VOneTextControl txtResponse;
        private System.Windows.Forms.GroupBox gbxResponse;
        private Common.Controls.VOneLabelControl lblApiVersion;
        private Common.Controls.VOneTextControl txtApiVersion;
    }
}
