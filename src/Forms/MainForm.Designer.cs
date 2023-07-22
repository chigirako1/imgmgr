namespace PictureManagerApp
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            startBtn = new System.Windows.Forms.Button();
            tabControl = new System.Windows.Forms.TabControl();
            tabPageAttr = new System.Windows.Forms.TabPage();
            label5 = new System.Windows.Forms.Label();
            numUD_MinFilesize = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            numUD_MaxFilesize = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            cmbBox_FilenameFilter = new System.Windows.Forms.ComboBox();
            chkListBox_Ext = new System.Windows.Forms.CheckedListBox();
            gpBox_Date = new System.Windows.Forms.GroupBox();
            checkBox_SameDate = new System.Windows.Forms.CheckBox();
            dtPicker_to = new System.Windows.Forms.DateTimePicker();
            dtPicker_from = new System.Windows.Forms.DateTimePicker();
            chkBox_to = new System.Windows.Forms.CheckBox();
            chkBox_from = new System.Windows.Forms.CheckBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            grpBox_PicOrient = new System.Windows.Forms.GroupBox();
            radioBtn_PicOrinet_LS = new System.Windows.Forms.RadioButton();
            radioBtn_PicOrinet_PR = new System.Windows.Forms.RadioButton();
            radioBtn_PicOrinet_All = new System.Windows.Forms.RadioButton();
            numUD_Height = new System.Windows.Forms.NumericUpDown();
            numUD_Width = new System.Windows.Forms.NumericUpDown();
            tabPage1 = new System.Windows.Forms.TabPage();
            txtBox_FileList = new System.Windows.Forms.TextBox();
            btnPaste = new System.Windows.Forms.Button();
            EndBtn = new System.Windows.Forms.Button();
            cmbBoxPath = new System.Windows.Forms.ComboBox();
            btnAppendSubDir = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            tabPageAttr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_MinFilesize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUD_MaxFilesize).BeginInit();
            gpBox_Date.SuspendLayout();
            tabPage2.SuspendLayout();
            grpBox_PicOrient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_Height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUD_Width).BeginInit();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(24, 33);
            label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(55, 30);
            label1.TabIndex = 1;
            label1.Text = "&Path";
            // 
            // startBtn
            // 
            startBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            startBtn.Location = new System.Drawing.Point(20, 1027);
            startBtn.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            startBtn.Name = "startBtn";
            startBtn.Size = new System.Drawing.Size(151, 46);
            startBtn.TabIndex = 4;
            startBtn.Text = "開始(&S)";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += btnStart_Click;
            // 
            // tabControl
            // 
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Controls.Add(tabPageAttr);
            tabControl.Controls.Add(tabPage2);
            tabControl.Controls.Add(tabPage1);
            tabControl.Location = new System.Drawing.Point(20, 93);
            tabControl.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1734, 900);
            tabControl.TabIndex = 3;
            // 
            // tabPageAttr
            // 
            tabPageAttr.Controls.Add(label5);
            tabPageAttr.Controls.Add(numUD_MinFilesize);
            tabPageAttr.Controls.Add(label4);
            tabPageAttr.Controls.Add(numUD_MaxFilesize);
            tabPageAttr.Controls.Add(label3);
            tabPageAttr.Controls.Add(cmbBox_FilenameFilter);
            tabPageAttr.Controls.Add(chkListBox_Ext);
            tabPageAttr.Controls.Add(gpBox_Date);
            tabPageAttr.Location = new System.Drawing.Point(4, 39);
            tabPageAttr.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPageAttr.Name = "tabPageAttr";
            tabPageAttr.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPageAttr.Size = new System.Drawing.Size(1726, 857);
            tabPageAttr.TabIndex = 0;
            tabPageAttr.Text = "file";
            tabPageAttr.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(20, 563);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(197, 30);
            label5.TabIndex = 10;
            label5.Text = "最小ファイルサイズ(kB)";
            // 
            // numUD_MinFilesize
            // 
            numUD_MinFilesize.Location = new System.Drawing.Point(233, 563);
            numUD_MinFilesize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numUD_MinFilesize.Name = "numUD_MinFilesize";
            numUD_MinFilesize.Size = new System.Drawing.Size(150, 35);
            numUD_MinFilesize.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(20, 604);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(197, 30);
            label4.TabIndex = 8;
            label4.Text = "最大ファイルサイズ(kB)";
            // 
            // numUD_MaxFilesize
            // 
            numUD_MaxFilesize.Location = new System.Drawing.Point(233, 604);
            numUD_MaxFilesize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numUD_MaxFilesize.Name = "numUD_MaxFilesize";
            numUD_MaxFilesize.Size = new System.Drawing.Size(150, 35);
            numUD_MaxFilesize.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(639, 33);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(303, 30);
            label3.TabIndex = 6;
            label3.Text = "ファイル名に含まれる文字列の指定";
            // 
            // cmbBox_FilenameFilter
            // 
            cmbBox_FilenameFilter.FormattingEnabled = true;
            cmbBox_FilenameFilter.Location = new System.Drawing.Point(639, 66);
            cmbBox_FilenameFilter.Name = "cmbBox_FilenameFilter";
            cmbBox_FilenameFilter.Size = new System.Drawing.Size(401, 38);
            cmbBox_FilenameFilter.TabIndex = 5;
            // 
            // chkListBox_Ext
            // 
            chkListBox_Ext.CheckOnClick = true;
            chkListBox_Ext.FormattingEnabled = true;
            chkListBox_Ext.Items.AddRange(new object[] { ".jpg,.jpeg", ".png", ".gif", ".bmp" });
            chkListBox_Ext.Location = new System.Drawing.Point(20, 391);
            chkListBox_Ext.Name = "chkListBox_Ext";
            chkListBox_Ext.Size = new System.Drawing.Size(528, 154);
            chkListBox_Ext.TabIndex = 4;
            // 
            // gpBox_Date
            // 
            gpBox_Date.Controls.Add(checkBox_SameDate);
            gpBox_Date.Controls.Add(dtPicker_to);
            gpBox_Date.Controls.Add(dtPicker_from);
            gpBox_Date.Controls.Add(chkBox_to);
            gpBox_Date.Controls.Add(chkBox_from);
            gpBox_Date.Location = new System.Drawing.Point(20, 33);
            gpBox_Date.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            gpBox_Date.Name = "gpBox_Date";
            gpBox_Date.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            gpBox_Date.Size = new System.Drawing.Size(528, 304);
            gpBox_Date.TabIndex = 0;
            gpBox_Date.TabStop = false;
            gpBox_Date.Text = "Date";
            // 
            // checkBox_SameDate
            // 
            checkBox_SameDate.AutoSize = true;
            checkBox_SameDate.Checked = true;
            checkBox_SameDate.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBox_SameDate.Location = new System.Drawing.Point(44, 52);
            checkBox_SameDate.Name = "checkBox_SameDate";
            checkBox_SameDate.Size = new System.Drawing.Size(77, 34);
            checkBox_SameDate.TabIndex = 0;
            checkBox_SameDate.Text = "同日";
            checkBox_SameDate.UseVisualStyleBackColor = true;
            checkBox_SameDate.CheckedChanged += checkBox_SameDate_CheckedChanged;
            // 
            // dtPicker_to
            // 
            dtPicker_to.Enabled = false;
            dtPicker_to.Location = new System.Drawing.Point(149, 225);
            dtPicker_to.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            dtPicker_to.Name = "dtPicker_to";
            dtPicker_to.Size = new System.Drawing.Size(340, 35);
            dtPicker_to.TabIndex = 4;
            // 
            // dtPicker_from
            // 
            dtPicker_from.Location = new System.Drawing.Point(149, 131);
            dtPicker_from.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            dtPicker_from.Name = "dtPicker_from";
            dtPicker_from.Size = new System.Drawing.Size(340, 35);
            dtPicker_from.TabIndex = 2;
            // 
            // chkBox_to
            // 
            chkBox_to.AutoSize = true;
            chkBox_to.Location = new System.Drawing.Point(44, 225);
            chkBox_to.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            chkBox_to.Name = "chkBox_to";
            chkBox_to.Size = new System.Drawing.Size(54, 34);
            chkBox_to.TabIndex = 3;
            chkBox_to.Text = "to";
            chkBox_to.UseVisualStyleBackColor = true;
            // 
            // chkBox_from
            // 
            chkBox_from.AutoSize = true;
            chkBox_from.Location = new System.Drawing.Point(44, 131);
            chkBox_from.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            chkBox_from.Name = "chkBox_from";
            chkBox_from.Size = new System.Drawing.Size(79, 34);
            chkBox_from.TabIndex = 1;
            chkBox_from.Text = "from";
            chkBox_from.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(grpBox_PicOrient);
            tabPage2.Controls.Add(numUD_Height);
            tabPage2.Controls.Add(numUD_Width);
            tabPage2.Location = new System.Drawing.Point(4, 39);
            tabPage2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPage2.Size = new System.Drawing.Size(1726, 857);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "pic";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(151, 55);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(247, 30);
            label2.TabIndex = 4;
            label2.Text = "Maximum Width : Height";
            // 
            // grpBox_PicOrient
            // 
            grpBox_PicOrient.Controls.Add(radioBtn_PicOrinet_LS);
            grpBox_PicOrient.Controls.Add(radioBtn_PicOrinet_PR);
            grpBox_PicOrient.Controls.Add(radioBtn_PicOrinet_All);
            grpBox_PicOrient.Location = new System.Drawing.Point(150, 221);
            grpBox_PicOrient.Name = "grpBox_PicOrient";
            grpBox_PicOrient.Size = new System.Drawing.Size(289, 244);
            grpBox_PicOrient.TabIndex = 2;
            grpBox_PicOrient.TabStop = false;
            grpBox_PicOrient.Text = "画像の向き";
            grpBox_PicOrient.Enter += groupBox1_Enter;
            // 
            // radioBtn_PicOrinet_LS
            // 
            radioBtn_PicOrinet_LS.AutoSize = true;
            radioBtn_PicOrinet_LS.Location = new System.Drawing.Point(48, 133);
            radioBtn_PicOrinet_LS.Name = "radioBtn_PicOrinet_LS";
            radioBtn_PicOrinet_LS.Size = new System.Drawing.Size(92, 34);
            radioBtn_PicOrinet_LS.TabIndex = 1;
            radioBtn_PicOrinet_LS.Text = "横向き";
            radioBtn_PicOrinet_LS.UseVisualStyleBackColor = true;
            // 
            // radioBtn_PicOrinet_PR
            // 
            radioBtn_PicOrinet_PR.AutoSize = true;
            radioBtn_PicOrinet_PR.Location = new System.Drawing.Point(48, 93);
            radioBtn_PicOrinet_PR.Name = "radioBtn_PicOrinet_PR";
            radioBtn_PicOrinet_PR.Size = new System.Drawing.Size(92, 34);
            radioBtn_PicOrinet_PR.TabIndex = 1;
            radioBtn_PicOrinet_PR.Text = "縦向き";
            radioBtn_PicOrinet_PR.UseVisualStyleBackColor = true;
            // 
            // radioBtn_PicOrinet_All
            // 
            radioBtn_PicOrinet_All.AutoSize = true;
            radioBtn_PicOrinet_All.Checked = true;
            radioBtn_PicOrinet_All.Location = new System.Drawing.Point(52, 53);
            radioBtn_PicOrinet_All.Name = "radioBtn_PicOrinet_All";
            radioBtn_PicOrinet_All.Size = new System.Drawing.Size(71, 34);
            radioBtn_PicOrinet_All.TabIndex = 0;
            radioBtn_PicOrinet_All.TabStop = true;
            radioBtn_PicOrinet_All.Text = "全て";
            radioBtn_PicOrinet_All.UseVisualStyleBackColor = true;
            // 
            // numUD_Height
            // 
            numUD_Height.Location = new System.Drawing.Point(308, 105);
            numUD_Height.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numUD_Height.Name = "numUD_Height";
            numUD_Height.Size = new System.Drawing.Size(150, 35);
            numUD_Height.TabIndex = 1;
            // 
            // numUD_Width
            // 
            numUD_Width.Location = new System.Drawing.Point(152, 105);
            numUD_Width.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numUD_Width.Name = "numUD_Width";
            numUD_Width.Size = new System.Drawing.Size(150, 35);
            numUD_Width.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(txtBox_FileList);
            tabPage1.Location = new System.Drawing.Point(4, 39);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(1726, 857);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "filelist";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtBox_FileList
            // 
            txtBox_FileList.Location = new System.Drawing.Point(64, 24);
            txtBox_FileList.Multiline = true;
            txtBox_FileList.Name = "txtBox_FileList";
            txtBox_FileList.Size = new System.Drawing.Size(1320, 804);
            txtBox_FileList.TabIndex = 0;
            // 
            // btnPaste
            // 
            btnPaste.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPaste.Location = new System.Drawing.Point(1758, 33);
            btnPaste.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(119, 38);
            btnPaste.TabIndex = 1;
            btnPaste.Text = "貼り付け(&V)";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // EndBtn
            // 
            EndBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            EndBtn.Location = new System.Drawing.Point(2018, 1027);
            EndBtn.Margin = new System.Windows.Forms.Padding(4);
            EndBtn.Name = "EndBtn";
            EndBtn.Size = new System.Drawing.Size(151, 46);
            EndBtn.TabIndex = 6;
            EndBtn.Text = "終了";
            EndBtn.UseVisualStyleBackColor = true;
            EndBtn.Click += EndBtn_Click;
            // 
            // cmbBoxPath
            // 
            cmbBoxPath.AllowDrop = true;
            cmbBoxPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbBoxPath.FormattingEnabled = true;
            cmbBoxPath.Location = new System.Drawing.Point(88, 33);
            cmbBoxPath.Name = "cmbBoxPath";
            cmbBoxPath.Size = new System.Drawing.Size(1662, 38);
            cmbBoxPath.TabIndex = 0;
            cmbBoxPath.DragDrop += cmbBoxPath_DragDrop;
            cmbBoxPath.DragEnter += cmbBoxPath_DragEnter;
            // 
            // btnAppendSubDir
            // 
            btnAppendSubDir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAppendSubDir.Location = new System.Drawing.Point(1885, 33);
            btnAppendSubDir.Name = "btnAppendSubDir";
            btnAppendSubDir.Size = new System.Drawing.Size(227, 38);
            btnAppendSubDir.TabIndex = 2;
            btnAppendSubDir.Text = "サブディレクトリを追加";
            btnAppendSubDir.UseVisualStyleBackColor = true;
            btnAppendSubDir.Click += btnAppendSubDir_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new System.Drawing.Point(193, 1027);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(143, 44);
            btnNext.TabIndex = 5;
            btnNext.Text = "次を開始";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // MainForm
            // 
            AcceptButton = startBtn;
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(2195, 1090);
            Controls.Add(btnNext);
            Controls.Add(btnAppendSubDir);
            Controls.Add(cmbBoxPath);
            Controls.Add(EndBtn);
            Controls.Add(btnPaste);
            Controls.Add(tabControl);
            Controls.Add(startBtn);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "MainForm";
            Load += MainForm_Load;
            tabControl.ResumeLayout(false);
            tabPageAttr.ResumeLayout(false);
            tabPageAttr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_MinFilesize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUD_MaxFilesize).EndInit();
            gpBox_Date.ResumeLayout(false);
            gpBox_Date.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            grpBox_PicOrient.ResumeLayout(false);
            grpBox_PicOrient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_Height).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUD_Width).EndInit();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAttr;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gpBox_Date;
        private System.Windows.Forms.DateTimePicker dtPicker_to;
        private System.Windows.Forms.DateTimePicker dtPicker_from;
        private System.Windows.Forms.CheckBox chkBox_to;
        private System.Windows.Forms.CheckBox chkBox_from;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button EndBtn;
        private System.Windows.Forms.CheckBox checkBox_SameDate;
        private System.Windows.Forms.ComboBox cmbBoxPath;
        private System.Windows.Forms.Button btnAppendSubDir;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.NumericUpDown numUD_Height;
        private System.Windows.Forms.NumericUpDown numUD_Width;
        private System.Windows.Forms.GroupBox grpBox_PicOrient;
        private System.Windows.Forms.CheckedListBox chkListBox_Ext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBox_FilenameFilter;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtBox_FileList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numUD_MaxFilesize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numUD_MinFilesize;
        private System.Windows.Forms.RadioButton radioBtn_PicOrinet_PR;
        private System.Windows.Forms.RadioButton radioBtn_PicOrinet_All;
        private System.Windows.Forms.RadioButton radioBtn_PicOrinet_LS;
    }
}

