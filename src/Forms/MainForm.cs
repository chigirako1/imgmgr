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

namespace PictureManagerApp
{
    public partial class MainForm : Form
    {
        private const int MAX_PIC_WIDTH = 1200 / 2;
        private const int MAX_PIC_HEIGHT = 1920 / 2;
        private const int MAX_FILE_SIZE = 140;

        private static readonly string[] CMBBOX_DIR_PATHS = [
            @"D:\download\PxDl",
            @"D:\download\PxDl-",
            @"D:\download\PxDl-0trash",
            @"D:\dl\AnkPixiv\Twitter",
            @"D:\dl\AnkPixiv\Twitter-0trash",
        ];

        private static readonly string[] SP_WORDS = [
            "-w2x",
            "-cnv",
            "",
        ];

        private int Radius { get; set; }
        private int Ox { get; set; }
        private int Oy { get; set; }

        private FileList mFavFileList = new();

        private DirList mDirList = new();

        private DataTable datatable { get; set; }



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
            cmbBoxPath.Text = path1;

            foreach (string path in CMBBOX_DIR_PATHS)
            {
                cmbBoxPath.Items.Add(path);
            }


            var dir = System.Environment.CurrentDirectory;
            dir = Directory.GetParent(dir).ToString();
            dir = Directory.GetParent(dir).ToString();
            cmbBoxPath.Items.Add(dir);
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
                this.Location = new Point(this.Location.X, this.Location.Y - 100);
            }
            else
            {
                //縦長画面の場合
                this.Width = w - 200;
                this.Height = h - 300;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(100, 100);
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

        private void Start(FileList filelist = null)
        {
            Log.trc($"[S]");
            var pathStr = cmbBoxPath.Text;

            var model = new PictureModel();
            setModelParam(model);
            try
            {
                // TODO: progress bar
                //...

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
                picForm.SetModel(model);
                picForm.ShowDialog();
                Log.trc($"picForm.ShowDialog() end");



                var pics = model.GetSelectedPic();
                foreach (var pic in pics)
                {
                    var fitem = (FileItem)pic;
                    mFavFileList.Add(fitem);
                }

                //mDirListの更新
                //...ページ数、サムネイル
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

                    DirItem diritem = new(f);
                    mDirList.Add(diritem);
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
            cmbBoxPath.SelectedIndex = idx;
            Start();
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
                cmbBoxPath.Text = dirpath;
                cmbBoxPath.Items.Add(dirpath);
            }
        }

        private void ToolStripMenuItem_SelPicShow_Click(object sender, EventArgs e)
        {
            Start(mFavFileList);
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabc = (TabControl)sender;
            var txt = tabc.SelectedTab.Text;
            if (txt == "DGM")
            {
                //string dbPath = Application.StartupPath + @"\Data.db";
                string DATA_SRC_FILENAME = "development.sqlite3";
                string DATA_SRC_PATH = @"D:\data\src\ror\myapp\db\" + DATA_SRC_FILENAME;
                var dbPath = DATA_SRC_PATH;

                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbPath))
                {
                    con.Open();

                    //空のテーブルを作ります。
                    //この時点では、DataGridViewと紐づいていません。
                    this.datatable = new DataTable();

                    //DataTableに読み込むデータをSQLで指定します。
                    //今回はDataTableを指定していないので、SELECTで表示する列名が
                    //のちのち紐づけを行った際のDataGridViewの列名になります。
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT twtid, twtname FROM twitters;", con);
                    adapter.Fill(this.datatable);

                    //データテーブルをDataGridViewに紐づけます。
                    this.dataGridView1.DataSource = this.datatable;
                }
            }
        }
    }
}
