using Microsoft.VisualBasic.ApplicationServices;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureManagerApp.src.Forms
{
    public partial class CombineForm : Form
    {
        private BindingList<ImageFileItem> mFilelist = new();
        private Image mCombinedImage;

        public CombineForm()
        {
            InitializeComponent();
        }

        private void updateImage()
        {
            List<Image> images = new List<Image>();

            foreach (ImageFileItem item in LbFiles.Items)
            {
                var img = ImageModule.GetImage(item.FullPath);
                images.Add(img);
            }

            //TODO:
            //・継ぎ目のピクセル指定対応
            //・センタリング対応
            //
            mCombinedImage = ImageModule.VerticalCombine(images, false);
            if (mCombinedImage != null)
            {
                pictureBox1.Image = mCombinedImage;

                stsImageSize.Text = $"{mCombinedImage.Width}x{mCombinedImage.Height}";
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Log.trc("[S]");

            // ドロップされたファイルパスを配列として取得
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files != null)
            {

                foreach (string path in files)
                {
                    // ListBoxに追加
                    // クラスのインスタンスを作成
                    var item = new ImageFileItem
                    {
                        FileName = System.IO.Path.GetFileName(path), // ファイル名だけ抽出
                        FullPath = path                              // フルパスを保存
                    };

                    mFilelist.Add(item);
                    // オブジェクトごと追加
                    //LbFiles.Items.Add(item);
                }
                LbFiles.DataSource = mFilelist.OrderBy(u => u.FullPath).ToList();
                LbFiles.DisplayMember = "FileName"; // 表示するプロパティを指定        }
            }
            //Refresh();
            updateImage();

            Log.trc("[E]");
        }

        private void LbFiles_DragEnter(object sender, DragEventArgs e)
        {
            // 引っ張ってきたデータが「ファイル」形式か確認
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // マウスカーソルを「コピー」の状態にする
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                // ファイル以外なら受け付けない
                e.Effect = DragDropEffects.None;
            }
        }

        /*
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (mCombinedImage != null)
            {
                var g = e.Graphics;
                g.DrawImage(mCombinedImage, 0, 0);
            }
        }*/

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateImage();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (mCombinedImage != null)
            {
                //var filepath = @"D:\dl\test.jpg";
                var fpath = mFilelist[0].FullPath;
                var filepath = MyFiles.GetUniqueFilePath(fpath);
                mCombinedImage.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show($"{filepath}を保存しました。",
                    "保存",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //LbFiles.Items.RemoveAt(LbFiles.SelectedIndex);
            if (LbFiles.SelectedItem != null)
            {
                mFilelist.RemoveAt(LbFiles.SelectedIndex);
            }
            updateImage();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            LbFiles.DataSource = mFilelist.OrderBy(u => u.FullPath).ToList();
            updateImage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (LbFiles.SelectedItem != null)
            {
                int index = LbFiles.SelectedIndex;

                // 一番上でなければ移動可能
                if (index > 0)
                {
                    var item = mFilelist[index];
                    mFilelist.RemoveAt(index);
                    mFilelist.Insert(index - 1, item);

                    // 選択状態を追いかける
                    LbFiles.SelectedIndex = index - 1;
                }
                updateImage();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int index = LbFiles.SelectedIndex;

            // 未選択でなく、かつ一番下でなければ移動可能
            if (index != -1 && index < mFilelist.Count - 1)
            {
                var item = mFilelist[index];
                mFilelist.RemoveAt(index);
                mFilelist.Insert(index + 1, item);

                // 選択状態を追いかける
                LbFiles.SelectedIndex = index + 1;

                updateImage();
            }
        }

        private void LbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = LbFiles.SelectedIndex;
            // 一番上なら「上へ」ボタンを無効化
            btnUp.Enabled = (index > 0);
            // 一番下なら「下へ」ボタンを無効化
            btnDown.Enabled = (index != -1 && index < mFilelist.Count - 1);
        }
    }

    public class ImageFileItem : IComparable<string>
    {
        public string FileName { get; set; } // 表示用（ファイル名）
        public string FullPath { get; set; } // 保持用（フルパス）

        public int CompareTo(string other)
        {
            return FullPath.CompareTo(other);
        }

        // ListBoxは表示の際にToString()を呼び出すので、ここをファイル名にする
        public override string ToString()
        {
            return FileName;
        }
    }
}
