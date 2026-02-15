//#define USE_TOOL_BAR

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

//using Microsoft.VisualBasic.Logging;

//using System.Windows.Threading;
using PictureManagerApp.src.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;
//using static System.Net.Mime.MediaTypeNames;
using static PictureManagerApp.src.Model.FileList;


namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        //=====================================================================
        // const
        //=====================================================================
        private const int TRANSITION_FPS = 30;
        private const int TRANSITION_TIMER_PERIOD = 1000 / TRANSITION_FPS;
        private const int TRANSITION_DUE_TIME = 100;

        //private const int THUMBNAIL_TIMER_PERIOD = 100;//100だと重くなる.
        //private const int THUMBNAIL_TIMER_PERIOD = 111;//111もだめ？
        private const int THUMBNAIL_TIMER_PERIOD = 122;
        //private const int THUMBNAIL_TIMER_PERIOD = 122;
        //private const int THUMBNAIL_TIMER_PERIOD = 133;
        //private const int THUMBNAIL_TIMER_PERIOD = 140;

        private const int SLIDESHOW_TIMER_PERIOD = 750;
        private const int PAGE_CHG_TIMER_PERIOD = 250;

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
        //private Boolean NextPic = false;
        private Image mCurrentImg, mPrevImg;
        private IMAGE_DISPLAY_MAGNIFICATION_TYPE mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND;

#if UNDEFINED
        private int ThumbnailCols = 4;
        private int ThumbnailRows = 3;
#else
        private int ThumbnailCols = 8;
        private int ThumbnailRows = 5;
#endif


        // transition
        private Boolean mTransitionEffect = false;
        private int mAlphaPercent = 0;
        private int mTransStartTickCnt;
        private System.Threading.Timer mTransitionTimer;

        // thumbnail
        private int mThumbMs = THUMBNAIL_TIMER_PERIOD;
        private int mThumbStartTickCnt;
        private System.Threading.Timer mThumbnailTimer;

        // slideshow
        private Boolean mSlideshow = false;
        private int mSlideMs = SLIDESHOW_TIMER_PERIOD;
        private int mSlideStartTickCnt;
        private System.Threading.Timer mSlideshowTimer;

        // 
        private Boolean mCont = false;
        private int mContMs = PAGE_CHG_TIMER_PERIOD;
        private int mContTickCnt;
        private System.Threading.Timer mContTimer;

