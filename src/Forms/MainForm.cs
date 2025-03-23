using System;
using System.Data.SQLite;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using static PictureManagerApp.src.Lib.TsvRow;

namespace PictureManagerApp
{
    public partial class MainForm : Form
    {
        private const int MAX_PIC_WIDTH = 1200 / 2;
        private const int MAX_PIC_HEIGHT = 1920 / 2;
        private const int MAX_FILE_SIZE = 165;//140;//kb

        private const int LIST_THUMBNAIL_WIDTH = 64;
        private const int LIST_THUMBNAIL_HEIGHT = 64;
        //private bool list_thumnail = true;
        private bool list_thumnail = false;

        private const string LIST_DGV_ZIP_CLM_IDX = "#";
        private const string LIST_DGV_ZIP_CLM_STAR = "★";
        private const string LIST_DGV_ZIP_CLM_PAGE_TOTAL = "全";
        private const string LIST_DGV_ZIP_CLM_PAGE_NOW = "今";
        private const string LIST_DGV_ZIP_CLM_PERCENT = "%";
        private const string LIST_DGV_ZIP_CLM_THUMB = "T";
        private const string LIST_DGV_ZIP_CLM_NAME = "名前";
        private const string LIST_DGV_ZIP_CLM_RATING = "評価";
        private const string LIST_DGV_ZIP_CLM_FILENAME = "ファイル名";
        private const string LIST_DGV_ZIP_CLM_PATH = "パス";


        private const string USB_MEMORY_DIR_PATH = @"work\r18";

        private static readonly string[] CMBBOX_DIR_PATHS = [
            @"D:\download\PxDl",
            @"D:\download\PxDl-",
            @"D:\download\PxDl-0trash",
            @"D:\download\PxDl--0trash",

            @"D:\dl\AnkPixiv\Twitter",
            @"D:\dl\AnkPixiv\Twitter-",
            @"D:\dl\AnkPixiv\Twitter-0trash",
            @"D:\dl\AnkPixiv\Twitter--0trash",
            @"D:\dl\AnkPixiv\Nijie",
            @"D:\dl\AnkPixiv\Nijie-0trash",

            @"D:\r18\dlPic\pxv",
            @"D:\r18\dlPic\twitter",
            @"D:\r18\dlPic\Nijie\nje",

            //@"D:\work\bin\r18",
            //@"D:\work\bin\r18t",

#if DEBUG
            @"D:\download\PxDl-\!pic_infos!.tsv"
#endif
        ];

        private static readonly string[] SP_WORDS = [
            "-w2x",
            "-iv",
            //"-cnv",
            "",
        ];

        private int Radius { get; set; }
        private int Ox { get; set; }
        private int Oy { get; set; }

        private FileList mFavFileList = new();

        private DirList mDirList = new();

        private TsvRowList mTsvRowList;

        private DataTable mTwtDatatable { get; set; }
        private DataTable mPxvDatatable { get; set; }


        //--------------
        //
        //--------------
        public MainForm()
        {
            Log.trc($"[S]");

            InitializeComponent();

            // check
            for (int i = 0; i < chkListBox_Ext.Items.Count; i++)
            {
                chkListBox_Ext.SetItemChecked(i, true);
            }

            Opacity = 0;

            Radius = (int)(Math.Sqrt(Width * Width + Height * Height) / 2);
            Ox = Width / 2;
            Oy = Height / 2;
            Region = new Region(new GraphicsPath());

            Log.trc($"[E]");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.trc($"[S]");

            string[] args = Environment.GetCommandLineArgs();

            //パス
            string path;
            if (args.Length > 1)
            {
                path = args[1];
            }
            else
            {
                path = CMBBOX_DIR_PATHS[0];
            }
            InitPathCmbBox(path);

            //指定文字
            cmbBox_FilenameFilter.Text = "";
            foreach (string word in SP_WORDS)
            {
                cmbBox_FilenameFilter.Items.Add(word);
            }

            InitWindow();

            InitAnimation();

            Log.trc($"[E]");
        }

