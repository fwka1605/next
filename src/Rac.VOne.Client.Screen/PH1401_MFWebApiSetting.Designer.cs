namespace Rac.VOne.Client.Screen
{
    partial class PH1401
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
            this.gbxBaseSetting = new System.Windows.Forms.GroupBox();
            this.lblConnectionStatusDescription = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCallbackURLDescription = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblScopeDescription = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtAuthorizationCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtClientSecret = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtClientId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblConnectionStatus = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAuthorizationCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCallbackURL = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblScope = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblClientSecret = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblClientId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxResponse = new System.Windows.Forms.GroupBox();
            this.txtResponse = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gbxBaseSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblConnectionStatusDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCallbackURLDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblScopeDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthorizationCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientSecret)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId)).BeginInit();
            this.gbxResponse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtResponse)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxBaseSetting
            // 
            this.gbxBaseSetting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxBaseSetting.Controls.Add(this.lblConnectionStatusDescription);
            this.gbxBaseSetting.Controls.Add(this.lblCallbackURLDescription);
            this.gbxBaseSetting.Controls.Add(this.lblScopeDescription);
            this.gbxBaseSetting.Controls.Add(this.txtAuthorizationCode);
            this.gbxBaseSetting.Controls.Add(this.txtClientSecret);
            this.gbxBaseSetting.Controls.Add(this.txtClientId);
            this.gbxBaseSetting.Controls.Add(this.lblConnectionStatus);
            this.gbxBaseSetting.Controls.Add(this.lblAuthorizationCode);
            this.gbxBaseSetting.Controls.Add(this.lblCallbackURL);
            this.gbxBaseSetting.Controls.Add(this.lblScope);
            this.gbxBaseSetting.Controls.Add(this.lblClientSecret);
            this.gbxBaseSetting.Controls.Add(this.lblClientId);
            this.gbxBaseSetting.Location = new System.Drawing.Point(15, 15);
            this.gbxBaseSetting.Name = "gbxBaseSetting";
            this.gbxBaseSetting.Size = new System.Drawing.Size(978, 198);
            this.gbxBaseSetting.TabIndex = 0;
            this.gbxBaseSetting.TabStop = false;
            this.gbxBaseSetting.Text = "基本設定";
            // 
            // lblConnectionStatusDescription
            // 
            this.lblConnectionStatusDescription.DropDown.AllowDrop = false;
            this.lblConnectionStatusDescription.Enabled = false;
            this.lblConnectionStatusDescription.HighlightText = true;
            this.lblConnectionStatusDescription.Location = new System.Drawing.Point(113, 162);
            this.lblConnectionStatusDescription.Name = "lblConnectionStatusDescription";
            this.lblConnectionStatusDescription.ReadOnly = true;
            this.lblConnectionStatusDescription.Required = false;
            this.lblConnectionStatusDescription.Size = new System.Drawing.Size(859, 22);
            this.lblConnectionStatusDescription.TabIndex = 7;
            // 
            // lblCallbackURLDescription
            // 
            this.lblCallbackURLDescription.DropDown.AllowDrop = false;
            this.lblCallbackURLDescription.Enabled = false;
            this.lblCallbackURLDescription.HighlightText = true;
            this.lblCallbackURLDescription.Location = new System.Drawing.Point(113, 106);
            this.lblCallbackURLDescription.Name = "lblCallbackURLDescription";
            this.lblCallbackURLDescription.ReadOnly = true;
            this.lblCallbackURLDescription.Required = false;
            this.lblCallbackURLDescription.Size = new System.Drawing.Size(859, 22);
            this.lblCallbackURLDescription.TabIndex = 5;
            // 
            // lblScopeDescription
            // 
            this.lblScopeDescription.DropDown.AllowDrop = false;
            this.lblScopeDescription.Enabled = false;
            this.lblScopeDescription.HighlightText = true;
            this.lblScopeDescription.Location = new System.Drawing.Point(113, 78);
            this.lblScopeDescription.Name = "lblScopeDescription";
            this.lblScopeDescription.ReadOnly = true;
            this.lblScopeDescription.Required = false;
            this.lblScopeDescription.Size = new System.Drawing.Size(859, 22);
            this.lblScopeDescription.TabIndex = 4;
            // 
            // txtAuthorizationCode
            // 
            this.txtAuthorizationCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtAuthorizationCode.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtAuthorizationCode.AlternateText.DisplayNull.Text = "リダイレクト先のURLを貼り付けてください。(例 https://www.r-ac.co.jp/?code=XXX...)";
            this.txtAuthorizationCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAuthorizationCode.DropDown.AllowDrop = false;
            this.txtAuthorizationCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtAuthorizationCode.Format = "@Aa9";
            this.txtAuthorizationCode.HighlightText = true;
            this.txtAuthorizationCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtAuthorizationCode.Location = new System.Drawing.Point(113, 134);
            this.txtAuthorizationCode.MaxLength = 100;
            this.txtAuthorizationCode.Name = "txtAuthorizationCode";
            this.txtAuthorizationCode.Required = true;
            this.txtAuthorizationCode.Size = new System.Drawing.Size(859, 22);
            this.txtAuthorizationCode.TabIndex = 6;
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
            this.txtClientSecret.Location = new System.Drawing.Point(113, 50);
            this.txtClientSecret.MaxLength = 100;
            this.txtClientSecret.Name = "txtClientSecret";
            this.txtClientSecret.Required = true;
            this.txtClientSecret.Size = new System.Drawing.Size(859, 22);
            this.txtClientSecret.TabIndex = 3;
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
            this.txtClientId.Location = new System.Drawing.Point(113, 22);
            this.txtClientId.MaxLength = 100;
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Required = true;
            this.txtClientId.Size = new System.Drawing.Size(859, 22);
            this.txtClientId.TabIndex = 2;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblConnectionStatus.Location = new System.Drawing.Point(24, 164);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(83, 16);
            this.lblConnectionStatus.TabIndex = 3;
            this.lblConnectionStatus.Text = "連携状態";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAuthorizationCode
            // 
            this.lblAuthorizationCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAuthorizationCode.Location = new System.Drawing.Point(24, 136);
            this.lblAuthorizationCode.Name = "lblAuthorizationCode";
            this.lblAuthorizationCode.Size = new System.Drawing.Size(83, 16);
            this.lblAuthorizationCode.TabIndex = 3;
            this.lblAuthorizationCode.Text = "認証コード";
            this.lblAuthorizationCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCallbackURL
            // 
            this.lblCallbackURL.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCallbackURL.Location = new System.Drawing.Point(24, 108);
            this.lblCallbackURL.Name = "lblCallbackURL";
            this.lblCallbackURL.Size = new System.Drawing.Size(83, 16);
            this.lblCallbackURL.TabIndex = 3;
            this.lblCallbackURL.Text = "Callback URL";
            this.lblCallbackURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScope
            // 
            this.lblScope.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblScope.Location = new System.Drawing.Point(24, 80);
            this.lblScope.Name = "lblScope";
            this.lblScope.Size = new System.Drawing.Size(83, 16);
            this.lblScope.TabIndex = 3;
            this.lblScope.Text = "Scope";
            this.lblScope.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientSecret
            // 
            this.lblClientSecret.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblClientSecret.Location = new System.Drawing.Point(24, 52);
            this.lblClientSecret.Name = "lblClientSecret";
            this.lblClientSecret.Size = new System.Drawing.Size(83, 16);
            this.lblClientSecret.TabIndex = 3;
            this.lblClientSecret.Text = "Client Secret";
            this.lblClientSecret.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClientId
            // 
            this.lblClientId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblClientId.Location = new System.Drawing.Point(24, 24);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(83, 16);
            this.lblClientId.TabIndex = 4;
            this.lblClientId.Text = "Client ID";
            this.lblClientId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxResponse
            // 
            this.gbxResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResponse.Controls.Add(this.txtResponse);
            this.gbxResponse.Location = new System.Drawing.Point(15, 219);
            this.gbxResponse.Name = "gbxResponse";
            this.gbxResponse.Size = new System.Drawing.Size(978, 387);
            this.gbxResponse.TabIndex = 1;
            this.gbxResponse.TabStop = false;
            this.gbxResponse.Text = "テスト接続結果";
            // 
            // txtResponse
            // 
            this.txtResponse.AlternateText.DisplayNull.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtResponse.AlternateText.DisplayNull.Text = "認証に成功した場合、事業所情報を表示します。";
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponse.DropDown.AllowDrop = false;
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
            this.txtResponse.Size = new System.Drawing.Size(972, 365);
            this.txtResponse.TabIndex = 8;
            // 
            // PH1401
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxResponse);
            this.Controls.Add(this.gbxBaseSetting);
            this.Name = "PH1401";
            this.gbxBaseSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblConnectionStatusDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCallbackURLDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblScopeDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuthorizationCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientSecret)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId)).EndInit();
            this.gbxResponse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtResponse)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxBaseSetting;
        private System.Windows.Forms.GroupBox gbxResponse;
        private Common.Controls.VOneTextControl txtClientSecret;
        private Common.Controls.VOneTextControl txtClientId;
        private Common.Controls.VOneLabelControl lblClientSecret;
        private Common.Controls.VOneLabelControl lblClientId;
        private Common.Controls.VOneTextControl txtResponse;
        private Common.Controls.VOneDispLabelControl lblCallbackURLDescription;
        private Common.Controls.VOneDispLabelControl lblScopeDescription;
        private Common.Controls.VOneLabelControl lblCallbackURL;
        private Common.Controls.VOneLabelControl lblScope;
        private Common.Controls.VOneLabelControl lblConnectionStatus;
        private Common.Controls.VOneLabelControl lblAuthorizationCode;
        private Common.Controls.VOneDispLabelControl lblConnectionStatusDescription;
        private Common.Controls.VOneTextControl txtAuthorizationCode;
    }
}
