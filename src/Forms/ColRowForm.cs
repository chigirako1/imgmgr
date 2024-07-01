using PictureManagerApp.src.Lib;
using System;
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

        public IMAGE_DISPLAY_MAGNIFICATION_TYPE GetMagType()
        {
            if (radioBtn_Mag_FitScreen.Checked)
            {
                return IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND;
            }
            else if (radioBtn_Mag_FitScreen_Mag.Checked)
            {
                return IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN;
            }
            else if (radioBtn_Mag_AsIs.Checked)
            {
                return IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_AS_IS;
            }

            return IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND;
        }

        public int GetThumbMs()
        {
            var thumbMs = decimal.ToInt32(numericUpDownThumbMS.Value);
            return thumbMs;
        }

        public ColRowForm(int col, int row)
        {
            InitializeComponent();

            this.AcceptButton = ColRowOkButton;
            this.CancelButton = ColRowCancelButton;

            ColNumUpDown.Value = col;
            RowNumUpDown.Value = row;

            radioBtn_Mag_FitScreen.Checked = true;
        }

        private void ColRowOkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ColRowCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void radioBtn_Mag_FitScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_Mag_FitScreen.Checked)
            {
            }
            else if (radioBtn_Mag_AsIs.Checked)
            {
            }
        }
    }
}