        private void InitPathCmbBox(string path1)
        {
            //if (Directory.Exists(path1))
            //  cmbBoxPath.Text = path1;

            var cdir = System.Environment.CurrentDirectory;
            //var dri = System.IO.Path.GetDirectoryName(cdir);
            var root_path = Path.GetPathRoot(cdir);
            var extmem_path = System.IO.Path.Combine(root_path, USB_MEMORY_DIR_PATH);

            if (Directory.Exists(extmem_path))
            {
                var dirs = Directory.EnumerateDirectories(extmem_path);
                foreach (var dir in dirs.OrderBy(x => x))
                {
                    cmbBoxPath.Items.Add(dir);
                }
            }
            else
            {
                foreach (string path in CMBBOX_DIR_PATHS)
                {
                    if (Directory.Exists(path))
                    {
                        cmbBoxPath.Items.Add(path);
                    }
#if DEBUG
                    else if (File.Exists(path))
                    {
                        cmbBoxPath.Items.Add(path);
                    }
#endif
                }
            }

            if (cmbBoxPath.Items.Count > 0)
            {
                cmbBoxPath.Text = (string)cmbBoxPath.Items[0];
            }

            /*
            var dir = System.Environment.CurrentDirectory;
            dir = Directory.GetParent(dir).ToString();
            dir = Directory.GetParent(dir).ToString();
            cmbBoxPath.Items.Add(dir);*/
        }

        private void InitWindow()
        {
            //高さ
            var w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            Log.trc($"{w}x{h}");
            bool landscape = w > h;
            //landscape = false;
            if (landscape)
            {
                //横長画面の場合
                this.StartPosition = FormStartPosition.CenterScreen;
                if (w <= 1920)
                {
                    this.Width = w - 100;
                    this.Height = h - 100;

                    var loc_x = (w - this.Width) / 2;
                    int loc_y = this.Location.Y;
                    loc_y = (h - this.Height) / 2;

                    this.Location = new Point(loc_x, loc_y);
                }
                else
                {
                    this.Width = w / 2;
                    this.Height = h / 2;

                    var loc_x = (w - this.Width) / 2;
                    int loc_y = this.Location.Y;
                    loc_y = (h - this.Height) / 2;

                    this.Location = new Point(loc_x, loc_y);
                }

            }
            else
            {
                //縦長画面の場合
                //this.Width = w - 10;
                //this.Height = h - 50;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(10, 10);
            }
        }

        private void InitAnimation()
        {
            int duration = 100;
            Animator.Animate(duration, (frame, frequency) =>
            {
                if (!Visible || IsDisposed) return false;
                Opacity = (double)frame / frequency;
                //Lib.log($"frame={frame}, freq={frequency}, opa={Opacity}");

                var graphicsPath = new GraphicsPath();
                var r = Radius * frame / frequency;// resolution;
                graphicsPath.AddEllipse(new Rectangle(Ox - r, Oy - r, r * 2, r * 2));
                Region = new Region(graphicsPath);
                //if (frame == resolution) Region = null;
                if (frame == frequency) Region = null;
                return true;
            });
        }

