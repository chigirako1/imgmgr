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
            ((System.ComponentModel.ISupportInitialize)ColNumUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RowNumUpDown).BeginInit();
            SuspendLayout();
            // 
            // ColRowOkButton
            // 
            ColRowOkButton.Location = new System.Drawing.Point(420, 309);
            ColRowOkButton.Name = "ColRowOkButton";
            ColRowOkButton.Size = new System.Drawing.Size(94, 53);
            ColRowOkButton.TabIndex = 0;
            ColRowOkButton.Text = "&OK";
            ColRowOkButton.UseVisualStyleBackColor = true;
            ColRowOkButton.Click += ColRowOkButton_Click;
            // 
            // ColRowCancelButton
            // 
            ColRowCancelButton.Location = new System.Drawing.Point(520, 309);
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
            // ColRowForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(626, 374);
            Controls.Add(RowNumUpDown);
            Controls.Add(ColNumUpDown);
            Controls.Add(ColRowCancelButton);
            Controls.Add(ColRowOkButton);
            Name = "ColRowForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "ColRowForm";
            ((System.ComponentModel.ISupportInitialize)ColNumUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)RowNumUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button ColRowOkButton;
        private System.Windows.Forms.Button ColRowCancelButton;
        private System.Windows.Forms.NumericUpDown ColNumUpDown;
        private System.Windows.Forms.NumericUpDown RowNumUpDown;
    }
}