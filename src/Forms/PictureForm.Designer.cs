namespace PictureManagerApp
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
            pictureBox = new System.Windows.Forms.PictureBox();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            statusLbl_No = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLbl_Dirname = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_Filename = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_WxH = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_ratio = new System.Windows.Forms.ToolStripStatusLabel();
            statusLbl_FileSize = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLbl_LWTime = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip = new System.Windows.Forms.MenuStrip();
            ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_FullScreen = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_MagSub = new System.Windows.Forms.ToolStripMenuItem();
            ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            設定SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MenuItem_TransitionEffect = new System.Windows.Forms.ToolStripMenuItem();
            ウィンドウToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rightPicBox = new System.Windows.Forms.PictureBox();
            サムネイル表示形式TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            x3ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            statusStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightPicBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBox.Location = new System.Drawing.Point(0, 42);
            pictureBox.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(1173, 1033);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Paint += PictureBox_Paint;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripProgressBar, statusLbl_No, StatusLbl_Dirname, statusLbl_Filename, statusLbl_WxH, statusLbl_ratio, statusLbl_FileSize, StatusLbl_LWTime });
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
            終了XToolStripMenuItem.Size = new System.Drawing.Size(224, 34);
            終了XToolStripMenuItem.Text = "終了(&X)";
            // 
            // 表示ToolStripMenuItem
            // 
            表示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_FullScreen, MenuItem_MagSub });
            表示ToolStripMenuItem.Name = "表示ToolStripMenuItem";
            表示ToolStripMenuItem.Size = new System.Drawing.Size(94, 34);
            表示ToolStripMenuItem.Text = "表示(&V)";
            // 
            // MenuItem_FullScreen
            // 
            MenuItem_FullScreen.Name = "MenuItem_FullScreen";
            MenuItem_FullScreen.ShortcutKeys = System.Windows.Forms.Keys.F1;
            MenuItem_FullScreen.Size = new System.Drawing.Size(261, 34);
            MenuItem_FullScreen.Text = "全画面表示(&F)";
            MenuItem_FullScreen.Click += MenuItem_FullScreen_Click;
            // 
            // MenuItem_MagSub
            // 
            MenuItem_MagSub.Name = "MenuItem_MagSub";
            MenuItem_MagSub.Size = new System.Drawing.Size(261, 34);
            MenuItem_MagSub.Text = "倍率";
            // 
            // ツールToolStripMenuItem
            // 
            ツールToolStripMenuItem.Name = "ツールToolStripMenuItem";
            ツールToolStripMenuItem.Size = new System.Drawing.Size(97, 34);
            ツールToolStripMenuItem.Text = "ツール(&T)";
            // 
            // 設定SToolStripMenuItem
            // 
            設定SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { MenuItem_TransitionEffect, サムネイル表示形式TToolStripMenuItem });
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
            // サムネイル表示形式TToolStripMenuItem
            // 
            サムネイル表示形式TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { x3ToolStripMenuItem, x2ToolStripMenuItem, x3ToolStripMenuItem1 });
            サムネイル表示形式TToolStripMenuItem.Name = "サムネイル表示形式TToolStripMenuItem";
            サムネイル表示形式TToolStripMenuItem.Size = new System.Drawing.Size(310, 34);
            サムネイル表示形式TToolStripMenuItem.Text = "サムネイル表示形式(&T)";
            // 
            // x3ToolStripMenuItem
            // 
            x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            x3ToolStripMenuItem.Size = new System.Drawing.Size(224, 34);
            x3ToolStripMenuItem.Text = "3x3";
            // 
            // x2ToolStripMenuItem
            // 
            x2ToolStripMenuItem.Name = "x2ToolStripMenuItem";
            x2ToolStripMenuItem.Size = new System.Drawing.Size(224, 34);
            x2ToolStripMenuItem.Text = "2x2";
            // 
            // x3ToolStripMenuItem1
            // 
            x3ToolStripMenuItem1.Name = "x3ToolStripMenuItem1";
            x3ToolStripMenuItem1.Size = new System.Drawing.Size(224, 34);
            x3ToolStripMenuItem1.Text = "4x3";
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
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 表示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ウィンドウToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_FullScreen;
        private System.Windows.Forms.ToolStripMenuItem ツールToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem サムネイル表示形式TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem1;
    }
}