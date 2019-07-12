using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Security;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Rac.VOne.Client.Common;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgRootFolderBrowser : Dialog
    {
        public string RootPath { private get; set; }
        public string SelectedPath { get; private set; }
        public List<string> MultiFiles { get; private set; }
        public string InitialDirectory { get; set; }
        public string FileName { get; set; }
        private FolderBrowserType browserType { get; set; }

        public FolderBrowserType FolderBrowserType
        {
            private get { return browserType; }
            set { browserType = value; }
        }

        #region アイコン取得用

        // SHGetFileInfo関数
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int DestroyIcon(IntPtr hIcon);


        // SHGetFileInfo関数で使用するフラグ
        private const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
        private const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
        private const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン
        private const uint SHGFI_OPENICON = 0x2;

        // SHGetFileInfo関数で使用する構造体
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        #endregion

        public dlgRootFolderBrowser()
        {
            InitializeComponent();
        }

        private void dlgRootFolderBrowser_Load(object sender, EventArgs e)
        {
            SetScreen();
        }

        #region 表示画面初期化

        private void SetScreen()
        {
            try
            {
                switch (browserType)
                {
                    case FolderBrowserType.SelectFile:
                    case FolderBrowserType.SelectMultiFile:
                        Text = "ファイルの選択";
                        trvFolderList.LabelEdit = false;
                        btnCreate.Visible = false;
                        btnDelete.Visible = false;
                        btnOK.Text = "OK";
                        lblFileName.Visible = true;
                        txtFileName.Visible = true;
                        txtFileName.ReadOnly = true;
                        txtFileName.BackColor = SystemColors.Control;
                        break;
                    case FolderBrowserType.SaveFile:
                        Text = "ファイルの保存";
                        trvFolderList.LabelEdit = false;
                        btnCreate.Visible = false;
                        btnDelete.Visible = false;
                        btnOK.Text = "保存";
                        lblFileName.Visible = true;
                        txtFileName.Visible = true;
                        txtFileName.ReadOnly = false;
                        txtFileName.Text = FileName;
                        break;
                    case FolderBrowserType.SelectFolder:
                    case FolderBrowserType.SelectClientFolder:
                        Text = "フォルダの選択";
                        trvFolderList.LabelEdit = false;
                        btnCreate.Visible = false;
                        btnDelete.Visible = false;
                        btnOK.Text = "OK";
                        lblFileName.Visible = false;
                        txtFileName.Visible = false;
                        break;
                    case FolderBrowserType.SelectAndEditFolder:
                        Text = "フォルダの選択";
                        trvFolderList.LabelEdit = true;
                        btnCreate.Visible = true;
                        btnDelete.Visible = true;
                        btnOK.Text = "OK";
                        lblFileName.Visible = false;
                        txtFileName.Visible = false;
                        break;
                }

                txtPath.ReadOnly = true;
                trvFolderList.ImageList = imlTreeView;
                lsvFileList.SmallImageList = imlListView;
                lsvFileList.MultiSelect = browserType == FolderBrowserType.SelectMultiFile;

                // アプリケーション・アイコンを取得
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hFolder = SHGetFileInfo(@"C:\WINDOWS", 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_SMALLICON | SHGFI_ICON);
                if (hFolder != IntPtr.Zero)
                {
                    imlTreeView.Images.Add("F_Close", Icon.FromHandle(shinfo.hIcon));
                    trvFolderList.ImageKey = "F_Close";
                    DestroyIcon(shinfo.hIcon);
                }

                IntPtr hFolderOpen = SHGetFileInfo(@"C:\WINDOWS", 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_OPENICON | SHGFI_SMALLICON | SHGFI_ICON);
                if (hFolderOpen != IntPtr.Zero)
                {
                    imlTreeView.Images.Add("F_Open", Icon.FromHandle(shinfo.hIcon));
                    trvFolderList.ImageKey = "F_Open";
                    DestroyIcon(shinfo.hIcon);
                }

                TreeNode rootNode;
                if (browserType == FolderBrowserType.SelectClientFolder)
                {
                    if (SystemInformation.TerminalServerSession)
                    {
                        rootNode = Directory.Exists(InitialDirectory) ? new TreeNode(Directory.GetDirectoryRoot(InitialDirectory)) : new TreeNode(DefaultClient + @"\C");
                        trvFolderList.Nodes.Add(rootNode);
                        rootNode.Nodes.Add("dummy");
                    }
                    else
                    {
                        foreach (var drive in Environment.GetLogicalDrives())
                        {
                            var treeNode = new TreeNode(drive);
                            trvFolderList.Nodes.Add(treeNode);
                            treeNode.Nodes.Add("dummy");
                        }
                        rootNode = trvFolderList.Nodes[0];
                    }
                }
                else
                {
                    var rootPath = RootPath;
                    if (rootPath.EndsWith(@"\") && rootPath.Length > 3)
                        rootPath = rootPath.Substring(0, rootPath.Length - 1);
                    rootNode = new TreeNode(rootPath);
                    trvFolderList.Nodes.Add(rootNode);
                    rootNode.Nodes.Add("dummy");
                }

                if (rootNode != null)
                {
                    trvFolderList.SelectedNode = rootNode;
                    rootNode.Expand();
                }

                var isInValidDir = InitialDirectory == null || 
                                    !Directory.Exists(InitialDirectory) ||
                                    !InitialDirectory.ToUpper().Contains(rootNode.FullPath.ToUpper());

                InitialDirectory = isInValidDir ? string.Empty : InitialDirectory.Replace(rootNode.FullPath, string.Empty);

                if (!string.IsNullOrEmpty(InitialDirectory))
                {
                    string[] dirs = InitialDirectory.Split('\\');
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        foreach (TreeNode node in trvFolderList.SelectedNode.Nodes)
                        {
                            if (node.Text == dirs[i])
                            {
                                trvFolderList.SelectedNode = node;
                                node.Expand();
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region TreeView, ListViewイベント
        private void trvFolderList_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                TreeNode selectNode = e.Node;
                selectNode.Nodes.Clear();
                Cursor = Cursors.WaitCursor;

                var selectedDir = new DirectoryInfo(selectNode.FullPath);
                if (!selectedDir.Exists) return;

                foreach (var di in selectedDir.GetDirectories())
                {
                    try
                    {
                        if (di.Exists && (di.Attributes & FileAttributes.Hidden) == 0)
                        {
                            IEnumerable<string> subSubDir = Directory.EnumerateDirectories(di.FullName);
                            bool isExists = false;
                            foreach (var ss in subSubDir)
                            {
                                isExists = true;
                                break;
                            }

                            TreeNode node = selectNode.Nodes.Add(di.Name);
                            if (isExists)
                                node.Nodes.Add("dummy");
                        }
                    }
                    catch(UnauthorizedAccessException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                    catch(IOException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void trvFolderList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                lsvFileList.Clear();
                Cursor = Cursors.WaitCursor;

                var di = new DirectoryInfo(e.Node.FullPath);

                if (di.Exists)
                {
                    ListViewItem item;
                    List<string> extension = new List<string> { ".exe", ".EXE", ".ico", ".ICO" };

                    lsvFileList.BeginUpdate();

                    foreach (FileInfo file in di.GetFiles())
                    {
                        if (file.Exists && ((File.GetAttributes(file.FullName) & FileAttributes.Hidden) == 0) && file.Extension != ".lnk")
                        {
                            if (!imlListView.Images.ContainsKey(file.Extension))
                            {
                                Icon iconForFile = SystemIcons.WinLogo;
                                SHFILEINFO shinfo = new SHFILEINFO();
                                IntPtr hSuccess = SHGetFileInfo(file.FullName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);

                                if (!hSuccess.Equals(IntPtr.Zero))
                                {
                                    iconForFile = Icon.FromHandle(shinfo.hIcon);
                                    if (extension.Contains(file.Extension))
                                        imlListView.Images.Add(file.Name, iconForFile);
                                    else
                                        imlListView.Images.Add(file.Extension, iconForFile);
                                    DestroyIcon(shinfo.hIcon);
                                }
                            }
                            item = new ListViewItem(file.Name, -1);
                            if (extension.Contains(file.Extension))
                                item.ImageKey = file.Name;
                            else
                                item.ImageKey = file.Extension;
                            lsvFileList.Items.Add(item);
                        }
                    }
                    lsvFileList.EndUpdate();
                }

                btnDelete.Enabled = e.Node.Parent != null;

                txtPath.Text = trvFolderList.SelectedNode.FullPath;
                txtPath.Text = txtPath.Text.Replace(@":\\",@":\");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void trvFolderList_KeyUp(object sender, KeyEventArgs e)
        {
            if (browserType != FolderBrowserType.SelectAndEditFolder) return;

            TreeView tv = (TreeView)sender;
            if (e.KeyCode == Keys.F2 && tv.SelectedNode != null && !tv.SelectedNode.IsEditing)
                tv.SelectedNode.BeginEdit();
        }

        private void trvFolderList_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                e.CancelEdit = true;
                return;
            }

            var di = new DirectoryInfo(e.Node.FullPath);
            if ((File.GetAttributes(di.FullName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                e.CancelEdit = true;
        }

        private void trvFolderList_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null || e.Node.Parent == null) return;

            try
            {
                FileSystem.RenameDirectory(e.Node.FullPath, e.Label);
                txtPath.Text = trvFolderList.SelectedNode.Parent.FullPath + @"\" + e.Label;
                txtPath.Text = txtPath.Text.Replace(@":\\", @":\");
            }
            catch (ArgumentNullException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngInputInvalidLetter, "フォルダ名");
                e.CancelEdit = true;
            }
            catch (NotSupportedException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngInputInvalidLetter, "フォルダ名");
                e.CancelEdit = true;
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess);
                e.CancelEdit = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
                e.CancelEdit = true;
            }
            catch (SecurityException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
                e.CancelEdit = true;
            }
            catch (PathTooLongException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngPathTooLong);
                e.CancelEdit = true;
            }
            catch (IOException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                if (ex.Message.Contains("既に存在"))
                    ShowWarningDialog(MsgWngAlreadyExistsFolderAndFileName);
                else
                    ShowWarningDialog(MsgErrFolderCreateError);
                e.CancelEdit = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "リネーム");
                e.CancelEdit = true;
            }
        }

        private void lsvFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvFileList.SelectedItems.Count == 0) return;

            var index = lsvFileList.SelectedItems[0].Index;
            txtFileName.Text = lsvFileList.Items[index].Text;
        }
        #endregion

        #region ボタン押下イベント

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (browserType != FolderBrowserType.SelectAndEditFolder) return;

            Cursor = Cursors.WaitCursor;

            try
            {
                string newDir = "新しいフォルダ";
                bool isExist;

                for (int count = 1; count <= 99; count++)
                {
                    isExist = false;
                    if (count > 1)
                        newDir = "新しいフォルダ(" + count.ToString() + ")";

                    foreach (TreeNode tn in trvFolderList.SelectedNode.Nodes)
                    {
                        if (tn.Text == newDir)
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist) break;
                }

                Directory.CreateDirectory(txtPath.Text + @"\" + newDir);
                trvFolderList.SelectedNode.Nodes.Add(newDir);
                trvFolderList.SelectedNode.Expand();

                foreach (TreeNode tn in trvFolderList.SelectedNode.Nodes)
                {
                    if (tn.Text == newDir)
                    {
                        trvFolderList.SelectedNode = tn;
                        trvFolderList.Focus();
                        trvFolderList.SelectedNode.BeginEdit();
                        break;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngInputInvalidLetter, "フォルダ名");
            }
            catch (NotSupportedException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngInputInvalidLetter, "フォルダ名");
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
            }
            catch (SecurityException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
            }
            catch (PathTooLongException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngPathTooLong);
            }
            catch (IOException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrFolderCreateError);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "新規フォルダ作成");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (browserType != FolderBrowserType.SelectAndEditFolder) return;

            Cursor = Cursors.WaitCursor;

            try
            {
                TreeNode tn = trvFolderList.SelectedNode;
                if (tn.Parent == null) return;

                if (!ShowConfirmDialog(MsgQstConfirmDeleteXXX, "指定したフォルダ以下全てのファイルとフォルダが削除されます。" + Environment.NewLine + "フォルダ")) return;

                FileSystem.DeleteDirectory(txtPath.Text, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
                DirectoryInfo di = new DirectoryInfo(tn.FullPath);
                if (!di.Exists)
                    tn.Remove();
                trvFolderList.Focus();
            }
            catch (ArgumentException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngInputInvalidLetter, "パス");
            }
            catch (NotSupportedException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngInputInvalidLetter, "パス");
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess, txtPath.Text);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
            }
            catch (SecurityException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
            }
            catch (PathTooLongException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgWngPathTooLong);
            }
            catch (IOException ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrFolderDeleteError, txtPath.Text);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "フォルダ削除処理");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (browserType == FolderBrowserType.SaveFile)
            {
                string extension = Path.GetExtension(FileName);

                if (!string.IsNullOrEmpty(extension))
                    txtFileName.Text = Path.ChangeExtension(txtFileName.Text, extension);

                FileInfo fi = new FileInfo(txtPath.Text + @"\" + txtFileName.Text);
                if (fi.Exists)
                    if (!ShowConfirmDialog(MsgQstConfirmFileOverWrite)) return;
            }

            if ((browserType == FolderBrowserType.SaveFile ||
                 browserType == FolderBrowserType.SelectFile ||
                 browserType == FolderBrowserType.SelectMultiFile)
                 && string.IsNullOrEmpty(txtFileName.Text))
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region フォームクロージングイベント
        private void dlgRootFolderBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                switch (browserType)
                {
                    case FolderBrowserType.SelectFile:
                    case FolderBrowserType.SaveFile:
                        SelectedPath = txtPath.Text + @"\" + txtFileName.Text;
                        MultiFiles = null;
                        break;
                    case FolderBrowserType.SelectMultiFile:
                        SelectedPath = txtPath.Text + @"\" + txtFileName.Text;

                        if (MultiFiles == null)
                            MultiFiles = new List<string>();

                        foreach (ListViewItem item in lsvFileList.SelectedItems)
                            MultiFiles.Add(txtPath.Text + @"\" + item.Text);
                        break;
                    default:
                        SelectedPath = txtPath.Text;
                        MultiFiles = null;
                        break;
                }
            }
            else
            {
                SelectedPath = string.Empty;
                MultiFiles = null;
            }
        }
        #endregion

    }
}
