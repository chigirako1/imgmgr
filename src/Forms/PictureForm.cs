using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;
using System;
using System.Drawing;
using System.IO;
///using System.Management.Instrumentation;
using System.Threading;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        private const int FPS = 60;
        private const int TIMER_PERIOD = 1000 / FPS;
        private const int DUE_TIME = 500;

        private const int ROW = 3;
        private const int COL = 3;

        //=====================================================================
        // field
        //=====================================================================
        private PictureModel mModel;
        private Boolean mFullscreen = false;
        private Image mCurrentImg, mPrevImg;
        private int mAlphaPercent = 0;
        private int startTickCnt;
        private Boolean mTransitionEffect = false;
        private System.Threading.Timer mTimer;

        //=====================================================================
        // public
        //=====================================================================
        public PictureForm()
        {
            Log.trc($"[S]");
            InitializeComponent();
            Log.trc($"[E]");
        }

        public void SetModel(PictureModel model)
        {
            mModel = model;
            mModel.UpDownCount = COL;
        }

        //=====================================================================
        // private
        //=====================================================================
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_Load(object sender, EventArgs e)
        {
            Log.trc($"[S]");
            this.KeyPreview = true;

            if (mModel.PictureTotalNumber == 0)
            {
                MessageBox.Show("no file",
                    "file 0",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            bool b = false; ///true;
            if (b)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }

            this.Width = 2900;
            this.Height = 1650;
            //this.StartPosition = FormStartPosition.CenterScreen;
            this.Top = 0;
            this.Left = 0;

            pictureBox.Width = 1300;

            mTimer = new System.Threading.Timer(TimerTick, null, 0, TIMER_PERIOD);
            mTimer.Change(Timeout.Infinite, Timeout.Infinite);

            UpdatePicture();
            Log.trc($"[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_Resize(object sender, EventArgs e)
        {
            Log.trc($"[S]");
            if (this.Width < 1600)
            {
                pictureBox.Width = (int)(this.Width * 0.8);
            }
            //pictureBox.Size = this.ClientSize;
            picBoxUpdate();
            Log.trc($"[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_KeyDown(object sender, KeyEventArgs e)
        {
            Log.trc($"[S]:{e.KeyCode}");
            switch (e.KeyCode)
            {
                case Keys.Left:
                    mModel.Prev();
                    UpdatePicture();//本来はモデルからの通知で動くべき（observer pattern)
                    break;
                case Keys.Right:
                    mModel.Next();
                    UpdatePicture();
                    break;
                case Keys.Up:
                    mModel.Up();
                    UpdatePicture();
                    break;
                case Keys.Down:
                    mModel.Down();
                    UpdatePicture();
                    break;
                case Keys.Home:
                    mModel.MovePos(POS_MOVE_TYPE.MOVE_HOME);
                    UpdatePicture();
                    break;
                case Keys.End:
                    mModel.MovePos(POS_MOVE_TYPE.MOVE_LAST);
                    UpdatePicture();
                    break;
                case Keys.NumPad0:
                    //mModel.RemoveCurrentFile();
                    mModel.toggleRemoveMark();
                    mModel.Next();
                    UpdatePicture();
                    break;
                case Keys.Escape:
                    if (mFullscreen)
                    {
                        ToggleFulscreen();
                    }
                    else
                    {
                        mModel.Batch();
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
            Log.trc($"[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void UpdatePicture()
        {
            Log.trc("[S]");
            if (mPrevImg != null)
            {
                mPrevImg.Dispose();
            }
            mPrevImg = mCurrentImg;

            string filepath = mModel.GetCurrentFilePath();
            Image img = GetImage(filepath);
            mCurrentImg = img;

            SetPic(img);
            if (mFullscreen == false)
            {
                SetStatusBar(filepath, img);
            }

            picBoxUpdate();

            //img.Dispose();
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void picBoxUpdate()
        {
            Log.trc("[S]");
            Refresh();
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private Image GetImage(string path)
        {
            return ImageModule.GetImage(path);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetPic(Image img)
        {
            Log.trc("[S]");
            startTickCnt = 0;
            mAlphaPercent = 0;

            int period = TIMER_PERIOD;

            //picTimer.Interval = interval;//10;
            //picTimer.Start();
            if (mTransitionEffect)
            {
                mTimer.Change(0, period);
            }
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetStatusBar(string filepath, Image img)
        {
            SetStatusBar_ProgressBar();
            SetStatusBar_FileNo();
            SetStatusBar_FileInfo(filepath);
            SetStatusBar_ImageSize(img);
            SetStatusBar_ImageRatio(img);
        }

        private void SetStatusBar_ProgressBar()
        {
            toolStripProgressBar.Value = (mModel.PictureNumber + 1) * 100 / mModel.PictureTotalNumber;
        }

        private void SetStatusBar_FileNo()
        {
            string no = String.Format("{0,5}/{1,5}", mModel.PictureNumber + 1, mModel.PictureTotalNumber);
            statusLbl_No.Text = no;
        }

        private void SetStatusBar_FileInfo(string filepath)
        {
            SetStatusBar_Dirname(filepath);
            SetStatusBar_Filename(filepath);
            SetStatusBar_FileSize(filepath);
            SetStatusBar_FileDateTime(filepath);
        }

        private void SetStatusBar_Dirname(string filepath)
        {
            Uri basepath = new Uri(mModel.Path + Path.DirectorySeparatorChar);
            string dirname = Path.GetDirectoryName(filepath); ;
            Uri dirUri = new Uri(dirname);

            Uri relUri = basepath.MakeRelativeUri(dirUri);

            StatusLbl_Dirname.Text = relUri.ToString();
        }

        private void SetStatusBar_Filename(string filepath)
        {
            statusLbl_Filename.Text = Path.GetFileName(filepath);
        }

        private void SetStatusBar_FileSize(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            //file.Name

            long filesize = file.Length;
            statusLbl_FileSize.Text = String.Format("{0:#,0} Bytes", filesize);
        }

        private void SetStatusBar_FileDateTime(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);

            string lwt = fi.LastWriteTime.ToShortDateString();
            StatusLbl_LWTime.Text = lwt;
        }

        private void SetStatusBar_ImageSize(Image img)
        {
            string imagesize = img.Width + "x" + img.Height;
            statusLbl_WxH.Text = imagesize;
        }

        private void SetStatusBar_ImageRatio(Image img)
        {
            string imageratio = img.Width + "x" + img.Height;
            statusLbl_ratio.Text = imageratio;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void MenuItem_FullScreen_Click(object sender, EventArgs e)
        {
            ToggleFulscreen();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void ToggleFulscreen()
        {
            if (mFullscreen)
            {
                // to normal
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                menuStrip.Visible = true;
                statusStrip.Visible = true;
                mFullscreen = false;
            }
            else
            {
                // to fullscreen
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                menuStrip.Visible = false;
                statusStrip.Visible = false;
                mFullscreen = true;
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void TimerTick(object state)
        {
            //Log.trc("[S]");

            int tickCnt = Environment.TickCount & Int32.MaxValue;
            if (startTickCnt == 0)
            {
                mAlphaPercent += 10;
                startTickCnt = tickCnt;
            }
            else
            {
                int delta = tickCnt - startTickCnt;
                Log.log($"delta={delta}");

                int incVal = (delta * 100) / DUE_TIME;
                mAlphaPercent = incVal;
                Log.log($"mAlphaPercent={mAlphaPercent}");
                if (100 <= mAlphaPercent)
                {
                    mAlphaPercent = 100;
                    mTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }

            UI_change();
            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void UI_change()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(UI_change));
                return;
            }
            picBoxUpdate();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void MenuItem_TransitionEffect_Click(object sender, EventArgs e)
        {
            if (MenuItem_TransitionEffect.Checked)
            {
                mTransitionEffect = MenuItem_TransitionEffect.Checked = false;
            }
            else
            {
                mTransitionEffect = MenuItem_TransitionEffect.Checked = true;
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mTimer != null)
            {
                //mTimer.Stop();
                mTimer.Dispose();
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            //Log.trc("[S]");
            Graphics g = e.Graphics;

            FileItem fitem = mModel.GetCurrentFileItem();
            Brush bgbrush;
            if (fitem.MarkRemove)
            {
                bgbrush = Brushes.Navy;
            }
            else
            {
                bgbrush = Brushes.Black;
            }
            g.FillRectangle(bgbrush, 0, 0, pictureBox.Width, pictureBox.Height);

            if (mCurrentImg == null)
            {
                Log.err("!!! image not found !!!");
                return;
            }

            int alphaPercent = 100;
            if (mTransitionEffect)
            {
                alphaPercent = mAlphaPercent;
            }

            DrawDimension d = ImageModule.DrawCompositedImage(
                g,
                pictureBox.Width,
                pictureBox.Height,
                mCurrentImg,
                mPrevImg,
                alphaPercent);


            // テキストの描画
            int fsize = 20;
            int x = 0;
            int y = 0;
            Brush txtbrush = Brushes.Aqua;
            Font fnt = new Font("MS ゴシック", fsize);

            //
            string txt = mModel.GetPictureInfoText();
            g.DrawString(txt, fnt, txtbrush, x, y);

            //
            y += fsize + 3;
            txt = string.Format("{0,4}x{1,4}", pictureBox.Width, pictureBox.Height);
            g.DrawString(txt, fnt, txtbrush, x, y);

            //
            y += fsize + 3;
            txt = string.Format("{0,4}x{1,4}", mCurrentImg.Width, mCurrentImg.Height);
            g.DrawString(txt, fnt, txtbrush, x, y);

            //
            y += fsize + 3;
            txt = string.Format("{0,4}x{1,4}({2}%)", d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1, d.ratio);
            g.DrawString(txt, fnt, txtbrush, x, y);


            fnt.Dispose();

            //Log.trc("[E]");
        }

        private void rightPicBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox p = rightPicBox;
            Graphics g = e.Graphics;
            //g.FillRectangle(Brushes.Beige, 0, 0, p.Width, p.Height);

            int col = COL;
            int row = ROW;

            int thumWidth = p.Width / col;
            int thumHeight = p.Height / row;

            int x = 0;
            int y = 0;
            for (int i = 0; i < (col * row); i++)
            {
                FileItem fitem = mModel.GetCurrentFileItemByRelativeIndex(i);
                Image img = fitem.GetImage();

                Brush bgbrush;
                if (fitem.MarkRemove)
                {
                    bgbrush = Brushes.Navy;
                }
                else
                {
                    bgbrush = Brushes.Black;
                }

                g.FillRectangle(bgbrush, x, y, thumWidth, thumHeight);

                DrawDimension d = ImageModule.DrawImage(
                    g,
                    x,
                    y,
                    thumWidth,
                    thumHeight,
                    img);

                if (img.Width * img.Height < 640 * 480)
                {
                    int fsize = 20;
                    //int txtx = 0;
                    //int txty = 0;

                    var opaqueBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
                    g.FillRectangle(opaqueBrush, x, y, thumWidth, thumHeight);

                    Brush txtbrush = Brushes.Red;
                    Font fnt = new Font("MS ゴシック", fsize);
                    string txt = string.Format("small({0,4}x{1,4})[{2,4}x{3,4}]", img.Width, img.Height, thumWidth, thumHeight);
                    g.DrawString(txt, fnt, txtbrush, x, y);
                }

                x += thumWidth;

                if ((i + 1) % col == 0)
                {
                    x = 0;
                    y += thumHeight;
                }
            }
        }
    }
}
