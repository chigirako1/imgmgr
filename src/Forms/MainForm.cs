using System;
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
        private static readonly string[] PATH = [
            //@"F:\download\PxDl",
            //@"F:\download\PxDl-0trash",
            @"D:\download\PxDl",
            @"D:\download\PxDl-0trash",
            @"D:\dl\AnkPixiv\Twitter",
            @"D:\dl\AnkPixiv\Twitter-0trash",
        ];

        private int Radius { get; set; }
        private int Ox { get; set; }
        private int Oy { get; set; }

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

            if (args.Length > 1)
            {
                cmbBoxPath.Text = args[1];
            }
            else
            {
                cmbBoxPath.Text = PATH[0];
            }

            foreach (string arg in PATH)
            {
                cmbBoxPath.Items.Add(arg);
            }
           

            cmbBox_FilenameFilter.Text = "-w2x";
            cmbBox_FilenameFilter.Items.Add(cmbBox_FilenameFilter.Text);
            cmbBox_FilenameFilter.Text = "";
            cmbBox_FilenameFilter.Items.Add(cmbBox_FilenameFilter.Text);

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
            Log.trc($"[E]");
        }

        private void Start()
        {
            var pathStr = cmbBoxPath.Text;

            var model = new PictureModel();
            setModelParam(model);
            try
            {
                if (txtBox_FileList.Text == "")
                {
                    model.BuildFileList(pathStr);
                }
                else
                {
                    model.BuildFileListFromText(txtBox_FileList.Text, pathStr);
                }
                // TODO: progress bar

                PictureForm picForm = new();
                picForm.SetModel(model);
                picForm.ShowDialog();
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
                }
            }


            Log.trc("[E]");
        }

        private void cmbBoxPath_DragDrop(object sender, DragEventArgs e)
        {
            Log.trc("[S]");

            // ファイルが渡されていなければ、何もしない
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chkListBox_Ext_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPicSizeToggle_Click(object sender, EventArgs e)
        {
            var width = (int)numUD_Width.Value;
            var height = (int)numUD_Height.Value;

            if (width == 0 && height == 0)
            {
                numUD_Width.Value = 1200 / 2;
                numUD_Height.Value = 1920 / 2;
            }
            else
            {
                numUD_Width.Value = 0;
                numUD_Height.Value = 0;
            }
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            DateTime dt = dtPicker_from.Value;
            dtPicker_from.Value = dt.AddDays(1);
            Start();
        }
    }
}
