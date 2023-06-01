using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;

namespace PictureManagerApp
{
    public partial class MainForm : Form
    {
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
            txtPath.Text = @"";

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

        private void btnStart_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            string pathStr = txtPath.Text;

            DateTime? dtFrom = null;// DateTime.MinValue;
            DateTime? dtTo = null;// DateTime.Now; ;
            if (chkBox_from.Checked)
            {
                DateTime date = dtPicker_from.Value;
                dtFrom = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            }
            if (chkBox_to.Checked)
            {
                DateTime date = dtPicker_to.Value;
                dtTo = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }

            PictureModel model = new PictureModel();
            model.setDate(dtFrom, dtTo);
            try
            {
                model.buildFileList(pathStr);
                // TODO: progress 

                PictureForm picForm = new PictureForm();
                picForm.SetModel(model);
                picForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.trc("{1}", ex.ToString());
                MessageBox.Show("入力されたパスが不正です",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Log.trc("[E]");
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            txtPath.Text = Clipboard.GetText();
            Log.trc("[E]");
        }

        private void EndBtn_Click(object sender, EventArgs e)
        {
            Log.trc("[S]");
            this.Close();
            Log.trc("[E]");
        }
    }
}
