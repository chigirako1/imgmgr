﻿namespace PictureManagerApp
{
    partial class PictureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox = new System.Windows.Forms.PictureBox();
            contextMenuStrip_pic = new System.Windows.Forms.ContextMenuStrip(components);
            ToolStripMenuItem_PathCopy = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_CopyParentDirPath = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_OpenFiler = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SaveFilesPath = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ToolStripMenuItem_hide = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SelSameDir = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ToolStripMenuItem_DB_op = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_rating_down = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_rating_up = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_sql = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_Slides = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ToolStripMenuItem_Quit = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_FileSel = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            statusLbl_No = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLbl_MarkCnt = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_WxH = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_ratio = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLbl_Dirname = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_Filename = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_FileSize = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLbl_LWTime = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip = new System.Windows.Forms.MenuStrip();
            ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_Close = new System.Windows.Forms.ToolStripMenuItem();
            表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_FullScreen = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SlideShow = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_ThumbnailOn = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_MagSub = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_MagSub_FitNoMag = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem_Sort_FilePath = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_Sort_Filename = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem_Sort_Title = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            ToolStripMenuItem_Sort_NumPixel = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_AspectRatio = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem_Sort_FileSize = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_Sort_FileHash = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_fwd_one = new System.Windows.Forms.ToolStripMenuItem();
            ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SelImg = new System.Windows.Forms.ToolStripMenuItem();
            sQLTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SelectedImageMove = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem_tsv = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem_Stat = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem_UnselectAll = new System.Windows.Forms.ToolStripMenuItem();
            設定SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_TransitionEffect = new System.Windows.Forms.ToolStripMenuItem();
            TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x4ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SlideShow_Ms = new System.Windows.Forms.ToolStripMenuItem();
            ウィンドウToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RightPicBox = new System.Windows.Forms.PictureBox();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            contextMenuStrip_pic.SuspendLayout();
            statusStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RightPicBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.ContextMenuStrip = contextMenuStrip_pic;
            pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBox.Location = new System.Drawing.Point(0, 33);
            pictureBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(1528, 691);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
            pictureBox.MouseDoubleClick += pictureBox_MouseDoubleClick;
            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseUp += pictureBox_MouseUp;
            // 
            // contextMenuStrip_pic
            // 
            contextMenuStrip_pic.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip_pic.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_PathCopy, ToolStripMenuItem_CopyParentDirPath, ToolStripMenuItem_OpenFiler, ToolStripMenuItem_SaveFilesPath, toolStripSeparator2, ToolStripMenuItem_hide, ToolStripMenuItem_SelSameDir, toolStripSeparator1, ToolStripMenuItem_DB_op, ToolStripMenuItem_sql, ToolStripMenuItem_Slides, toolStripSeparator3, ToolStripMenuItem_Quit, ToolStripMenuItem_FileSel });
            contextMenuStrip_pic.Name = "contextMenuStrip_pic";
            contextMenuStrip_pic.Size = new System.Drawing.Size(399, 330);
            // 
            // ToolStripMenuItem_PathCopy
            // 
            ToolStripMenuItem_PathCopy.Name = "ToolStripMenuItem_PathCopy";
            ToolStripMenuItem_PathCopy.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_PathCopy.Text = "ファイルのパスをクリップボードにコピー";
            ToolStripMenuItem_PathCopy.Click += ToolStripMenuItem_PathCopy_Click;
            // 
            // ToolStripMenuItem_CopyParentDirPath
            // 
            ToolStripMenuItem_CopyParentDirPath.Name = "ToolStripMenuItem_CopyParentDirPath";
            ToolStripMenuItem_CopyParentDirPath.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_CopyParentDirPath.Text = "ファイルの親フォルダのパスをクリップボードにコピー";
            ToolStripMenuItem_CopyParentDirPath.Click += ToolStripMenuItem_CopyParentDirPath_Click;
            // 
            // ToolStripMenuItem_OpenFiler
            // 
            ToolStripMenuItem_OpenFiler.Name = "ToolStripMenuItem_OpenFiler";
            ToolStripMenuItem_OpenFiler.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_OpenFiler.Text = "エクスプローラで表示";
            ToolStripMenuItem_OpenFiler.Click += ToolStripMenuItem_OpenFiler_Click;
            // 
            // ToolStripMenuItem_SaveFilesPath
            // 
            ToolStripMenuItem_SaveFilesPath.Name = "ToolStripMenuItem_SaveFilesPath";
            ToolStripMenuItem_SaveFilesPath.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_SaveFilesPath.Text = "全ファイルのパスを保存";
            ToolStripMenuItem_SaveFilesPath.Click += ToolStripMenuItem_SaveFilesPath_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(395, 6);
            // 
            // ToolStripMenuItem_hide
            // 
            ToolStripMenuItem_hide.Name = "ToolStripMenuItem_hide";
            ToolStripMenuItem_hide.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_hide.Text = "表示対象から外す";
            ToolStripMenuItem_hide.Click += ToolStripMenuItem_hide_Click;
            // 
            // ToolStripMenuItem_SelSameDir
            // 
            ToolStripMenuItem_SelSameDir.Name = "ToolStripMenuItem_SelSameDir";
            ToolStripMenuItem_SelSameDir.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_SelSameDir.Text = "これ以降の同一ディレクトリ内のファイルを選択";
            ToolStripMenuItem_SelSameDir.Click += ToolStripMenuItem_SelSameDir_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(395, 6);
            // 
            // ToolStripMenuItem_DB_op
            // 
            ToolStripMenuItem_DB_op.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_rating_down, ToolStripMenuItem_rating_up });
            ToolStripMenuItem_DB_op.Name = "ToolStripMenuItem_DB_op";
            ToolStripMenuItem_DB_op.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_DB_op.Text = "DB操作";
            // 
            // ToolStripMenuItem_rating_down
            // 
            ToolStripMenuItem_rating_down.Name = "ToolStripMenuItem_rating_down";
            ToolStripMenuItem_rating_down.Size = new System.Drawing.Size(237, 28);
            ToolStripMenuItem_rating_down.Text = "評価ダウンリストに記録";
            // 
            // ToolStripMenuItem_rating_up
            // 
            ToolStripMenuItem_rating_up.Name = "ToolStripMenuItem_rating_up";
            ToolStripMenuItem_rating_up.Size = new System.Drawing.Size(237, 28);
            ToolStripMenuItem_rating_up.Text = "評価アップリストに記録";
            // 
            // ToolStripMenuItem_sql
            // 
            ToolStripMenuItem_sql.Name = "ToolStripMenuItem_sql";
            ToolStripMenuItem_sql.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_sql.Text = "SQL";
            ToolStripMenuItem_sql.Click += ToolStripMenuItem_sql_Click;
            // 
            // ToolStripMenuItem_Slides
            // 
            ToolStripMenuItem_Slides.Name = "ToolStripMenuItem_Slides";
            ToolStripMenuItem_Slides.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_Slides.Text = "スライドショー";
            ToolStripMenuItem_Slides.Click += ToolStripMenuItem_Slides_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(395, 6);
            // 
            // ToolStripMenuItem_Quit
            // 
            ToolStripMenuItem_Quit.Name = "ToolStripMenuItem_Quit";
            ToolStripMenuItem_Quit.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_Quit.Text = "終了";
            ToolStripMenuItem_Quit.Click += ToolStripMenuItem_Quit_Click;
            // 
            // ToolStripMenuItem_FileSel
            // 
            ToolStripMenuItem_FileSel.Name = "ToolStripMenuItem_FileSel";
            ToolStripMenuItem_FileSel.Size = new System.Drawing.Size(398, 28);
            ToolStripMenuItem_FileSel.Text = "ファイル選択";
            ToolStripMenuItem_FileSel.Click += ToolStripMenuItem_FileSel_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripProgressBar, statusLbl_No, StatusLbl_MarkCnt, statusLbl_WxH, statusLbl_ratio, StatusLbl_Dirname, statusLbl_Filename, statusLbl_FileSize, StatusLbl_LWTime });
            statusStrip.Location = new System.Drawing.Point(0, 724);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 20, 0);
            statusStrip.Size = new System.Drawing.Size(1528, 37);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new System.Drawing.Size(151, 31);
            // 
            // statusLbl_No
            // 
            statusLbl_No.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            statusLbl_No.Name = "statusLbl_No";
            statusLbl_No.Size = new System.Drawing.Size(39, 32);
            statusLbl_No.Text = "0/0";
            // 
            // StatusLbl_MarkCnt
            // 
            StatusLbl_MarkCnt.Name = "StatusLbl_MarkCnt";
            StatusLbl_MarkCnt.Size = new System.Drawing.Size(77, 32);
            StatusLbl_MarkCnt.Text = "mark cnt";
            // 
            // statusLbl_WxH
            // 
            statusLbl_WxH.Name = "statusLbl_WxH";
            statusLbl_WxH.Size = new System.Drawing.Size(40, 32);
            statusLbl_WxH.Text = "wxh";
            // 
            // statusLbl_ratio
            // 
            statusLbl_ratio.Name = "statusLbl_ratio";
            statusLbl_ratio.Size = new System.Drawing.Size(44, 32);
            statusLbl_ratio.Text = "倍率";
            // 
            // StatusLbl_Dirname
            // 
            StatusLbl_Dirname.Name = "StatusLbl_Dirname";
            StatusLbl_Dirname.Size = new System.Drawing.Size(73, 32);
            StatusLbl_Dirname.Text = "dirname";
            // 
            // statusLbl_Filename
            // 
            statusLbl_Filename.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            statusLbl_Filename.Name = "statusLbl_Filename";
            statusLbl_Filename.Size = new System.Drawing.Size(80, 32);
            statusLbl_Filename.Text = "filename";
            // 
            // statusLbl_FileSize
            // 
            statusLbl_FileSize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            statusLbl_FileSize.Name = "statusLbl_FileSize";
            statusLbl_FileSize.Size = new System.Drawing.Size(65, 32);
            statusLbl_FileSize.Text = "filesize";
            // 
            // StatusLbl_LWTime
            // 
            StatusLbl_LWTime.Name = "StatusLbl_LWTime";
            StatusLbl_LWTime.Size = new System.Drawing.Size(83, 32);
            StatusLbl_LWTime.Text = "DateTime";
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ファイルToolStripMenuItem, 表示ToolStripMenuItem, ツールToolStripMenuItem, 設定SToolStripMenuItem, ウィンドウToolStripMenuItem, ヘルプToolStripMenuItem });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            menuStrip.Size = new System.Drawing.Size(1528, 33);
            menuStrip.TabIndex = 3;
            menuStrip.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_Close });
            ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            ファイルToolStripMenuItem.Size = new System.Drawing.Size(88, 27);
            ファイルToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // ToolStripMenuItem_Close
            // 
            ToolStripMenuItem_Close.Name = "ToolStripMenuItem_Close";
            ToolStripMenuItem_Close.Size = new System.Drawing.Size(134, 28);
            ToolStripMenuItem_Close.Text = "終了(&X)";
            ToolStripMenuItem_Close.Click += ToolStripMenuItem_Close_Click;
            // 
            // 表示ToolStripMenuItem
            // 
            表示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_SlideShow, MenuItem_FullScreen, toolStripSeparator7, ToolStripMenuItem_ThumbnailOn, MenuItem_MagSub, toolStripMenuItem1, toolStripSeparator8, ToolStripMenuItem_fwd_one });
            表示ToolStripMenuItem.Name = "表示ToolStripMenuItem";
            表示ToolStripMenuItem.Size = new System.Drawing.Size(77, 27);
            表示ToolStripMenuItem.Text = "表示(&V)";
            // 
            // MenuItem_FullScreen
            // 
            MenuItem_FullScreen.Name = "MenuItem_FullScreen";
            MenuItem_FullScreen.ShortcutKeys = System.Windows.Forms.Keys.F3;
            MenuItem_FullScreen.Size = new System.Drawing.Size(378, 28);
            MenuItem_FullScreen.Text = "全画面表示(&F)";
            MenuItem_FullScreen.Click += MenuItem_FullScreen_Click;
            // 
            // ToolStripMenuItem_SlideShow
            // 
            ToolStripMenuItem_SlideShow.Name = "ToolStripMenuItem_SlideShow";
            ToolStripMenuItem_SlideShow.ShortcutKeys = System.Windows.Forms.Keys.F1;
            ToolStripMenuItem_SlideShow.Size = new System.Drawing.Size(378, 28);
            ToolStripMenuItem_SlideShow.Text = "スライドショー";
            ToolStripMenuItem_SlideShow.Click += ToolStripMenuItem_SlideShow_Click;
            // 
            // ToolStripMenuItem_ThumbnailOn
            // 
            ToolStripMenuItem_ThumbnailOn.Checked = true;
            ToolStripMenuItem_ThumbnailOn.CheckOnClick = true;
            ToolStripMenuItem_ThumbnailOn.CheckState = System.Windows.Forms.CheckState.Checked;
            ToolStripMenuItem_ThumbnailOn.Name = "ToolStripMenuItem_ThumbnailOn";
            ToolStripMenuItem_ThumbnailOn.ShortcutKeys = System.Windows.Forms.Keys.F4;
            ToolStripMenuItem_ThumbnailOn.Size = new System.Drawing.Size(378, 28);
            ToolStripMenuItem_ThumbnailOn.Text = "サムネイル表示/非表示切替";
            ToolStripMenuItem_ThumbnailOn.Click += ToolStripMenuItem_ThumbnailOn_Click;
            // 
            // MenuItem_MagSub
            // 
            MenuItem_MagSub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_MagSub_FitNoMag });
            MenuItem_MagSub.Name = "MenuItem_MagSub";
            MenuItem_MagSub.Size = new System.Drawing.Size(378, 28);
            MenuItem_MagSub.Text = "倍率";
            // 
            // MenuItem_MagSub_FitNoMag
            // 
            MenuItem_MagSub_FitNoMag.Checked = true;
            MenuItem_MagSub_FitNoMag.CheckState = System.Windows.Forms.CheckState.Checked;
            MenuItem_MagSub_FitNoMag.Name = "MenuItem_MagSub_FitNoMag";
            MenuItem_MagSub_FitNoMag.Size = new System.Drawing.Size(204, 28);
            MenuItem_MagSub_FitNoMag.Text = "縮小のみ拡大なし";
            MenuItem_MagSub_FitNoMag.Click += MenuItem_MagSub_FitNoMag_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem_Sort_FilePath, ToolStripMenuItem_Sort_Filename, toolStripMenuItem_Sort_Title, toolStripSeparator5, ToolStripMenuItem_Sort_NumPixel, ToolStripMenuItem_AspectRatio, toolStripSeparator6, toolStripMenuItem_Sort_FileSize, ToolStripMenuItem_Sort_FileHash });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(378, 28);
            toolStripMenuItem1.Text = "表示/ソート順";
            // 
            // toolStripMenuItem_Sort_FilePath
            // 
            toolStripMenuItem_Sort_FilePath.Name = "toolStripMenuItem_Sort_FilePath";
            toolStripMenuItem_Sort_FilePath.Size = new System.Drawing.Size(194, 28);
            toolStripMenuItem_Sort_FilePath.Text = "ファイルパス";
            toolStripMenuItem_Sort_FilePath.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // ToolStripMenuItem_Sort_Filename
            // 
            ToolStripMenuItem_Sort_Filename.Name = "ToolStripMenuItem_Sort_Filename";
            ToolStripMenuItem_Sort_Filename.Size = new System.Drawing.Size(194, 28);
            ToolStripMenuItem_Sort_Filename.Text = "ファイル名";
            ToolStripMenuItem_Sort_Filename.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // toolStripMenuItem_Sort_Title
            // 
            toolStripMenuItem_Sort_Title.Name = "toolStripMenuItem_Sort_Title";
            toolStripMenuItem_Sort_Title.Size = new System.Drawing.Size(194, 28);
            toolStripMenuItem_Sort_Title.Text = "タイトル";
            toolStripMenuItem_Sort_Title.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(191, 6);
            // 
            // ToolStripMenuItem_Sort_NumPixel
            // 
            ToolStripMenuItem_Sort_NumPixel.Name = "ToolStripMenuItem_Sort_NumPixel";
            ToolStripMenuItem_Sort_NumPixel.Size = new System.Drawing.Size(194, 28);
            ToolStripMenuItem_Sort_NumPixel.Text = "ピクセル数";
            ToolStripMenuItem_Sort_NumPixel.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // ToolStripMenuItem_AspectRatio
            // 
            ToolStripMenuItem_AspectRatio.Name = "ToolStripMenuItem_AspectRatio";
            ToolStripMenuItem_AspectRatio.Size = new System.Drawing.Size(194, 28);
            ToolStripMenuItem_AspectRatio.Text = "アスペクト比";
            ToolStripMenuItem_AspectRatio.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripMenuItem_Sort_FileSize
            // 
            toolStripMenuItem_Sort_FileSize.Name = "toolStripMenuItem_Sort_FileSize";
            toolStripMenuItem_Sort_FileSize.Size = new System.Drawing.Size(194, 28);
            toolStripMenuItem_Sort_FileSize.Text = "ファイルサイズ";
            toolStripMenuItem_Sort_FileSize.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // ToolStripMenuItem_Sort_FileHash
            // 
            ToolStripMenuItem_Sort_FileHash.Name = "ToolStripMenuItem_Sort_FileHash";
            ToolStripMenuItem_Sort_FileHash.Size = new System.Drawing.Size(194, 28);
            ToolStripMenuItem_Sort_FileHash.Text = "ファイルハッシュ値";
            ToolStripMenuItem_Sort_FileHash.Click += toolStripMenuItem_Sort_FilePath_Click;
            // 
            // ToolStripMenuItem_fwd_one
            // 
            ToolStripMenuItem_fwd_one.Name = "ToolStripMenuItem_fwd_one";
            ToolStripMenuItem_fwd_one.ShortcutKeys = System.Windows.Forms.Keys.F9;
            ToolStripMenuItem_fwd_one.Size = new System.Drawing.Size(378, 28);
            ToolStripMenuItem_fwd_one.Text = "現在の画像の表示位置を一つ前に移動";
            ToolStripMenuItem_fwd_one.Click += ToolStripMenuItem_fwd_one_Click;
            // 
            // ツールToolStripMenuItem
            // 
            ツールToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_SelImg, sQLTestToolStripMenuItem, ToolStripMenuItem_SelectedImageMove, toolStripMenuItem_tsv, toolStripMenuItem_Stat, toolStripSeparator4, toolStripMenuItem_UnselectAll });
            ツールToolStripMenuItem.Name = "ツールToolStripMenuItem";
            ツールToolStripMenuItem.Size = new System.Drawing.Size(79, 27);
            ツールToolStripMenuItem.Text = "ツール(&T)";
            // 
            // ToolStripMenuItem_SelImg
            // 
            ToolStripMenuItem_SelImg.Name = "ToolStripMenuItem_SelImg";
            ToolStripMenuItem_SelImg.ShortcutKeys = System.Windows.Forms.Keys.F5;
            ToolStripMenuItem_SelImg.Size = new System.Drawing.Size(274, 28);
            ToolStripMenuItem_SelImg.Text = "選択中画像のみ表示(&S)";
            ToolStripMenuItem_SelImg.Click += ToolStripMenuItem_SelImg_Click;
            // 
            // sQLTestToolStripMenuItem
            // 
            sQLTestToolStripMenuItem.Name = "sQLTestToolStripMenuItem";
            sQLTestToolStripMenuItem.Size = new System.Drawing.Size(274, 28);
            sQLTestToolStripMenuItem.Text = "SQL test";
            // 
            // ToolStripMenuItem_SelectedImageMove
            // 
            ToolStripMenuItem_SelectedImageMove.Name = "ToolStripMenuItem_SelectedImageMove";
            ToolStripMenuItem_SelectedImageMove.Size = new System.Drawing.Size(274, 28);
            ToolStripMenuItem_SelectedImageMove.Text = "選択中の画像を移動";
            ToolStripMenuItem_SelectedImageMove.Click += ToolStripMenuItem_SelectedImageMove_Click;
            // 
            // toolStripMenuItem_tsv
            // 
            toolStripMenuItem_tsv.Name = "toolStripMenuItem_tsv";
            toolStripMenuItem_tsv.Size = new System.Drawing.Size(274, 28);
            toolStripMenuItem_tsv.Text = "TSV";
            toolStripMenuItem_tsv.Click += toolStripMenuItem_tsv_Click;
            // 
            // toolStripMenuItem_Stat
            // 
            toolStripMenuItem_Stat.Name = "toolStripMenuItem_Stat";
            toolStripMenuItem_Stat.Size = new System.Drawing.Size(274, 28);
            toolStripMenuItem_Stat.Text = "統計情報";
            toolStripMenuItem_Stat.Click += toolStripMenuItem_Stat_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(271, 6);
            // 
            // toolStripMenuItem_UnselectAll
            // 
            toolStripMenuItem_UnselectAll.Name = "toolStripMenuItem_UnselectAll";
            toolStripMenuItem_UnselectAll.Size = new System.Drawing.Size(274, 28);
            toolStripMenuItem_UnselectAll.Text = "すべての選択を解除";
            toolStripMenuItem_UnselectAll.Click += toolStripMenuItem_UnselectAll_Click;
            // 
            // 設定SToolStripMenuItem
            // 
            設定SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_TransitionEffect, TToolStripMenuItem, ToolStripMenuItem_SlideShow_Ms });
            設定SToolStripMenuItem.Name = "設定SToolStripMenuItem";
            設定SToolStripMenuItem.Size = new System.Drawing.Size(75, 27);
            設定SToolStripMenuItem.Text = "設定(&S)";
            // 
            // MenuItem_TransitionEffect
            // 
            MenuItem_TransitionEffect.Name = "MenuItem_TransitionEffect";
            MenuItem_TransitionEffect.Size = new System.Drawing.Size(252, 28);
            MenuItem_TransitionEffect.Text = "切り替えエフェクト使用(&E)";
            MenuItem_TransitionEffect.Click += MenuItem_TransitionEffect_Click;
            // 
            // TToolStripMenuItem
            // 
            TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { x3ToolStripMenuItem, x2ToolStripMenuItem, x4ToolStripMenuItem1 });
            TToolStripMenuItem.Name = "TToolStripMenuItem";
            TToolStripMenuItem.Size = new System.Drawing.Size(258, 28);
            TToolStripMenuItem.Text = "サムネイル表示形式...(&T)";
            TToolStripMenuItem.Click += TToolStripMenuItem_Click;
            // 
            // x3ToolStripMenuItem
            // 
            x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            x3ToolStripMenuItem.Size = new System.Drawing.Size(106, 28);
            x3ToolStripMenuItem.Text = "3x3";
            // 
            // x2ToolStripMenuItem
            // 
            x2ToolStripMenuItem.Name = "x2ToolStripMenuItem";
            x2ToolStripMenuItem.Size = new System.Drawing.Size(106, 28);
            x2ToolStripMenuItem.Text = "2x2";
            x2ToolStripMenuItem.Click += X2ToolStripMenuItem_Click;
            // 
            // x4ToolStripMenuItem1
            // 
            x4ToolStripMenuItem1.Name = "x4ToolStripMenuItem1";
            x4ToolStripMenuItem1.Size = new System.Drawing.Size(106, 28);
            x4ToolStripMenuItem1.Text = "4x3";
            x4ToolStripMenuItem1.Click += X4ToolStripMenuItem1_Click;
            // 
            // ToolStripMenuItem_SlideShow_Ms
            // 
            ToolStripMenuItem_SlideShow_Ms.Name = "ToolStripMenuItem_SlideShow_Ms";
            ToolStripMenuItem_SlideShow_Ms.Size = new System.Drawing.Size(258, 28);
            ToolStripMenuItem_SlideShow_Ms.Text = "スライドショーの更新間隔...";
            ToolStripMenuItem_SlideShow_Ms.Click += ToolStripMenuItem_SlideShow_Ms_Click;
            // 
            // ウィンドウToolStripMenuItem
            // 
            ウィンドウToolStripMenuItem.Name = "ウィンドウToolStripMenuItem";
            ウィンドウToolStripMenuItem.Size = new System.Drawing.Size(109, 27);
            ウィンドウToolStripMenuItem.Text = "ウィンドウ(&W)";
            // 
            // ヘルプToolStripMenuItem
            // 
            ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            ヘルプToolStripMenuItem.Size = new System.Drawing.Size(85, 27);
            ヘルプToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // RightPicBox
            // 
            RightPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            RightPicBox.Location = new System.Drawing.Point(1528, 33);
            RightPicBox.Margin = new System.Windows.Forms.Padding(2);
            RightPicBox.Name = "RightPicBox";
            RightPicBox.Size = new System.Drawing.Size(0, 691);
            RightPicBox.TabIndex = 4;
            RightPicBox.TabStop = false;
            RightPicBox.Paint += rightPicBox_Paint;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(375, 6);
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(375, 6);
            // 
            // PictureForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1528, 761);
            Controls.Add(RightPicBox);
            Controls.Add(pictureBox);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Name = "PictureForm";
            Text = "PictureForm";
            FormClosed += PictureForm_FormClosed;
            Load += PictureForm_Load;
            KeyDown += PictureForm_KeyDown;
            Resize += PictureForm_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            contextMenuStrip_pic.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)RightPicBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 表示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ウィンドウToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_FullScreen;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl_No;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl_Filename;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl_WxH;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl_FileSize;
        private System.Windows.Forms.ToolStripMenuItem 設定SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_TransitionEffect;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl_Dirname;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MagSub;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl_LWTime;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl_ratio;
        private System.Windows.Forms.PictureBox RightPicBox;
        private System.Windows.Forms.ToolStripMenuItem TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x4ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl_MarkCnt;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_fwd_one;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Close;
        private System.Windows.Forms.ToolStripMenuItem ツールToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SelImg;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_pic;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_PathCopy;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SelectedImageMove;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ThumbnailOn;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MagSub_FitNoMag;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CopyParentDirPath;
        private System.Windows.Forms.ToolStripMenuItem sQLTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_sql;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_OpenFiler;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SlideShow;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Quit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Sort_FilePath;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Sort_FileSize;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Slides;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Sort_NumPixel;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SelSameDir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_AspectRatio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SlideShow_Ms;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_hide;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_DB_op;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_rating_down;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_rating_up;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Sort_FileHash;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_tsv;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Stat;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SaveFilesPath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_UnselectAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_FileSel;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Sort_Filename;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Sort_Title;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    }
}