using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using PictureManagerApp.src.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;

namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        //=====================================================================
        // const
        //=====================================================================
        private const int TRANSITION_FPS = 60;
        private const int TRANSITION_TIMER_PERIOD = 1000 / TRANSITION_FPS;
        private const int TRANSITION_DUE_TIME = 500;
        //private const int THUMBNAIL_TIMER_PERIOD = 250;
        private const int THUMBNAIL_TIMER_PERIOD = 150;
        private static Brush BRUSH_MARK = Brushes.DarkRed;
        private static Brush BG_BRUSH = Brushes.Black;
        private static Color COLOR_MARK = Color.Red;

        private int Col = 4;
        private int Row = 3;

        //=====================================================================
        // delegate
        //=====================================================================
        private delegate bool KeyDownFunc(object sender, KeyEventArgs e);

        //=====================================================================
        // field
        //=====================================================================
        private PictureModel mModel;
        private Boolean mFullscreen = false;
        private Image mCurrentImg, mPrevImg;
        private IMAGE_DISPLAY_MAGNIFICATION_TYPE mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND;
        private int mAlphaPercent = 0;
        private int startTickCnt;
        private Boolean mTransitionEffect = false;
        private System.Threading.Timer mTransitionTimer;
        private System.Threading.Timer mThumbnailTimer;

        private Dictionary<Keys, KeyDownFunc> KeyFuncTbl = new Dictionary<Keys, KeyDownFunc>();

        //=====================================================================
        // public
        //=====================================================================
        public PictureForm()
        {
            Log.trc($"[S]");
            InitializeComponent();

            InitKeys();

            Log.trc($"[E]");
        }

        public void SetModel(PictureModel model)
        {
            mModel = model;
            mModel.UpDownCount = Col;
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

            mTransitionTimer = new System.Threading.Timer(TimerTickTransition, null, 0, TRANSITION_TIMER_PERIOD);
            mTransitionTimer.Change(Timeout.Infinite, Timeout.Infinite);

            mThumbnailTimer = new System.Threading.Timer(TimerTickThumbnail, null, 0, THUMBNAIL_TIMER_PERIOD);
            mThumbnailTimer.Change(Timeout.Infinite, Timeout.Infinite);
            mThumbnailTimer.Change(0, THUMBNAIL_TIMER_PERIOD);


            UpdatePicture();

            Log.trc($"[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mTransitionTimer != null)
            {
                //mTimer.Stop();
                mTransitionTimer.Dispose();
                Log.trc("timer dispose");
            }

            if (mThumbnailTimer != null)
            {
                mThumbnailTimer.Dispose();
                Log.trc("timer dispose");
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_Resize(object sender, EventArgs e)
        {
            Log.trc($"[S]");

            if (this.WindowState == FormWindowState.Minimized)
            {
                //最小化されたときは何もしない
                //nop
            }
            else
            {
                if (this.Width < 1000)
                {
                    pictureBox.Width = (int)(this.Width * 0.8);
                    Log.trc($"picbox w={pictureBox.Width}, this w={this.Width}");
                }
                //pictureBox.Size = this.ClientSize;
            }
            picBoxUpdate();
            Log.trc($"[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------

        private void InitKeys()
        {
            KeyFuncTbl[Keys.Left] = KeyDownFunc_Left;
            KeyFuncTbl[Keys.Right] = KeyDownFunc_Right;
            KeyFuncTbl[Keys.Up] = KeyDownFunc_Up;
            KeyFuncTbl[Keys.Down] = KeyDownFunc_Down;
            KeyFuncTbl[Keys.Home] = KeyDownFunc_Home;
            KeyFuncTbl[Keys.End] = KeyDownFunc_End;
            KeyFuncTbl[Keys.NumPad0] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Space] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Escape] = KeyDownFunc_Escape;
        }


        private bool KeyDownFunc_Left(object sender, KeyEventArgs e)
        {
            mModel.Prev();
            return true;
        }

        private bool KeyDownFunc_Right(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                mModel.NextMarkedImage();
            }
            else
            {
                mModel.Next();
            }
            return true;
        }

        private bool KeyDownFunc_Up(object sender, KeyEventArgs e)
        {
            mModel.Up();
            return true;
        }

        private bool KeyDownFunc_Down(object sender, KeyEventArgs e)
        {
            mModel.Down();
            return true;
        }

        private bool KeyDownFunc_Home(object sender, KeyEventArgs e)
        {
            mModel.MovePos(POS_MOVE_TYPE.MOVE_HOME);
            return true;
        }

        private bool KeyDownFunc_End(object sender, KeyEventArgs e)
        {
            mModel.MovePos(POS_MOVE_TYPE.MOVE_LAST);
            return true;
        }

        private bool KeyDownFunc_SelectToggle(object sender, KeyEventArgs e)
        {
            mModel.toggleMark();
            mModel.Next();
            return true;
        }

        private bool KeyDownFunc_Escape(object sender, KeyEventArgs e)
        {
            if (mFullscreen)
            {
                ToggleFulscreen();
                return false;
            }

            if (mModel.mMarkCount == 0)
            {
                Close();
                return false;
            }

            DialogResult result = MessageBox.Show("選択したファイルを移動しますか？",
                "移動？",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);
            if (result == DialogResult.Yes)
            {
                mModel.Batch();
            }
            if (result != DialogResult.Cancel)
            {
                Close();
            }

            return false;
        }

        private void PictureForm_KeyDown(object sender, KeyEventArgs e)
        {
            Log.trc($"[S]:{e.KeyCode}");
            if (KeyFuncTbl.ContainsKey(e.KeyCode))
            {
                if (KeyFuncTbl[e.KeyCode](sender, e))
                {
                    UpdatePicture();
                }
            }
            Log.trc($"[E]");
        }

        private void KeyDownFunc_(object sender, KeyEventArgs e)
        {
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void UpdatePicture()
        {
            Log.trc("[S]");
            if (mPrevImg != null)
            {
                //mPrevImg.Dispose();
            }
            mPrevImg = mCurrentImg;

            var fitem = mModel.GetCurrentFileItem();
            var img = fitem.GetImage();
            mCurrentImg = img;

            SetPic();
            if (mFullscreen == false)
            {
                SetStatusBar(fitem, img);
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
            //Log.trc("[S]");
            Refresh();
            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetPic()
        {
            Log.trc("[S]");
            startTickCnt = 0;
            mAlphaPercent = 0;

            int period = TRANSITION_TIMER_PERIOD;

            //picTimer.Interval = interval;//10;
            //picTimer.Start();
            if (mTransitionEffect)
            {
                mTransitionTimer.Change(0, period);
            }
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetStatusBar(FileItem fitem, Image img)
        {
            SetStatusBar_ProgressBar();
            SetStatusBar_FileNo();
            SetStatusBar_FileInfo(fitem);
            SetStatusBar_ImageSize(img);
            SetStatusBar_ImageRatio(img);
            SetStatusBar_SelectedItemCount();
        }

        private void SetStatusBar_ProgressBar()
        {
            toolStripProgressBar.Value = (mModel.PictureNumber + 1) * 100 / mModel.PictureTotalNumber;
        }

        private void SetStatusBar_FileNo()
        {
            string no = String.Format("{0,5}/{1,5}", mModel.PictureNumber + 1, mModel.PictureTotalNumber);
            statusLbl_No.Text = no;

            /*if (mModel.PictureNumber == 0)
            {
                statusLbl_No.BackColor = Color.Red;
            }
            else
            {
                statusLbl_No.BackColor = Color.Gray;
            }*/
        }

        private void SetStatusBar_FileInfo(FileItem fitem)
        {
            StatusLbl_Dirname.Text = fitem.GetRelativePath(mModel.WorkingRootPath);
            statusLbl_Filename.Text = Path.GetFileName(fitem.FilePath);

            if (fitem.IsZipEntry)
            {

            }
            else
            {
                FileInfo fi = new FileInfo(fitem.FilePath);
                statusLbl_FileSize.Text = String.Format("{0:#,0} Bytes", fi.Length);
                StatusLbl_LWTime.Text = fi.LastWriteTime.ToShortDateString();
            }
        }

        private void SetStatusBar_ImageSize(Image img)
        {
            string imagesize = "";
            if (img != null)
            {
                imagesize = img.Width + "x" + img.Height;
            }
            statusLbl_WxH.Text = imagesize;
        }

        private void SetStatusBar_ImageRatio(Image img)
        {
            string imageratio = "";
            if (img != null)
            {
                imageratio = "?";// img.Width + "x" + img.Height;
            }
            statusLbl_ratio.Text = imageratio;
        }

        private void SetStatusBar_SelectedItemCount()
        {
            StatusLbl_MarkCnt.Text = mModel.mMarkCount.ToString();
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
        private void TimerTickTransition(object state)
        {
            Log.trc("[S]");

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

                int incVal = (delta * 100) / TRANSITION_DUE_TIME;
                mAlphaPercent = incVal;
                Log.log($"mAlphaPercent={mAlphaPercent}");
                if (100 <= mAlphaPercent)
                {
                    mAlphaPercent = 100;
                    mTransitionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }

            UI_change();
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void TimerTickThumbnail(object state)
        {
            //Log.trc("[S]");

            int tickCnt = Environment.TickCount & Int32.MaxValue;
            if (startTickCnt == 0)
            {
                startTickCnt = tickCnt;
            }
            else
            {
                var tsize = GetThumbnailSize();
                var done = mModel.MakeThumbnail(tsize.Width, tsize.Height);
                if (done)
                {
                    Log.log("done");
                    mThumbnailTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
            if (this != null)
            {
                UI_change();
            }

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

        private void x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Col = 2;
            this.Row = 2;
            mModel.UpDownCount = this.Col;
            Refresh();
        }

        private void x4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Col = 4;
            this.Row = 3;
            mModel.UpDownCount = this.Col;
            Refresh();
        }

        private void TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColRowForm crForm = new ColRowForm(this.Col, this.Row);
            DialogResult result = crForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                (int col, int row) = crForm.GetColRow();
                this.Col = col;
                this.Row = row;
                mModel.UpDownCount = this.Col;

                mMagType = crForm.GetMagType();

                Refresh();
            }
        }

        private void ToolStripMenuItem_SelImg_Click(object sender, EventArgs e)
        {

            if (mModel.mMarkCount == 0)
            {
                MessageBox.Show("no selected file",
                    "file 0",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            PictureModel model = mModel.DuplicateSelectOnly();

            PictureForm picForm = new PictureForm();
            picForm.SetModel(model);
            picForm.ShowDialog();

            //TODO: 選択が解除されていたらこちらのほうの選択も解除する
            //picForm.
        }

        private void ToolStripMenuItem_fwd_one_Click(object sender, EventArgs e)
        {
            mModel.ChangeOrderForward();
            Refresh();
        }


        private void ToolStripMenuItem_PathCopy_Click(object sender, EventArgs e)
        {
            FileItem fitem = mModel.GetCurrentFileItem();
            string path = fitem.FilePath;
            Clipboard.SetText(path);
        }

        private void ToolStripMenuItem_SelectedImageMove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("選択中のファイルを移動しますか？",
                "移動？",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                mModel.Batch();
                UpdatePicture();
            }
        }

        private void ToolStripMenuItem_ThumbnailOn_Click(object sender, EventArgs e)
        {
            if (ToolStripMenuItem_ThumbnailOn.Checked)
            {
                rightPicBox.Visible = true;
            }
            else
            {
                rightPicBox.Visible = false;
                rightPicBox.Width = 0;
                rightPicBox.Height = 0;
            }
        }

        private void MenuItem_MagSub_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_CopyParentDirPath_Click(object sender, EventArgs e)
        {
            FileItem fitem = mModel.GetCurrentFileItem();
            string parentPath = Path.GetDirectoryName(fitem.FilePath);
            Clipboard.SetText(parentPath);
        }
    }
}
