using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;

namespace PictureManagerApp
{
    public partial class MainForm : Form
    {
        private static string PATH = @"F:\download\PxDl";

        private int Radius { get; set; }
        private int Ox { get; set; }
        private int Oy { get; set; }

        public MainForm()
        {
            Log.trc($"[S]");

            InitializeComponent();
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

            if (args.Count() > 1)
            {
                cmbBoxPath.Text = args[1];
            }
            else
            {
                cmbBoxPath.Text = PATH;
            }
            cmbBoxPath.Items.Add(cmbBoxPath.Text);

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
            string pathStr = cmbBoxPath.Text;

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

            PictureModel model = new PictureModel();
            model.SetDate(dtFrom, dtTo);
            try
            {
                model.BuildFileList(pathStr);
                // TODO: progress 

                PictureForm picForm = new PictureForm();
                picForm.SetModel(model);
                picForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.trc("{1}", ex.ToString());
                MessageBox.Show(ex.ToString(),//"入力されたパスが不正です",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
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
    }
}
