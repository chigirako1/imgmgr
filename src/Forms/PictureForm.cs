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
        //private const int THUMBNAIL_TIMER_PERIOD = 133;//100だと重くなる.200=OK
        private const int THUMBNAIL_TIMER_PERIOD = 140;
        private static readonly Brush BRUSH_0 = Brushes.Blue;
        private static readonly Brush BRUSH_MARK = Brushes.DarkRed;
        private static readonly Brush BG_BRUSH = Brushes.Black;
        private static readonly Color COLOR_MARK = Color.Red;

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
        private Boolean DisplayTxt = true;
        private Boolean NextPic = false;
        private Image mCurrentImg, mPrevImg;
        private IMAGE_DISPLAY_MAGNIFICATION_TYPE mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND;
        private int mThumbMs = THUMBNAIL_TIMER_PERIOD;
        private int mAlphaPercent = 0;
        private int startTickCnt;
        private Boolean mTransitionEffect = false;
        private System.Threading.Timer mTransitionTimer;
        private System.Threading.Timer mThumbnailTimer;

        private readonly Dictionary<Keys, KeyDownFunc> KeyFuncTbl = [];

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
            mModel.PageCount = this.Col * this.Row;
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

            bool b = false;
            //b = true;
            if (b)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }

            var w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            Log.trc($"{w}x{h}");
            bool landscape = h < w;
            if (mModel.IsZip())
            {
                //デバッグ用。zipのときは強制フルスクリーン
                landscape = false;
            }

            if (landscape)
            {
                this.Width = w - 100;
                this.Height = h - 50;

                pictureBox.Width = this.Width / 2;
            }
            else
            {
                //縦長フルスクリーン表示設定
                this.Width = w - 0;
                this.Height = h - 0;

                ToolStripMenuItem_ThumbnailOn.Checked = false;
                ThumbnailAreaView(false);
                ToggleFulscreen();
                mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN;
                DisplayTxt = false;
            }

            //this.StartPosition = FormStartPosition.CenterScreen;
            this.Top = 0;
            this.Left = 0;


            mTransitionTimer = new System.Threading.Timer(TimerTickTransition, null, 0, TRANSITION_TIMER_PERIOD);
            mTransitionTimer.Change(Timeout.Infinite, Timeout.Infinite);

            mThumbnailTimer = new System.Threading.Timer(TimerTickThumbnail, null, 0, mThumbMs);
            mThumbnailTimer.Change(Timeout.Infinite, Timeout.Infinite);
            mThumbnailTimer.Change(0, mThumbMs);

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
                if (RightPicBox.Visible) //this.Width < 1000)
                {
                    pictureBox.Width = (int)(this.Width * 0.8);
                    Log.trc($"picbox w={pictureBox.Width}, this w={this.Width}");

                    RightPicBox.Width = (int)(this.Width * 0.2);
                }
                else
                {
                    pictureBox.Size = this.ClientSize;
                }
                
            }
            PicBoxUpdate();
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

            KeyFuncTbl[Keys.PageUp] = KeyDownFunc_PageUp;
            KeyFuncTbl[Keys.PageDown] = KeyDownFunc_PageDown;

            KeyFuncTbl[Keys.Home] = KeyDownFunc_Home;
            KeyFuncTbl[Keys.End] = KeyDownFunc_End;

            KeyFuncTbl[Keys.NumPad0] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Space] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Escape] = KeyDownFunc_Escape;
            //KeyFuncTbl[Keys.F1] = ;
            //KeyFuncTbl[Keys.F2] = ;
            KeyFuncTbl[Keys.F3] = KeyDownFunc_F3;
            //KeyFuncTbl[Keys.F5] = ;
            //KeyFuncTbl[Keys.F9] = ;
        }


        private bool KeyDownFunc_Left(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                //mModel.PrevMarkedImage();
            }
            else if (e.Control)
            {
                mModel.PrevDirImage();
            }
            else
            {
                mModel.Prev();
            }
            return true;
        }

        private bool KeyDownFunc_Right(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                mModel.NextMarkedImage();
            }
            else if (e.Control)
            {
                mModel.NextDirImage();
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

        private bool KeyDownFunc_PageUp(object sender, KeyEventArgs e)
        {
            mModel.PageUp();
            return true;
        }

        private bool KeyDownFunc_PageDown(object sender, KeyEventArgs e)
        {
            mModel.PageDown();
            return true;
        }

        private bool KeyDownFunc_Home(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MovePos(POS_MOVE_TYPE.MOVE_HOME);
            }
            else
            {
                mModel.PrevDirImage();
            }
            return true;
        }

        private bool KeyDownFunc_End(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MovePos(POS_MOVE_TYPE.MOVE_LAST);
            }
            else
            {
                mModel.MoveToDirEndImage();
            }
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
            return WindowQuitOp();
        }

        private bool KeyDownFunc_F3(object sender, KeyEventArgs e)
        {
            NextPic = !NextPic;
            return true;
        }

        private void PictureForm_KeyDown(object sender, KeyEventArgs e)
        {
            Log.trc($"[S]:{e.KeyCode}");
            if (KeyFuncTbl.TryGetValue(e.KeyCode, out KeyDownFunc value))
            {
                if (value(sender, e))
                {
                    UpdatePicture();
                }
            }
            Log.trc($"[E]");
        }

        private bool KeyDownFunc_(object sender, KeyEventArgs e)
        {
            return true;
        }

        private bool WindowQuitOp()
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

            var msg = String.Format("選択したファイル({0})を移動しますか？", mModel.mMarkCount.ToString());
            DialogResult result = MessageBox.Show(msg,
                "移動？",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
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

            PicBoxUpdate();

            //img.Dispose();
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PicBoxUpdate()
        {
            //Log.trc("[S]");
            Refresh();
            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void ThumbnailAreaView(bool disp)
        {
            Log.trc("[S]");
            if (disp)
            {
                RightPicBox.Visible = true;

                pictureBox.Width = this.Width / 2;
                //pictureBox.Height = this.Height;
            }
            else
            {
                RightPicBox.Visible = false;
                RightPicBox.Width = 0;
                RightPicBox.Height = 0;

                pictureBox.Width = this.Width;
                //pictureBox.Height = this.Height;
            }
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetPic()
        {
            //Log.trc("[S]");
            startTickCnt = 0;
            mAlphaPercent = 0;

            int period = TRANSITION_TIMER_PERIOD;

            //picTimer.Interval = interval;//10;
            //picTimer.Start();
            if (mTransitionEffect)
            {
                mTransitionTimer.Change(0, period);
            }
            //Log.trc("[E]");
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
                FileInfo fi = new(fitem.FilePath);
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
                if (!RightPicBox.Visible)
                {
                    return;
                }
                var tsize = GetThumbnailSize();
                var done = mModel.MakeThumbnail(tsize.Width, tsize.Height);
                if (done)
                {
                    Log.log("thumbnail making done");
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
            PicBoxUpdate();
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

        private void X2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Col = 2;
            this.Row = 2;
            mModel.UpDownCount = this.Col;
            mModel.PageCount = this.Col * this.Row;
            Refresh();
        }

        private void X4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Col = 4;
            this.Row = 3;
            mModel.UpDownCount = this.Col;
            mModel.PageCount = this.Col * this.Row;
            Refresh();
        }

        private void TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColRowForm crForm = new(this.Col, this.Row);
            DialogResult result = crForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                (int col, int row) = crForm.GetColRow();
                this.Col = col;
                this.Row = row;
                mModel.UpDownCount = this.Col;
                mModel.PageCount = this.Col * this.Row;

                mMagType = crForm.GetMagType();

                mThumbMs = crForm.GetThumbMs();

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
            var model = mModel.DuplicateSelectOnly();

            PictureForm picForm = new();
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
            var fitem = mModel.GetCurrentFileItem();
            var path = fitem.FilePath;
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
                ThumbnailAreaView(true);
            }
            else
            {
                ThumbnailAreaView(false);
            }
        }

        private void MenuItem_MagSub_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_CopyParentDirPath_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            var parentPath = Path.GetDirectoryName(fitem.FilePath);
            Clipboard.SetText(parentPath);
        }

        private void sQLTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            var path = fitem.FilePath;
            var pxv = new PxvArtist(path);

            var caption = $"{pxv.PxvName}({pxv.PxvID})";
            var msg = $"{pxv.Rating}\n{pxv.R18}\n{pxv.Feature}";
            MessageBox.Show(msg, caption);
        }

        private void ToolStripMenuItem_OpenFiler_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            //var opt = @"/select, """ + fitem.FilePath + @"""";
            var opt = Path.GetDirectoryName(fitem.FilePath);
            Log.trc($"{opt}");
            System.Diagnostics.Process.Start(
                "EXPLORER.EXE",
                opt);
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.Location.ToString());

            var loc = e.Location;

            var pos = this.Width / 3;
            if (loc.X > this.Width - pos)
            {

                mModel.Next();
                UpdatePicture();
            }
            else if (loc.X < pos)
            {
                mModel.Prev();
                UpdatePicture();
            }
        }

        private void MenuItem_MagSub_FitNoMag_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_SlideShow_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Quit_Click(object sender, EventArgs e)
        {
            WindowQuitOp();
        }

        private void ToolStripMenuItem_Close_Click(object sender, EventArgs e)
        {
            WindowQuitOp();
        }
    }
}
