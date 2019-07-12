namespace Rac.VOne.Client.Screen
{
    partial class PH1201
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
            this.gbxResponse = new System.Windows.Forms.GroupBox();
            this.gbxBaseSettings = new System.Windows.Forms.GroupBox();
            this.txtResponse = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblApiVersion = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtClientSecret = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtAccessToken = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtRefreshToken = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtClientId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtApiVersion = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBaseUri = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblBaseUri = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblClientSecret = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRefreshToken = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAccessToken = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblClientId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxResponse.SuspendLayout();
            this.gbxBaseSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtResponse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientSecret)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccessToken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefreshToken)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApiVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseUri)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxResponse
            // 
            this.gbxResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResponse.Controls.Add(this.txtResponse);
            this.gbxResponse.Location = new System.Drawing.Point(15, 190);
            this.gbxResponse.Name = "gbxResponse";
            this.gbxResponse.Size = new System.Drawing.Size(967, 416);
            this.gbxResponse.TabIndex = 1;
            this.gbxResponse.TabStop = false;
            this.gbxResponse.Text = "テスト接続結果";
            // 
            // gbxBaseSettings
            // 
            this.gbxBaseSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxBaseSettings.Controls.Add(this.lblApiVersion);
            this.gbxBaseSettings.Controls.Add(this.txtClientSecret);
            this.gbxBaseSettings.Controls.Add(this.txtAccessToken);
            this.gbxBaseSettings.Controls.Add(this.txtRefreshToken);
            this.gbxBaseSettings.Controls.Add(this.txtClientId);
            this.gbxBaseSettings.Controls.Add(this.txtApiVersion);
            this.gbxBaseSettings.Controls.Add(this.txtBaseUri);
            this.gbxBaseSettings.Controls.Add(this.lblBaseUri);
            this.gbxBaseSettings.Controls.Add(this.lblClientSecret);
            this.gbxBaseSettings.Controls.Add(this.lblRefreshToken);
            this.gbxBaseSettings.Controls.Add(this.lblAccessToken);
            this.gbxBaseSettings.Controls.Add(this.lblClientId);
            this.gbxBaseSettings.Location = new System.Drawing.Point(15, 15);
            this.gbxBaseSettings.Name = "gbxBaseSettings";
            this.gbxBaseSettings.Size = new System.Drawing.Size(967, 169);
            this.gbxBaseSettings.TabIndex = 0;
            this.gbxBaseSettings.TabStop = false;
            this.gbxBaseSettings.Text = "基本設定";
            // 
            // txtResponse
            // 
            this.txtResponse.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtResponse.AlternateText.DisplayNull.Text = "PCA会計DX Web API 設定で認証に成功した場合、会社基本情報を表示します。";
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponse.DropDown.AllowDrop = false;
            this.txtResponse.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtResponse.HighlightText = true;
            this.txtResponse.IgnoreFocusChange = true;
            this.txtResponse.IgnoreFontSet = true;
            this.txtResponse.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtResponse.Location = new System.Drawing.Point(3, 19);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.Required = false;
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponse.Size = new System.Drawing.Size(961, 394);
            this.txtResponse.TabIndex = 0;
            // 
            // lblApiVersion
            // 
            this.lblApiVersion.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblApiVersion.Location = new System.Drawing.Point(711, 78);
            this.lblApiVersion.Name = "lblApiVersion";
            this.lblApiVersion.Size = new System.Drawing.Size(83, 22);
            this.lblApiVersion.TabIndex = 0;
            this.lblApiVersion.Text = "APIバージョン";
            this.lblApiVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClientSecret
            // 
            this.txtClientSecret.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtClientSecret.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtClientSecret.DropDown.AllowDrop = false;
            this.txtClientSecret.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtClientSecret.Format = "@Aa9";
            this.txtClientSecret.HighlightText = true;
            this.txtClientSecret.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtClientSecret.Location = new System.Drawing.Point(151, 50);
            this.txtClientSecret.MaxLength = 100;
            this.txtClientSecret.Name = "txtClientSecret";
            this.txtClientSecret.Required = true;
            this.txtClientSecret.Size = new System.Drawing.Size(810, 22);
            this.txtClientSecret.TabIndex = 1;
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.AllowSpace = GrapeCity.Win.Editors.AllowSpace.Narrow;
            this.txtAccessToken.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAccessToken.DropDown.AllowDrop = false;
            this.txtAccessToken.Ellipsis = GrapeCity.Win.Editors.EllipsisMode.EllipsisEnd;
            this.txtAccessToken.Enabled = false;
            this.txtAccessToken.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtAccessToken.HighlightText = true;
            this.txtAccessToken.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtAccessToken.Location = new System.Drawing.Point(151, 106);
            this.txtAccessToken.MaxLength = 1000;
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.Required = false;
            this.txtAccessToken.Size = new System.Drawing.Size(810, 22);
            this.txtAccessToken.TabIndex = 4;
            // 
            // txtRefreshToken
            // 
            this.txtRefreshToken.AllowSpace = GrapeCity.Win.Editors.AllowSpace.Narrow;
            this.txtRefreshToken.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtRefreshToken.DropDown.AllowDrop = false;
            this.txtRefreshToken.Ellipsis = GrapeCity.Win.Editors.EllipsisMode.EllipsisEnd;
            this.txtRefreshToken.Enabled = false;
            this.txtRefreshToken.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtRefreshToken.HighlightText = true;
            this.txtRefreshToken.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtRefreshToken.Location = new System.Drawing.Point(151, 134);
            this.txtRefreshToken.MaxLength = 1000;
            this.txtRefreshToken.Name = "txtRefreshToken";
            this.txtRefreshToken.Required = false;
            this.txtRefreshToken.Size = new System.Drawing.Size(810, 22);
            this.txtRefreshToken.TabIndex = 5;
            // 
            // txtClientId
            // 
            this.txtClientId.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtClientId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtClientId.DropDown.AllowDrop = false;
            this.txtClientId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtClientId.Format = "a9";
            this.txtClientId.HighlightText = true;
            this.txtClientId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtClientId.Location = new System.Drawing.Point(151, 22);
            this.txtClientId.MaxLength = 100;
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Required = true;
            this.txtClientId.Size = new System.Drawing.Size(810, 22);
            this.txtClientId.TabIndex = 0;
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
            this.txtApiVersion.Location = new System.Drawing.Point(800, 78);
            this.txtApiVersion.MaxLength = 5;
            this.txtApiVersion.Name = "txtApiVersion";
            this.txtApiVersion.Required = true;
            this.txtApiVersion.Size = new System.Drawing.Size(161, 22);
            this.txtApiVersion.TabIndex = 3;
            this.txtApiVersion.TabStop = false;
            this.txtApiVersion.Text = "v1";
            // 
            // txtBaseUri
            // 
            this.txtBaseUri.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBaseUri.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtBaseUri.AlternateText.DisplayNull.Text = "https://xxxx99.pcawebapi.jp/ まで入力してください。";
            this.txtBaseUri.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBaseUri.DropDown.AllowDrop = false;
            this.txtBaseUri.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBaseUri.Format = "Aa@9";
            this.txtBaseUri.HighlightText = true;
            this.txtBaseUri.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBaseUri.Location = new System.Drawing.Point(151, 78);
            this.txtBaseUri.MaxLength = 100;
            this.txtBaseUri.Name = "txtBaseUri";
            this.txtBaseUri.Required = true;
            this.txtBaseUri.Size = new System.Drawing.Size(527, 22);
            this.txtBaseUri.TabIndex = 2;
            // 
            // lblBaseUri
            // 
            this.lblBaseUri.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBaseUri.Location = new System.Drawing.Point(15, 78);
            this.lblBaseUri.Name = "lblBaseUri";
            this.lblBaseUri.Size = new System.Drawing.Size(130, 22);
            this.lblBaseUri.TabIndex = 0;
            this.lblBaseUri.Text = "PCA会計DX URL 設定";
            this.lblBaseUri.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientSecret
            // 
            this.lblClientSecret.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblClientSecret.Location = new System.Drawing.Point(15, 50);
            this.lblClientSecret.Name = "lblClientSecret";
            this.lblClientSecret.Size = new System.Drawing.Size(130, 22);
            this.lblClientSecret.TabIndex = 0;
            this.lblClientSecret.Text = "client secret";
            this.lblClientSecret.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRefreshToken
            // 
            this.lblRefreshToken.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRefreshToken.Location = new System.Drawing.Point(15, 134);
            this.lblRefreshToken.Name = "lblRefreshToken";
            this.lblRefreshToken.Size = new System.Drawing.Size(130, 22);
            this.lblRefreshToken.TabIndex = 0;
            this.lblRefreshToken.Text = "refresh_token";
            this.lblRefreshToken.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAccessToken
            // 
            this.lblAccessToken.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAccessToken.Location = new System.Drawing.Point(15, 106);
            this.lblAccessToken.Name = "lblAccessToken";
            this.lblAccessToken.Size = new System.Drawing.Size(130, 22);
            this.lblAccessToken.TabIndex = 0;
            this.lblAccessToken.Text = "access_token";
            this.lblAccessToken.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientId
            // 
            this.lblClientId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblClientId.Location = new System.Drawing.Point(15, 22);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(130, 22);
            this.lblClientId.TabIndex = 0;
            this.lblClientId.Text = "client id";
            this.lblClientId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PH1201
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxResponse);
            this.Controls.Add(this.gbxBaseSettings);
            this.Name = "PH1201";
            this.gbxResponse.ResumeLayout(false);
            this.gbxBaseSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtResponse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientSecret)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccessToken)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefreshToken)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApiVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseUri)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxBaseSettings;
        private Common.Controls.VOneLabelControl lblApiVersion;
        private Common.Controls.VOneTextControl txtClientId;
        private Common.Controls.VOneTextControl txtApiVersion;
        private Common.Controls.VOneTextControl txtBaseUri;
        private Common.Controls.VOneLabelControl lblBaseUri;
        private Common.Controls.VOneLabelControl lblClientId;
        private Common.Controls.VOneTextControl txtClientSecret;
        private Common.Controls.VOneLabelControl lblClientSecret;
        private Common.Controls.VOneTextControl txtAccessToken;
        private Common.Controls.VOneTextControl txtRefreshToken;
        private Common.Controls.VOneLabelControl lblRefreshToken;
        private Common.Controls.VOneLabelControl lblAccessToken;
        private System.Windows.Forms.GroupBox gbxResponse;
        private Common.Controls.VOneTextControl txtResponse;
    }
}
