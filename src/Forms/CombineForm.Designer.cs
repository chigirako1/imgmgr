namespace PictureManagerApp.src.Forms
{
    partial class CombineForm
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
            LbFiles = new System.Windows.Forms.ListBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            btnUpdate = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            colorDialog1 = new System.Windows.Forms.ColorDialog();
            listBox2 = new System.Windows.Forms.ListBox();
            btnSave = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            btnRemove = new System.Windows.Forms.Button();
            btnSort = new System.Windows.Forms.Button();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            stsImageSize = new System.Windows.Forms.ToolStripStatusLabel();
            btnUp = new System.Windows.Forms.Button();
            btnDown = new System.Windows.Forms.Button();
            btnColorPicker = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // LbFiles
            // 
            LbFiles.AllowDrop = true;
            LbFiles.FormattingEnabled = true;
            LbFiles.Location = new System.Drawing.Point(41, 84);
            LbFiles.Name = "LbFiles";
            LbFiles.Size = new System.Drawing.Size(299, 349);
            LbFiles.TabIndex = 0;
            LbFiles.SelectedIndexChanged += LbFiles_SelectedIndexChanged;
            LbFiles.DragDrop += listBox1_DragDrop;
            LbFiles.DragEnter += LbFiles_DragEnter;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox1.Location = new System.Drawing.Point(498, 84);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(491, 659);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(148, 463);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(120, 30);
            numericUpDown1.TabIndex = 2;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new System.Drawing.Point(370, 390);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(101, 43);
            btnUpdate.TabIndex = 3;
            btnUpdate.Text = "更新(&U)";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(41, 463);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(79, 23);
            label1.TabIndex = 4;
            label1.Text = "隙間pixel";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.Items.AddRange(new object[] { "縦", "横", "縦x横" });
            listBox2.Location = new System.Drawing.Point(41, 532);
            listBox2.Name = "listBox2";
            listBox2.Size = new System.Drawing.Size(120, 96);
            listBox2.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSave.Location = new System.Drawing.Point(370, 700);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(101, 43);
            btnSave.TabIndex = 3;
            btnSave.Text = "保存(&S)";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(41, 48);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(58, 23);
            label2.TabIndex = 4;
            label2.Text = "ファイル";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(41, 506);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(78, 23);
            label3.TabIndex = 4;
            label3.Text = "連結方向";
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(370, 292);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(101, 43);
            btnRemove.TabIndex = 3;
            btnRemove.Text = "削除(&R)";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnSort
            // 
            btnSort.Location = new System.Drawing.Point(370, 193);
            btnSort.Name = "btnSort";
            btnSort.Size = new System.Drawing.Size(101, 43);
            btnSort.TabIndex = 3;
            btnSort.Text = "ソート(&S)";
            btnSort.UseVisualStyleBackColor = true;
            btnSort.Click += btnSort_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { stsImageSize });
            statusStrip1.Location = new System.Drawing.Point(0, 764);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1032, 22);
            statusStrip1.TabIndex = 6;
            statusStrip1.Text = "statusStrip1";
            // 
            // stsImageSize
            // 
            stsImageSize.Name = "stsImageSize";
            stsImageSize.Size = new System.Drawing.Size(0, 17);
            // 
            // btnUp
            // 
            btnUp.Location = new System.Drawing.Point(370, 84);
            btnUp.Name = "btnUp";
            btnUp.Size = new System.Drawing.Size(101, 43);
            btnUp.TabIndex = 3;
            btnUp.Text = "↑(&U)";
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += btnUp_Click;
            // 
            // btnDown
            // 
            btnDown.Location = new System.Drawing.Point(370, 133);
            btnDown.Name = "btnDown";
            btnDown.Size = new System.Drawing.Size(101, 43);
            btnDown.TabIndex = 3;
            btnDown.Text = "↓(&D)";
            btnDown.UseVisualStyleBackColor = true;
            btnDown.Click += btnDown_Click;
            // 
            // btnColorPicker
            // 
            btnColorPicker.Location = new System.Drawing.Point(288, 463);
            btnColorPicker.Name = "btnColorPicker";
            btnColorPicker.Size = new System.Drawing.Size(101, 43);
            btnColorPicker.TabIndex = 3;
            btnColorPicker.Text = "色の選択(&C)";
            btnColorPicker.UseVisualStyleBackColor = true;
            // 
            // CombineForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1032, 786);
            Controls.Add(statusStrip1);
            Controls.Add(listBox2);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Controls.Add(btnSort);
            Controls.Add(btnDown);
            Controls.Add(btnUp);
            Controls.Add(btnRemove);
            Controls.Add(btnColorPicker);
            Controls.Add(btnUpdate);
            Controls.Add(numericUpDown1);
            Controls.Add(pictureBox1);
            Controls.Add(LbFiles);
            Name = "CombineForm";
            Text = "画像の結合";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox LbFiles;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stsImageSize;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnColorPicker;
    }
}