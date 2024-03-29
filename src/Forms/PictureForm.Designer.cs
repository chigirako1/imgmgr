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
            終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_FullScreen = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_MagSub = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_MagSub_FitNoMag = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_fwd_one = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_ThumbnailOn = new System.Windows.Forms.ToolStripMenuItem();
            ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SelImg = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SelectedImageMove = new System.Windows.Forms.ToolStripMenuItem();
            sQLTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            設定SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_TransitionEffect = new System.Windows.Forms.ToolStripMenuItem();
            TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x4ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ウィンドウToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rightPicBox = new System.Windows.Forms.PictureBox();
            sQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            contextMenuStrip_pic.SuspendLayout();
            statusStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightPicBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.ContextMenuStrip = contextMenuStrip_pic;
            pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBox.Location = new System.Drawing.Point(0, 42);
            pictureBox.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(1173, 1033);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Paint += PictureBox_Paint;
            // 
            // contextMenuStrip_pic
            // 
            contextMenuStrip_pic.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip_pic.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_PathCopy, ToolStripMenuItem_CopyParentDirPath, sQLToolStripMenuItem });
            contextMenuStrip_pic.Name = "contextMenuStrip_pic";
            contextMenuStrip_pic.Size = new System.Drawing.Size(479, 134);
            // 
            // ToolStripMenuItem_PathCopy
            // 
            ToolStripMenuItem_PathCopy.Name = "ToolStripMenuItem_PathCopy";
            ToolStripMenuItem_PathCopy.Size = new System.Drawing.Size(478, 34);
            ToolStripMenuItem_PathCopy.Text = "ファイルのパスをクリップボードにコピー";
            ToolStripMenuItem_PathCopy.Click += ToolStripMenuItem_PathCopy_Click;
            // 
            // ToolStripMenuItem_CopyParentDirPath
            // 
            ToolStripMenuItem_CopyParentDirPath.Name = "ToolStripMenuItem_CopyParentDirPath";
            ToolStripMenuItem_CopyParentDirPath.Size = new System.Drawing.Size(478, 34);
            ToolStripMenuItem_CopyParentDirPath.Text = "ファイルの親フォルダのパスをクリップボードにコピー";
            ToolStripMenuItem_CopyParentDirPath.Click += ToolStripMenuItem_CopyParentDirPath_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripProgressBar, statusLbl_No, StatusLbl_MarkCnt, statusLbl_WxH, statusLbl_ratio, StatusLbl_Dirname, statusLbl_Filename, statusLbl_FileSize, StatusLbl_LWTime });
            statusStrip.Location = new System.Drawing.Point(0, 1075);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new System.Windows.Forms.Padding(3, 0, 27, 0);
            statusStrip.Size = new System.Drawing.Size(2038, 47);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new System.Drawing.Size(201, 39);
            // 
            // statusLbl_No
            // 
            statusLbl_No.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            statusLbl_No.Name = "statusLbl_No";
            statusLbl_No.Size = new System.Drawing.Size(47, 41);
            statusLbl_No.Text = "0/0";
            // 
            // StatusLbl_MarkCnt
            // 
            StatusLbl_MarkCnt.Name = "StatusLbl_MarkCnt";
            StatusLbl_MarkCnt.Size = new System.Drawing.Size(94, 41);
            StatusLbl_MarkCnt.Text = "mark cnt";
            // 
            // statusLbl_WxH
            // 
            statusLbl_WxH.Name = "statusLbl_WxH";
            statusLbl_WxH.Size = new System.Drawing.Size(50, 41);
            statusLbl_WxH.Text = "wxh";
            // 
            // statusLbl_ratio
            // 
            statusLbl_ratio.Name = "statusLbl_ratio";
            statusLbl_ratio.Size = new System.Drawing.Size(55, 41);
            statusLbl_ratio.Text = "倍率";
            // 
            // StatusLbl_Dirname
            // 
            StatusLbl_Dirname.Name = "StatusLbl_Dirname";
            StatusLbl_Dirname.Size = new System.Drawing.Size(89, 41);
            StatusLbl_Dirname.Text = "dirname";
            // 
            // statusLbl_Filename
            // 
            statusLbl_Filename.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            statusLbl_Filename.Name = "statusLbl_Filename";
            statusLbl_Filename.Size = new System.Drawing.Size(98, 41);
            statusLbl_Filename.Text = "filename";
            // 
            // statusLbl_FileSize
            // 
            statusLbl_FileSize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            statusLbl_FileSize.Name = "statusLbl_FileSize";
            statusLbl_FileSize.Size = new System.Drawing.Size(81, 41);
            statusLbl_FileSize.Text = "filesize";
            // 
            // StatusLbl_LWTime
            // 
            StatusLbl_LWTime.Name = "StatusLbl_LWTime";
            StatusLbl_LWTime.Size = new System.Drawing.Size(102, 41);
            StatusLbl_LWTime.Text = "DateTime";
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ファイルToolStripMenuItem, 表示ToolStripMenuItem, ツールToolStripMenuItem, 設定SToolStripMenuItem, ウィンドウToolStripMenuItem, ヘルプToolStripMenuItem });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            menuStrip.Size = new System.Drawing.Size(2038, 42);
            menuStrip.TabIndex = 3;
            menuStrip.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 終了XToolStripMenuItem });
            ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            ファイルToolStripMenuItem.Size = new System.Drawing.Size(108, 34);
            ファイルToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 終了XToolStripMenuItem
            // 
            終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
            終了XToolStripMenuItem.Size = new System.Drawing.Size(166, 34);
            終了XToolStripMenuItem.Text = "終了(&X)";
            // 
            // 表示ToolStripMenuItem
            // 
            表示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_FullScreen, MenuItem_MagSub, ToolStripMenuItem_fwd_one, ToolStripMenuItem_ThumbnailOn });
            表示ToolStripMenuItem.Name = "表示ToolStripMenuItem";
            表示ToolStripMenuItem.Size = new System.Drawing.Size(94, 34);
            表示ToolStripMenuItem.Text = "表示(&V)";
            // 
            // MenuItem_FullScreen
            // 
            MenuItem_FullScreen.Name = "MenuItem_FullScreen";
            MenuItem_FullScreen.ShortcutKeys = System.Windows.Forms.Keys.F1;
            MenuItem_FullScreen.Size = new System.Drawing.Size(468, 34);
            MenuItem_FullScreen.Text = "全画面表示(&F)";
            MenuItem_FullScreen.Click += MenuItem_FullScreen_Click;
            // 
            // MenuItem_MagSub
            // 
            MenuItem_MagSub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_MagSub_FitNoMag });
            MenuItem_MagSub.Name = "MenuItem_MagSub";
            MenuItem_MagSub.Size = new System.Drawing.Size(468, 34);
            MenuItem_MagSub.Text = "倍率";
            MenuItem_MagSub.Click += MenuItem_MagSub_Click;
            // 
            // MenuItem_MagSub_FitNoMag
            // 
            MenuItem_MagSub_FitNoMag.Checked = true;
            MenuItem_MagSub_FitNoMag.CheckState = System.Windows.Forms.CheckState.Checked;
            MenuItem_MagSub_FitNoMag.Name = "MenuItem_MagSub_FitNoMag";
            MenuItem_MagSub_FitNoMag.Size = new System.Drawing.Size(253, 34);
            MenuItem_MagSub_FitNoMag.Text = "縮小のみ拡大なし";
            // 
            // ToolStripMenuItem_fwd_one
            // 
            ToolStripMenuItem_fwd_one.Name = "ToolStripMenuItem_fwd_one";
            ToolStripMenuItem_fwd_one.ShortcutKeys = System.Windows.Forms.Keys.F9;
            ToolStripMenuItem_fwd_one.Size = new System.Drawing.Size(468, 34);
            ToolStripMenuItem_fwd_one.Text = "現在の画像の表示位置を一つ前に移動";
            ToolStripMenuItem_fwd_one.Click += ToolStripMenuItem_fwd_one_Click;
            // 
            // ToolStripMenuItem_ThumbnailOn
            // 
            ToolStripMenuItem_ThumbnailOn.Checked = true;
            ToolStripMenuItem_ThumbnailOn.CheckOnClick = true;
            ToolStripMenuItem_ThumbnailOn.CheckState = System.Windows.Forms.CheckState.Checked;
            ToolStripMenuItem_ThumbnailOn.Name = "ToolStripMenuItem_ThumbnailOn";
            ToolStripMenuItem_ThumbnailOn.Size = new System.Drawing.Size(468, 34);
            ToolStripMenuItem_ThumbnailOn.Text = "サムネイル表示";
            ToolStripMenuItem_ThumbnailOn.Click += ToolStripMenuItem_ThumbnailOn_Click;
            // 
            // ツールToolStripMenuItem
            // 
            ツールToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_SelImg, ToolStripMenuItem_SelectedImageMove, sQLTestToolStripMenuItem });
            ツールToolStripMenuItem.Name = "ツールToolStripMenuItem";
            ツールToolStripMenuItem.Size = new System.Drawing.Size(97, 34);
            ツールToolStripMenuItem.Text = "ツール(&T)";
            // 
            // ToolStripMenuItem_SelImg
            // 
            ToolStripMenuItem_SelImg.Name = "ToolStripMenuItem_SelImg";
            ToolStripMenuItem_SelImg.ShortcutKeys = System.Windows.Forms.Keys.F5;
            ToolStripMenuItem_SelImg.Size = new System.Drawing.Size(340, 34);
            ToolStripMenuItem_SelImg.Text = "選択中画像のみ表示(&S)";
            ToolStripMenuItem_SelImg.Click += ToolStripMenuItem_SelImg_Click;
            // 
            // ToolStripMenuItem_SelectedImageMove
            // 
            ToolStripMenuItem_SelectedImageMove.Name = "ToolStripMenuItem_SelectedImageMove";
            ToolStripMenuItem_SelectedImageMove.Size = new System.Drawing.Size(340, 34);
            ToolStripMenuItem_SelectedImageMove.Text = "選択中の画像を移動";
            ToolStripMenuItem_SelectedImageMove.Click += ToolStripMenuItem_SelectedImageMove_Click;
            // 
            // sQLTestToolStripMenuItem
            // 
            sQLTestToolStripMenuItem.Name = "sQLTestToolStripMenuItem";
            sQLTestToolStripMenuItem.Size = new System.Drawing.Size(340, 34);
            sQLTestToolStripMenuItem.Text = "SQL test";
            sQLTestToolStripMenuItem.Click += sQLTestToolStripMenuItem_Click;
            // 
            // 設定SToolStripMenuItem
            // 
            設定SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_TransitionEffect, TToolStripMenuItem });
            設定SToolStripMenuItem.Name = "設定SToolStripMenuItem";
            設定SToolStripMenuItem.Size = new System.Drawing.Size(92, 34);
            設定SToolStripMenuItem.Text = "設定(&S)";
            // 
            // MenuItem_TransitionEffect
            // 
            MenuItem_TransitionEffect.Name = "MenuItem_TransitionEffect";
            MenuItem_TransitionEffect.Size = new System.Drawing.Size(310, 34);
            MenuItem_TransitionEffect.Text = "切り替えエフェクト使用(&E)";
            MenuItem_TransitionEffect.Click += MenuItem_TransitionEffect_Click;
            // 
            // TToolStripMenuItem
            // 
            TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { x3ToolStripMenuItem, x2ToolStripMenuItem, x4ToolStripMenuItem1 });
            TToolStripMenuItem.Name = "TToolStripMenuItem";
            TToolStripMenuItem.Size = new System.Drawing.Size(310, 34);
            TToolStripMenuItem.Text = "サムネイル表示形式(&T)";
            TToolStripMenuItem.Click += TToolStripMenuItem_Click;
            // 
            // x3ToolStripMenuItem
            // 
            x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            x3ToolStripMenuItem.Size = new System.Drawing.Size(132, 34);
            x3ToolStripMenuItem.Text = "3x3";
            // 
            // x2ToolStripMenuItem
            // 
            x2ToolStripMenuItem.Name = "x2ToolStripMenuItem";
            x2ToolStripMenuItem.Size = new System.Drawing.Size(132, 34);
            x2ToolStripMenuItem.Text = "2x2";
            x2ToolStripMenuItem.Click += X2ToolStripMenuItem_Click;
            // 
            // x4ToolStripMenuItem1
            // 
            x4ToolStripMenuItem1.Name = "x4ToolStripMenuItem1";
            x4ToolStripMenuItem1.Size = new System.Drawing.Size(132, 34);
            x4ToolStripMenuItem1.Text = "4x3";
            x4ToolStripMenuItem1.Click += X4ToolStripMenuItem1_Click;
            // 
            // ウィンドウToolStripMenuItem
            // 
            ウィンドウToolStripMenuItem.Name = "ウィンドウToolStripMenuItem";
            ウィンドウToolStripMenuItem.Size = new System.Drawing.Size(134, 34);
            ウィンドウToolStripMenuItem.Text = "ウィンドウ(&W)";
            // 
            // ヘルプToolStripMenuItem
            // 
            ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            ヘルプToolStripMenuItem.Size = new System.Drawing.Size(104, 34);
            ヘルプToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // rightPicBox
            // 
            rightPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            rightPicBox.Location = new System.Drawing.Point(1173, 42);
            rightPicBox.Name = "rightPicBox";
            rightPicBox.Size = new System.Drawing.Size(865, 1033);
            rightPicBox.TabIndex = 4;
            rightPicBox.TabStop = false;
            rightPicBox.Paint += rightPicBox_Paint;
            // 
            // sQLToolStripMenuItem
            // 
            sQLToolStripMenuItem.Name = "sQLToolStripMenuItem";
            sQLToolStripMenuItem.Size = new System.Drawing.Size(478, 34);
            sQLToolStripMenuItem.Text = "SQL";
            sQLToolStripMenuItem.Click += sQLToolStripMenuItem_Click;
            // 
            // PictureForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(2038, 1122);
            Controls.Add(rightPicBox);
            Controls.Add(pictureBox);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
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
            ((System.ComponentModel.ISupportInitialize)rightPicBox).EndInit();
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
        private System.Windows.Forms.PictureBox rightPicBox;
        private System.Windows.Forms.ToolStripMenuItem TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x4ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl_MarkCnt;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_fwd_one;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ツールToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SelImg;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_pic;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_PathCopy;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SelectedImageMove;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ThumbnailOn;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MagSub_FitNoMag;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CopyParentDirPath;
        private System.Windows.Forms.ToolStripMenuItem sQLTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQLToolStripMenuItem;
    }
}