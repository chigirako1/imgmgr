using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PictureManagerApp.src.Forms
{
    public partial class ColRowForm : Form
    {
        public (int col, int row) GetColRow()
        {
            var col = decimal.ToInt32(ColNumUpDown.Value);
            var row = decimal.ToInt32(RowNumUpDown.Value);
            return (col, row);
        }

        public ColRowForm(int col, int row)
        {
            InitializeComponent();

            this.AcceptButton = ColRowOkButton;
            this.CancelButton = ColRowCancelButton;

            ColNumUpDown.Value = col;
            RowNumUpDown.Value = row;
        }

        private void ColRowOkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ColRowCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
