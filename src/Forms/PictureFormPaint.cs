using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using PictureManagerApp.src.Lib;
using PictureManagerApp.src.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace PictureManagerApp
{
    public partial class PictureForm : Form
    {
        //---------------------------------------------------------------------
        // const
        //---------------------------------------------------------------------

        private const int SMALL_PIXEL_NUMBER = 600 * 960;

        private const string FONT_NAME = "Yu Gotic UI";// MS ゴシック";
        private const int FONT_SIZE = 20;
        private const int FONT_SPACE = 3;
        private const int OPA_VAL = 128;

        private static readonly Brush FONT_COLOR = Brushes.Aqua;
        //private static readonly Pen PEN_COLOR_FRAME = Pens.Crimson;
        private static readonly Pen PEN_COLOR_FRAME = new Pen(Color.Crimson, 3);

        private const int PEN_PROGRESS_WIDTH = 5;
        //private static readonly Pen PEN_COLOR_PROGRESS = new Pen(Color.LawnGreen, 15);
        private static readonly Pen PEN_COLOR_PROGRESS = new Pen(Color.Gray, PEN_PROGRESS_WIDTH);

        private static readonly Brush BRUSH_0 = Brushes.Blue;
        private static readonly Brush BRUSH_MARK = Brushes.DarkRed;
        private static readonly Brush BRUSH_SLIDESHOW = Brushes.Gray;
        private static readonly Brush BG_BRUSH = Brushes.Black;
        private static readonly Color COLOR_MARK = Color.Red;


        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private Brush _BgBrush = BG_BRUSH;

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var fitem = mModel.GetCurrentFileItem();

            if (fitem.IsGroupEntry)
            {
                PictureBox_Paint_Group(sender, e, (GroupItem)fitem);
            }
            else
            {
                switch (mModel.ThumbViewType)
                {
                    case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_MANGA:
                        PictureBox_Paint_Manga(sender, e);
                        break;
                    default:
                        PictureBox_Paint_OnePic(sender, e, fitem);
                        //rightPicBox_Paint_thumbnail(e, true);
                        break;
                }
            }
        }

        private void DrawPicInfo(Graphics g, GroupItem gitem)
        {
            var fsize = FONT_SIZE;
            var inc = FONT_SPACE;
            var x = 0;
            var y = 0;
            var txtbrush = FONT_COLOR;

            using (var fnt = new Font(FONT_NAME, fsize))
            {
                var txt = "";

                txt = $"'{gitem.FilePath}'";
                g.DrawString(txt, fnt, txtbrush, x, y);
                y += fsize + inc;

                txt = $"{gitem.MemberCount()}ファイル";
                g.DrawString(txt, fnt, txtbrush, x, y);
                y += fsize + inc;
            }
        }

        private void PictureBox_Paint_Group(object sender, PaintEventArgs e, GroupItem gitem)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(BRUSH_0, 0, 0, pictureBox.Width, pictureBox.Height);

            //var imgs = gitem.GetGroupImages();
            //var img = ImageModule.CreateCompositedImage(imgs, pictureBox.Width, pictureBox.Height);
            var img = gitem.GetGroupImage(pictureBox.Width, pictureBox.Height);
            g.DrawImage(img, 0, 0);

            DrawPicInfo(g, gitem);

            g.DrawRectangle(PEN_COLOR_FRAME, 0, 0, pictureBox.Width, pictureBox.Height);
        }

        private void PictureBox_Paint_Manga(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(_BgBrush, 0, 0, pictureBox.Width, pictureBox.Height);

            var fitem1 = mModel.GetCurrentFileItem();
            var fitem2 = mModel.GetCurrentFileItemByRelativeIndex(1);
            var img1 = fitem1.GetImage();
            var img2 = fitem2.GetImage();
            bool mihiraki;

            if (mModel.IsLastNow())
            {
                mihiraki = false;

                var triangle = GetTriangleLB(0, 0, pictureBox.Width, pictureBox.Height);
                g.FillPath(BRUSH_0, triangle);
            }
            else
            {
                var size1 = fitem1.GetImageSize();
                var size2 = fitem2.GetImageSize();
                if (PictureModel.IsMihiraki(size1, size2))
                {   //縦長画像2枚
                    mihiraki = true;
                }
                else
                {   //横長の場合は一枚のみ表示
                    mihiraki = false;
                }
            }

            if (mihiraki)
            {
                var img = ImageModule.GetOneImage(img1, img2, pictureBox.Width, pictureBox.Height);
                //画像描画
                g.DrawImage(img, 0, 0);
                DrawPageNo(g, pictureBox.Width, pictureBox.Height);
            }
            else
            {
                ImageModule.DrawImage(
                    g,
                    0, 0, pictureBox.Width, pictureBox.Height,
                    img1,
                    IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN);
                DrawPageNo(g, pictureBox.Width, pictureBox.Height);
            }

            DrawProgressBar(g);
        }

        private void DrawProgressBar(Graphics g)
        {
            // 進捗プログレスバー的な
            //左に進む
            var (p1, p2) = GetProgressPointRtoL();
            g.DrawLine(PEN_COLOR_PROGRESS, p2, p1);
        }

        private void FillBG(Graphics g, FileItem fitem)
        {
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
                bgbrush = _BgBrush;
            }
            g.FillRectangle(bgbrush, 0, 0, pictureBox.Width, pictureBox.Height);
        }

        private void PictureBox_Paint_OnePic(object sender, PaintEventArgs e, FileItem fitem)
        {
            Graphics g = e.Graphics;

            FillBG(g, fitem);

            if (mCurrentImg == null)
            {
                Log.err("!!! image not found !!!");

                //TODO: 異常画像情報の表示
                DrawString(g, "invalid", 0, 0);
            }
            else
            {
                int alphaPercent = 100;
                if (mTransitionEffect)
                {
                    alphaPercent = mAlphaPercent;
                }

                var magType = mMagType;
                Image img;

                switch (mModel.ROT_TYPE)
                {
                    case DISP_ROT_TYPE.DISP_ROT_R090:
                        img = ImageModule.RotateImage(mCurrentImg, true);
                        break;
                    case DISP_ROT_TYPE.DISP_ROT_R270:
                        img = ImageModule.RotateImage(mCurrentImg, false);
                        break;
                    case DISP_ROT_TYPE.DISP_ROT_R180:
                    case DISP_ROT_TYPE.DISP_ROT_NONE:
                    default:
                        img = mCurrentImg;
                        break;
                }

                //画像描画
                var d = ImageModule.DrawCompositedImage(
                    g,
                    pictureBox.Width,
                    pictureBox.Height,
                    img,//mCurrentImg,
                    mPrevImg,
                    alphaPercent,
                    magType);

                // 画像情報描画
                PictureBox_PaintTxt(g, d, fitem);
            }


            // 進捗プログレスバー的な
            var (p1, p2) = GetProgressPoint();
            g.DrawLine(PEN_COLOR_PROGRESS, p1, p2);
        }

        private (Point, Point) GetProgressPoint()
        {
            var width = GetProgressPointWidth();
            var y = pictureBox.Height - PEN_PROGRESS_WIDTH;

            var p1 = new Point(0, y);
            var p2 = new Point(width, y);
            return (p1, p2);
        }

        private (Point, Point) GetProgressPointRtoL()
        {
            var width = GetProgressPointWidth();
            var y = pictureBox.Height - PEN_PROGRESS_WIDTH;

            var p1 = new Point(pictureBox.Width, y);
            var p2 = new Point(pictureBox.Width - width, y);
            return (p1, p2);
        }

        private int GetProgressPointWidth()
        {
            var unit = 10000;
            var (now, total) = mModel.GetIndex();
            var width = pictureBox.Width * (now * unit / total) / unit;

            return width;
        }

        private void PictureBox_PaintTxt(Graphics g, DrawDimension d, FileItem fitem)
        {
            // テキストの描画
            int fsize = FONT_SIZE;
            var inc = FONT_SPACE;
            int x = 0;
            int y = 0;
            var txtbrush = FONT_COLOR;
            using (var fnt = new Font(FONT_NAME, fsize))
            {
                string txt;

                //
                if (DisplayTxt)
                {
                    txt = mModel.GetPictureInfoText();
                    if (mSlideshow)
                    {
                        txt = "▶️" + txt;
                    }
                    g.DrawString(txt, fnt, txtbrush, x, y);
                    y += fsize + inc;

                    //
                    txt = mModel.GetArtistInfoFromDB();
                    g.DrawString(txt, fnt, txtbrush, x, y);
                    y += fsize + inc;

                    //
                    txt = string.Format("{0,4}x{1,4}", pictureBox.Width, pictureBox.Height);
                    g.DrawString(txt, fnt, txtbrush, x, y);
                    y += fsize + inc;

                    //
                    var asp = ImageModule.GetAspectRatio16_9(mCurrentImg.Width, mCurrentImg.Height);
                    txt = string.Format("{0,4}x{1,4}[{2}]", mCurrentImg.Width, mCurrentImg.Height, asp);
                    g.DrawString(txt, fnt, txtbrush, x, y);
                    y += fsize + inc;

                    //
                    txt = string.Format("{0,4}x{1,4}({2}%)", d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1, d.GetPercent());
                    g.DrawString(txt, fnt, txtbrush, x, y);
                    y += fsize + inc;
                }
                else
                {
                    txt = mModel.GetPictureInfoText(true);
                    if (mSlideshow)
                    {
                        txt = "▶️" + txt;
                    }
                    var txt2 = string.Format("[{0,4}x{1,4}]", mCurrentImg.Width, mCurrentImg.Height);
                    g.DrawString(txt + txt2, fnt, txtbrush, x, y);

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
            }
        }

        private Size GetThumbnailSize()
        {
            return GetThumbnailSize(ThumbnailCols, ThumbnailRows);
        }

        private Size GetThumbnailSize(int cols, int rows)
        {
            PictureBox p = RightPicBox;
            return new Size(p.Width / cols, p.Height / rows);
        }

        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        private void rightPicBox_Paint(object sender, PaintEventArgs e)
        {
            switch (mModel.ThumbViewType)
            {
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_NEXT:
                    rightPicBox_Paint_next(e);
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_LIST:
                    rightPicBox_Paint_list(e);
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_OVERVIEW:
                    rightPicBox_Paint_overview(e);
                    break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_GROUP:
                    //rightPicBox_Paint_group(e);
                    //break;
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_DIRECTORY:
                case THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_TILE:
                default:
                    rightPicBox_Paint_thumbnail(e);
                    break;
            }
        }

        private Size GetSize(bool main)
        {
            Size tsize;
            if (main)
            {
                var p = pictureBox;
                tsize = new Size(p.Width / 4, p.Height / 3);
            }
            else
            {
                tsize = GetThumbnailSize();
            }
            return tsize;
        }


        private void rightPicBox_Paint_thumbnail(PaintEventArgs e, bool main = false)
        {
            var g = e.Graphics;

            int col = ThumbnailCols;
            int row = ThumbnailRows;

            var tsize = GetSize(main);
            int thumWidth = tsize.Width;
            int thumHeight = tsize.Height;

            int x = 0;
            int y = 0;
            var dispNum = col * row;
            for (int i = 0; i < dispNum && i < mModel.PictureTotalNumber; i++)
            {
                var idx = mModel.GetAbsIdx(i);
                var fitem = mModel.GetFileItem(idx);//GetCurrentFileItemByRelativeIndex(i);
                var imgsize = fitem.GetImageSize();
                Image thumbImg;
                if (main)
                {
                    thumbImg = fitem.GetImage();
                }
                else
                {
                    thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight);
                }

                FillRectangle(g, fitem, x, y, thumWidth, thumHeight);

                if (idx == 0)
                {
                    //先頭ファイルの目印
                    DrawTopMark(g, x, y, thumWidth, thumHeight);
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

                    if (imgsize.Width * imgsize.Height < SMALL_PIXEL_NUMBER)
                    {
                        int fsize = FONT_SIZE;
                        var fnt = new Font(FONT_NAME, fsize);
                        string txt = string.Format("small({0,4}x{1,4})[{2,4}x{3,4}]", imgsize.Width, imgsize.Height, thumWidth, thumHeight);
                        var txtbrush = Brushes.Red;
                        g.DrawString(txt, fnt, txtbrush, x, y);
                    }
                }
                else if (fitem.InvalidFile)
                {
                    var txt = "invalid";
                    DrawString2(g, txt, x, y, Brushes.Red);
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

            if (mModel.PictureTotalNumber > dispNum)
            {
                DrawPercent(g, dispNum);
            }
        }

        private void DrawTopMark(Graphics g, int x, int y, int thumWidth, int thumHeight)
        {
            var t = false;
            if (t)
            {
                g.FillRectangle(BRUSH_0, x, y, thumWidth / 2, thumHeight);
            }
            else
            {
                var triangle = GetTriangle(x, y, thumWidth, thumHeight);
                g.FillPath(BRUSH_0, triangle);
            }

        }

        private void DrawPercent(Graphics g, int dispNum)
        {
            var w_ = 150;
            var h_ = 30;
            var x_ = RightPicBox.Width - w_;
            var y_ = RightPicBox.Height - h_;

            var opaqueBrush = new SolidBrush(Color.FromArgb(OPA_VAL, COLOR_MARK));
            g.FillRectangle(opaqueBrush, x_, y_, w_, h_);

            var idx = mModel.GetAbsIdx(dispNum - 1) + 1;
            //var idx = mModel.GetAbsIdx(0);
            var current_page_no = Math.Ceiling((decimal)idx / dispNum);// + 1;
            var total_page = Math.Ceiling((decimal)mModel.PictureTotalNumber / dispNum);

            int fsize = FONT_SIZE;
            var fnt = new Font(FONT_NAME, fsize);
            //string txt = string.Format($"{idx * 100 / mModel.PictureTotalNumber}%");
            string txt = string.Format($"{current_page_no}/{total_page}({idx * 100 / mModel.PictureTotalNumber}%)");
            var txtbrush = Brushes.White;
            g.DrawString(txt, fnt, txtbrush, x_, y_);
        }

        private GraphicsPath GetTriangle(int x, int y, int w, int h)
        {
            var triangle = new System.Drawing.Drawing2D.GraphicsPath();
            Point p1 = new Point(x, y);
            Point p2 = new Point(x, y + h);
            Point p3 = new Point(x + w, y);
            triangle.AddPolygon(new Point[] { p1, p2, p3 });
            return triangle;
        }

        private GraphicsPath GetTriangleLB(int x, int y, int w, int h)
        {
            var triangle = new System.Drawing.Drawing2D.GraphicsPath();
            Point p1 = new Point(x, y);
            Point p2 = new Point(x, y + h);
            Point p3 = new Point(x + w, y + h);
            triangle.AddPolygon(new Point[] { p1, p2, p3 });
            return triangle;
        }


        private void rightPicBox_Paint_overview(PaintEventArgs e)
        {
            var g = e.Graphics;

            int col = 6;// ThumbnailCols;
            int row = 6;// ThumbnailRows;

            rightPicBox_Paint_overview_core(g, col, row);
        }

        private void rightPicBox_Paint_overview_core(Graphics g, int col, int row)
        { 
            var tsize = GetThumbnailSize(col, row);
            int thumWidth = tsize.Width;
            int thumHeight = tsize.Height;

            int x = 0;
            int y = 0;
            var dispNum = col * row;
            for (int i = 0; i < dispNum && i < mModel.PictureTotalNumber; i++)
            {
                var idx = mModel.GetAbsIdx(i);
                var fitem = mModel.GetFileItem(idx);
                var imgsize = fitem.GetImageSize();
                var thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight);

                FillRectangle(g, fitem, x, y, thumWidth, thumHeight);

                if (idx == 0)
                {
                    //先頭ファイルの目印
                    var triangle = GetTriangle(x, y, thumWidth, thumHeight);
                    g.FillPath(BRUSH_0, triangle);
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

                    if (imgsize.Width * imgsize.Height < SMALL_PIXEL_NUMBER)
                    {
                        string txt = string.Format("small({0,4}x{1,4})[{2,4}x{3,4}]", imgsize.Width, imgsize.Height, thumWidth, thumHeight);
                        var txtbrush = Brushes.Red;
                        DrawString2(g, txt, x, y, txtbrush);
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

        private void rightPicBox_Paint_list(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //Log.trc($"Col={Col} Row={Row}");

            var tsize = GetThumbnailSize();
            int thumWidth = tsize.Width;
            int thumHeight = tsize.Height;

            //var dispNum = col * row;

            int pixel_x = 0;
            int pixel_y = 0;

            int offset = 0;
            int offsetoffset = 0;
            int currpos = 0;

            int r = 0;
            while (r < ThumbnailRows)
            {
                //TODO:わかりにくので枠線を動かすようにする
                //端に来たら画像をスクロールする
                //...
                var start_i = 0;

                //if (offset > mModel.)
                var absIdx = mModel.GetAbsIdx(offset);
                List<FileItem> filelist;
                if (offset == 0)
                {
                    filelist = mModel.GetSameDirFileItemList(absIdx, ref offsetoffset, out currpos);
                    if (currpos >= ThumbnailCols)
                    {
                        start_i = filelist.Count - currpos;
                    }
                }
                else
                {
                    filelist = mModel.GetSameDirFileItemList2(absIdx);
                    offsetoffset = filelist.Count;
                }
                //Log.trc($"j={j} offset={offset} absIdx={absIdx}");

                string dirtxt = filelist[0].DirectoryName;
                int cnt = 0;
                //foreach (FileItem fitem in filelist) {
                for (int i = start_i; i < filelist.Count; i++)
                {
                    var fitem = filelist[i];

                    //Log.trc($"i={i}");
                    // 背景を塗りつぶす
                    FillRectangle(g, fitem, pixel_x, pixel_y, thumWidth, thumHeight);

                    // 画像の描画
                    var thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight);
                    if (thumbImg != null)
                    {
                        ImageModule.DrawImage(
                                                g,
                                                pixel_x,
                                                pixel_y,
                                                thumWidth,
                                                thumHeight,
                                                thumbImg);
                    }

                    if (fitem.Mark)
                    {
                        var opaqueBrush = new SolidBrush(Color.FromArgb(OPA_VAL, COLOR_MARK));
                        g.FillRectangle(opaqueBrush, pixel_x, pixel_y, thumWidth, thumHeight);
                    }

                    //枠線描画
                    if (r == 0 && i == currpos)
                    {
                        //var opaqueBrush = new SolidBrush(Color.FromArgb(OPA_VAL, COLOR_MARK));
                        //g.FillRectangle(opaqueBrush, x, y, thumWidth, thumHeight);
                        g.DrawRectangle(PEN_COLOR_FRAME, pixel_x, pixel_y, thumWidth - 1, thumHeight - 1);

                        int fsize = FONT_SIZE;
                        string txt = string.Format($"{absIdx + 1}");
                        DrawString(g, txt, pixel_x, pixel_y + fsize);
                    }

                    //番号描画
                    {
                        int fsize = FONT_SIZE;
                        string txt = string.Format($"{i + 1}/{filelist.Count}");
                        DrawString(g, txt, pixel_x, pixel_y + fsize + fsize);
                    }

                    cnt++;
                    if (cnt > ThumbnailCols)
                    {
                        break;
                    }

                    pixel_x += thumWidth;
                }
                {
                    int fsize = FONT_SIZE;
                    //var fnt = new System.Drawing.Font(FONT_NAME, fsize);
                    //string txt = string.Format($"{filelist.Count}");
                    //string txt = string.Format($"j={r}/{offset}:{absIdx}");
                    //DrawString(g, dirtxt, 0, y);
                    //DrawString(g, txt, 0, pixel_y + fsize * 2);

                    if (filelist.Count > ThumbnailCols)
                    {
                        DrawString(g, "◀", 0, pixel_y + (thumHeight / 2));
                        DrawString(g, "▶", RightPicBox.Size.Width - fsize, pixel_y + (thumHeight / 2));
                    }
                }
                DrawString(g, dirtxt, 0, pixel_y);

                //if (offset)
                offset += offsetoffset;
                pixel_x = 0;
                pixel_y += thumHeight;
                r++;
            }
        }

        private void rightPicBox_Paint_next(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var idx = mModel.GetAbsIdx(1);
            var fitem = mModel.GetCurrentFileItemByRelativeIndex(1);

            FillRectangle(g, fitem, 0, 0, RightPicBox.Width, RightPicBox.Height);

            if (idx == 0)
            {
                //g.FillRectangle(BRUSH_0, x, y, thumWidth / 2, thumHeight);
            }

            //TODO: センタリング？
            //int x = 0;
            //int y = 0;
            var img = fitem.GetImage();
            if (img != null)
            {
                /*ImageModule.DrawImage(
                                        g,
                                        x,
                                        y,
                                        img.Width,
                                        img.Height,
                                        img);*/
                DrawDimension d = ImageModule.DrawCompositedImage(
                    g,
                    RightPicBox.Width,
                    RightPicBox.Height,
                    img);
            }
        }

        private void FillRectangle(Graphics g, FileItem fitem, int x, int y, int thumWidth, int thumHeight)
        {
            Brush bgbrush;
            if (fitem.Mark)
            {
                bgbrush = BRUSH_MARK;
            }
            else
            {
                bgbrush = _BgBrush;
            }
            g.FillRectangle(bgbrush, x, y, thumWidth, thumHeight);
        }

        private void DrawString(Graphics g, string txt, int x, int y)
        {
            int fsize = FONT_SIZE;
            using (var fnt = new Font(FONT_NAME, fsize))
            {
                var txtbrush = FONT_COLOR;
                //g.DrawString(txt, fnt, txtbrush, x, y + fsize);
                g.DrawString(txt, fnt, txtbrush, x, y);
            }
        }

        private void DrawString2(Graphics g, string txt, int x, int y, Brush txtbrush)
        {
            int fsize = FONT_SIZE;
            using (var fnt = new Font(FONT_NAME, fsize))
            {
                g.DrawString(txt, fnt, txtbrush, x, y);
            }
        }

        private void DrawStringRB(Graphics g, string txt, int canvasWidth, int canvasHeight)
        {
            int fsize = FONT_SIZE;
            using (var fnt = new Font(FONT_NAME, fsize))
            {
                SizeF textSize = g.MeasureString(txt, fnt);
                int widthInPixels = (int)Math.Ceiling(textSize.Width);
                int heightInPixels = (int)Math.Ceiling(textSize.Height);

                int x = canvasWidth - widthInPixels;
                int y = canvasHeight - heightInPixels;

                var txtbrush = FONT_COLOR;
                g.DrawString(txt, fnt, txtbrush, x, y);
            }
        }

        private void DrawPageNo(Graphics g, int canvasWidth, int canvasHeight)
        {
            var (now, total) = mModel.GetIndex();
            var txt = $"{now}/{total}";

            if (now == total)
            {
                txt += "終";
            }

            DrawStringRB(g, txt, canvasWidth, canvasHeight);
        }
    }
}
