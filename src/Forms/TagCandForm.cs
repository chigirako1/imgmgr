using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureManagerApp.src.Forms
{
    public class TagCandForm : Form
    {
        private ComboBox _cmbbox;

        public TagCandForm(List<string> list)
        {
            var w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            var win_w = w / 3;
            var win_h = h / 2;

            this.Text = "window";
            this.Size = new Size(win_w, win_h);
            this.StartPosition = FormStartPosition.CenterScreen;

            _cmbbox = new ComboBox();
            _cmbbox.Location = new Point(10, 10);

            //_cmbbox.Items.Add("test");
            //_cmbbox.Items.Add("test2");
            //var list = new List<string> { "test", "test2" };
            _cmbbox.DataSource = list;
            //_cmbbox.SelectedItem = "test";

            //_cmbbox.Text = "tt";
            _cmbbox.DropDownStyle = ComboBoxStyle.Simple;
            _cmbbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _cmbbox.AutoCompleteSource = AutoCompleteSource.ListItems;

            _cmbbox.Height = win_h - (win_h / 10);

            var btn = new Button();
            btn.Text = "選択";
            btn.Location = new Point(win_w - 200, win_h - 100);
            btn.Click += OkButton_Click;
            this.AcceptButton = btn;

            var btn2 = new Button();
            btn2.Text = "キャンセル";
            btn2.Location = new Point(win_w - 100, win_h - 100);
            btn2.Click += CancelButton_Click;//(sender, e) => this.Close();
            this.CancelButton = btn2;

            this.Controls.Add(_cmbbox);

            this.Controls.Add(btn);
            this.Controls.Add(btn2);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string GetSelectedTag()
        {
            return _cmbbox.SelectedItem.ToString();
        }
    }
}
