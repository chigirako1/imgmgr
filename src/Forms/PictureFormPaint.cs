using System.Drawing;
using System.Windows.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;

namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            FileItem fitem = mModel.GetCurrentFileItem();
            Brush bgbrush;
            if (fitem.Mark)
            {
                bgbrush = BRUSH_MARK;
            }
            else
            {
                bgbrush = BG_BRUSH;
            }
            g.FillRectangle(bgbrush, 0, 0, pictureBox.Width, pictureBox.Height);

            if (mCurrentImg == null)
            {
                Log.err("!!! image not found !!!");
                return;
            }

            int alphaPercent = 100;
            if (mTransitionEffect)
            {
                alphaPercent = mAlphaPercent;
            }


            IMAGE_DISPLAY_MAGNIFICATION_TYPE magType = mMagType;
            //magType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_AS_IS;
            DrawDimension d = ImageModule.DrawCompositedImage(
                g,
                pictureBox.Width,
                pictureBox.Height,
                mCurrentImg,
                mPrevImg,
                alphaPercent,
                magType);

            PictureBox_PaintTxt(g, d);
        }

        private void PictureBox_PaintTxt(Graphics g, DrawDimension d)
        {
            // テキストの描画
            int fsize = 20;
            int x = 0;
            int y = 0;
            Brush txtbrush = Brushes.Aqua;
            Font fnt = new("MS ゴシック", fsize);

            //
            string txt = mModel.GetPictureInfoText();
            g.DrawString(txt, fnt, txtbrush, x, y);

            //
            y += fsize + 3;
            txt = string.Format("{0,4}x{1,4}", pictureBox.Width, pictureBox.Height);
            g.DrawString(txt, fnt, txtbrush, x, y);

            //
            y += fsize + 3;
            txt = string.Format("{0,4}x{1,4}", mCurrentImg.Width, mCurrentImg.Height);
            g.DrawString(txt, fnt, txtbrush, x, y);

            //
            y += fsize + 3;
            txt = string.Format("{0,4}x{1,4}({2}%)", d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1, d.ratio);
            g.DrawString(txt, fnt, txtbrush, x, y);

            fnt.Dispose();
        }


        private Size GetThumbnailSize()
        {
            PictureBox p = rightPicBox;
            return new Size(p.Width / Col, p.Height / Row);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void rightPicBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int col = Col;
            int row = Row;

            var tsize = GetThumbnailSize();
            int thumWidth = tsize.Width;
            int thumHeight = tsize.Height;

            int x = 0;
            int y = 0;
            for (int i = 0; i < (col * row) && i < mModel.PictureTotalNumber; i++)
            {
                FileItem fitem = mModel.GetCurrentFileItemByRelativeIndex(i);
                Size imgsize = fitem.GetImageSize();
                Image thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight);

                Brush bgbrush;
                if (fitem.Mark)
                {
                    bgbrush = BRUSH_MARK;
                }
                else
                {
                    bgbrush = BG_BRUSH;
                }

                g.FillRectangle(bgbrush, x, y, thumWidth, thumHeight);

                if (thumbImg != null)
                {
                    ImageModule.DrawImage(
                                          g,
                                          x,
                                          y,
                                          thumWidth,
                                          thumHeight,
                                          thumbImg);

                    if (imgsize.Width * imgsize.Height < 600 * 960)
                    {
                        int fsize = 20;
                        //int txtx = 0;
                        //int txty = 0;

                        //var opaqueBrush = new SolidBrush(Color.FromArgb(64, Color.Black));
                        //g.FillRectangle(opaqueBrush, x, y, thumWidth, thumHeight);

                        Brush txtbrush = Brushes.Red;
                        Font fnt = new("MS ゴシック", fsize);
                        string txt = string.Format("small({0,4}x{1,4})[{2,4}x{3,4}]", imgsize.Width, imgsize.Height, thumWidth, thumHeight);
                        g.DrawString(txt, fnt, txtbrush, x, y);
                    }
                }

                if (fitem.Mark)
                {
                    var opaqueBrush = new SolidBrush(Color.FromArgb(128, COLOR_MARK));
                    g.FillRectangle(opaqueBrush, x, y, thumWidth, thumHeight);
                }

                x += thumWidth;

                if ((i + 1) % col == 0)
                {
                    x = 0;
                    y += thumHeight;
                }
            }
        }

    }
}
