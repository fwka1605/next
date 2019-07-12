namespace Rac.VOne.Client.Screen
{
    partial class PH0401
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
            this.pnlLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnServerFileChoose = new System.Windows.Forms.Button();
            this.btnServerCheckAll = new System.Windows.Forms.Button();
            this.btnServerUncheckAll = new System.Windows.Forms.Button();
            this.btnServerFileDelete = new System.Windows.Forms.Button();
            this.btnClientFileDelete = new System.Windows.Forms.Button();
            this.btnClientUncheckAll = new System.Windows.Forms.Button();
            this.btnClientCheckAll = new System.Windows.Forms.Button();
            this.btnClientFolderChoose = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnDL = new System.Windows.Forms.Button();
            this.grdServer = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.grdClient = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.txtServerPath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblServer = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtClientPath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblClient = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientPath)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.ColumnCount = 11;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0122F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.9878F));
            this.pnlLayout.Controls.Add(this.btnServerFileChoose, 6, 3);
            this.pnlLayout.Controls.Add(this.btnServerCheckAll, 8, 3);
            this.pnlLayout.Controls.Add(this.btnServerUncheckAll, 9, 3);
            this.pnlLayout.Controls.Add(this.btnServerFileDelete, 10, 3);
            this.pnlLayout.Controls.Add(this.btnClientFileDelete, 4, 3);
            this.pnlLayout.Controls.Add(this.btnClientUncheckAll, 3, 3);
            this.pnlLayout.Controls.Add(this.btnClientCheckAll, 2, 3);
            this.pnlLayout.Controls.Add(this.btnClientFolderChoose, 0, 3);
            this.pnlLayout.Controls.Add(this.btnTransfer, 5, 2);
            this.pnlLayout.Controls.Add(this.btnDL, 5, 1);
            this.pnlLayout.Controls.Add(this.grdServer, 6, 1);
            this.pnlLayout.Controls.Add(this.grdClient, 0, 1);
            this.pnlLayout.Controls.Add(this.txtServerPath, 7, 0);
            this.pnlLayout.Controls.Add(this.lblServer, 6, 0);
            this.pnlLayout.Controls.Add(this.txtClientPath, 1, 0);
            this.pnlLayout.Controls.Add(this.lblClient, 0, 0);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlLayout.Location = new System.Drawing.Point(12, 12);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.RowCount = 4;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.pnlLayout.Size = new System.Drawing.Size(984, 597);
            this.pnlLayout.TabIndex = 0;
            // 
            // btnServerFileChoose
            // 
            this.btnServerFileChoose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pnlLayout.SetColumnSpan(this.btnServerFileChoose, 2);
            this.btnServerFileChoose.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServerFileChoose.Location = new System.Drawing.Point(540, 560);
            this.btnServerFileChoose.Name = "btnServerFileChoose";
            this.btnServerFileChoose.Size = new System.Drawing.Size(90, 30);
            this.btnServerFileChoose.TabIndex = 20;
            this.btnServerFileChoose.Text = "フォルダ選択";
            this.btnServerFileChoose.UseVisualStyleBackColor = true;
            this.btnServerFileChoose.Click += new System.EventHandler(this.btnServerFileChoose_Click);
            // 
            // btnServerCheckAll
            // 
            this.btnServerCheckAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnServerCheckAll.Enabled = false;
            this.btnServerCheckAll.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServerCheckAll.Location = new System.Drawing.Point(640, 560);
            this.btnServerCheckAll.Name = "btnServerCheckAll";
            this.btnServerCheckAll.Size = new System.Drawing.Size(90, 30);
            this.btnServerCheckAll.TabIndex = 21;
            this.btnServerCheckAll.Text = "全選択";
            this.btnServerCheckAll.UseVisualStyleBackColor = true;
            this.btnServerCheckAll.Click += new System.EventHandler(this.btnServerCheckAll_Click);
            // 
            // btnServerUncheckAll
            // 
            this.btnServerUncheckAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnServerUncheckAll.Enabled = false;
            this.btnServerUncheckAll.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServerUncheckAll.Location = new System.Drawing.Point(740, 560);
            this.btnServerUncheckAll.Name = "btnServerUncheckAll";
            this.btnServerUncheckAll.Size = new System.Drawing.Size(90, 30);
            this.btnServerUncheckAll.TabIndex = 22;
            this.btnServerUncheckAll.Text = "全解除";
            this.btnServerUncheckAll.UseVisualStyleBackColor = true;
            this.btnServerUncheckAll.Click += new System.EventHandler(this.btnServerUncheckAll_Click);
            // 
            // btnServerFileDelete
            // 
            this.btnServerFileDelete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnServerFileDelete.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServerFileDelete.Location = new System.Drawing.Point(840, 560);
            this.btnServerFileDelete.Name = "btnServerFileDelete";
            this.btnServerFileDelete.Size = new System.Drawing.Size(90, 30);
            this.btnServerFileDelete.TabIndex = 23;
            this.btnServerFileDelete.Text = "ファイル削除";
            this.btnServerFileDelete.UseVisualStyleBackColor = true;
            this.btnServerFileDelete.Click += new System.EventHandler(this.btnServerFileDelete_Click);
            // 
            // btnClientFileDelete
            // 
            this.btnClientFileDelete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClientFileDelete.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientFileDelete.Location = new System.Drawing.Point(303, 560);
            this.btnClientFileDelete.Name = "btnClientFileDelete";
            this.btnClientFileDelete.Size = new System.Drawing.Size(90, 30);
            this.btnClientFileDelete.TabIndex = 19;
            this.btnClientFileDelete.Text = "ファイル削除";
            this.btnClientFileDelete.UseVisualStyleBackColor = true;
            this.btnClientFileDelete.Click += new System.EventHandler(this.btnClientFileDelete_Click);
            // 
            // btnClientUncheckAll
            // 
            this.btnClientUncheckAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClientUncheckAll.Enabled = false;
            this.btnClientUncheckAll.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientUncheckAll.Location = new System.Drawing.Point(203, 560);
            this.btnClientUncheckAll.Name = "btnClientUncheckAll";
            this.btnClientUncheckAll.Size = new System.Drawing.Size(90, 30);
            this.btnClientUncheckAll.TabIndex = 18;
            this.btnClientUncheckAll.Text = "全解除";
            this.btnClientUncheckAll.UseVisualStyleBackColor = true;
            this.btnClientUncheckAll.Click += new System.EventHandler(this.btnClientUncheckAll_Click);
            // 
            // btnClientCheckAll
            // 
            this.btnClientCheckAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClientCheckAll.Enabled = false;
            this.btnClientCheckAll.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientCheckAll.Location = new System.Drawing.Point(103, 560);
            this.btnClientCheckAll.Name = "btnClientCheckAll";
            this.btnClientCheckAll.Size = new System.Drawing.Size(90, 30);
            this.btnClientCheckAll.TabIndex = 17;
            this.btnClientCheckAll.Text = "全選択";
            this.btnClientCheckAll.UseVisualStyleBackColor = true;
            this.btnClientCheckAll.Click += new System.EventHandler(this.btnClientCheckAll_Click);
            // 
            // btnClientFolderChoose
            // 
            this.btnClientFolderChoose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pnlLayout.SetColumnSpan(this.btnClientFolderChoose, 2);
            this.btnClientFolderChoose.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientFolderChoose.Location = new System.Drawing.Point(3, 560);
            this.btnClientFolderChoose.Name = "btnClientFolderChoose";
            this.btnClientFolderChoose.Size = new System.Drawing.Size(90, 30);
            this.btnClientFolderChoose.TabIndex = 16;
            this.btnClientFolderChoose.Text = "フォルダ選択";
            this.btnClientFolderChoose.UseVisualStyleBackColor = true;
            this.btnClientFolderChoose.Click += new System.EventHandler(this.btnClientFolderChoose_Click);
            // 
            // btnTransfer
            // 
            this.btnTransfer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTransfer.Enabled = false;
            this.btnTransfer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfer.Location = new System.Drawing.Point(454, 307);
            this.btnTransfer.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(75, 42);
            this.btnTransfer.TabIndex = 15;
            this.btnTransfer.Text = "》 転送";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnDL
            // 
            this.btnDL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDL.Enabled = false;
            this.btnDL.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDL.Location = new System.Drawing.Point(454, 245);
            this.btnDL.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.btnDL.Name = "btnDL";
            this.btnDL.Size = new System.Drawing.Size(75, 42);
            this.btnDL.TabIndex = 14;
            this.btnDL.Text = "《 DL";
            this.btnDL.UseVisualStyleBackColor = true;
            this.btnDL.Click += new System.EventHandler(this.btnDL_Click);
            // 
            // grdServer
            // 
            this.grdServer.AllowAutoExtend = true;
            this.grdServer.AllowUserToAddRows = false;
            this.grdServer.AllowUserToShiftSelect = true;
            this.pnlLayout.SetColumnSpan(this.grdServer, 5);
            this.grdServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdServer.Location = new System.Drawing.Point(540, 44);
            this.grdServer.Name = "grdServer";
            this.pnlLayout.SetRowSpan(this.grdServer, 2);
            this.grdServer.Size = new System.Drawing.Size(441, 506);
            this.grdServer.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdServer.TabIndex = 13;
            this.grdServer.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdServer_CellValueChanged);
            this.grdServer.CellContentClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdServer_CellContentClick);
            // 
            // grdClient
            // 
            this.grdClient.AllowAutoExtend = true;
            this.grdClient.AllowUserToAddRows = false;
            this.grdClient.AllowUserToShiftSelect = true;
            this.pnlLayout.SetColumnSpan(this.grdClient, 5);
            this.grdClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdClient.Location = new System.Drawing.Point(3, 44);
            this.grdClient.Name = "grdClient";
            this.pnlLayout.SetRowSpan(this.grdClient, 2);
            this.grdClient.Size = new System.Drawing.Size(441, 506);
            this.grdClient.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdClient.TabIndex = 10;
            this.grdClient.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdClient_CellValueChanged);
            this.grdClient.CellContentClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdClient_CellContentClick);
            // 
            // txtServerPath
            // 
            this.txtServerPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLayout.SetColumnSpan(this.txtServerPath, 4);
            this.txtServerPath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtServerPath.DropDown.AllowDrop = false;
            this.txtServerPath.Enabled = false;
            this.txtServerPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerPath.HighlightText = true;
            this.txtServerPath.Location = new System.Drawing.Point(610, 9);
            this.txtServerPath.Name = "txtServerPath";
            this.txtServerPath.Required = false;
            this.txtServerPath.Size = new System.Drawing.Size(371, 22);
            this.txtServerPath.TabIndex = 4;
            // 
            // lblServer
            // 
            this.lblServer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblServer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblServer.Location = new System.Drawing.Point(547, 12);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(50, 16);
            this.lblServer.TabIndex = 3;
            this.lblServer.Text = "サーバー";
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClientPath
            // 
            this.txtClientPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLayout.SetColumnSpan(this.txtClientPath, 4);
            this.txtClientPath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtClientPath.DropDown.AllowDrop = false;
            this.txtClientPath.Enabled = false;
            this.txtClientPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientPath.HighlightText = true;
            this.txtClientPath.Location = new System.Drawing.Point(83, 9);
            this.txtClientPath.Name = "txtClientPath";
            this.txtClientPath.Required = false;
            this.txtClientPath.Size = new System.Drawing.Size(361, 22);
            this.txtClientPath.TabIndex = 2;
            // 
            // lblClient
            // 
            this.lblClient.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblClient.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblClient.Location = new System.Drawing.Point(8, 12);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(64, 16);
            this.lblClient.TabIndex = 1;
            this.lblClient.Text = "クライアント";
            this.lblClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PH0401
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlLayout);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH0401";
            this.Load += new System.EventHandler(this.PH0401_Load);
            this.pnlLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientPath)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlLayout;
        private Common.Controls.VOneLabelControl lblClient;
        private Common.Controls.VOneTextControl txtClientPath;
        private Common.Controls.VOneLabelControl lblServer;
        private Common.Controls.VOneTextControl txtServerPath;
        private Common.Controls.VOneGridControl grdClient;
        private Common.Controls.VOneGridControl grdServer;
        private System.Windows.Forms.Button btnDL;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnClientFolderChoose;
        private System.Windows.Forms.Button btnClientCheckAll;
        private System.Windows.Forms.Button btnClientUncheckAll;
        private System.Windows.Forms.Button btnClientFileDelete;
        private System.Windows.Forms.Button btnServerFileChoose;
        private System.Windows.Forms.Button btnServerCheckAll;
        private System.Windows.Forms.Button btnServerUncheckAll;
        private System.Windows.Forms.Button btnServerFileDelete;
    }
}
