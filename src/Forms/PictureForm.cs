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
        //private const int THUMBNAIL_TIMER_PERIOD = 133;
        //private const int THUMBNAIL_TIMER_PERIOD = 140;

        private const int SLIDESHOW_TIMER_PERIOD = 750;
        private const int PAGE_CHG_TIMER_PERIOD = 250;

        private static readonly Brush BRUSH_0 = Brushes.Blue;
        private static readonly Brush BRUSH_MARK = Brushes.DarkRed;
        private static readonly Brush BRUSH_SLIDESHOW = Brushes.Gray;
        private static readonly Brush BG_BRUSH = Brushes.Black;
        private static readonly Color COLOR_MARK = Color.Red;

        private int ThumbnailCols = 4;
        private int ThumbnailRows = 3;

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
        private readonly Dictionary<Keys, KeyDownFunc> KeyFuncTbl = [];

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

            InitWindow();

            InitToolBar();

            InitTimers();

            UpdatePicture();

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
            {
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
                }
                else
                {
                    ToolStripMenuItem_ThumbnailOn.Checked = false;
                    ThumbnailAreaView(false);
                }

                ToggleFulscreen();
                mMagType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN;
                DisplayTxt = false;
            }
            else
            {
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

                    if (false)
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

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void InitKeys()
        {

            KeyFuncTbl[Keys.PageUp] = KeyDownFunc_PageUp;
            KeyFuncTbl[Keys.PageDown] = KeyDownFunc_PageDown;

            KeyFuncTbl[Keys.NumPad0] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Space] = KeyDownFunc_SelectToggle;
            KeyFuncTbl[Keys.Escape] = KeyDownFunc_Escape;
            //KeyFuncTbl[Keys.F1] = ;used
            //KeyFuncTbl[Keys.F2] = ;used
            //KeyFuncTbl[Keys.F3] = ;used
            //KeyFuncTbl[Keys.F4] = ;used
            //KeyFuncTbl[Keys.F5] = ;used
            KeyFuncTbl[Keys.F6] = KeyDownFunc_ThumbnailChg;
            //KeyFuncTbl[Keys.F9] = ;used
            KeyFuncTbl[Keys.A] = KeyDownFunc_Left;
            KeyFuncTbl[Keys.S] = KeyDownFunc_Right;
            KeyFuncTbl[Keys.Q] = KeyDownFunc_Home;
            KeyFuncTbl[Keys.Z] = KeyDownFunc_End;

            switch (mModel.ThumbViewType)
            {
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LIST:
                    KeyFuncTbl[Keys.Left] = KeyDownFunc_List_Left;
                    KeyFuncTbl[Keys.Right] = KeyDownFunc_List_Right;
                    KeyFuncTbl[Keys.Up] = KeyDownFunc_List_Up;
                    KeyFuncTbl[Keys.Down] = KeyDownFunc_List_Down;

                    KeyFuncTbl[Keys.Home] = KeyDownFunc_List_Home;
                    KeyFuncTbl[Keys.End] = KeyDownFunc_List_End;
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_TILE:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_OVERVIEW:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_NEXT:
                default:
                    KeyFuncTbl[Keys.Left] = KeyDownFunc_Left;
                    KeyFuncTbl[Keys.Right] = KeyDownFunc_Right;
                    KeyFuncTbl[Keys.Up] = KeyDownFunc_Up;
                    KeyFuncTbl[Keys.Down] = KeyDownFunc_Down;

                    KeyFuncTbl[Keys.Home] = KeyDownFunc_Home;
                    KeyFuncTbl[Keys.End] = KeyDownFunc_End;

                    break;
            }

        }

        private bool KeyDownFunc_List_Left(object sender, KeyEventArgs e)
        {
            //前のファイルに移動（ディレクトリ内をループ）
            mModel.ListPrev();
            return true;
        }

        private bool KeyDownFunc_List_Right(object sender, KeyEventArgs e)
        {
            //次のファイルに移動（ディレクトリ内をループ）
            mModel.ListNext();
            return true;
        }

        private bool KeyDownFunc_List_Up(object sender, KeyEventArgs e)
        {
            mModel.ListUp();
            return true;
        }

        private bool KeyDownFunc_List_Down(object sender, KeyEventArgs e)
        {
            mModel.ListDown();
            return true;
        }

        private bool KeyDownFunc_List_Home(object sender, KeyEventArgs e)
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
                //同一ディレクトリの先頭に移動
                mModel.MoveToDirTopImage();
            }
            return true;
        }

        private bool KeyDownFunc_List_End(object sender, KeyEventArgs e)
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
                //同一ディレクトリの末尾に移動
                mModel.MoveToDirEndImage();
            }
            return true;
        }

        //
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
            if (e.Shift)
            {
            }
            else if (e.Control)
            {
                mModel.MarkAllSameDirFiles();
            }
            else
            {
                mModel.toggleMark();
                mModel.Next();
            }
            return true;
        }

        private bool KeyDownFunc_Escape(object sender, KeyEventArgs e)
        {
            return WindowQuitOp();
        }

        private bool KeyDownFunc_ThumbnailChg(object sender, KeyEventArgs e)
        {
            //NextPic = !NextPic;
            mModel.ToggleThumbView();
            InitKeys();

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

        private bool WindowQuitOp(bool forceClose = false)
        {
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

            mModel.UpdateListFile();

            //表示中ファイル位置の記憶
            //TODO

            if (mModel.mMarkCount == 0 || mModel.IsZip())
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
            this.ThumbnailCols = 2;
            this.ThumbnailRows = 2;
            mModel.UpDownCount = this.ThumbnailCols;
            mModel.PageCount = this.ThumbnailCols * this.ThumbnailRows;
            Refresh();
        }

        private void X4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.ThumbnailCols = 4;
            this.ThumbnailRows = 3;
            mModel.UpDownCount = this.ThumbnailCols;
            mModel.PageCount = this.ThumbnailCols * this.ThumbnailRows;
            Refresh();
        }

        private void TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColRowForm crForm = new(this.ThumbnailCols, this.ThumbnailRows);
            DialogResult result = crForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                (int col, int row) = crForm.GetColRow();
                this.ThumbnailCols = col;
                this.ThumbnailRows = row;
                mModel.UpDownCount = this.ThumbnailCols;
                mModel.PageCount = this.ThumbnailCols * this.ThumbnailRows;

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
            var pxv = new PxvArtist(path);

            var caption = $"{pxv.PxvName}({pxv.PxvID})";
            var msg = $"{pxv.Rating}\n{pxv.R18}\n{pxv.Feature}";
            MessageBox.Show(msg, caption);

            string str = Microsoft.VisualBasic.Interaction.InputBox("数値を入力してください", "rating", pxv.Rating.ToString(), -1, -1);
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
                this.toolStripMenuItem_Sort_FileSize,
                this.ToolStripMenuItem_Sort_NumPixel,
                this.ToolStripMenuItem_AspectRatio,
                this.ToolStripMenuItem_Sort_FileHash,
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
            string str = Microsoft.VisualBasic.Interaction.InputBox("数値を入力してください", "ms", mSlideMs.ToString(), -1, -1);
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
    }
}
