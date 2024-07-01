namespace PictureManagerApp.src.Forms
{
    partial class ColRowForm
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
            ColRowOkButton = new System.Windows.Forms.Button();
            ColRowCancelButton = new System.Windows.Forms.Button();
            ColNumUpDown = new System.Windows.Forms.NumericUpDown();
            RowNumUpDown = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioBtn_Mag_AsIs = new System.Windows.Forms.RadioButton();
            radioBtn_Mag_FitScreen_Mag = new System.Windows.Forms.RadioButton();
            radioBtn_Mag_FitScreen = new System.Windows.Forms.RadioButton();
            numericUpDownThumbMS = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)ColNumUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RowNumUpDown).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThumbMS).BeginInit();
            SuspendLayout();
            // 
            // ColRowOkButton
            // 
            ColRowOkButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ColRowOkButton.Location = new System.Drawing.Point(680, 515);
            ColRowOkButton.Name = "ColRowOkButton";
            ColRowOkButton.Size = new System.Drawing.Size(94, 53);
            ColRowOkButton.TabIndex = 0;
            ColRowOkButton.Text = "&OK";
            ColRowOkButton.UseVisualStyleBackColor = true;
            ColRowOkButton.Click += ColRowOkButton_Click;
            // 
            // ColRowCancelButton
            // 
            ColRowCancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ColRowCancelButton.Location = new System.Drawing.Point(780, 515);
            ColRowCancelButton.Name = "ColRowCancelButton";
            ColRowCancelButton.Size = new System.Drawing.Size(94, 53);
            ColRowCancelButton.TabIndex = 0;
            ColRowCancelButton.Text = "&Cancel";
            ColRowCancelButton.UseVisualStyleBackColor = true;
            ColRowCancelButton.Click += ColRowCancelButton_Click;
            // 
            // ColNumUpDown
            // 
            ColNumUpDown.Location = new System.Drawing.Point(150, 90);
            ColNumUpDown.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            ColNumUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            ColNumUpDown.Name = "ColNumUpDown";
            ColNumUpDown.Size = new System.Drawing.Size(80, 35);
            ColNumUpDown.TabIndex = 1;
            ColNumUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            ColNumUpDown.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // RowNumUpDown
            // 
            RowNumUpDown.Location = new System.Drawing.Point(259, 90);
            RowNumUpDown.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            RowNumUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            RowNumUpDown.Name = "RowNumUpDown";
            RowNumUpDown.Size = new System.Drawing.Size(80, 35);
            RowNumUpDown.TabIndex = 1;
            RowNumUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            RowNumUpDown.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(150, 41);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(158, 30);
            label1.TabIndex = 2;
            label1.Text = "サムネイル表示数";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioBtn_Mag_AsIs);
            groupBox1.Controls.Add(radioBtn_Mag_FitScreen_Mag);
            groupBox1.Controls.Add(radioBtn_Mag_FitScreen);
            groupBox1.Location = new System.Drawing.Point(150, 192);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(429, 191);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "画像の表示方式";
            // 
            // radioBtn_Mag_AsIs
            // 
            radioBtn_Mag_AsIs.AutoSize = true;
            radioBtn_Mag_AsIs.Location = new System.Drawing.Point(6, 129);
            radioBtn_Mag_AsIs.Name = "radioBtn_Mag_AsIs";
            radioBtn_Mag_AsIs.Size = new System.Drawing.Size(118, 34);
            radioBtn_Mag_AsIs.TabIndex = 1;
            radioBtn_Mag_AsIs.Text = "等倍表示";
            radioBtn_Mag_AsIs.UseVisualStyleBackColor = true;
            // 
            // radioBtn_Mag_FitScreen_Mag
            // 
            radioBtn_Mag_FitScreen_Mag.AutoSize = true;
            radioBtn_Mag_FitScreen_Mag.Location = new System.Drawing.Point(6, 89);
            radioBtn_Mag_FitScreen_Mag.Name = "radioBtn_Mag_FitScreen_Mag";
            radioBtn_Mag_FitScreen_Mag.Size = new System.Drawing.Size(292, 34);
            radioBtn_Mag_FitScreen_Mag.TabIndex = 0;
            radioBtn_Mag_FitScreen_Mag.Text = "表示範囲に合わせる(拡大あり)";
            radioBtn_Mag_FitScreen_Mag.UseVisualStyleBackColor = true;
            radioBtn_Mag_FitScreen_Mag.CheckedChanged += radioBtn_Mag_FitScreen_CheckedChanged;
            // 
            // radioBtn_Mag_FitScreen
            // 
            radioBtn_Mag_FitScreen.AutoSize = true;
            radioBtn_Mag_FitScreen.Checked = true;
            radioBtn_Mag_FitScreen.Location = new System.Drawing.Point(6, 49);
            radioBtn_Mag_FitScreen.Name = "radioBtn_Mag_FitScreen";
            radioBtn_Mag_FitScreen.Size = new System.Drawing.Size(207, 34);
            radioBtn_Mag_FitScreen.TabIndex = 0;
            radioBtn_Mag_FitScreen.TabStop = true;
            radioBtn_Mag_FitScreen.Text = "表示範囲に合わせる";
            radioBtn_Mag_FitScreen.UseVisualStyleBackColor = true;
            radioBtn_Mag_FitScreen.CheckedChanged += radioBtn_Mag_FitScreen_CheckedChanged;
            // 
            // numericUpDownThumbMS
            // 
            numericUpDownThumbMS.Location = new System.Drawing.Point(150, 419);
            numericUpDownThumbMS.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numericUpDownThumbMS.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownThumbMS.Name = "numericUpDownThumbMS";
            numericUpDownThumbMS.Size = new System.Drawing.Size(80, 35);
            numericUpDownThumbMS.TabIndex = 1;
            numericUpDownThumbMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            numericUpDownThumbMS.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(150, 375);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(218, 30);
            label2.TabIndex = 2;
            label2.Text = "サムネイル更新間隔(ms)";
            // 
            // ColRowForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(886, 580);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUpDownThumbMS);
            Controls.Add(RowNumUpDown);
            Controls.Add(ColNumUpDown);
            Controls.Add(ColRowCancelButton);
            Controls.Add(ColRowOkButton);
            Name = "ColRowForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "ColRowForm";
            ((System.ComponentModel.ISupportInitialize)ColNumUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)RowNumUpDown).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThumbMS).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button ColRowOkButton;
        private System.Windows.Forms.Button ColRowCancelButton;
        private System.Windows.Forms.NumericUpDown ColNumUpDown;
        private System.Windows.Forms.NumericUpDown RowNumUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBtn_Mag_AsIs;
        private System.Windows.Forms.RadioButton radioBtn_Mag_FitScreen;
        private System.Windows.Forms.RadioButton radioBtn_Mag_FitScreen_Mag;
        private System.Windows.Forms.NumericUpDown numericUpDownThumbMS;
        private System.Windows.Forms.Label label2;
    }
}