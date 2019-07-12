namespace Rac.VOne.Client.Screen
{
    partial class PH0201
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
            this.grpDeleteData = new System.Windows.Forms.GroupBox();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.datDeleteDate = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblDeleteDataNote1 = new System.Windows.Forms.Label();
            this.lblDeleteDataNote2 = new System.Windows.Forms.Label();
            this.lblDeleteDataNote3 = new System.Windows.Forms.Label();
            this.lblDeleteDate = new System.Windows.Forms.Label();
            this.lblDeleteData2 = new System.Windows.Forms.Label();
            this.lblDeleteData1 = new System.Windows.Forms.Label();
            this.lblDeleteData = new System.Windows.Forms.Label();
            this.grpBackupDB = new System.Windows.Forms.GroupBox();
            this.lblDBBackupNote = new System.Windows.Forms.Label();
            this.btnBackupDB = new System.Windows.Forms.Button();
            this.btnSelectDBBackupFile = new System.Windows.Forms.Button();
            this.txtDBBackupFilePath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDBBackupFile = new System.Windows.Forms.Label();
            this.lblBackupDB = new System.Windows.Forms.Label();
            this.lblRestoreDB = new System.Windows.Forms.Label();
            this.grpRestoreDB = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDBRestoreNote = new System.Windows.Forms.Label();
            this.btnRestoreDB = new System.Windows.Forms.Button();
            this.btnSelectDBRestoreFile = new System.Windows.Forms.Button();
            this.txtDBRestoreFilePath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDBRestoreFile = new System.Windows.Forms.Label();
            this.grpDeleteData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datDeleteDate)).BeginInit();
            this.grpBackupDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBBackupFilePath)).BeginInit();
            this.grpRestoreDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBRestoreFilePath)).BeginInit();
            this.SuspendLayout();
            // 
            // grpDeleteData
            // 
            this.grpDeleteData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDeleteData.Controls.Add(this.btnDeleteData);
            this.grpDeleteData.Controls.Add(this.datDeleteDate);
            this.grpDeleteData.Controls.Add(this.lblDeleteDataNote1);
            this.grpDeleteData.Controls.Add(this.lblDeleteDataNote2);
            this.grpDeleteData.Controls.Add(this.lblDeleteDataNote3);
            this.grpDeleteData.Controls.Add(this.lblDeleteDate);
            this.grpDeleteData.Controls.Add(this.lblDeleteData2);
            this.grpDeleteData.Controls.Add(this.lblDeleteData1);
            this.grpDeleteData.Location = new System.Drawing.Point(15, 51);
            this.grpDeleteData.Name = "grpDeleteData";
            this.grpDeleteData.Size = new System.Drawing.Size(978, 165);
            this.grpDeleteData.TabIndex = 1;
            this.grpDeleteData.TabStop = false;
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteData.Location = new System.Drawing.Point(794, 67);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(160, 23);
            this.btnDeleteData.TabIndex = 1000;
            this.btnDeleteData.Text = "不要データを削除する";
            this.btnDeleteData.UseVisualStyleBackColor = true;
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // datDeleteDate
            // 
            this.datDeleteDate.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datDeleteDate.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datDeleteDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datDeleteDate.Location = new System.Drawing.Point(201, 27);
            this.datDeleteDate.Name = "datDeleteDate";
            this.datDeleteDate.Required = true;
            this.datDeleteDate.Size = new System.Drawing.Size(120, 22);
            this.datDeleteDate.Spin.AllowSpin = false;
            this.datDeleteDate.TabIndex = 1;
            this.datDeleteDate.Value = new System.DateTime(2017, 4, 19, 0, 0, 0, 0);
            // 
            // lblDeleteDataNote1
            // 
            this.lblDeleteDataNote1.AutoSize = true;
            this.lblDeleteDataNote1.Location = new System.Drawing.Point(25, 80);
            this.lblDeleteDataNote1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDeleteDataNote1.Name = "lblDeleteDataNote1";
            this.lblDeleteDataNote1.Size = new System.Drawing.Size(240, 15);
            this.lblDeleteDataNote1.TabIndex = 999;
            this.lblDeleteDataNote1.Text = "※ 入金データと請求データは同時に削除されます";
            // 
            // lblDeleteDataNote2
            // 
            this.lblDeleteDataNote2.AutoSize = true;
            this.lblDeleteDataNote2.Location = new System.Drawing.Point(25, 105);
            this.lblDeleteDataNote2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDeleteDataNote2.Name = "lblDeleteDataNote2";
            this.lblDeleteDataNote2.Size = new System.Drawing.Size(217, 15);
            this.lblDeleteDataNote2.TabIndex = 999;
            this.lblDeleteDataNote2.Text = "※ 実行時間は数分程かかる場合があります";
            // 
            // lblDeleteDataNote3
            // 
            this.lblDeleteDataNote3.AutoSize = true;
            this.lblDeleteDataNote3.ForeColor = System.Drawing.Color.Red;
            this.lblDeleteDataNote3.Location = new System.Drawing.Point(50, 130);
            this.lblDeleteDataNote3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDeleteDataNote3.Name = "lblDeleteDataNote3";
            this.lblDeleteDataNote3.Size = new System.Drawing.Size(420, 15);
            this.lblDeleteDataNote3.TabIndex = 999;
            this.lblDeleteDataNote3.Text = "※ 削除したデータを元に戻すことはできませんので、日付の設定には十分注意して下さい";
            // 
            // lblDeleteDate
            // 
            this.lblDeleteDate.AutoSize = true;
            this.lblDeleteDate.Location = new System.Drawing.Point(327, 30);
            this.lblDeleteDate.Name = "lblDeleteDate";
            this.lblDeleteDate.Size = new System.Drawing.Size(121, 15);
            this.lblDeleteDate.TabIndex = 999;
            this.lblDeleteDate.Text = "以前のデータを削除する";
            // 
            // lblDeleteData2
            // 
            this.lblDeleteData2.AutoSize = true;
            this.lblDeleteData2.Location = new System.Drawing.Point(25, 55);
            this.lblDeleteData2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDeleteData2.Name = "lblDeleteData2";
            this.lblDeleteData2.Size = new System.Drawing.Size(131, 15);
            this.lblDeleteData2.TabIndex = 999;
            this.lblDeleteData2.Text = "請求データ：入金予定日";
            // 
            // lblDeleteData1
            // 
            this.lblDeleteData1.AutoSize = true;
            this.lblDeleteData1.Location = new System.Drawing.Point(25, 30);
            this.lblDeleteData1.Name = "lblDeleteData1";
            this.lblDeleteData1.Size = new System.Drawing.Size(107, 15);
            this.lblDeleteData1.TabIndex = 999;
            this.lblDeleteData1.Text = "入金データ：入金日";
            // 
            // lblDeleteData
            // 
            this.lblDeleteData.AutoSize = true;
            this.lblDeleteData.Location = new System.Drawing.Point(15, 42);
            this.lblDeleteData.Name = "lblDeleteData";
            this.lblDeleteData.Size = new System.Drawing.Size(109, 15);
            this.lblDeleteData.TabIndex = 999;
            this.lblDeleteData.Text = "□ 不要データの削除";
            // 
            // grpBackupDB
            // 
            this.grpBackupDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBackupDB.Controls.Add(this.lblDBBackupNote);
            this.grpBackupDB.Controls.Add(this.btnBackupDB);
            this.grpBackupDB.Controls.Add(this.btnSelectDBBackupFile);
            this.grpBackupDB.Controls.Add(this.txtDBBackupFilePath);
            this.grpBackupDB.Controls.Add(this.lblDBBackupFile);
            this.grpBackupDB.Location = new System.Drawing.Point(15, 303);
            this.grpBackupDB.Name = "grpBackupDB";
            this.grpBackupDB.Size = new System.Drawing.Size(978, 120);
            this.grpBackupDB.TabIndex = 2;
            this.grpBackupDB.TabStop = false;
            // 
            // lblDBBackupNote
            // 
            this.lblDBBackupNote.AutoSize = true;
            this.lblDBBackupNote.Location = new System.Drawing.Point(25, 62);
            this.lblDBBackupNote.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDBBackupNote.Name = "lblDBBackupNote";
            this.lblDBBackupNote.Size = new System.Drawing.Size(217, 15);
            this.lblDBBackupNote.TabIndex = 999;
            this.lblDBBackupNote.Text = "※ 実行時間は数分程かかる場合があります";
            // 
            // btnBackupDB
            // 
            this.btnBackupDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackupDB.Location = new System.Drawing.Point(794, 67);
            this.btnBackupDB.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnBackupDB.Name = "btnBackupDB";
            this.btnBackupDB.Size = new System.Drawing.Size(160, 23);
            this.btnBackupDB.TabIndex = 3;
            this.btnBackupDB.Text = "DBをバックアップする";
            this.btnBackupDB.UseVisualStyleBackColor = true;
            this.btnBackupDB.Click += new System.EventHandler(this.btnBackupDB_Click);
            // 
            // btnSelectDBBackupFile
            // 
            this.btnSelectDBBackupFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDBBackupFile.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSelectDBBackupFile.Location = new System.Drawing.Point(930, 26);
            this.btnSelectDBBackupFile.Name = "btnSelectDBBackupFile";
            this.btnSelectDBBackupFile.Size = new System.Drawing.Size(24, 24);
            this.btnSelectDBBackupFile.TabIndex = 2;
            this.btnSelectDBBackupFile.UseVisualStyleBackColor = true;
            this.btnSelectDBBackupFile.Click += new System.EventHandler(this.btnSelectDBBackupFile_Click);
            // 
            // txtDBBackupFilePath
            // 
            this.txtDBBackupFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDBBackupFilePath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDBBackupFilePath.DropDown.AllowDrop = false;
            this.txtDBBackupFilePath.HighlightText = true;
            this.txtDBBackupFilePath.Location = new System.Drawing.Point(72, 27);
            this.txtDBBackupFilePath.MaxLength = 260;
            this.txtDBBackupFilePath.Name = "txtDBBackupFilePath";
            this.txtDBBackupFilePath.Required = true;
            this.txtDBBackupFilePath.Size = new System.Drawing.Size(852, 22);
            this.txtDBBackupFilePath.TabIndex = 1;
            // 
            // lblDBBackupFile
            // 
            this.lblDBBackupFile.AutoSize = true;
            this.lblDBBackupFile.Location = new System.Drawing.Point(25, 30);
            this.lblDBBackupFile.Name = "lblDBBackupFile";
            this.lblDBBackupFile.Size = new System.Drawing.Size(41, 15);
            this.lblDBBackupFile.TabIndex = 999;
            this.lblDBBackupFile.Text = "ファイル";
            // 
            // lblBackupDB
            // 
            this.lblBackupDB.AutoSize = true;
            this.lblBackupDB.Location = new System.Drawing.Point(15, 294);
            this.lblBackupDB.Name = "lblBackupDB";
            this.lblBackupDB.Size = new System.Drawing.Size(100, 15);
            this.lblBackupDB.TabIndex = 999;
            this.lblBackupDB.Text = "□ DBのバックアップ";
            // 
            // lblRestoreDB
            // 
            this.lblRestoreDB.AutoSize = true;
            this.lblRestoreDB.Location = new System.Drawing.Point(15, 443);
            this.lblRestoreDB.Name = "lblRestoreDB";
            this.lblRestoreDB.Size = new System.Drawing.Size(83, 15);
            this.lblRestoreDB.TabIndex = 999;
            this.lblRestoreDB.Text = "□ DBのリストア";
            // 
            // grpRestoreDB
            // 
            this.grpRestoreDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRestoreDB.Controls.Add(this.label1);
            this.grpRestoreDB.Controls.Add(this.lblDBRestoreNote);
            this.grpRestoreDB.Controls.Add(this.btnRestoreDB);
            this.grpRestoreDB.Controls.Add(this.btnSelectDBRestoreFile);
            this.grpRestoreDB.Controls.Add(this.txtDBRestoreFilePath);
            this.grpRestoreDB.Controls.Add(this.lblDBRestoreFile);
            this.grpRestoreDB.Location = new System.Drawing.Point(15, 452);
            this.grpRestoreDB.Name = "grpRestoreDB";
            this.grpRestoreDB.Size = new System.Drawing.Size(978, 120);
            this.grpRestoreDB.TabIndex = 3;
            this.grpRestoreDB.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(50, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 15);
            this.label1.TabIndex = 1000;
            this.label1.Text = "※ 全ユーザーの全データベース接続が強制的に切断されますので、実行前に十分確認して下さい";
            // 
            // lblDBRestoreNote
            // 
            this.lblDBRestoreNote.AutoSize = true;
            this.lblDBRestoreNote.Location = new System.Drawing.Point(25, 62);
            this.lblDBRestoreNote.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDBRestoreNote.Name = "lblDBRestoreNote";
            this.lblDBRestoreNote.Size = new System.Drawing.Size(217, 15);
            this.lblDBRestoreNote.TabIndex = 999;
            this.lblDBRestoreNote.Text = "※ 実行時間は数分程かかる場合があります";
            // 
            // btnRestoreDB
            // 
            this.btnRestoreDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestoreDB.Location = new System.Drawing.Point(794, 67);
            this.btnRestoreDB.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnRestoreDB.Name = "btnRestoreDB";
            this.btnRestoreDB.Size = new System.Drawing.Size(160, 23);
            this.btnRestoreDB.TabIndex = 3;
            this.btnRestoreDB.Text = "DBを復元する";
            this.btnRestoreDB.UseVisualStyleBackColor = true;
            this.btnRestoreDB.Click += new System.EventHandler(this.btnRestoreDB_Click);
            // 
            // btnSelectDBRestoreFile
            // 
            this.btnSelectDBRestoreFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDBRestoreFile.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSelectDBRestoreFile.Location = new System.Drawing.Point(930, 26);
            this.btnSelectDBRestoreFile.Name = "btnSelectDBRestoreFile";
            this.btnSelectDBRestoreFile.Size = new System.Drawing.Size(24, 24);
            this.btnSelectDBRestoreFile.TabIndex = 2;
            this.btnSelectDBRestoreFile.UseVisualStyleBackColor = true;
            this.btnSelectDBRestoreFile.Click += new System.EventHandler(this.btnSelectDBRestoreFile_Click);
            // 
            // txtDBRestoreFilePath
            // 
            this.txtDBRestoreFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDBRestoreFilePath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDBRestoreFilePath.DropDown.AllowDrop = false;
            this.txtDBRestoreFilePath.HighlightText = true;
            this.txtDBRestoreFilePath.Location = new System.Drawing.Point(72, 27);
            this.txtDBRestoreFilePath.MaxLength = 260;
            this.txtDBRestoreFilePath.Name = "txtDBRestoreFilePath";
            this.txtDBRestoreFilePath.Required = true;
            this.txtDBRestoreFilePath.Size = new System.Drawing.Size(852, 22);
            this.txtDBRestoreFilePath.TabIndex = 1;
            // 
            // lblDBRestoreFile
            // 
            this.lblDBRestoreFile.AutoSize = true;
            this.lblDBRestoreFile.Location = new System.Drawing.Point(25, 30);
            this.lblDBRestoreFile.Name = "lblDBRestoreFile";
            this.lblDBRestoreFile.Size = new System.Drawing.Size(41, 15);
            this.lblDBRestoreFile.TabIndex = 999;
            this.lblDBRestoreFile.Text = "ファイル";
            // 
            // PH0201
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblRestoreDB);
            this.Controls.Add(this.grpRestoreDB);
            this.Controls.Add(this.lblBackupDB);
            this.Controls.Add(this.grpBackupDB);
            this.Controls.Add(this.lblDeleteData);
            this.Controls.Add(this.grpDeleteData);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PH0201";
            this.Load += new System.EventHandler(this.PH0201_Load);
            this.grpDeleteData.ResumeLayout(false);
            this.grpDeleteData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datDeleteDate)).EndInit();
            this.grpBackupDB.ResumeLayout(false);
            this.grpBackupDB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBBackupFilePath)).EndInit();
            this.grpRestoreDB.ResumeLayout(false);
            this.grpRestoreDB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBRestoreFilePath)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDeleteData;
        private System.Windows.Forms.Label lblDeleteData;
        private System.Windows.Forms.GroupBox grpBackupDB;
        private System.Windows.Forms.Label lblBackupDB;
        private System.Windows.Forms.Label lblRestoreDB;
        private System.Windows.Forms.GroupBox grpRestoreDB;
        private System.Windows.Forms.Label lblDeleteDataNote1;
        private System.Windows.Forms.Label lblDeleteDataNote2;
        private System.Windows.Forms.Label lblDeleteDataNote3;
        private System.Windows.Forms.Label lblDeleteDate;
        private System.Windows.Forms.Label lblDeleteData2;
        private System.Windows.Forms.Label lblDeleteData1;
        private Common.Controls.VOneDateControl datDeleteDate;
        private Common.Controls.VOneTextControl txtDBBackupFilePath;
        private System.Windows.Forms.Label lblDBBackupFile;
        private Common.Controls.VOneTextControl txtDBRestoreFilePath;
        private System.Windows.Forms.Label lblDBRestoreFile;
        private System.Windows.Forms.Button btnSelectDBBackupFile;
        private System.Windows.Forms.Button btnSelectDBRestoreFile;
        private System.Windows.Forms.Button btnBackupDB;
        private System.Windows.Forms.Button btnRestoreDB;
        private System.Windows.Forms.Label lblDBBackupNote;
        private System.Windows.Forms.Label lblDBRestoreNote;
        private System.Windows.Forms.Button btnDeleteData;
        private System.Windows.Forms.Label label1;
    }
}
