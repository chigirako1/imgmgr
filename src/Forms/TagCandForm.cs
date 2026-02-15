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
        public TagCandForm()
        {
            this.Text = "window";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            var btn = new Button();
            btn.Text = "選択";
            btn.Location = new Point(150, 100);
            btn.Click += (sender, e) => this.Close();

            var btn2 = new Button();
            btn2.Text = "キャンセル";
            btn2.Location = new Point(300, 100);
            btn2.Click += (sender, e) => this.Close();

            this.Controls.Add(btn);
            this.Controls.Add(btn2);
        }

        public string GetSelectedTag()
        {
            return "";
        }
    }
}
