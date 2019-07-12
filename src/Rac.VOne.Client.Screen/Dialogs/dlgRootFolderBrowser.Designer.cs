namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgRootFolderBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgRootFolderBrowser));
            Rac.VOne.Message.XmlMessenger xmlMessenger1 = new Rac.VOne.Message.XmlMessenger();
            this.spcFolderFileList = new System.Windows.Forms.SplitContainer();
            this.trvFolderList = new System.Windows.Forms.TreeView();
            this.lsvFileList = new System.Windows.Forms.ListView();
            this.lblFolderPath = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblFileName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtPath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtFileName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.imlTreeView = new System.Windows.Forms.ImageList(this.components);
            this.imlListView = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.spcFolderFileList)).BeginInit();
            this.spcFolderFileList.Panel1.SuspendLayout();
            this.spcFolderFileList.Panel2.SuspendLayout();
            this.spcFolderFileList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName)).BeginInit();
            this.SuspendLayout();
            // 
            // spcFolderFileList
            // 
            this.spcFolderFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spcFolderFileList.Location = new System.Drawing.Point(12, 12);
            this.spcFolderFileList.Name = "spcFolderFileList";
            // 
            // spcFolderFileList.Panel1
            // 
            this.spcFolderFileList.Panel1.Controls.Add(this.trvFolderList);
            // 
            // spcFolderFileList.Panel2
            // 
            this.spcFolderFileList.Panel2.Controls.Add(this.lsvFileList);
            this.spcFolderFileList.Size = new System.Drawing.Size(560, 352);
            this.spcFolderFileList.SplitterDistance = 275;
            this.spcFolderFileList.TabIndex = 0;
            // 
            // trvFolderList
            // 
            this.trvFolderList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trvFolderList.Location = new System.Drawing.Point(3, 3);
            this.trvFolderList.Name = "trvFolderList";
            this.trvFolderList.Size = new System.Drawing.Size(269, 346);
            this.trvFolderList.TabIndex = 0;
            this.trvFolderList.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.trvFolderList_BeforeLabelEdit);
            this.trvFolderList.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.trvFolderList_AfterLabelEdit);
            this.trvFolderList.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvFolderList_BeforeExpand);
            this.trvFolderList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvFolderList_AfterSelect);
            this.trvFolderList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trvFolderList_KeyUp);
            // 
            // lsvFileList
            // 
            this.lsvFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvFileList.Location = new System.Drawing.Point(3, 3);
            this.lsvFileList.Name = "lsvFileList";
            this.lsvFileList.Size = new System.Drawing.Size(275, 346);
            this.lsvFileList.TabIndex = 0;
            this.lsvFileList.UseCompatibleStateImageBehavior = false;
            this.lsvFileList.View = System.Windows.Forms.View.List;
            this.lsvFileList.SelectedIndexChanged += new System.EventHandler(this.lsvFileList_SelectedIndexChanged);
            // 
            // lblFolderPath
            // 
            this.lblFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFolderPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFolderPath.Location = new System.Drawing.Point(12, 372);
            this.lblFolderPath.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblFolderPath.Name = "lblFolderPath";
            this.lblFolderPath.Size = new System.Drawing.Size(84, 16);
            this.lblFolderPath.TabIndex = 1;
            this.lblFolderPath.Text = "フォルダのパス：";
            this.lblFolderPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFileName
            // 
            this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(12, 400);
            this.lblFileName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(84, 16);
            this.lblFileName.TabIndex = 3;
            this.lblFileName.Text = "ファイル名：";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.BackColor = System.Drawing.SystemColors.Control;
            this.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPath.DropDown.AllowDrop = false;
            this.txtPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.HighlightText = true;
            this.txtPath.Location = new System.Drawing.Point(102, 370);
            this.txtPath.MaxLength = 260;
            this.txtPath.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Required = false;
            this.txtPath.Size = new System.Drawing.Size(470, 22);
            this.txtPath.TabIndex = 2;
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFileName.DropDown.AllowDrop = false;
            this.txtFileName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.HighlightText = true;
            this.txtFileName.Location = new System.Drawing.Point(102, 398);
            this.txtFileName.MaxLength = 260;
            this.txtFileName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Required = false;
            this.txtFileName.Size = new System.Drawing.Size(470, 22);
            this.txtFileName.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(497, 428);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(416, 428);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(288, 428);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(122, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "フォルダの削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(160, 428);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(122, 23);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "新しいフォルダ作成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // imlTreeView
            // 
            this.imlTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlTreeView.ImageSize = new System.Drawing.Size(16, 16);
            this.imlTreeView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imlListView
            // 
            this.imlListView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlListView.ImageSize = new System.Drawing.Size(16, 16);
            this.imlListView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // dlgRootFolderBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblFolderPath);
            this.Controls.Add(this.spcFolderFileList);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dlgRootFolderBrowser";
            this.Text = "";
            this.XmlMessenger = xmlMessenger1;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dlgRootFolderBrowser_FormClosing);
            this.Load += new System.EventHandler(this.dlgRootFolderBrowser_Load);
            this.Controls.SetChildIndex(this.spcFolderFileList, 0);
            this.Controls.SetChildIndex(this.lblFolderPath, 0);
            this.Controls.SetChildIndex(this.lblFileName, 0);
            this.Controls.SetChildIndex(this.txtPath, 0);
            this.Controls.SetChildIndex(this.txtFileName, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnCreate, 0);
            this.spcFolderFileList.Panel1.ResumeLayout(false);
            this.spcFolderFileList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcFolderFileList)).EndInit();
            this.spcFolderFileList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.SplitContainer spcFolderFileList;
        private Common.Controls.VOneLabelControl lblFolderPath;
        private Common.Controls.VOneLabelControl lblFileName;
        private Common.Controls.VOneTextControl txtPath;
        private Common.Controls.VOneTextControl txtFileName;
        private System.Windows.Forms.TreeView trvFolderList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.ImageList imlTreeView;
        private System.Windows.Forms.ImageList imlListView;
        internal System.Windows.Forms.ListView lsvFileList;
    }
}