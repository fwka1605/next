namespace Rac.VOne.Client.Screen
{
    partial class PE0109
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
            this.lblCaption = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCustomerCode = new System.Windows.Forms.Button();
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Location = new System.Drawing.Point(12, 27);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(313, 16);
            this.lblCaption.TabIndex = 1;
            this.lblCaption.Text = "前受振替を行います。振替先の得意先コードを指定してください。";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.Location = new System.Drawing.Point(87, 57);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Required = false;
            this.txtCustomerCode.Size = new System.Drawing.Size(90, 22);
            this.txtCustomerCode.TabIndex = 4;
            // 
            // btnCustomerCode
            // 
            this.btnCustomerCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCode.Location = new System.Drawing.Point(183, 56);
            this.btnCustomerCode.Name = "btnCustomerCode";
            this.btnCustomerCode.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCode.TabIndex = 6;
            this.btnCustomerCode.UseVisualStyleBackColor = true;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.DropDown.AllowDrop = false;
            this.lblCustomerName.Enabled = false;
            this.lblCustomerName.HighlightText = true;
            this.lblCustomerName.Location = new System.Drawing.Point(213, 57);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.ReadOnly = true;
            this.lblCustomerName.Required = false;
            this.lblCustomerName.Size = new System.Drawing.Size(203, 22);
            this.lblCustomerName.TabIndex = 7;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Location = new System.Drawing.Point(12, 59);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(69, 16);
            this.lblCustomerCode.TabIndex = 5;
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PE0109
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtCustomerCode);
            this.Controls.Add(this.btnCustomerCode);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblCustomerCode);
            this.Controls.Add(this.lblCaption);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(440, 200);
            this.Name = "PE0109";
            this.Size = new System.Drawing.Size(440, 200);
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl lblCaption;
        private Common.Controls.VOneTextControl txtCustomerCode;
        private System.Windows.Forms.Button btnCustomerCode;
        private Common.Controls.VOneDispLabelControl lblCustomerName;
        private Common.Controls.VOneLabelControl lblCustomerCode;
    }
}