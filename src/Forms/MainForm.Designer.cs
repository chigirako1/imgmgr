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
            gpBox_Date = new System.Windows.Forms.GroupBox();
            checkBox_SameDate = new System.Windows.Forms.CheckBox();
            dtPicker_to = new System.Windows.Forms.DateTimePicker();
            dtPicker_from = new System.Windows.Forms.DateTimePicker();
            chkBox_to = new System.Windows.Forms.CheckBox();
            chkBox_from = new System.Windows.Forms.CheckBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            btnPaste = new System.Windows.Forms.Button();
            EndBtn = new System.Windows.Forms.Button();
            cmbBoxPath = new System.Windows.Forms.ComboBox();
            btnAppendSubDir = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            tabPageAttr.SuspendLayout();
            gpBox_Date.SuspendLayout();
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
            startBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            startBtn.Location = new System.Drawing.Point(1385, 920);
            startBtn.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            startBtn.Name = "startBtn";
            startBtn.Size = new System.Drawing.Size(151, 46);
            startBtn.TabIndex = 2;
            startBtn.Text = "&Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += btnStart_Click;
            // 
            // tabControl
            // 
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Controls.Add(tabPageAttr);
            tabControl.Controls.Add(tabPage2);
            tabControl.Location = new System.Drawing.Point(20, 93);
            tabControl.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1098, 900);
            tabControl.TabIndex = 4;
            // 
            // tabPageAttr
            // 
            tabPageAttr.Controls.Add(gpBox_Date);
            tabPageAttr.Location = new System.Drawing.Point(4, 39);
            tabPageAttr.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPageAttr.Name = "tabPageAttr";
            tabPageAttr.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPageAttr.Size = new System.Drawing.Size(1090, 857);
            tabPageAttr.TabIndex = 0;
            tabPageAttr.Text = "Attr";
            tabPageAttr.UseVisualStyleBackColor = true;
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
            checkBox_SameDate.TabIndex = 4;
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
            dtPicker_to.TabIndex = 3;
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
            chkBox_to.TabIndex = 1;
            chkBox_to.Text = "to";
            chkBox_to.UseVisualStyleBackColor = true;
            // 
            // chkBox_from
            // 
            chkBox_from.AutoSize = true;
            chkBox_from.Checked = true;
            chkBox_from.CheckState = System.Windows.Forms.CheckState.Checked;
            chkBox_from.Location = new System.Drawing.Point(44, 131);
            chkBox_from.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            chkBox_from.Name = "chkBox_from";
            chkBox_from.Size = new System.Drawing.Size(79, 34);
            chkBox_from.TabIndex = 0;
            chkBox_from.Text = "from";
            chkBox_from.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new System.Drawing.Point(4, 39);
            tabPage2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            tabPage2.Size = new System.Drawing.Size(1229, 857);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            btnPaste.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPaste.Location = new System.Drawing.Point(1134, 33);
            btnPaste.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(119, 50);
            btnPaste.TabIndex = 5;
            btnPaste.Text = "Paste(&V)";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // EndBtn
            // 
            EndBtn.Location = new System.Drawing.Point(1385, 989);
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
            cmbBoxPath.FormattingEnabled = true;
            cmbBoxPath.Location = new System.Drawing.Point(88, 33);
            cmbBoxPath.Name = "cmbBoxPath";
            cmbBoxPath.Size = new System.Drawing.Size(1030, 38);
            cmbBoxPath.TabIndex = 7;
            // 
            // btnAppendSubDir
            // 
            btnAppendSubDir.Location = new System.Drawing.Point(1134, 132);
            btnAppendSubDir.Name = "btnAppendSubDir";
            btnAppendSubDir.Size = new System.Drawing.Size(275, 50);
            btnAppendSubDir.TabIndex = 8;
            btnAppendSubDir.Text = "サブディレクトリを追加";
            btnAppendSubDir.UseVisualStyleBackColor = true;
            btnAppendSubDir.Click += btnAppendSubDir_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1559, 1090);
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
            gpBox_Date.ResumeLayout(false);
            gpBox_Date.PerformLayout();
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
    }
}