#if USE_TOOL_BAR
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
#endif

        //=====================================================================
        // public
        //=====================================================================
        public PictureForm()
        {
            Log.trc($"[S]");
            InitializeComponent();

            //this.pictureBox.DoubleBuffered = true;
            InitNumKeyMap();

            Log.trc($"[E]");
        }

        public void SetModel(PictureModel model)
        {
            mModel = model;

            SetMoveNum();

            InitKeys();
        }

        //=====================================================================
        // private
        //=====================================================================
        private void SetMoveNum()
        {
            mModel.UpDownCount = ThumbnailCols;
            mModel.PageCount = this.ThumbnailCols * this.ThumbnailRows;
        }

        private void SetThumb(int c, int r)
        {
            this.ThumbnailCols = c;
            this.ThumbnailRows = r;
            mModel.UpDownCount = this.ThumbnailCols;
            mModel.PageCount = this.ThumbnailCols * this.ThumbnailRows;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureForm_Load(object sender, EventArgs e)
        {
            Log.trc($"[S]");

            this.KeyPreview = true;

            if (mModel.PictureTotalNumber == 0)
            {
                var text = "no file";
                var caption = "ファイルなし";
                MessageBox.Show(text,
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                InitWindow();

                InitToolBar();

                InitTimers();

                try
                {
                    if (mModel.IsIndexInvalid())
                    {
                        mModel.SetCurrentFileIndex(0);
                    }
                    UpdatePicture();
                }
                catch (System.ArgumentOutOfRangeException ex)
                {

                    Log.trc(ex.Message);
                }
                //
            }

            Log.trc($"[E]");
        }

        private void InitWindow()
        {
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
            {   //zipファイルの場合
                this.Width = w - 0;
                this.Height = h - 0;

                if (false)
                {
                }

                if (landscape)
                {
                    SetPicboxSize();

                    this.ThumbnailCols = 1;
                    this.ThumbnailRows = RightPicBox.Height / RightPicBox.Width;
                    SetMoveNum();

                    if (true)
                    {
                        //サムネイル非表示
                        ToolStripMenuItem_ThumbnailOn.Checked = false;
                        ThumbnailAreaView(false);
                    }
                }
                else
                {
                    //縦画面の場合はサムネイル非表示
                    ToolStripMenuItem_ThumbnailOn.Checked = false;
                    ThumbnailAreaView(false);
                }

                ToggleFulscreen();
                mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN;
                DisplayTxt = false;
            }
            else
            {   //通常のフォルダの場合
                if (landscape)
                {
                    this.Width = w - 100;
                    this.Height = h - 50;

                    pictureBox.Width = this.Width / 2;
                    if (pictureBox.Width > 1200)
                    {
                        pictureBox.Width = 1200;
                    }
                }
                else
                {
                    //縦長フルスクリーン表示設定
                    this.Width = w - 0;
                    this.Height = h - 0;

                    bool flg = false;
                    if (flg)
                    {
                        ToolStripMenuItem_ThumbnailOn.Checked = false;
                        ThumbnailAreaView(false);
                    }
                    ToggleFulscreen();
                    mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN;
                    DisplayTxt = false;
                }
            }

            //this.StartPosition = FormStartPosition.CenterScreen;
            this.Top = 0;
            this.Left = 0;
        }

        private void InitTimers()
        {
            mTransitionTimer = new System.Threading.Timer(TimerTickTransition, null, 0, TRANSITION_TIMER_PERIOD);
            mTransitionTimer.Change(Timeout.Infinite, Timeout.Infinite);

            mThumbnailTimer = new System.Threading.Timer(TimerTickThumbnail, null, 0, mThumbMs);
            //mThumbnailTimer.Change(Timeout.Infinite, Timeout.Infinite);
            mThumbnailTimer.Change(0, mThumbMs);

            mSlideshowTimer = new System.Threading.Timer(TimerTickSlideshow, null, 0, SLIDESHOW_TIMER_PERIOD);
            //mSlideshowTimer.Change(0, mSlideMs);
            mSlideshowTimer.Change(Timeout.Infinite, Timeout.Infinite);

            mContTimer = new System.Threading.Timer(TimerTickCont, null, 0, mContMs);
            mContTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void InitToolBar()
        {
#if USE_TOOL_BAR
            //ToolStripオブジェクトを作成
            this.toolStrip1 = new ToolStrip();

            //レイアウトを一時停止
            this.SuspendLayout();
            this.toolStrip1.SuspendLayout();

            //ToolStripButtonを作成
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripButton1.Text = "開く(&O)";
            //Clickイベントハンドラを追加
            //this.toolStripButton1.Click += toolStripButton1_Click;

            //ボタンをもう1つ作成
            this.toolStripButton2 = new ToolStripButton();
            this.toolStripButton2.Text = "保存(&S)";
            //this.toolStripButton2.Image = Image.FromFile("save.gif");
            //this.toolStripButton2.Click += toolStripButton2_Click;

            //ToolStripにボタンを追加
            this.toolStrip1.Items.Add(this.toolStripButton1);
            this.toolStrip1.Items.Add(this.toolStripButton2);

            //フォームにToolStripを追加
            this.Controls.Add(this.toolStrip1);

            //レイアウトを再開
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
#endif
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

            if (this.mSlideshowTimer != null)
            {
                mSlideshowTimer.Dispose();
                Log.trc("timer dispose");
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetPicboxSize()
        {
            if (RightPicBox.Visible)
            {
                if (mModel.IsZip())
                {
                    if (this.Width > this.Height)
                    {
                        var w = (int)(this.Width * 0.1);
                        if (w < 128)
                        {
                            w = 128;
                        }
                        pictureBox.Width = this.Width - w;
                    }
                    else
                    {
                        pictureBox.Width = this.Width;
                        pictureBox.Height = (int)(this.Height * 0.8);

                        RightPicBox.Anchor = AnchorStyles.Left;// | AnchorStyles.Bottom;
                        RightPicBox.Location = new Point(pictureBox.Left, pictureBox.Top + pictureBox.Height);
                        RightPicBox.Width = this.Width;

                        Log.trc($"PicBox={pictureBox.Width}x{pictureBox.Height}/({pictureBox.Left},{pictureBox.Top})");
                        Log.trc($"RightPicBox={RightPicBox.Width}x{RightPicBox.Height}/({RightPicBox.Left},{RightPicBox.Top})");
                    }
                }
                else
                {
                    pictureBox.Width = this.Width / 2;
                    if (pictureBox.Width > 1200)
                    {
                        pictureBox.Width = 1200;
                    }
                }

                Log.trc($"picbox w={pictureBox.Width}, this w={this.Width}");
            }
            else
            {
                pictureBox.Size = this.ClientSize;
            }
        }

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
                SetPicboxSize();
            }
            PicBoxUpdate();
            Log.trc($"[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void DoAction(ACTION_TYPE act)
        {
            switch (act)
            {
                case ACTION_TYPE.ACTION_MOV_NEXT:
                    mModel.NextIfHasNext();
                    UpdatePicture();
                    break;
                case ACTION_TYPE.ACTION_MOV_PREV:
                    mModel.Prev();
                    UpdatePicture();
                    break;
                case ACTION_TYPE.ACTION_MOV_NEXT_DIR:
                    mModel.NextDirImage();
                    UpdatePicture();
                    break;
                case ACTION_TYPE.ACTION_MOV_PREV_DIR:
                    mModel.PrevDirImage();
                    UpdatePicture();
                    break;
                case ACTION_TYPE.ACTION_ADD_DEL_LIST:
                    mModel.AddDelList();
                    PicBoxUpdate();
                    break;
                case ACTION_TYPE.ACTION_ADD_FAV_LIST:
                    mModel.AddFavList();
                    mModel.toggleMark();
                    PicBoxUpdate();
                    break;
                case ACTION_TYPE.ACTION_MOV_NEXT_CONT_START:
                    StartContinuousNext();
                    break;
                case ACTION_TYPE.ACTION_MOV_NEXT_CONT_STOP:
                    StopContinuousNext();
                    break;
                case ACTION_TYPE.ACTION_MOV_SLIDESHOW:
                    ToggleSlideshow(mSlideMs);
                    break;
                case ACTION_TYPE.ACTION_QUIT_CONF:
                    WindowQuitOp();
                    break;
                case ACTION_TYPE.ACTION_DO_NOTHING:
                default:
                    break;
            }

        }

        private bool WindowQuitOp(bool forceClose = false)
        {
            if (mModel.IsZip())
            {
                if (mSlideshow)
                {
                    ToggleSlideshow(mSlideMs);
                    return false;
                }

                //表示中ファイル位置の記憶
                //TODO


                Close();
                return false;
            }

            if (!forceClose)
            {
                if (mSlideshow)
                {
                    ToggleSlideshow(mSlideMs);
                    return false;
                }

                if (mFullscreen)
                {
                    ToggleFulscreen();
                    return false;
                }
            }

            if (mModel.NoAtoShori())
            {
                Close();
                return false;
            }


            //TODO:サムネイル生成を停止しないとバグる気がする
            //とまらないね？？
            PauseMakingThumbnail();


            var rmv_cand_file_num = mModel.mMarkCount;
            var msg = String.Format($"選択したファイル({rmv_cand_file_num})を移動しますか？''");
            DialogResult result = MessageBox.Show(msg,
                "移動？",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {

                var del_cnt = mModel.Batch();

                MessageBox.Show($"{del_cnt}ファイルを移動しました。",
                    "ファイル移動",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (rmv_cand_file_num != del_cnt)
                {
                    MessageBox.Show($"移動に失敗したファイルがあります。",
                        "ファイル移動",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            if (result == DialogResult.Cancel)
            {
                RestartMakingThumbnail();
            }
            else
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
            //Log.trc("[S]");

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
            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PicBoxUpdate()
        {
            //Log.trc("[S]");
            Refresh();
            //pictureBox.Refresh();
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
            PicBoxUpdate();
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void SetPic()
        {
            //Log.trc("[S]");

            this.mTransStartTickCnt = 0;
            this.mAlphaPercent = 0;

            if (mTransitionEffect)
            {
                int period = TRANSITION_TIMER_PERIOD;

                //picTimer.Interval = interval;//10;
                //picTimer.Start();

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
            SetStatusBar_DispMode();
        }

        private void SetStatusBar_ProgressBar()
        {
            toolStripProgressBar.Value = (mModel.PictureNumber + 1) * 100 / mModel.PictureTotalNumber;
        }

        private void SetStatusBar_FileNo()
        {
            var thumbcnt = mModel.CountIfHasThumbnail();
            string no = String.Format("{0,5}/{1,5}({2})", mModel.PictureNumber + 1, mModel.PictureTotalNumber, thumbcnt);
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

            string filesize;
            if (fitem.IsZipEntry)
            {
                filesize = String.Format("{0:#,0} Bytes ({1:#,0})", fitem.FileSize, fitem.CompressedLength);
            }
            else
            {
                filesize = String.Format("{0:#,0} Bytes", fitem.FileSize);
            }
            statusLbl_FileSize.Text = filesize;
            StatusLbl_LWTime.Text = fitem.LastWriteTime.ToShortDateString();
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
            StatusLbl_MarkCnt.Text = $"{mModel.mMarkCount}選択中";//mModel.mMarkCount.ToString();
        }

        private void SetStatusBar_DispMode()
        {
            StatusLbl_DispMode.Text = mModel.ThumbViewType.ToString();
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
            //Log.trc("[S]");

            int tickCnt = Environment.TickCount & Int32.MaxValue;
            if (mTransStartTickCnt == 0)
            {
                mAlphaPercent += 10;
                mTransStartTickCnt = tickCnt;
            }
            else
            {
                int delta = tickCnt - mTransStartTickCnt;
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
            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PauseMakingThumbnail()
        {
            mThumbnailTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void RestartMakingThumbnail()
        {
            mThumbnailTimer.Change(0, mThumbMs);
        }

        private void TimerTickThumbnail(object state)
        {
            //Log.trc("[S]");

            int tickCnt = Environment.TickCount & Int32.MaxValue;
            if (mThumbStartTickCnt == 0)
            {
                mThumbStartTickCnt = tickCnt;
            }
            else
            {
                if (!RightPicBox.Visible)
                {
                    return;
                }

                var tsize = GetThumbnailSize();
                var idx = mModel.MakeThumbnail(tsize.Width, tsize.Height);
                if (idx == -1)
                {
                    Log.log("thumbnail making done");
                    mThumbnailTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }

                //SetStatusBar_FileNo();

                if (this != null)
                {
                    /// TODO: 必要な場合のみ画面更新
                    var update = true;
                    if (update)
                    {
                        //UI_change();
                        RightPicBox.Invalidate();
                    }
                }
            }

            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void TimerTickSlideshow(object state)
        {
            //Log.trc("[S]");

            int tickCnt = Environment.TickCount & Int32.MaxValue;
            if (mSlideStartTickCnt == 0)
            {
                mSlideStartTickCnt = tickCnt;
            }
            else
            {
                if (!this.mSlideshow)
                {
                    return;
                }

                mModel.Next();
                //UpdatePicture();

            }
            if (this != null)
            {
                UI_change2();
            }

            //Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void TimerTickCont(object state)
        {

            int tickCnt = Environment.TickCount & Int32.MaxValue;
            if (mContTickCnt == 0)
            {
                mContTickCnt = tickCnt;
            }
            else
            {
                if (!this.mCont)
                {
                    return;
                }

                mModel.NextIfHasNext();


                if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                {
                    //Log.trc("マウスの左ボタンが押されています。");
                }
                else
                {
                    //タイマーを停止させたい
                    //Log.trc("マウスの左ボタンが押されてない。");
                    StopContinuousNext();
                }

            }
            if (this != null)
            {
                UI_change2();
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void ToggleSlideshow(int period)
        {
            mSlideshow = !mSlideshow;

            if (mSlideshow)
            {
                Log.trc("slideshow start");
                mSlideshowTimer.Change(0, period);
            }
            else
            {
                Log.trc("slideshow end");
                mSlideshowTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            UpdatePicture();
        }

        private void StartContinuousNext()
        {
            if (mCont)
            {
                //すでに実行中
                //do nothing
            }
            else
            {
                mCont = true;
                mContTimer.Change(0, PAGE_CHG_TIMER_PERIOD);
                Log.trc("start");
            }
        }

        private void StopContinuousNext()
        {
            if (mCont)
            {
                mCont = false;
                mContTimer.Change(Timeout.Infinite, Timeout.Infinite);
                Log.trc("end");
            }
            else
            {
                //すでに停止
                //do nothing
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void UI_change()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Invoke(new Action(UI_change));
                }
                catch (System.Exception e)
                {
                    Log.fatal(e.Message);
                }
                return;
            }

            PicBoxUpdate();
        }

        private void UI_change2()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(UI_change2));
                return;
            }
            UpdatePicture();
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
            SetThumb(2, 2);
            Refresh();
        }

        private void X4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetThumb(4, 3);
            Refresh();
        }

        private void TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var crForm = new ColRowForm(this.ThumbnailCols, this.ThumbnailRows, mModel.GetRootPath());
            var result = crForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                (int col, int row) = crForm.GetColRow();
                //this.ThumbnailCols = col;
                //this.ThumbnailRows = row;
                //mModel.UpDownCount = this.ThumbnailCols;
                //mModel.PageCount = this.ThumbnailCols * this.ThumbnailRows;
                SetThumb(col, row);

                mMagType = crForm.GetMagType();
                mThumbMs = crForm.GetThumbMs();
                mModel.SetDstRootPath(crForm.GetDstRootPath());

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
            DialogResult result = MessageBox.Show("選択中のファイルを移動して終了しますか？",
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

        private void ToolStripMenuItem_CopyParentDirPath_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            var parentPath = Path.GetDirectoryName(fitem.FilePath);
            Clipboard.SetText(parentPath);
        }

        private void ToolStripMenuItem_sql_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            var path = fitem.FilePath;


            if (mModel.mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_PXV)
            {
                var pxv = new PxvArtist(path);

                var caption = $"{pxv.PxvName}({pxv.PxvID})";
                var msg = $"{pxv.Rating}\n{pxv.R18}\n{pxv.Feature}";
                MessageBox.Show(msg, caption);

                var prompt = "数値を入力してください";
                var title = "rating";
                var value = pxv.Rating.ToString();
                var str = Util.InputBox(prompt, title, value);
                try
                {
                    long numVal = Int32.Parse(str);
                    Sqlite.UpdatePxvArtistRating(pxv.PxvID, numVal);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0}: Bad Format", str);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("{0}: Overflow", str);
                }
            }
        }

        private void ToolStripMenuItem_DB_Edit_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            var pxv = new PxvArtist(fitem.FilePath);

            var prompt = "削除情報を入力してください";
            var title = "削除情報";
            var value = pxv.DelInfo;
            var inputval = Util.InputBox(prompt, title, value);
            try
            {
                if (inputval != value)
                {
                    Sqlite.UpdatePxvArtistDelInfo(pxv.PxvID, inputval);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("{0}: Bad Format", inputval);
            }
            catch (OverflowException)
            {
                Console.WriteLine("{0}: Overflow", inputval);
            }
        }

        private void t()
        {

        }

        private void ToolStripMenuItem_OpenFiler_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            //var opt = @"/select, """ + fitem.FilePath + @"""";
            var opt = fitem.FilePath;//Path.GetDirectoryName(fitem.FilePath);
            MyFiles.OpenGuiShellFile(opt);
        }

        private void MenuItem_MagSub_FitNoMag_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_SlideShow_Click(object sender, EventArgs e)
        {
            ToggleSlideshow(mSlideMs);
        }

        private void ToolStripMenuItem_Quit_Click(object sender, EventArgs e)
        {
            WindowQuitOp(true);
        }

        private void ToolStripMenuItem_Close_Click(object sender, EventArgs e)
        {
            WindowQuitOp(true);
        }

        private void toolStripMenuItem_Sort_FilePath_Click(object sender, EventArgs e)
        {
            //グループのToolStripMenuItemを配列にしておく
            ToolStripMenuItem[] groupMenuItems = new ToolStripMenuItem[]
            {
                this.toolStripMenuItem_Sort_FilePath,
                this.ToolStripMenuItem_Sort_Filename,
                this.toolStripMenuItem_Sort_FileSize,
                this.ToolStripMenuItem_Sort_NumPixel,
                this.ToolStripMenuItem_AspectRatio,
                this.ToolStripMenuItem_Sort_FileHash,
                this.ToolStripMenuItem_Sort_Title,
                this.ToolStripMenuItem_Sort_ID,
            };

            //グループのToolStripMenuItemを列挙する
            foreach (ToolStripMenuItem item in groupMenuItems)
            {
                if (object.ReferenceEquals(item, sender))
                {
                    //ClickされたToolStripMenuItemならば、Indeterminateにする
                    item.CheckState = CheckState.Indeterminate;

                    SORT_TYPE sort_type;

                    if (item == this.toolStripMenuItem_Sort_FileSize)
                    {
                        sort_type = SORT_TYPE.SORT_FILESIZE;
                    }
                    else if (item == this.ToolStripMenuItem_Sort_NumPixel)
                    {
                        sort_type = SORT_TYPE.SORT_NUM_PIXEL;
                    }
                    else if (item == this.ToolStripMenuItem_AspectRatio)
                    {
                        sort_type = SORT_TYPE.SORT_ASPECT_RATIO;
                    }
                    else if (item == this.ToolStripMenuItem_Sort_FileHash)
                    {
                        sort_type = SORT_TYPE.SORT_FILE_HASH;
                    }
                    else if (item == this.ToolStripMenuItem_Sort_Filename)
                    {
                        sort_type = SORT_TYPE.SORT_FILENAME;
                    }
                    else if (item == this.ToolStripMenuItem_Sort_Title)
                    {
                        sort_type = SORT_TYPE.SORT_TITLE;
                    }
                    else if (item == this.ToolStripMenuItem_Sort_ID)
                    {
                        sort_type = SORT_TYPE.SORT_ARTWORK_ID;
                    }
                    else
                    {
                        sort_type = SORT_TYPE.SORT_PATH;
                    }
                    mModel.Sort(sort_type);
                }
                else
                {
                    //ClickされたToolStripMenuItemでなければ、Uncheckedにする
                    item.CheckState = CheckState.Unchecked;
                }
            }
            mModel.MovePos(POS_MOVE_TYPE.MOVE_HOME);
            UpdatePicture();
        }

        private void ToolStripMenuItem_Slides_Click(object sender, EventArgs e)
        {
            ToggleSlideshow(mSlideMs);
        }

        private void ToolStripMenuItem_SelSameDir_Click(object sender, EventArgs e)
        {
            mModel.MarkAllSameDirFiles();
        }

        private void ToolStripMenuItem_SlideShow_Ms_Click(object sender, EventArgs e)
        {
            //スライドショーの更新間隔(ms)を設定
            //string str = Microsoft.VisualBasic.Interaction.InputBox("数値を入力してください", "ms", mSlideMs.ToString(), -1, -1);
            var str = Util.InputBox("数値を入力してください", "ms", mSlideMs.ToString());
            if (str != "")
            {
                var numVal = Int32.Parse(str);

                mSlideMs = numVal;
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Log.trc("[S]");

            if (e.Location.X < this.Width / 3 && e.Location.Y < this.Height / 3)
            {
                ACTION_TYPE act = ACTION_TYPE.ACTION_MOV_NEXT_CONT_START;
                DoAction(act);
                Log.trc("start");
            }

            Log.trc("[E]");
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Log.trc("[S]}");

            //if (e.Location.X < this.Width / 3 && e.Location.Y < this.Height / 3)
            if (mCont)
            {
                ACTION_TYPE act = ACTION_TYPE.ACTION_MOV_NEXT_CONT_STOP;
                DoAction(act);
                Log.trc("end");
            }

            Log.trc("[E]");
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Log.trc("[S]}");

            if (mSlideshow)
            {
                ToggleSlideshow(mSlideMs);
                PicBoxUpdate();
                Log.trc("[E] toggle end");
                return;
            }

            Log.trc("{0}", e.Location.ToString());
            var act = GetMouseAction(e.Location);
            DoAction(act);

            Log.trc("[E]");
        }

        private ACTION_TYPE GetMouseAction(Point loc)
        {
            ACTION_TYPE act = ACTION_TYPE.ACTION_DO_NOTHING;

            var scrn_unit_w = this.Width / 3;
            var scrn_unit_h = this.Height / 3;

            if (loc.X < scrn_unit_w)
            {   // 左

                if (loc.Y < scrn_unit_h)
                {
                    //連続で次へ
                    //down/upで処理
                }
                else if (loc.Y < scrn_unit_h * 2)
                {
                    //中
                    act = ACTION_TYPE.ACTION_MOV_NEXT;
                }
                else
                {
                    //下
                    act = ACTION_TYPE.ACTION_MOV_PREV;
                }
            }
            else if (loc.X < scrn_unit_w * 2)
            {   //中

                if (loc.Y < scrn_unit_h)
                {//上
                    act = ACTION_TYPE.ACTION_ADD_FAV_LIST;
                }
                else if (loc.Y < scrn_unit_h * 2)
                {//中
                    act = ACTION_TYPE.ACTION_MOV_SLIDESHOW;
                }
                else
                {//下
                    act = ACTION_TYPE.ACTION_ADD_DEL_LIST;
                }
            }
            else
            {   //右

                if (loc.Y < scrn_unit_h)
                {//上
                    act = ACTION_TYPE.ACTION_MOV_PREV;
                }
                else if (loc.Y < scrn_unit_h * 2)
                {//中
                    act = ACTION_TYPE.ACTION_MOV_PREV;
                }
                else
                {//下
                    act = ACTION_TYPE.ACTION_QUIT_CONF;
                }
            }

            return act;
        }

        //MouseDoubleClickイベントの前にはMouseClickが発生するらしい。めんどくさい。。。
        private void pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
#if false
            Log.trc("[S]");
            var loc = e.Location;
            ACTION_TYPE act = ACTION_TYPE.ACTION_DO_NOTHING;
            var pos = this.Width / 3;
            if (loc.X > this.Width - pos)
            {
                act = ACTION_TYPE.ACTION_MOV_PREV_DIR;
            }
            else if (loc.X < pos)
            {
                act = ACTION_TYPE.ACTION_MOV_NEXT_DIR;
            }

            DoAction(act);
            Log.trc("[E]");
#endif
        }

        private void ToolStripMenuItem_hide_Click(object sender, EventArgs e)
        {
            mModel.RemoveCurrentFile();
            UpdatePicture();
        }

        private void toolStripMenuItem_tsv_Click(object sender, EventArgs e)
        {
            mModel.tsv();
        }

        private void toolStripMenuItem_Stat_Click(object sender, EventArgs e)
        {
            var str = mModel.Stat();
            MessageBox.Show(str,
                "stat",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ToolStripMenuItem_SaveFilesPath_Click(object sender, EventArgs e)
        {
            mModel.SaveFilesPath();
        }

        private void toolStripMenuItem_UnselectAll_Click(object sender, EventArgs e)
        {
            mModel.UnmarkAll();
            UpdatePicture();
        }

        private void ToolStripMenuItem_FileSel_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_SelectSameHashValFile_Click(object sender, EventArgs e)
        {
            mModel.MarkSameHashFiles();
            UpdatePicture();
        }

        private void contextMenuStrip_pic_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ToolStripMenuItem_rating_down_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_AllImages_Click(object sender, EventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();
            var dirpath = fitem.DirectoryName;
            var model = mModel.GetSameDirImages(dirpath);

            model.SetThumbView(THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_DIRECTORY);

            var picForm = new PictureForm();
            picForm.SetModel(model);
            picForm.ShowDialog();
        }

        private void ToolStripMenuItem_slct_NoSameFs_Click(object sender, EventArgs e)
        {
            mModel.MarkNoSameFileSize();

            var rmv_cnt = mModel.RemoveAllSelectedFiles();

            var caption = "非表示";
            var text = $"{rmv_cnt}ファイルを非表示にしました";
            MessageBox.Show(text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            UpdatePicture();
        }

        private void ToolStripMenuItem_slct_NoSameHash_Click(object sender, EventArgs e)
        {
            mModel.MarkNoSameHashValue();

            var rmv_cnt = mModel.RemoveAllSelectedFiles();

            var caption = "非表示";
            var text = $"{rmv_cnt}ファイルを非表示にしました";
            MessageBox.Show(text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            UpdatePicture();
        }
    }
}
