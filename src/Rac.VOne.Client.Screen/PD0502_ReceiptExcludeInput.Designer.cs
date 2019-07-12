namespace Rac.VOne.Client.Screen
{
    partial class PD0502
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
            this.gbxHeader = new System.Windows.Forms.GroupBox();
            this.txtReceiptId = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblReceiptId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.grdReceiptExcInput = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblRemainAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExcludeAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblReceiptAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblReceiptAmountTotal = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblExcludeAmountTotal = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDifferenceAmountTotal = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptExcInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDifferenceAmountTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbxHeader.Controls.Add(this.txtReceiptId);
            this.gbxHeader.Controls.Add(this.lblReceiptId);
            this.gbxHeader.Controls.Add(this.lblCurrencyCode);
            this.gbxHeader.Controls.Add(this.txtCurrencyCode);
            this.gbxHeader.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxHeader.Location = new System.Drawing.Point(164, 94);
            this.gbxHeader.Name = "gbxHeader";
            this.gbxHeader.Size = new System.Drawing.Size(670, 70);
            this.gbxHeader.TabIndex = 1;
            this.gbxHeader.TabStop = false;
            // 
            // txtReceiptId
            // 
            this.txtReceiptId.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.txtReceiptId.DropDown.AllowDrop = false;
            this.txtReceiptId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReceiptId.HighlightText = true;
            this.txtReceiptId.Location = new System.Drawing.Point(69, 30);
            this.txtReceiptId.Margin = new System.Windows.Forms.Padding(3, 9, 12, 3);
            this.txtReceiptId.Name = "txtReceiptId";
            this.txtReceiptId.Required = false;
            this.txtReceiptId.Size = new System.Drawing.Size(115, 22);
            this.txtReceiptId.TabIndex = 0;
            // 
            // lblReceiptId
            // 
            this.lblReceiptId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptId.Location = new System.Drawing.Point(18, 32);
            this.lblReceiptId.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.lblReceiptId.Name = "lblReceiptId";
            this.lblReceiptId.Size = new System.Drawing.Size(45, 16);
            this.lblReceiptId.TabIndex = 1;
            this.lblReceiptId.Text = "入金ID";
            this.lblReceiptId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyCode.Location = new System.Drawing.Point(208, 32);
            this.lblCurrencyCode.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(57, 16);
            this.lblCurrencyCode.TabIndex = 1;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.Location = new System.Drawing.Point(271, 30);
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = false;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 0;
            // 
            // grdReceiptExcInput
            // 
            this.grdReceiptExcInput.AllowAutoExtend = true;
            this.grdReceiptExcInput.AllowUserToAddRows = false;
            this.grdReceiptExcInput.AllowUserToShiftSelect = true;
            this.grdReceiptExcInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdReceiptExcInput.Location = new System.Drawing.Point(164, 170);
            this.grdReceiptExcInput.Name = "grdReceiptExcInput";
            this.grdReceiptExcInput.Size = new System.Drawing.Size(670, 242);
            this.grdReceiptExcInput.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdReceiptExcInput.TabIndex = 2;
            this.grdReceiptExcInput.Text = "vOneGridControl1";
            this.grdReceiptExcInput.CellValidated += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grid_CellValidated);
            this.grdReceiptExcInput.CellContentButtonClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grid_CellContentButtonClick);
            this.grdReceiptExcInput.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grid_CellEditedFormattedValueChanged);
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblRemainAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainAmount.Location = new System.Drawing.Point(658, 420);
            this.lblRemainAmount.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Size = new System.Drawing.Size(55, 16);
            this.lblRemainAmount.TabIndex = 1;
            this.lblRemainAmount.Text = "未消込額";
            this.lblRemainAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExcludeAmount
            // 
            this.lblExcludeAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblExcludeAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcludeAmount.Location = new System.Drawing.Point(622, 448);
            this.lblExcludeAmount.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblExcludeAmount.Name = "lblExcludeAmount";
            this.lblExcludeAmount.Size = new System.Drawing.Size(91, 16);
            this.lblExcludeAmount.TabIndex = 1;
            this.lblExcludeAmount.Text = "合計対象外金額";
            this.lblExcludeAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblReceiptAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptAmount.Location = new System.Drawing.Point(670, 476);
            this.lblReceiptAmount.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Size = new System.Drawing.Size(43, 16);
            this.lblReceiptAmount.TabIndex = 1;
            this.lblReceiptAmount.Text = "入金残";
            this.lblReceiptAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceiptAmountTotal
            // 
            this.lblReceiptAmountTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblReceiptAmountTotal.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblReceiptAmountTotal.DropDown.AllowDrop = false;
            this.lblReceiptAmountTotal.Enabled = false;
            this.lblReceiptAmountTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptAmountTotal.HighlightText = true;
            this.lblReceiptAmountTotal.Location = new System.Drawing.Point(714, 418);
            this.lblReceiptAmountTotal.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblReceiptAmountTotal.Name = "lblReceiptAmountTotal";
            this.lblReceiptAmountTotal.ReadOnly = true;
            this.lblReceiptAmountTotal.Required = false;
            this.lblReceiptAmountTotal.Size = new System.Drawing.Size(120, 22);
            this.lblReceiptAmountTotal.TabIndex = 18;
            // 
            // lblExcludeAmountTotal
            // 
            this.lblExcludeAmountTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblExcludeAmountTotal.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblExcludeAmountTotal.DropDown.AllowDrop = false;
            this.lblExcludeAmountTotal.Enabled = false;
            this.lblExcludeAmountTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcludeAmountTotal.HighlightText = true;
            this.lblExcludeAmountTotal.Location = new System.Drawing.Point(714, 446);
            this.lblExcludeAmountTotal.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblExcludeAmountTotal.Name = "lblExcludeAmountTotal";
            this.lblExcludeAmountTotal.ReadOnly = true;
            this.lblExcludeAmountTotal.Required = false;
            this.lblExcludeAmountTotal.Size = new System.Drawing.Size(120, 22);
            this.lblExcludeAmountTotal.TabIndex = 19;
            // 
            // lblDifferenceAmountTotal
            // 
            this.lblDifferenceAmountTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDifferenceAmountTotal.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDifferenceAmountTotal.DropDown.AllowDrop = false;
            this.lblDifferenceAmountTotal.Enabled = false;
            this.lblDifferenceAmountTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifferenceAmountTotal.HighlightText = true;
            this.lblDifferenceAmountTotal.Location = new System.Drawing.Point(714, 474);
            this.lblDifferenceAmountTotal.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblDifferenceAmountTotal.Name = "lblDifferenceAmountTotal";
            this.lblDifferenceAmountTotal.ReadOnly = true;
            this.lblDifferenceAmountTotal.Required = false;
            this.lblDifferenceAmountTotal.Size = new System.Drawing.Size(120, 22);
            this.lblDifferenceAmountTotal.TabIndex = 20;
            // 
            // PD0502
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblReceiptAmount);
            this.Controls.Add(this.lblExcludeAmount);
            this.Controls.Add(this.lblDifferenceAmountTotal);
            this.Controls.Add(this.lblRemainAmount);
            this.Controls.Add(this.lblExcludeAmountTotal);
            this.Controls.Add(this.gbxHeader);
            this.Controls.Add(this.lblReceiptAmountTotal);
            this.Controls.Add(this.grdReceiptExcInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PD0502";
            this.Load += new System.EventHandler(this.PD0502_Load);
            this.gbxHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptExcInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDifferenceAmountTotal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblRemainAmount;
        private Common.Controls.VOneLabelControl lblExcludeAmount;
        private Common.Controls.VOneLabelControl lblReceiptAmount;
        private Common.Controls.VOneGridControl grdReceiptExcInput;
        private System.Windows.Forms.GroupBox gbxHeader;
        private Common.Controls.VOneTextControl txtReceiptId;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private Common.Controls.VOneLabelControl lblReceiptId;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneDispLabelControl lblReceiptAmountTotal;
        private Common.Controls.VOneDispLabelControl lblExcludeAmountTotal;
        private Common.Controls.VOneDispLabelControl lblDifferenceAmountTotal;
    }
}