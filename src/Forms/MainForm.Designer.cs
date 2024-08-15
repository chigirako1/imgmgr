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
            tabPage_Pic = new System.Windows.Forms.TabPage();
            btnPicSizeToggle = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            grpBox_PicOrient = new System.Windows.Forms.GroupBox();
            radioBtn_PicOrinet_LS = new System.Windows.Forms.RadioButton();
            radioBtn_PicOrinet_PR = new System.Windows.Forms.RadioButton();
            radioBtn_PicOrinet_All = new System.Windows.Forms.RadioButton();
            numUD_Height = new System.Windows.Forms.NumericUpDown();
            numUD_Width = new System.Windows.Forms.NumericUpDown();
            tabPage_FileList = new System.Windows.Forms.TabPage();
            txtBox_FileList = new System.Windows.Forms.TextBox();
            tabPage_zipList = new System.Windows.Forms.TabPage();
            tabPage_DGM = new System.Windows.Forms.TabPage();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            btnPaste = new System.Windows.Forms.Button();
            EndBtn = new System.Windows.Forms.Button();
            cmbBoxPath = new System.Windows.Forms.ComboBox();
            btnAppendSubDir = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            btnNextDay = new System.Windows.Forms.Button();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ツールTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_AddPath = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem_SelPicShow = new System.Windows.Forms.ToolStripMenuItem();
            ヘルプHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabControl.SuspendLayout();
            tabPageAttr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_MinFilesize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUD_MaxFilesize).BeginInit();
            gpBox_Date.SuspendLayout();
            tabPage_Pic.SuspendLayout();
            grpBox_PicOrient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_Height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUD_Width).BeginInit();
            tabPage_FileList.SuspendLayout();
            tabPage_DGM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 33);
            label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 23);
            label1.TabIndex = 0;
            label1.Text = "&Path";
            // 
            // startBtn
            // 
            startBtn.Location = new System.Drawing.Point(382, 73);
            startBtn.Margin = new System.Windows.Forms.Padding(6);
            startBtn.Name = "startBtn";
            startBtn.Size = new System.Drawing.Size(166, 73);
            startBtn.TabIndex = 5;
            startBtn.Text = "開始(&S)";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += btnStart_Click;
            // 
            // tabControl
            // 
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Controls.Add(tabPageAttr);
            tabControl.Controls.Add(tabPage_Pic);
            tabControl.Controls.Add(tabPage_FileList);
            tabControl.Controls.Add(tabPage_zipList);
            tabControl.Controls.Add(tabPage_DGM);
            tabControl.Location = new System.Drawing.Point(16, 179);
            tabControl.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(767, 574);
            tabControl.TabIndex = 2;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            tabControl.Selected += tabControl_Selected;
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
            tabPageAttr.Location = new System.Drawing.Point(4, 32);
            tabPageAttr.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            tabPageAttr.Name = "tabPageAttr";
            tabPageAttr.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            tabPageAttr.Size = new System.Drawing.Size(759, 538);
            tabPageAttr.TabIndex = 0;
            tabPageAttr.Text = "file";
            tabPageAttr.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(16, 450);
            label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(159, 23);
            label5.TabIndex = 10;
            label5.Text = "最小ファイルサイズ(kB)";
            // 
            // numUD_MinFilesize
            // 
            numUD_MinFilesize.Location = new System.Drawing.Point(186, 450);
            numUD_MinFilesize.Margin = new System.Windows.Forms.Padding(2);
            numUD_MinFilesize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numUD_MinFilesize.Name = "numUD_MinFilesize";
            numUD_MinFilesize.Size = new System.Drawing.Size(120, 30);
            numUD_MinFilesize.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(16, 483);
            label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(159, 23);
            label4.TabIndex = 8;
            label4.Text = "最大ファイルサイズ(kB)";
            // 
            // numUD_MaxFilesize
            // 
            numUD_MaxFilesize.Location = new System.Drawing.Point(186, 483);
            numUD_MaxFilesize.Margin = new System.Windows.Forms.Padding(2);
            numUD_MaxFilesize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numUD_MaxFilesize.Name = "numUD_MaxFilesize";
            numUD_MaxFilesize.Size = new System.Drawing.Size(120, 30);
            numUD_MaxFilesize.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(445, 26);
            label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(245, 23);
            label3.TabIndex = 6;
            label3.Text = "ファイル名に含まれる文字列の指定";
            // 
            // cmbBox_FilenameFilter
            // 
            cmbBox_FilenameFilter.FormattingEnabled = true;
            cmbBox_FilenameFilter.Location = new System.Drawing.Point(444, 51);
            cmbBox_FilenameFilter.Margin = new System.Windows.Forms.Padding(2);
            cmbBox_FilenameFilter.Name = "cmbBox_FilenameFilter";
            cmbBox_FilenameFilter.Size = new System.Drawing.Size(136, 31);
            cmbBox_FilenameFilter.TabIndex = 5;
            // 
            // chkListBox_Ext
            // 
            chkListBox_Ext.CheckOnClick = true;
            chkListBox_Ext.FormattingEnabled = true;
            chkListBox_Ext.Items.AddRange(new object[] { ".jpg,.jpeg", ".png", ".gif", ".bmp" });
            chkListBox_Ext.Location = new System.Drawing.Point(16, 313);
            chkListBox_Ext.Margin = new System.Windows.Forms.Padding(2);
            chkListBox_Ext.Name = "chkListBox_Ext";
            chkListBox_Ext.Size = new System.Drawing.Size(423, 104);
            chkListBox_Ext.TabIndex = 4;
            // 
            // gpBox_Date
            // 
            gpBox_Date.Controls.Add(checkBox_SameDate);
            gpBox_Date.Controls.Add(dtPicker_to);
            gpBox_Date.Controls.Add(dtPicker_from);
            gpBox_Date.Controls.Add(chkBox_to);
            gpBox_Date.Controls.Add(chkBox_from);
            gpBox_Date.Location = new System.Drawing.Point(16, 26);
            gpBox_Date.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gpBox_Date.Name = "gpBox_Date";
            gpBox_Date.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gpBox_Date.Size = new System.Drawing.Size(422, 261);
            gpBox_Date.TabIndex = 0;
            gpBox_Date.TabStop = false;
            gpBox_Date.Text = "Date";
            // 
            // checkBox_SameDate
            // 
            checkBox_SameDate.AutoSize = true;
            checkBox_SameDate.Checked = true;
            checkBox_SameDate.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBox_SameDate.Location = new System.Drawing.Point(35, 42);
            checkBox_SameDate.Margin = new System.Windows.Forms.Padding(2);
            checkBox_SameDate.Name = "checkBox_SameDate";
            checkBox_SameDate.Size = new System.Drawing.Size(63, 27);
            checkBox_SameDate.TabIndex = 0;
            checkBox_SameDate.Text = "同日";
            checkBox_SameDate.UseVisualStyleBackColor = true;
            checkBox_SameDate.CheckedChanged += checkBox_SameDate_CheckedChanged;
            // 
            // dtPicker_to
            // 
            dtPicker_to.Enabled = false;
            dtPicker_to.Location = new System.Drawing.Point(119, 180);
            dtPicker_to.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            dtPicker_to.Name = "dtPicker_to";
            dtPicker_to.Size = new System.Drawing.Size(273, 30);
            dtPicker_to.TabIndex = 4;
            // 
            // dtPicker_from
            // 
            dtPicker_from.Location = new System.Drawing.Point(119, 105);
            dtPicker_from.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            dtPicker_from.Name = "dtPicker_from";
            dtPicker_from.Size = new System.Drawing.Size(273, 30);
            dtPicker_from.TabIndex = 2;
            // 
            // chkBox_to
            // 
            chkBox_to.AutoSize = true;
            chkBox_to.Location = new System.Drawing.Point(35, 180);
            chkBox_to.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            chkBox_to.Name = "chkBox_to";
            chkBox_to.Size = new System.Drawing.Size(45, 27);
            chkBox_to.TabIndex = 3;
            chkBox_to.Text = "to";
            chkBox_to.UseVisualStyleBackColor = true;
            // 
            // chkBox_from
            // 
            chkBox_from.AutoSize = true;
            chkBox_from.Location = new System.Drawing.Point(35, 105);
            chkBox_from.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            chkBox_from.Name = "chkBox_from";
            chkBox_from.Size = new System.Drawing.Size(65, 27);
            chkBox_from.TabIndex = 1;
            chkBox_from.Text = "from";
            chkBox_from.UseVisualStyleBackColor = true;
            // 
            // tabPage_Pic
            // 
            tabPage_Pic.Controls.Add(btnPicSizeToggle);
            tabPage_Pic.Controls.Add(label2);
            tabPage_Pic.Controls.Add(grpBox_PicOrient);
            tabPage_Pic.Controls.Add(numUD_Height);
            tabPage_Pic.Controls.Add(numUD_Width);
            tabPage_Pic.Location = new System.Drawing.Point(4, 32);
            tabPage_Pic.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            tabPage_Pic.Name = "tabPage_Pic";
            tabPage_Pic.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            tabPage_Pic.Size = new System.Drawing.Size(759, 538);
            tabPage_Pic.TabIndex = 1;
            tabPage_Pic.Text = "pic";
            tabPage_Pic.UseVisualStyleBackColor = true;
            // 
            // btnPicSizeToggle
            // 
            btnPicSizeToggle.Location = new System.Drawing.Point(389, 84);
            btnPicSizeToggle.Margin = new System.Windows.Forms.Padding(2);
            btnPicSizeToggle.Name = "btnPicSizeToggle";
            btnPicSizeToggle.Size = new System.Drawing.Size(90, 28);
            btnPicSizeToggle.TabIndex = 5;
            btnPicSizeToggle.Text = "変更";
            btnPicSizeToggle.UseVisualStyleBackColor = true;
            btnPicSizeToggle.Click += btnPicSizeToggle_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(121, 44);
            label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(202, 23);
            label2.TabIndex = 4;
            label2.Text = "Maximum Width : Height";
            // 
            // grpBox_PicOrient
            // 
            grpBox_PicOrient.Controls.Add(radioBtn_PicOrinet_LS);
            grpBox_PicOrient.Controls.Add(radioBtn_PicOrinet_PR);
            grpBox_PicOrient.Controls.Add(radioBtn_PicOrinet_All);
            grpBox_PicOrient.Location = new System.Drawing.Point(120, 177);
            grpBox_PicOrient.Margin = new System.Windows.Forms.Padding(2);
            grpBox_PicOrient.Name = "grpBox_PicOrient";
            grpBox_PicOrient.Padding = new System.Windows.Forms.Padding(2);
            grpBox_PicOrient.Size = new System.Drawing.Size(231, 195);
            grpBox_PicOrient.TabIndex = 2;
            grpBox_PicOrient.TabStop = false;
            grpBox_PicOrient.Text = "画像の向き";
            // 
            // radioBtn_PicOrinet_LS
            // 
            radioBtn_PicOrinet_LS.AutoSize = true;
            radioBtn_PicOrinet_LS.Location = new System.Drawing.Point(38, 106);
            radioBtn_PicOrinet_LS.Margin = new System.Windows.Forms.Padding(2);
            radioBtn_PicOrinet_LS.Name = "radioBtn_PicOrinet_LS";
            radioBtn_PicOrinet_LS.Size = new System.Drawing.Size(75, 27);
            radioBtn_PicOrinet_LS.TabIndex = 1;
            radioBtn_PicOrinet_LS.Text = "横向き";
            radioBtn_PicOrinet_LS.UseVisualStyleBackColor = true;
            // 
            // radioBtn_PicOrinet_PR
            // 
            radioBtn_PicOrinet_PR.AutoSize = true;
            radioBtn_PicOrinet_PR.Location = new System.Drawing.Point(38, 74);
            radioBtn_PicOrinet_PR.Margin = new System.Windows.Forms.Padding(2);
            radioBtn_PicOrinet_PR.Name = "radioBtn_PicOrinet_PR";
            radioBtn_PicOrinet_PR.Size = new System.Drawing.Size(75, 27);
            radioBtn_PicOrinet_PR.TabIndex = 1;
            radioBtn_PicOrinet_PR.Text = "縦向き";
            radioBtn_PicOrinet_PR.UseVisualStyleBackColor = true;
            // 
            // radioBtn_PicOrinet_All
            // 
            radioBtn_PicOrinet_All.AutoSize = true;
            radioBtn_PicOrinet_All.Checked = true;
            radioBtn_PicOrinet_All.Location = new System.Drawing.Point(42, 42);
            radioBtn_PicOrinet_All.Margin = new System.Windows.Forms.Padding(2);
            radioBtn_PicOrinet_All.Name = "radioBtn_PicOrinet_All";
            radioBtn_PicOrinet_All.Size = new System.Drawing.Size(58, 27);
            radioBtn_PicOrinet_All.TabIndex = 0;
            radioBtn_PicOrinet_All.TabStop = true;
            radioBtn_PicOrinet_All.Text = "全て";
            radioBtn_PicOrinet_All.UseVisualStyleBackColor = true;
            // 
            // numUD_Height
            // 
            numUD_Height.Location = new System.Drawing.Point(246, 84);
            numUD_Height.Margin = new System.Windows.Forms.Padding(2);
            numUD_Height.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numUD_Height.Name = "numUD_Height";
            numUD_Height.Size = new System.Drawing.Size(120, 30);
            numUD_Height.TabIndex = 1;
            // 
            // numUD_Width
            // 
            numUD_Width.Location = new System.Drawing.Point(122, 84);
            numUD_Width.Margin = new System.Windows.Forms.Padding(2);
            numUD_Width.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numUD_Width.Name = "numUD_Width";
            numUD_Width.Size = new System.Drawing.Size(120, 30);
            numUD_Width.TabIndex = 0;
            // 
            // tabPage_FileList
            // 
            tabPage_FileList.Controls.Add(txtBox_FileList);
            tabPage_FileList.Location = new System.Drawing.Point(4, 32);
            tabPage_FileList.Margin = new System.Windows.Forms.Padding(2);
            tabPage_FileList.Name = "tabPage_FileList";
            tabPage_FileList.Padding = new System.Windows.Forms.Padding(2);
            tabPage_FileList.Size = new System.Drawing.Size(759, 538);
            tabPage_FileList.TabIndex = 2;
            tabPage_FileList.Text = "filelist";
            tabPage_FileList.UseVisualStyleBackColor = true;
            // 
            // txtBox_FileList
            // 
            txtBox_FileList.Dock = System.Windows.Forms.DockStyle.Fill;
            txtBox_FileList.Location = new System.Drawing.Point(2, 2);
            txtBox_FileList.Margin = new System.Windows.Forms.Padding(2);
            txtBox_FileList.Multiline = true;
            txtBox_FileList.Name = "txtBox_FileList";
            txtBox_FileList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtBox_FileList.Size = new System.Drawing.Size(755, 534);
            txtBox_FileList.TabIndex = 0;
            // 
            // tabPage_zipList
            // 
            tabPage_zipList.Location = new System.Drawing.Point(4, 32);
            tabPage_zipList.Margin = new System.Windows.Forms.Padding(2);
            tabPage_zipList.Name = "tabPage_zipList";
            tabPage_zipList.Padding = new System.Windows.Forms.Padding(2);
            tabPage_zipList.Size = new System.Drawing.Size(759, 538);
            tabPage_zipList.TabIndex = 3;
            tabPage_zipList.Text = "zip list";
            tabPage_zipList.UseVisualStyleBackColor = true;
            // 
            // tabPage_DGM
            // 
            tabPage_DGM.Controls.Add(dataGridView1);
            tabPage_DGM.Location = new System.Drawing.Point(4, 32);
            tabPage_DGM.Margin = new System.Windows.Forms.Padding(2);
            tabPage_DGM.Name = "tabPage_DGM";
            tabPage_DGM.Padding = new System.Windows.Forms.Padding(2);
            tabPage_DGM.Size = new System.Drawing.Size(759, 538);
            tabPage_DGM.TabIndex = 4;
            tabPage_DGM.Text = "DGM";
            tabPage_DGM.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(2, 2);
            dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new System.Drawing.Size(755, 534);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            dataGridView1.RowEnter += dataGridView1_RowEnter;
            // 
            // btnPaste
            // 
            btnPaste.Location = new System.Drawing.Point(558, 37);
            btnPaste.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(112, 42);
            btnPaste.TabIndex = 3;
            btnPaste.Text = "貼り付け(&V)";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // EndBtn
            // 
            EndBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            EndBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            EndBtn.Location = new System.Drawing.Point(18, 762);
            EndBtn.Name = "EndBtn";
            EndBtn.Size = new System.Drawing.Size(121, 37);
            EndBtn.TabIndex = 8;
            EndBtn.Text = "終了";
            EndBtn.UseVisualStyleBackColor = false;
            EndBtn.Click += EndBtn_Click;
            // 
            // cmbBoxPath
            // 
            cmbBoxPath.AllowDrop = true;
            cmbBoxPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbBoxPath.FormattingEnabled = true;
            cmbBoxPath.Location = new System.Drawing.Point(70, 33);
            cmbBoxPath.Margin = new System.Windows.Forms.Padding(2);
            cmbBoxPath.Name = "cmbBoxPath";
            cmbBoxPath.Size = new System.Drawing.Size(482, 31);
            cmbBoxPath.TabIndex = 1;
            cmbBoxPath.DragDrop += cmbBoxPath_DragDrop;
            cmbBoxPath.DragEnter += cmbBoxPath_DragEnter;
            // 
            // btnAppendSubDir
            // 
            btnAppendSubDir.Location = new System.Drawing.Point(20, 73);
            btnAppendSubDir.Margin = new System.Windows.Forms.Padding(2);
            btnAppendSubDir.Name = "btnAppendSubDir";
            btnAppendSubDir.Size = new System.Drawing.Size(120, 72);
            btnAppendSubDir.TabIndex = 4;
            btnAppendSubDir.Text = "サブディレクトリを追加";
            btnAppendSubDir.UseVisualStyleBackColor = true;
            btnAppendSubDir.Click += btnAppendSubDir_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new System.Drawing.Point(556, 87);
            btnNext.Margin = new System.Windows.Forms.Padding(2);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(140, 35);
            btnNext.TabIndex = 6;
            btnNext.Text = "次のパスで開始";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnNextDay
            // 
            btnNextDay.Location = new System.Drawing.Point(700, 92);
            btnNextDay.Margin = new System.Windows.Forms.Padding(2);
            btnNextDay.Name = "btnNextDay";
            btnNextDay.Size = new System.Drawing.Size(140, 35);
            btnNextDay.TabIndex = 7;
            btnNextDay.Text = "次の日付で開始";
            btnNextDay.UseVisualStyleBackColor = true;
            btnNextDay.Click += btnNextDay_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ファイルFToolStripMenuItem, ツールTToolStripMenuItem, ヘルプHToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(914, 31);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            ファイルFToolStripMenuItem.Size = new System.Drawing.Size(88, 27);
            ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // ツールTToolStripMenuItem
            // 
            ツールTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItem_AddPath, ToolStripMenuItem_SelPicShow });
            ツールTToolStripMenuItem.Name = "ツールTToolStripMenuItem";
            ツールTToolStripMenuItem.Size = new System.Drawing.Size(79, 27);
            ツールTToolStripMenuItem.Text = "ツール(&T)";
            // 
            // ToolStripMenuItem_AddPath
            // 
            ToolStripMenuItem_AddPath.Name = "ToolStripMenuItem_AddPath";
            ToolStripMenuItem_AddPath.Size = new System.Drawing.Size(234, 28);
            ToolStripMenuItem_AddPath.Text = "パス追加";
            ToolStripMenuItem_AddPath.Click += ToolStripMenuItem_AddPath_Click;
            // 
            // ToolStripMenuItem_SelPicShow
            // 
            ToolStripMenuItem_SelPicShow.Name = "ToolStripMenuItem_SelPicShow";
            ToolStripMenuItem_SelPicShow.ShortcutKeys = System.Windows.Forms.Keys.F1;
            ToolStripMenuItem_SelPicShow.Size = new System.Drawing.Size(234, 28);
            ToolStripMenuItem_SelPicShow.Text = "選択した画像表示";
            ToolStripMenuItem_SelPicShow.Click += ToolStripMenuItem_SelPicShow_Click;
            // 
            // ヘルプHToolStripMenuItem
            // 
            ヘルプHToolStripMenuItem.Name = "ヘルプHToolStripMenuItem";
            ヘルプHToolStripMenuItem.Size = new System.Drawing.Size(85, 27);
            ヘルプHToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // MainForm
            // 
            AcceptButton = startBtn;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(914, 858);
            Controls.Add(startBtn);
            Controls.Add(btnNextDay);
            Controls.Add(btnNext);
            Controls.Add(btnAppendSubDir);
            Controls.Add(cmbBoxPath);
            Controls.Add(EndBtn);
            Controls.Add(btnPaste);
            Controls.Add(tabControl);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            Location = new System.Drawing.Point(20, 20);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(6);
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
            tabPage_Pic.ResumeLayout(false);
            tabPage_Pic.PerformLayout();
            grpBox_PicOrient.ResumeLayout(false);
            grpBox_PicOrient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numUD_Height).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUD_Width).EndInit();
            tabPage_FileList.ResumeLayout(false);
            tabPage_FileList.PerformLayout();
            tabPage_DGM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAttr;
        private System.Windows.Forms.TabPage tabPage_Pic;
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
        private System.Windows.Forms.TabPage tabPage_FileList;
        private System.Windows.Forms.TextBox txtBox_FileList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numUD_MaxFilesize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numUD_MinFilesize;
        private System.Windows.Forms.RadioButton radioBtn_PicOrinet_PR;
        private System.Windows.Forms.RadioButton radioBtn_PicOrinet_All;
        private System.Windows.Forms.RadioButton radioBtn_PicOrinet_LS;
        private System.Windows.Forms.Button btnPicSizeToggle;
        private System.Windows.Forms.Button btnNextDay;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ツールTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_AddPath;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SelPicShow;
        private System.Windows.Forms.TabPage tabPage_zipList;
        private System.Windows.Forms.TabPage tabPage_DGM;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

