using System.Drawing;
using System.Windows.Forms;
using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;

namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        private const string FONT_NAME = "MS ゴシック";
        private const int FONT_SIZE = 20;
        private const int FONT_SPACE = 3;
        private const int OPA_VAL = 128;
        private static readonly Brush FONT_COLOR = Brushes.Aqua;

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
            else if (mSlideshow)
            {
                bgbrush = BRUSH_SLIDESHOW;
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
            DrawDimension d = ImageModule.DrawCompositedImage(
                g,
                pictureBox.Width,
                pictureBox.Height,
                mCurrentImg,
                mPrevImg,
                alphaPercent,
                magType);

            {
                PictureBox_PaintTxt(g, d, fitem);
            }
        }

        private void PictureBox_PaintTxt(Graphics g, DrawDimension d, FileItem fitem)
        {
            // テキストの描画
            int fsize = FONT_SIZE;
            var inc = FONT_SPACE;
            int x = 0;
            int y = 0;
            Brush txtbrush = FONT_COLOR;
            Font fnt = new(FONT_NAME, fsize);

            //
            var txt = mModel.GetPictureInfoText();
            g.DrawString(txt, fnt, txtbrush, x, y);

            if (DisplayTxt)
            {
                //
                y += fsize + inc;
                txt = mModel.GetArtistInfoFromDB();
                g.DrawString(txt, fnt, txtbrush, x, y);

                //
                y += fsize + inc;
                txt = string.Format("{0,4}x{1,4}", pictureBox.Width, pictureBox.Height);
                g.DrawString(txt, fnt, txtbrush, x, y);

                //
                y += fsize + inc;
                txt = string.Format("{0,4}x{1,4}", mCurrentImg.Width, mCurrentImg.Height);
                g.DrawString(txt, fnt, txtbrush, x, y);

                //
                y += fsize + inc;
                txt = string.Format("{0,4}x{1,4}({2}%)", d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1, d.ratio);
                g.DrawString(txt, fnt, txtbrush, x, y);
            }
            else
            { 
                //
                y += fsize + inc;
                txt = mModel.GetZipEntryname();
                g.DrawString(txt, fnt, txtbrush, x, y);
            }

            if (fitem.Fav)
            {
                txt = "❤";
                y = pictureBox.Height - fsize;
                g.DrawString(txt, fnt, txtbrush, 0, y);
            }
            if (fitem.Del)
            {
                txt = "❌️";
                y = pictureBox.Height - fsize - fsize;
                g.DrawString(txt, fnt, txtbrush, 0, y);
            }

            fnt.Dispose();
        }


        private Size GetThumbnailSize()
        {
            PictureBox p = RightPicBox;
            return new Size(p.Width / Col, p.Height / Row);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void rightPicBox_Paint(object sender, PaintEventArgs e)
        {
            //if (NextPic)
            switch (mModel.ThumbViewType)
            {
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_NEXT:
                    rightPicBox_Paint_next(e);
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LIST:
                    rightPicBox_Paint_list(e);
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LINEAR:
                default:
                    rightPicBox_Paint_thumbnail(e);
                    break;
            }
        }

        private void rightPicBox_Paint_thumbnail(PaintEventArgs e)
        { 
            Graphics g = e.Graphics;

            int col = Col;
            int row = Row;

            var tsize = GetThumbnailSize();
            int thumWidth = tsize.Width;
            int thumHeight = tsize.Height;

            int x = 0;
            int y = 0;
            var dispNum = col * row;
            for (int i = 0; i < dispNum && i < mModel.PictureTotalNumber; i++)
            {
                var idx = mModel.GetAbsIdx(i);
                FileItem fitem = mModel.GetFileItem(idx);//GetCurrentFileItemByRelativeIndex(i);
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

                if (idx == 0)
                {
                    g.FillRectangle(BRUSH_0, x, y, thumWidth / 2, thumHeight);
                }

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
                        Font fnt = new(FONT_NAME, fsize);
                        string txt = string.Format("small({0,4}x{1,4})[{2,4}x{3,4}]", imgsize.Width, imgsize.Height, thumWidth, thumHeight);
                        g.DrawString(txt, fnt, txtbrush, x, y);
                    }
                }

                if (fitem.Mark)
                {
                    var opaqueBrush = new SolidBrush(Color.FromArgb(OPA_VAL, COLOR_MARK));
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

        private void rightPicBox_Paint_next(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var idx = mModel.GetAbsIdx(1);
            FileItem fitem = mModel.GetFileItem(idx);
            Image img = fitem.GetImage();

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

            int x = 0;
            int y = 0;
            if (img != null)
            {
                ImageModule.DrawImage(
                                        g,
                                        x,
                                        y,
                                        img.Width,
                                        img.Height,
                                        img);
            }
        }

        private void rightPicBox_Paint_list(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int col = Col;
            int row = Row;

            var tsize = GetThumbnailSize();
            int thumWidth = tsize.Width;
            int thumHeight = tsize.Height;

            int x = 0;
            int y = 0;
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    var idx = mModel.GetAbsIdxList(i, j);
                    FileItem fitem = mModel.GetFileItem(idx);
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
                    }

                    if (fitem.Mark)
                    {
                        var opaqueBrush = new SolidBrush(Color.FromArgb(OPA_VAL, COLOR_MARK));
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
}
