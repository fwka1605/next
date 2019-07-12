namespace Rac.VOne.Client.Screen
{
    partial class PI0204
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
            GrapeCity.Win.MultiRow.ShortcutKeyManager shortcutKeyManager1 = new GrapeCity.Win.MultiRow.ShortcutKeyManager();
            this.tbcReminder = new System.Windows.Forms.TabControl();
            this.tabReminderSearch = new System.Windows.Forms.TabPage();
            this.nmbOutputNoTo = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.nmbOutputNoFrom = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.lblOutputAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datOutputAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datOutputAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.btnCurrency = new System.Windows.Forms.Button();
            this.lblOutputNo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCurrency = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCustomer = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtFromCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblToCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnFromCustomer = new System.Windows.Forms.Button();
            this.btnToCustomer = new System.Windows.Forms.Button();
            this.lblFromCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtToCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCustomerWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxCustomer = new System.Windows.Forms.CheckBox();
            this.lblOutputAtWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputNoWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.tabReminderResult = new System.Windows.Forms.TabPage();
            this.grdReminder = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.tbcReminder.SuspendLayout();
            this.tabReminderSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbOutputNoTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbOutputNoFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datOutputAtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datOutputAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToCustomerCode)).BeginInit();
            this.tabReminderResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReminder)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcReminder
            // 
            this.tbcReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcReminder.Controls.Add(this.tabReminderSearch);
            this.tbcReminder.Controls.Add(this.tabReminderResult);
            this.tbcReminder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcReminder.Location = new System.Drawing.Point(15, 15);
            this.tbcReminder.Name = "tbcReminder";
            this.tbcReminder.SelectedIndex = 0;
            this.tbcReminder.Size = new System.Drawing.Size(978, 591);
            this.tbcReminder.TabIndex = 0;
            // 
            // tabReminderSearch
            // 
            this.tabReminderSearch.Controls.Add(this.nmbOutputNoTo);
            this.tabReminderSearch.Controls.Add(this.nmbOutputNoFrom);
            this.tabReminderSearch.Controls.Add(this.lblOutputAt);
            this.tabReminderSearch.Controls.Add(this.datOutputAtTo);
            this.tabReminderSearch.Controls.Add(this.datOutputAtFrom);
            this.tabReminderSearch.Controls.Add(this.btnCurrency);
            this.tabReminderSearch.Controls.Add(this.lblOutputNo);
            this.tabReminderSearch.Controls.Add(this.lblCurrency);
            this.tabReminderSearch.Controls.Add(this.txtCurrencyCode);
            this.tabReminderSearch.Controls.Add(this.lblCustomer);
            this.tabReminderSearch.Controls.Add(this.txtFromCustomerCode);
            this.tabReminderSearch.Controls.Add(this.lblToCustomerName);
            this.tabReminderSearch.Controls.Add(this.btnFromCustomer);
            this.tabReminderSearch.Controls.Add(this.btnToCustomer);
            this.tabReminderSearch.Controls.Add(this.lblFromCustomerName);
            this.tabReminderSearch.Controls.Add(this.txtToCustomerCode);
            this.tabReminderSearch.Controls.Add(this.lblCustomerWave);
            this.tabReminderSearch.Controls.Add(this.cbxCustomer);
            this.tabReminderSearch.Controls.Add(this.lblOutputAtWave);
            this.tabReminderSearch.Controls.Add(this.lblOutputNoWave);
            this.tabReminderSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabReminderSearch.Location = new System.Drawing.Point(4, 24);
            this.tabReminderSearch.Name = "tabReminderSearch";
            this.tabReminderSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabReminderSearch.Size = new System.Drawing.Size(970, 563);
            this.tabReminderSearch.TabIndex = 0;
            this.tabReminderSearch.Text = "検索条件";
            this.tabReminderSearch.UseVisualStyleBackColor = true;
            // 
            // nmbOutputNoTo
            // 
            this.nmbOutputNoTo.AllowDeleteToNull = true;
            this.nmbOutputNoTo.DropDown.AllowDrop = false;
            this.nmbOutputNoTo.Fields.IntegerPart.MinDigits = 1;
            this.nmbOutputNoTo.HighlightText = true;
            this.nmbOutputNoTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbOutputNoTo.Location = new System.Drawing.Point(205, 48);
            this.nmbOutputNoTo.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmbOutputNoTo.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbOutputNoTo.Name = "nmbOutputNoTo";
            this.nmbOutputNoTo.Required = false;
            this.nmbOutputNoTo.Size = new System.Drawing.Size(79, 20);
            this.nmbOutputNoTo.Spin.AllowSpin = false;
            this.nmbOutputNoTo.TabIndex = 5;
            this.nmbOutputNoTo.Value = null;
            // 
            // nmbOutputNoFrom
            // 
            this.nmbOutputNoFrom.AllowDeleteToNull = true;
            this.nmbOutputNoFrom.DropDown.AllowDrop = false;
            this.nmbOutputNoFrom.Fields.IntegerPart.MinDigits = 1;
            this.nmbOutputNoFrom.HighlightText = true;
            this.nmbOutputNoFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbOutputNoFrom.Location = new System.Drawing.Point(94, 48);
            this.nmbOutputNoFrom.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmbOutputNoFrom.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbOutputNoFrom.Name = "nmbOutputNoFrom";
            this.nmbOutputNoFrom.Required = false;
            this.nmbOutputNoFrom.Size = new System.Drawing.Size(79, 20);
            this.nmbOutputNoFrom.Spin.AllowSpin = false;
            this.nmbOutputNoFrom.TabIndex = 3;
            this.nmbOutputNoFrom.Value = null;
            // 
            // lblOutputAt
            // 
            this.lblOutputAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputAt.Location = new System.Drawing.Point(21, 20);
            this.lblOutputAt.Margin = new System.Windows.Forms.Padding(18, 0, 3, 0);
            this.lblOutputAt.Name = "lblOutputAt";
            this.lblOutputAt.Size = new System.Drawing.Size(67, 16);
            this.lblOutputAt.TabIndex = 0;
            this.lblOutputAt.Text = "発行日";
            this.lblOutputAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datOutputAtTo
            // 
            this.datOutputAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datOutputAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datOutputAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datOutputAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datOutputAtTo.Location = new System.Drawing.Point(241, 18);
            this.datOutputAtTo.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.datOutputAtTo.Name = "datOutputAtTo";
            this.datOutputAtTo.Required = false;
            this.datOutputAtTo.Size = new System.Drawing.Size(115, 22);
            this.datOutputAtTo.Spin.AllowSpin = false;
            this.datOutputAtTo.TabIndex = 2;
            this.datOutputAtTo.Value = null;
            // 
            // datOutputAtFrom
            // 
            this.datOutputAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datOutputAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datOutputAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datOutputAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datOutputAtFrom.Location = new System.Drawing.Point(94, 18);
            this.datOutputAtFrom.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.datOutputAtFrom.Name = "datOutputAtFrom";
            this.datOutputAtFrom.Required = false;
            this.datOutputAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datOutputAtFrom.Spin.AllowSpin = false;
            this.datOutputAtFrom.TabIndex = 1;
            this.datOutputAtFrom.Value = null;
            // 
            // btnCurrency
            // 
            this.btnCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurrency.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrency.Location = new System.Drawing.Point(140, 77);
            this.btnCurrency.Name = "btnCurrency";
            this.btnCurrency.Size = new System.Drawing.Size(24, 24);
            this.btnCurrency.TabIndex = 7;
            this.btnCurrency.UseVisualStyleBackColor = true;
            this.btnCurrency.Visible = false;
            this.btnCurrency.Click += new System.EventHandler(this.btnCurrency_Click);
            // 
            // lblOutputNo
            // 
            this.lblOutputNo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputNo.Location = new System.Drawing.Point(21, 48);
            this.lblOutputNo.Name = "lblOutputNo";
            this.lblOutputNo.Size = new System.Drawing.Size(67, 16);
            this.lblOutputNo.TabIndex = 4;
            this.lblOutputNo.Text = "発行番号";
            this.lblOutputNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrency
            // 
            this.lblCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrency.Location = new System.Drawing.Point(21, 81);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(67, 16);
            this.lblCurrency.TabIndex = 7;
            this.lblCurrency.Text = "通貨コード";
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrency.Visible = false;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(94, 78);
            this.txtCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 6;
            this.txtCurrencyCode.Visible = false;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(415, 20);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(81, 16);
            this.lblCustomer.TabIndex = 0;
            this.lblCustomer.Text = "得意先コード";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFromCustomerCode
            // 
            this.txtFromCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtFromCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFromCustomerCode.DropDown.AllowDrop = false;
            this.txtFromCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromCustomerCode.HighlightText = true;
            this.txtFromCustomerCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFromCustomerCode.Location = new System.Drawing.Point(502, 18);
            this.txtFromCustomerCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFromCustomerCode.Name = "txtFromCustomerCode";
            this.txtFromCustomerCode.Required = false;
            this.txtFromCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtFromCustomerCode.TabIndex = 8;
            this.txtFromCustomerCode.Validated += new System.EventHandler(this.txtFromCustomerCode_Validated);
            // 
            // lblToCustomerName
            // 
            this.lblToCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblToCustomerName.DropDown.AllowDrop = false;
            this.lblToCustomerName.Enabled = false;
            this.lblToCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToCustomerName.HighlightText = true;
            this.lblToCustomerName.Location = new System.Drawing.Point(653, 48);
            this.lblToCustomerName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblToCustomerName.Name = "lblToCustomerName";
            this.lblToCustomerName.ReadOnly = true;
            this.lblToCustomerName.Required = false;
            this.lblToCustomerName.Size = new System.Drawing.Size(290, 22);
            this.lblToCustomerName.TabIndex = 0;
            // 
            // btnFromCustomer
            // 
            this.btnFromCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromCustomer.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnFromCustomer.Location = new System.Drawing.Point(623, 16);
            this.btnFromCustomer.Name = "btnFromCustomer";
            this.btnFromCustomer.Size = new System.Drawing.Size(24, 24);
            this.btnFromCustomer.TabIndex = 9;
            this.btnFromCustomer.UseVisualStyleBackColor = true;
            this.btnFromCustomer.Click += new System.EventHandler(this.btnFromCustomer_Click);
            // 
            // btnToCustomer
            // 
            this.btnToCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToCustomer.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnToCustomer.Location = new System.Drawing.Point(623, 46);
            this.btnToCustomer.Name = "btnToCustomer";
            this.btnToCustomer.Size = new System.Drawing.Size(24, 24);
            this.btnToCustomer.TabIndex = 11;
            this.btnToCustomer.UseVisualStyleBackColor = true;
            this.btnToCustomer.Click += new System.EventHandler(this.btnToCustomer_Click);
            // 
            // lblFromCustomerName
            // 
            this.lblFromCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFromCustomerName.DropDown.AllowDrop = false;
            this.lblFromCustomerName.Enabled = false;
            this.lblFromCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromCustomerName.HighlightText = true;
            this.lblFromCustomerName.Location = new System.Drawing.Point(653, 18);
            this.lblFromCustomerName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblFromCustomerName.Name = "lblFromCustomerName";
            this.lblFromCustomerName.ReadOnly = true;
            this.lblFromCustomerName.Required = false;
            this.lblFromCustomerName.Size = new System.Drawing.Size(290, 22);
            this.lblFromCustomerName.TabIndex = 0;
            // 
            // txtToCustomerCode
            // 
            this.txtToCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtToCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtToCustomerCode.DropDown.AllowDrop = false;
            this.txtToCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToCustomerCode.HighlightText = true;
            this.txtToCustomerCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtToCustomerCode.Location = new System.Drawing.Point(502, 48);
            this.txtToCustomerCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtToCustomerCode.Name = "txtToCustomerCode";
            this.txtToCustomerCode.Required = false;
            this.txtToCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtToCustomerCode.TabIndex = 10;
            this.txtToCustomerCode.Validated += new System.EventHandler(this.txtToCustomerCode_Validated);
            // 
            // lblCustomerWave
            // 
            this.lblCustomerWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerWave.Location = new System.Drawing.Point(459, 49);
            this.lblCustomerWave.Margin = new System.Windows.Forms.Padding(3);
            this.lblCustomerWave.Name = "lblCustomerWave";
            this.lblCustomerWave.Size = new System.Drawing.Size(20, 16);
            this.lblCustomerWave.TabIndex = 22;
            this.lblCustomerWave.Text = "～";
            this.lblCustomerWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.Location = new System.Drawing.Point(480, 50);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(16, 18);
            this.cbxCustomer.TabIndex = 0;
            this.cbxCustomer.TabStop = false;
            this.cbxCustomer.UseVisualStyleBackColor = true;
            // 
            // lblOutputAtWave
            // 
            this.lblOutputAtWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputAtWave.Location = new System.Drawing.Point(215, 20);
            this.lblOutputAtWave.Margin = new System.Windows.Forms.Padding(3);
            this.lblOutputAtWave.Name = "lblOutputAtWave";
            this.lblOutputAtWave.Size = new System.Drawing.Size(20, 16);
            this.lblOutputAtWave.TabIndex = 0;
            this.lblOutputAtWave.Text = "～";
            this.lblOutputAtWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutputNoWave
            // 
            this.lblOutputNoWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputNoWave.Location = new System.Drawing.Point(179, 50);
            this.lblOutputNoWave.Margin = new System.Windows.Forms.Padding(3);
            this.lblOutputNoWave.Name = "lblOutputNoWave";
            this.lblOutputNoWave.Size = new System.Drawing.Size(20, 16);
            this.lblOutputNoWave.TabIndex = 4;
            this.lblOutputNoWave.Text = "～";
            this.lblOutputNoWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabReminderResult
            // 
            this.tabReminderResult.Controls.Add(this.grdReminder);
            this.tabReminderResult.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabReminderResult.Location = new System.Drawing.Point(4, 24);
            this.tabReminderResult.Name = "tabReminderResult";
            this.tabReminderResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabReminderResult.Size = new System.Drawing.Size(970, 563);
            this.tabReminderResult.TabIndex = 1;
            this.tabReminderResult.Text = "検索結果";
            this.tabReminderResult.UseVisualStyleBackColor = true;
            // 
            // grdReminder
            // 
            this.grdReminder.AllowAutoExtend = true;
            this.grdReminder.AllowUserToAddRows = false;
            this.grdReminder.AllowUserToShiftSelect = true;
            this.grdReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdReminder.HideSelection = true;
            this.grdReminder.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdReminder.Location = new System.Drawing.Point(6, 6);
            this.grdReminder.Name = "grdReminder";
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager1.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            this.grdReminder.ShortcutKeyManager = shortcutKeyManager1;
            this.grdReminder.Size = new System.Drawing.Size(958, 551);
            this.grdReminder.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdReminder.TabIndex = 0;
            this.grdReminder.Text = "vOneGridControl1";
            this.grdReminder.CellClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReminder_CellClick);
            // 
            // PI0204
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbcReminder);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PI0204";
            this.Load += new System.EventHandler(this.PI0203_Load);
            this.tbcReminder.ResumeLayout(false);
            this.tabReminderSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbOutputNoTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbOutputNoFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datOutputAtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datOutputAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToCustomerCode)).EndInit();
            this.tabReminderResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReminder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcReminder;
        private System.Windows.Forms.TabPage tabReminderSearch;
        private System.Windows.Forms.TabPage tabReminderResult;
        private Common.Controls.VOneGridControl grdReminder;
        private Common.Controls.VOneLabelControl lblOutputAt;
        private Common.Controls.VOneLabelControl lblCurrency;
        private Common.Controls.VOneDateControl datOutputAtFrom;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrency;
        private Common.Controls.VOneNumberControl nmbOutputNoTo;
        private Common.Controls.VOneNumberControl nmbOutputNoFrom;
        private Common.Controls.VOneLabelControl lblOutputNo;
        private Common.Controls.VOneLabelControl lblCustomer;
        private Common.Controls.VOneTextControl txtFromCustomerCode;
        private Common.Controls.VOneDispLabelControl lblToCustomerName;
        private System.Windows.Forms.Button btnFromCustomer;
        private System.Windows.Forms.Button btnToCustomer;
        private Common.Controls.VOneDispLabelControl lblFromCustomerName;
        private Common.Controls.VOneTextControl txtToCustomerCode;
        private Common.Controls.VOneLabelControl lblCustomerWave;
        private System.Windows.Forms.CheckBox cbxCustomer;
        private Common.Controls.VOneLabelControl lblOutputNoWave;
        private Common.Controls.VOneDateControl datOutputAtTo;
        private Common.Controls.VOneLabelControl lblOutputAtWave;
    }
}
