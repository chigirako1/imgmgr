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
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PictureManagerApp
{
    public partial class MainForm : Form
    {
        private const int MAX_PIC_WIDTH = 1600 / 2;//1200 / 2;
        private const int MAX_PIC_HEIGHT = 1920 / 2;
        private const int MAX_FILE_SIZE = 0;//165;//140;//kb

        private const int DGV_COL_IDX_STAR = 1;
        private const int DGV_COL_IDX_PAGE_NOW = 3;
        private const int DGV_COL_IDX_PERCENT = 4;

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

        private Config Config { get; set; }

        //--------------
        //
        //--------------
        public MainForm(Config cfg)
        {
            Log.trc($"[S]");

            InitializeComponent();

            this.Config = cfg;

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
            InitPathCmbBox();

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

        private void InitPathCmbBox()
        {
            var cfgPath = this.Config.LastPath;
            if (cfgPath != null)
            {
                if (Directory.Exists(cfgPath))
                {
                    cmbBoxPath.Items.Add(cfgPath);
                }
            }

            var list = PictureModel.GetDirPathList();
            foreach (string path in list)
            {
                if (Directory.Exists(path))
                {
                    cmbBoxPath.Items.Add(path);
                }
#if DEBUG
                else if (System.IO.File.Exists(path))
                {
                    cmbBoxPath.Items.Add(path);
                }
#endif
            }

            if (cmbBoxPath.Items.Count > 0)
            {
                cmbBoxPath.Text = (string)cmbBoxPath.Items[0];
            }
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

                    var di = mDirList.GetDirItem(pathStr);
                    if (di == null)
                    {
                        var diritem = new DirItem(pathStr);
                        mDirList.Add(diritem);
                    }
                    else
                    {
                        try
                        {
                            model.SetCurrentFileIndex(di.PageNo);
                        }
                        catch (Exception ex)
                        {
                            Log.log($"{ex}/{di.PageNo}");
                            MessageBox.Show(ex.ToString(),
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    model.BuildFileListFromText(txtBox_FileList.Text, pathStr);
                }

                if (model.HasInvalidFile())
                {
                    MessageBox.Show("不正なファイルが含まれています",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }


                var picForm = new PictureForm();
                picForm.Text = $"{pathStr} [{cmbBoxPath.Items.IndexOf(cmbBoxPath.Text)}/{cmbBoxPath.Items.Count}]";
                picForm.SetModel(model);
                picForm.ShowDialog();
                Log.trc($"picForm.ShowDialog() end");

                bool desktop;
                if (Config.DelListSavePos == "desktop")
                {
                    desktop = true;
                }
                else
                {
                    desktop = false;
                }
                model.UpdateListFile(desktop);

                //選択されたファイルを記録する
                var pics = model.GetSelectedPic();
                foreach (var pic in pics)
                {
                    var fitem = (FileItem)pic;
                    mFavFileList.Add(fitem);
                }

                if (filelist == null)
                {
                    mDirList.Update(model);
                }
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

            model.SetDelListPath("");

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

            var pixel = (int)numUD_Pixel.Value;
            if (pixel != 0)
            {
                model.SetMaxPixelCount(pixel);
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
            else if (radioBtn_PicOrinet_Square.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_SQUARE);
            }
            else if (radioBtn_PicOrinet_Long.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_LONG);
            }
            else if (radioBtn_PicOrinet_Custom.Checked)
            {
                model.SetPicOrient(PIC_ORIENT_TYPE.PIC_ORINET_CUSTOM);
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

            var path = cmbBoxPath.Text;
            var cfg = new Config();
            cfg.SavePath(path);
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
            string path = cmbBoxPath.Text;
            Log.trc($"[S]'{path}'");
            if (!Directory.Exists(path))
            {
                MessageBox.Show("存在しないディレクトリです",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            cmbBoxPath.Items.Add("---区切り---");

            //ファイルの追加
            var files = FileList.EnumerateFiles(path);
            foreach (var f in files.OrderBy(x => x))
            {
                {
                    cmbBoxPath.Items.Add(f);

                    var diritem = new DirItem(f);
                    mDirList.Add(diritem);
                }
            }

            cmbBoxPath.Items.Add("---区切り---");

            // ディレクトリの登録
            bool sort;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                sort = true;
                Log.trc("ファイル数ソート");
            }
            else
            {
                sort = false;
            }
            var dirs = DirList.EnumerateDirectories(path, sort);
            foreach (var dir in dirs)
            {
                cmbBoxPath.Items.Add(dir);
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
                string path = cmbBoxPath.Text;

                if (!Directory.Exists(path))
                {
                    //指定されたパスのディレクトリが存在しなければだめ
                    MessageBox.Show($"'{path}'は存在しないかディレクトリではありません",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                InitZipListDGV(path);
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
            this.mPxvDatatable = new DataTable();

            Dgv.InitPxvDGV(this.mPxvDatatable);
            this.DgvPxv.DataSource = this.mPxvDatatable;
        }

        private void InitZipListDGV(string path)
        {

            var dgv = this.DirListDGV;

            dgv.ContextMenuStrip = this.contextMenuStrip1;
            var list_thumbnail = false;

            string[] patterns = { ".zip" };
            var zipfiles = MyFiles.GetAllFiles(path, patterns);
            Dgv.InitiZipList(zipfiles, this.DirListDGV, list_thumbnail);
        }

        private void StartDgvRow_(DataGridViewRow row)
        {
            var fullpath = Dgv.GetPath(this.DirListDGV, row);
            StartFilePath(fullpath);

            UpdateDgv(fullpath);
        }

        private void UpdateDgv(string fullpath)
        {
            var di = mDirList.GetDirItem(fullpath);
            if (di != null)
            {
                var row_idx = GetRowIdx();

                var faved = false;
                if (faved)
                {
                    Dgv.SetColumnValue(DirListDGV, row_idx, DGV_COL_IDX_STAR, "test");
                }

                var pn = (di.PageNo + 1);
                var percent = pn * 100 / di.TotalPageNo;

                Dgv.SetColumnValue(DirListDGV, row_idx, DGV_COL_IDX_PAGE_NOW, pn.ToString());
                Dgv.SetColumnValue(DirListDGV, row_idx, DGV_COL_IDX_PERCENT, percent.ToString());

                Dgv.SetThumbnail(DirListDGV, row_idx, fullpath, pn - 1);
            }
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

        private int GetRowIdx()
        {
            var row_idx = Dgv.GetRowIdx(DirListDGV);
            return row_idx;
        }

        private void SetColumnValue(int row_idx, int col_idx, string chara)
        {
            Dgv.SetColumnValue(DirListDGV, row_idx, col_idx, chara);
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

        private void ToolStripMenuItem_FileDel_Click(object sender, EventArgs e)
        {
            //var row_idx = GetRowIdx();
            var idx = DirListDGV.CurrentCell.RowIndex;
            var row = DirListDGV.Rows[idx];
            var fullpath = Dgv.GetPath(this.DirListDGV, row);



            var msg = $"ファイルを削除しますか？'{fullpath}'";
            var title = "ファイル削除?" + Path.GetFileName(fullpath);
            var result = MessageBox.Show(msg,
                        title,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                Log.log(msg);
            }
        }

        private void ToolStripMenuItem_test_Click(object sender, EventArgs e)
        {
            var row_idx = GetRowIdx();
            DirListDGV.Rows[row_idx].Selected = true;
            SetColumnValue(row_idx, DGV_COL_IDX_STAR, "★");
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

            try
            {
                mTsvRowList = PictureModel.MakeTsvRowList();//new TsvRowList(@"D:\download\del_list.tsv");
            }
            catch (Exception ex)
            {
                Log.log($"{ex}");
                MessageBox.Show(ex.ToString(),
                    "tsv エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }


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
                    Log.warning($"不明:'{filename}'");
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