        private void Start(FileList filelist = null, FILTER_TYPE filterType = FILTER_TYPE.FILTER_NONE)
        {
            Log.trc($"[S]");

            var model = new PictureModel();
            setModelParam(model);

            if (filterType != FILTER_TYPE.FILTER_NONE)
            {
                model.SetFilter(filterType);
            }

            try
            {
                // TODO: progress bar
                //...

                var pathStr = cmbBoxPath.Text;
                if (filelist != null)
                {
                    model.SetFileList(filelist);
                }
                else if (txtBox_FileList.Text == "")
                {
                    model.BuildFileList(pathStr);
                }
                else
                {
                    model.BuildFileListFromText(txtBox_FileList.Text, pathStr);
                }

                PictureForm picForm = new();
                picForm.Text = $"{pathStr} [{cmbBoxPath.Items.IndexOf(cmbBoxPath.Text)}/{cmbBoxPath.Items.Count}]";
                picForm.SetModel(model);
                picForm.ShowDialog();
                Log.trc($"picForm.ShowDialog() end");

                model.UpdateListFile();

                //選択されたファイルを記録する
                var pics = model.GetSelectedPic();
                foreach (var pic in pics)
                {
                    var fitem = (FileItem)pic;
                    mFavFileList.Add(fitem);
                }

                mDirList.Update(model);
            }
            catch (Exception ex)
            {
                Log.trc("{1}", ex.ToString());
                MessageBox.Show(ex.ToString(),
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
            }

            Log.trc($"[E]");
        }

        private void setModelParam(PictureModel model)
        {
            setModelParam_date(model);
            setModelParam_picSize(model);

            //ファイルサイズmin
            var minfilesize = (int)numUD_MinFilesize.Value;
            if (minfilesize != 0)
            {
                model.SetMinFileSize(minfilesize * 1024);
            }

            //ファイルサイズmax
            var maxfilesize = (int)numUD_MaxFilesize.Value;
            if (maxfilesize != 0)
            {
                model.SetMaxFileSize(maxfilesize * 1024);
            }

            // 拡張子
            var ext = "";
            foreach (var item in chkListBox_Ext.CheckedItems)
            {
                if (ext != "")
                {
                    ext += ",";
                }
                ext += item.ToString();
            }
            model.SetExt(ext);

            // ファイル名フィルター
            if (cmbBox_FilenameFilter.Text != "")
            {
                var word = cmbBox_FilenameFilter.Text;
                model.SetSeachWord(word);
            }
        }

        private void setModelParam_date(PictureModel model)
        {
            DateTime? dtFrom = null;// DateTime.MinValue;
            DateTime? dtTo = null;// DateTime.Now; ;
            if (chkBox_from.Checked)
            {
                DateTime date = dtPicker_from.Value;
                dtFrom = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            }

            if (checkBox_SameDate.Checked && chkBox_from.Checked)
            {
                DateTime date = dtPicker_from.Value;
                dtTo = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }
            else if (chkBox_to.Checked)
            {
                DateTime date = dtPicker_to.Value;
                dtTo = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }
            model.SetDate(dtFrom, dtTo);
        }

        private void setModelParam_picSize(PictureModel model)
        {
            var width = (int)numUD_Width.Value;
            var height = (int)numUD_Height.Value;
            if (width != 0 && height != 0)
            {
                Size size = new(width, height);

                model.SetMaxPicSize(size);
            }

            if (radioBtn_PicOrinet_All.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_ALL);
            }
            else if (radioBtn_PicOrinet_PR.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_PORTRAIT);
            }
            else if (radioBtn_PicOrinet_LS.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_LANDSCAPE);
            }
            else if (radioBtn_PicOrinet_LS_only.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_LANDSCAPE_ONLY);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            Start();
            Log.trc("[E]");
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            var txt = Clipboard.GetText();
            cmbBoxPath.Text = txt;
            cmbBoxPath.Items.Add(txt);
            Log.trc("[E]");
        }

        private void EndBtn_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            /*MessageBox.Show($"{this.Width}x{this.Height}",
                    "title",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);*/
            this.Close();
            Log.trc("[E]");
        }

        private void checkBox_SameDate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SameDate.Checked)
            {
                dtPicker_to.Enabled = false;
            }
            else
            {
                dtPicker_to.Enabled = true;
            }
        }

        private void btnAppendSubDir_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");

            string path = cmbBoxPath.Text;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("存在しないディレクトリです",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var dirs = Directory.EnumerateDirectories(path);
            foreach (var dir in dirs.OrderBy(x => x))
            {
                cmbBoxPath.Items.Add(dir);
            }

            var fileArray = Directory.GetFiles(
                path,
                "*.zip",
                SearchOption.TopDirectoryOnly);

            foreach (var f in fileArray.OrderBy(x => x))
            {
                {
                    cmbBoxPath.Items.Add(f);

                    //DirItem diritem = new(f);
                    //mDirList.Add(diritem);
                }
            }

            Log.trc("[E]");
        }

        private void cmbBoxPath_DragDrop(object sender, DragEventArgs e)
        {
            Log.trc("[S]");

            // ファイルが渡されていなければ、何もしない
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            // 渡されたファイルに対して処理を行う
            foreach (var filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                cmbBoxPath.Text = filePath;
                cmbBoxPath.Items.Add(filePath);
            }

            Log.trc("[E]");
        }

        private void cmbBoxPath_DragEnter(object sender, DragEventArgs e)
        {
            // ドラッグドロップ時にカーソルの形状を変更
            e.Effect = DragDropEffects.All;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var bContinue = true;
            while (bContinue)
            {
                var idx = cmbBoxPath.Items.IndexOf(cmbBoxPath.Text);
                idx++;
                if (idx >= cmbBoxPath.Items.Count)
                {
                    MessageBox.Show("次はない",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;

                }

                string msg = "次も？";
                DialogResult result = MessageBox.Show(msg,
                            msg,
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    cmbBoxPath.SelectedIndex = idx;
                    Start();
                }
                else if (result != DialogResult.Cancel)
                {
                    break;
                }
            }
        }

        private void btnPicSizeToggle_Click(object sender, EventArgs e)
        {
            var width = (int)numUD_Width.Value;
            var height = (int)numUD_Height.Value;

            if (width == 0 && height == 0)
            {
                numUD_Width.Value = MAX_PIC_WIDTH;
                numUD_Height.Value = MAX_PIC_HEIGHT;

                numUD_MaxFilesize.Value = MAX_FILE_SIZE;
            }
            else
            {
                numUD_Width.Value = 0;
                numUD_Height.Value = 0;

                numUD_MaxFilesize.Value = 0;
            }
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            DateTime dt = dtPicker_from.Value;
            dtPicker_from.Value = dt.AddDays(1);
            Start();
        }

        private void ToolStripMenuItem_AddPath_Click(object sender, EventArgs e)
        {
            if (sender == this.ToolStripMenuItem_AddPath)
            {

            }

            var where = Sqlite.SQL_STATEMENT_WHERE_AI;
            where = Sqlite.SQL_STATEMENT_WHERE_AI_SUSPEND;
            where = Sqlite.SQL_STATEMENT_WHERE_3D;
            where = Sqlite.SQL_STATEMENT_WHERE_AI_NO_UPDATE;
            where = Sqlite.SQL_STATEMENT_WHERE_AI;

            var dirpaths = Pxv.GetDirPaths(where);
            foreach (var dirpath in dirpaths)
            {
                //cmbBoxPath.Text = dirpath;
                cmbBoxPath.Items.Add(dirpath);
            }
        }

        private void ToolStripMenuItem_SelPicShow_Click(object sender, EventArgs e)
        {
            Start(mFavFileList);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabc = (TabControl)sender;
            var txt = tabc.SelectedTab.Text;
            if (txt == "DGM")
            {
                if (this.TwtDataGridView.DataSource == null)
                {
                    InitTwtDGV();
                }
            }
            else if (txt == "DGV(Pxv)")
            {
                if (this.DgvPxv.DataSource == null)
                {
                    InitPxvDGV();
                }
            }
            else if (txt == "zip list")
            {
                InitZipListDGV();
            }
            else
            {
            }
        }

        private void InitTwtDGV()
        {
            var dbPath = Sqlite.GetSqliteFilePath();

            using (var con = new SQLiteConnection("Data Source=" + dbPath))
            {
                con.Open();

                this.mTwtDatatable = new DataTable();

                var tblname = "twitters";
                var colname = "id, twtid, twtname, rating, drawing_method, filenum, status";
                var adapter = Sqlite.GetSQLiteDataAdapter(con, tblname, colname);
                adapter.Fill(this.mTwtDatatable);

                this.TwtDataGridView.DataSource = this.mTwtDatatable;
            }
        }

        private void InitPxvDGV()
        {
            var dbPath = Sqlite.GetSqliteFilePath();

            using (var con = new SQLiteConnection("Data Source=" + dbPath))
            {
                con.Open();

                this.mPxvDatatable = new DataTable();

                var tblname = "artists";
                var colname = "id, pxvid, pxvname, feature, rating, filenum, status";
                var where_p = "status = '停止'";
                where_p += "AND feature = 'AI' AND rating >= 100 ORDER BY rating DESC";
                var adapter = Sqlite.GetSQLiteDataAdapter(con, tblname, colname, where_p);
                adapter.Fill(this.mPxvDatatable);

                this.DgvPxv.DataSource = this.mPxvDatatable;
            }
        }

        private void InitZipListDGV()
        {
            var dgv = this.DirListDGV;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AllowUserToAddRows = false;

            if (list_thumnail)
            {
                dgv.RowTemplate.MinimumHeight = LIST_THUMBNAIL_HEIGHT;
            }

            dgv.ReadOnly = true;                      //読取専用
            dgv.AllowUserToDeleteRows = false;        //行削除禁止
            dgv.AllowUserToAddRows = false;           //行挿入禁止
            dgv.AllowUserToResizeRows = false;        //行の高さ変更禁止
            dgv.RowHeadersVisible = false;            //行ヘッダーを非表示にする
            dgv.MultiSelect = false;                  //セル、行、列が複数選択禁止
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;       //セルを選択すると行全体が選択されるようにする

            dgv.ContextMenuStrip = this.contextMenuStrip1;

            var ds = GetDataTable(cmbBoxPath.Text);
            if (ds == null)
            {
                return;
            }
            dgv.DataSource = ds;

            dgv.Columns[LIST_DGV_ZIP_CLM_STAR].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns[LIST_DGV_ZIP_CLM_IDX].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_PAGE_NOW].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_PAGE_TOTAL].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_PERCENT].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[LIST_DGV_ZIP_CLM_RATING].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private DataTable GetDataTable(string path)
        {
            if (!Directory.Exists(path))
            {
                //指定されたパスのディレクトリが存在しなければだめ
                MessageBox.Show($"'{path}'は存在しないかディレクトリではありません",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

            var fileArray = Directory.GetFiles(
                            path,
                            "*.*",
                            SearchOption.AllDirectories);
            var files = fileArray.Where(
                f => String.Compare(Path.GetExtension(f), ".zip", true) == 0
            );

            var typeint32 = Type.GetType("System.Int32");

            var dt = new DataTable();

            var col_idx = dt.Columns.Add(LIST_DGV_ZIP_CLM_IDX, typeint32);
            var col_star = dt.Columns.Add(LIST_DGV_ZIP_CLM_STAR);

            var col_page_total = dt.Columns.Add(LIST_DGV_ZIP_CLM_PAGE_TOTAL, typeint32);
            var col_page_now = dt.Columns.Add(LIST_DGV_ZIP_CLM_PAGE_NOW, typeint32);
            var col_percent = dt.Columns.Add(LIST_DGV_ZIP_CLM_PERCENT, typeint32);

            var col_thumbnail = dt.Columns.Add(LIST_DGV_ZIP_CLM_THUMB);
            col_thumbnail.DataType = System.Type.GetType("System.Byte[]");
            col_thumbnail.AllowDBNull = true;

            var col_name = dt.Columns.Add(LIST_DGV_ZIP_CLM_NAME);
            var col_rating = dt.Columns.Add(LIST_DGV_ZIP_CLM_RATING, typeint32);

            var col_filename = dt.Columns.Add(LIST_DGV_ZIP_CLM_FILENAME);
            var col_path = dt.Columns.Add(LIST_DGV_ZIP_CLM_PATH);

            int i = 0;
            foreach (var f in files.OrderBy(x => x))
            {
                i++;

                var newRow = dt.NewRow();
                newRow.SetField(col_idx, i);
                newRow.SetField(col_star, "");

                if (list_thumnail)
                {
                    var ba = MyFiles.GetThumbnailByteArray(f, LIST_THUMBNAIL_WIDTH, LIST_THUMBNAIL_HEIGHT, 1);
                    newRow.SetField(col_thumbnail, ba);
                }

                var filename = System.IO.Path.GetFileName(f);
                newRow.SetField(col_filename, filename);
                newRow.SetField(col_path, f);

                var info = DirItem.GetInfoFromFilename(filename);
                newRow.SetField(col_name, info.Name);
                newRow.SetField(col_rating, info.Rating);

                newRow.SetField(col_page_total, info.PageTotal);
                newRow.SetField(col_page_now, 0);
                newRow.SetField(col_percent, 0);

                dt.Rows.Add(newRow);
            }

            return dt;
        }

        private void StartDgvRow_(DataGridViewRow row)
        {
            var idx = this.DirListDGV.Columns[LIST_DGV_ZIP_CLM_PATH].Index;

            var filepath = (string)row.Cells[idx].Value;
            StartFilePath(filepath);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            /*MessageBox.Show("test h",
                "test t",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);*/
            //foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                //Console.Error.WriteLine(r.Index);
            }
        }

        private void AddTwtDir(string screen_name)
        {
            var path = Twt.GetArchiveDirPath(screen_name);
            Log.trc($"'{screen_name}' => '{path}'");
            if (path != "")
            {
                cmbBoxPath.Text = path;
                cmbBoxPath.Items.Add(path);
            }

            path = Twt.GetCurrentDirPath(screen_name);
            Log.trc($"'{screen_name}' => '{path}'");
            if (path != "")
            {
                cmbBoxPath.Items.Add(path);
            }
        }

        private void TwtDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            TwtDataGridView.Rows[e.RowIndex].Selected = true;

            var row = TwtDataGridView.Rows[e.RowIndex];
            var screen_name = (string)row.Cells[1].Value;

            AddTwtDir(screen_name);
        }


        private void AddPxvDir(long pxv_usr_id)
        {
            var path = Pxv.GetArchiveDirPath(pxv_usr_id);
            Log.trc($"'{pxv_usr_id}' => '{path}'");
            if (path != "")
            {
                cmbBoxPath.Text = path;
                cmbBoxPath.Items.Add(path);
            }

            path = Pxv.GetCurrentDirPath(pxv_usr_id);
            Log.trc($"'{pxv_usr_id}' => '{path}'");
            if (path != "")
            {
                cmbBoxPath.Items.Add(path);
            }
        }

        private void DgvPxv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DgvPxv.Rows[e.RowIndex].Selected = true;

            var row = DgvPxv.Rows[e.RowIndex];
            var pxv_usr_id = (long)row.Cells[1].Value;
            AddPxvDir(pxv_usr_id);
        }

        private void StartFilePath(string filepath)
        {
            Log.trc($"'{filepath}' => '{filepath}'");
            cmbBoxPath.Text = filepath;
            //cmbBoxPath.Items.Add(filepath);
            Start();
        }

        private void StartDgvRow(DataGridViewRow row)
        {
            StartDgvRow_(row);
        }

        private void DirListDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DirListDGV.Rows[e.RowIndex].Selected = true;

            var row = DirListDGV.Rows[e.RowIndex];
            StartDgvRow(row);
        }

        private void ToolStripMenuItem_SelStt_Click(object sender, EventArgs e)
        {
            try
            {
                var idx = DirListDGV.CurrentCell.RowIndex;
                var row = DirListDGV.Rows[idx];
                StartDgvRow(row);
            }
            catch (Exception ex)
            {
                Log.trc(ex.ToString());
            }
            finally { }
        }

        private void ToolStripMenuItem_test_Click(object sender, EventArgs e)
        {
            DataTable dt2 = (DataTable)DirListDGV.DataSource;
            DataRow dr2 = dt2.Rows[DirListDGV.CurrentRow.Index];
            var row_idx = DirListDGV.CurrentRow.Index;

            var bm = DirListDGV.BindingContext[DirListDGV.DataSource, DirListDGV.DataMember];
            var drv = (DataRowView)bm.Current;
            var dr = drv.Row;

            DirListDGV.Rows[row_idx].Selected = true;

            var row = DirListDGV.Rows[row_idx];
            //row.SetField(cl, "★");

            var datasource = (DataTable)this.DirListDGV.DataSource;
            //var cl = datasource.Columns[4];
            //datasource.Rows[0].SetField(cl, "★");
            var cl = datasource.Columns[1];

            string val = datasource.Rows[row_idx].Field<string>(cl);

            datasource.Rows[row_idx].SetField(cl, "★");

        }

        private void ToolStripMenuItem_SameHashFile_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            Start(null, FILTER_TYPE.FILTER_HASH);
            Log.trc("[E]");
        }

        private void toolStripMenuItem_TsvRead_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");

            mTsvRowList = new TsvRowList(@"D:\download\del_list.tsv");

            HashSet<long> pxvids = new();
            HashSet<string> screen_names = new();

            foreach (var x in mTsvRowList.GetRowList())
            {
                var filename = Path.GetFileName(x.FileName);
                if (filename.StartsWith("px-"))
                {
                    var pxvid = PictureModel.GetPxvIdFromPath(filename);
                    if (pxvid != 0)
                    {
                        pxvids.Add(pxvid);
                    }
                }
                else if (filename.StartsWith("tw-"))
                {
                    var screen_name = Twt.GetScreenNameFromDirName(filename);
                    if (screen_name != null)
                    {
                        screen_names.Add(screen_name);
                    }
                }
                else
                {
                }
            }

            foreach (var pxvid in pxvids)
            {
                //Log.trc($"{pxvid}");
                AddPxvDir(pxvid);
            }

            foreach (var screen_name in screen_names)
            {
                //Log.trc($"{screen_name}");
                AddTwtDir(screen_name);
            }

            Log.trc("[E]");
        }

        private void btnOpenExplorer_Click(object sender, EventArgs e)
        {
            var opt = cmbBoxPath.Text;
            MyFiles.OpenGuiShell(opt);
        }

        private void btnGroupListStart_Click(object sender, EventArgs e)
        {
            var pathStr = cmbBoxPath.Text;
            var model = new PictureModel();
            model.BuildFileList(pathStr);

            PictureForm picForm = new();
            picForm.Text = $"{pathStr} [{cmbBoxPath.Items.IndexOf(cmbBoxPath.Text)}/{cmbBoxPath.Items.Count}]";
            picForm.SetModel(model);
            picForm.ShowDialog();
        }
    }
}
