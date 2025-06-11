using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureManagerApp.src.Lib
{
    public enum IMAGE_DISPLAY_MAGNIFICATION_TYPE
    {
        IMG_DISP_MAG_NONE,
        // はみ出さない
        IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND,      /* 縦も横も画面内に収める(はみ出さない。画面より小さい場合は等倍表示 */
        IMG_DISP_MAG_FIT_SCREEN,                /* 縦も横も画面内に収める(はみ出さない。小さい画像は拡大する) */
        // はみ出る場合あり
        IMG_DISP_MAG_SPECIFY,                   /* 拡大縮小率を指定 */
        IMG_DISP_MAG_OPTIMIZE_MAX,              /* 縦か横を画面に合わせる（縦長、横長による）。はみ出る場合あり。 */
        IMG_DISP_MAG_FIT_WIDTH,                 /* 幅に合わせて拡大縮小(縦にはみ出る場合がある) */
        IMG_DISP_MAG_FIT_HEIGHT,				/* 高さを画面に合わせて拡大縮小(横がはみ出る場合がある */
        IMG_DISP_MAG_AS_IS,                     /* 拡大縮小を行わない */

        IMG_DISP_MAG_MAX
    }

    public enum IMAGE_DISPLAY_ALIGNMENT_TYPE
    {
        IMAGE_DISPLAY__ALIGNMENT_CENTER,
        IMAGE_DISPLAY__ALIGNMENT_RIGHT,
        IMAGE_DISPLAY__ALIGNMENT_LEFT,

        IMAGE_DISPLAY__ALIGNMENT_MAX,
    }

    public struct DrawDimension
    {
        public static int PER = 10000;

        public int width, height;

        public int src_x1, src_y1;
        public int src_x2, src_y2;

        public int dst_x1, dst_y1;
        public int dst_x2, dst_y2;

        public int ratio;
        public int GetPercent()
        {
            int percent = ratio / (PER / 100);
            return percent;
        }
    }

    public static class ImageModule
    {
        public static Image GetImage(string path)
        {
            // Log.trc($"path={path}");
            using FileStream fs = new(
                        path,
                        FileMode.Open,
                        FileAccess.Read);

            Image img = Image.FromStream(fs);

            fs.Close();

            return img;
        }

        public static byte[] ConvImageToByteArray(System.Drawing.Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image GetThumbnailImage(Image orgImg, int thumbWidth, int thumbHeight)
        {
            var bmpCanvas = new Bitmap(thumbWidth, thumbHeight);
            using (var g = Graphics.FromImage(bmpCanvas))
            //var g = Graphics.FromImage(bmpCanvas);
            {
                var d = getDispParam(thumbWidth, thumbHeight, orgImg.Width, orgImg.Height);
                var dstRect = new Rectangle(
                                    d.dst_x1,
                                    d.dst_y1,
                                    d.dst_x2 - d.dst_x1,
                                    d.dst_y2 - d.dst_y1);
                g.DrawImage(orgImg,
                    dstRect,
                    0,
                    0,
                    orgImg.Width,
                    orgImg.Height,
                    GraphicsUnit.Pixel);
            }

            return bmpCanvas;
        }

        public static Image GetGroupThumbnailImage(int thumbWidth, int thumbHeight, string txt)
        {
            var bmpCanvas = new Bitmap(thumbWidth, thumbHeight);
            Graphics g = Graphics.FromImage(bmpCanvas);

            Brush BRUSH_0 = Brushes.Blue;
            g.FillRectangle(BRUSH_0, 0, 0, thumbWidth, thumbHeight);

            var FONT_COLOR = Brushes.Red;
            var FONT_SIZE = 5;
            var FONT_NAME = "MS ゴシック";
            var txtbrush = FONT_COLOR;
            var fsize = FONT_SIZE;
            var fnt = new Font(FONT_NAME, fsize);
            g.DrawString(txt, fnt, txtbrush, 0, 0);

            return bmpCanvas;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static Image CreateTranslucentImage(Image img, float alpha)
        {
            Bitmap transImg = new(img.Width, img.Height);
            Graphics g = Graphics.FromImage(transImg);

            System.Drawing.Imaging.ColorMatrix cm =
                new();
            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix33 = alpha;
            cm.Matrix44 = 1;
            System.Drawing.Imaging.ImageAttributes ia =
                new();
            ia.SetColorMatrix(cm);
            g.DrawImage(img,
                new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();

            return transImg;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static float GetAspectRatio(int width, int height)
        {
            return (float)height / (float)width;
        }

        public static string GetAspectRatio16_9(int width, int height)
        {
            if (width > height)
            {
                var ratio = GetAspectRatio(width, height);
                var r16 = (int)(ratio * 16);
                return $"16:{r16}";
            }
            else
            {
                var ratio = GetAspectRatio(height, width);
                var r16 = (int)(ratio * 16);
                return $"{r16}:16";
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static Image CreateCompositedImage(
            int w,
            int h,
            Image currentImg,
            Image prevImg,
            int alphaPercent)
        {
            Bitmap drawImg = new(w, h);
            Graphics g = Graphics.FromImage(drawImg);

            System.Drawing.Imaging.ColorMatrix cm =
                new();
            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix44 = 1;
            System.Drawing.Imaging.ImageAttributes ia =
                new();

            if (prevImg != null)
            {
                cm.Matrix33 = (100 - alphaPercent) * 0.01f;
                ia.SetColorMatrix(cm);

                DrawDimension dp = getDispParam(w, h, prevImg.Width, prevImg.Height);

                g.DrawImage(prevImg,
                    new Rectangle(dp.dst_x1, dp.dst_y1, dp.dst_x2 - dp.dst_x1, dp.dst_y2 - dp.dst_y1),
                    dp.src_x1, dp.src_y1, dp.src_x2 - dp.src_x1, dp.src_y2 - dp.src_y1,
                    GraphicsUnit.Pixel, ia);
            }

            cm.Matrix33 = alphaPercent * 0.01f;
            ia.SetColorMatrix(cm);

            DrawDimension d = getDispParam(w, h, currentImg.Width, currentImg.Height);
            g.DrawImage(currentImg,
                new Rectangle(d.dst_x1, d.dst_y1, d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1),
                0, 0, currentImg.Width, currentImg.Height,
                GraphicsUnit.Pixel, ia);

            g.Dispose();

            return drawImg;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static Image CreateCompositedImage(List<Image> imgs, int canvasW, int canvasH)
        {
            var drawImg = new Bitmap(canvasW, canvasH);
            using (var g = Graphics.FromImage(drawImg))
            { 
                //var ia = new System.Drawing.Imaging.ImageAttributes();

                int x = 0;
                int y = 0;
                int w;
                int h;

                // 画面サイズと画像数から各サムネイル画像の幅、高さを算出
                (w,h) = CalcThumbnailImageSize(imgs.Count(), canvasW, canvasH);

                // 描画
                foreach (var img in imgs)
                {
                    //左から右、上から下に描画
                    DrawImage(g, x, y, w, h, img, IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_OPTIMIZE_MAX);
                    x += w;

                    if (x >= canvasW)
                    {
                        x = 0;
                        y += h;
                    }
                }
            }

            return drawImg;
        }

        //---------------------------------------------------------------------
        // |2|1|　横に2枚の画像（２←１）を並べたImage生成
        //---------------------------------------------------------------------
        public static Image GetOneImage(Image img1, Image img2, int canvasW, int canvasH)
        {
            var newImg = new Bitmap(canvasW, canvasH);
            using (var g = Graphics.FromImage(newImg))
            {

                int x = canvasW / 2;//右側から描画
                int y = 0;
                int w = canvasW / 2;
                int h = canvasH;

                var mag = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN;
                // 描画
                DrawImage(g, x, y, w, h, img1, mag, IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_LEFT);

                x = 0;
                DrawImage(g, x, y, w, h, img2, mag, IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_RIGHT);
            }

            return newImg;
        }

        public static (int, int) CalcThumbnailImageSize(int numOfImg, int canvasW, int canvasH)
        {
            int col = (int)Math.Sqrt(numOfImg);
            int row = (int)Math.Sqrt(numOfImg);
            //splitNo.Col = Math.Min(max, (int)Math.Sqrt(count));
            //splitNo.Row = Math.Min(max, (int)Math.Sqrt(count));
            if (col * row < numOfImg)
            {
                row++;
            }
            if (col * row < numOfImg)
            {
                col++;
            }

            int w = canvasW / col;
            int h = canvasH / row;

            return (w, h);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static DrawDimension DrawImage(
            Graphics g,
            int x,
            int y,
            int w,
            int h,
            Image img,
            IMAGE_DISPLAY_MAGNIFICATION_TYPE mag = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND,
            IMAGE_DISPLAY_ALIGNMENT_TYPE alignment = IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_CENTER
            )
        {
            var dim = getDispParam(w, h, img.Width, img.Height, mag, alignment);
            DrawImage_Draw(g, x, y, w, h, img, dim);
            return dim;
        }

        public static void DrawImage_Draw(
            Graphics g,
            int x,
            int y,
            int w,
            int h,
            Image img,
            DrawDimension dim
            )
        {
            var dstRect = new Rectangle(
                dim.dst_x1 + x,
                dim.dst_y1 + y,
                dim.dst_x2 - dim.dst_x1,
                dim.dst_y2 - dim.dst_y1);
            var hoge = true;
            //hoge = false;
            if (hoge)
            {
                var srcRect = new Rectangle(
                    dim.src_x1,
                    dim.src_y1,
                    dim.src_x2 - dim.src_x1,
                    dim.src_y2 - dim.src_y1);

                //Log.trc($"[{w}x{h}] img={img.Width}x{img.Height} dst={dstRect} src={srcRect}");

                g.DrawImage(img,
                    dstRect,
                    srcRect,
                    GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(img,
                    dstRect,
                    0,
                    0,
                    img.Width,
                    img.Height,
                    GraphicsUnit.Pixel);
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static DrawDimension DrawCompositedImage(
            Graphics g,
            int w,
            int h,
            Image currentImg,
            Image prevImg = null,
            int alphaPercent = 100,
            IMAGE_DISPLAY_MAGNIFICATION_TYPE magType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND
            )
        {
            if (alphaPercent != 100)
            {
                System.Drawing.Imaging.ColorMatrix cm =
                    new();
                cm.Matrix00 = 1;
                cm.Matrix11 = 1;
                cm.Matrix22 = 1;
                cm.Matrix44 = 1;
                System.Drawing.Imaging.ImageAttributes ia =
                    new();

                if (prevImg != null)
                {
                    cm.Matrix33 = (100 - alphaPercent) * 0.01f;
                    ia.SetColorMatrix(cm);

                    DrawDimension dp = getDispParam(w, h, prevImg.Width, prevImg.Height, magType);

                    g.DrawImage(prevImg,
                        new Rectangle(dp.dst_x1, dp.dst_y1, dp.dst_x2 - dp.dst_x1, dp.dst_y2 - dp.dst_y1),
                        dp.src_x1, dp.src_y1, dp.src_x2 - dp.src_x1, dp.src_y2 - dp.src_y1,
                        GraphicsUnit.Pixel, ia);
                }

                cm.Matrix33 = alphaPercent * 0.01f;
                ia.SetColorMatrix(cm);

                DrawDimension d = getDispParam(w, h, currentImg.Width, currentImg.Height, magType);
                g.DrawImage(currentImg,
                    new Rectangle(d.dst_x1, d.dst_y1, d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1),
                    0, 0, currentImg.Width, currentImg.Height,
                    GraphicsUnit.Pixel, ia);

                return d;
            }
            else
            {
                DrawDimension d = getDispParam(w, h, currentImg.Width, currentImg.Height, magType);
                var dstRect = new Rectangle(d.dst_x1, d.dst_y1, d.dst_x2 - d.dst_x1, d.dst_y2 - d.dst_y1);
                g.DrawImage(currentImg,
                    dstRect,
                    //0, 0, currentImg.Width, currentImg.Height,
                    d.src_x1,
                    d.src_y1,
                    d.src_x2 - d.src_x1,
                    d.src_y2 - d.src_y1,
                    GraphicsUnit.Pixel);
                return d;
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private static DrawDimension getDispParam(
            int scrnW,
            int scrnH,
            int imgW,
            int imgH,
            IMAGE_DISPLAY_MAGNIFICATION_TYPE magType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND,
            IMAGE_DISPLAY_ALIGNMENT_TYPE alignment = IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_CENTER
            )
        {
            int ratio = GetDispRatio(scrnW, scrnH, imgW, imgH, magType);

            DrawDimension d;
            d.ratio = ratio;

            // 画面に表示するサイズ
            d.width = imgW * ratio / DrawDimension.PER;
            d.height = imgH * ratio / DrawDimension.PER;

            if (scrnW >= d.width)
            {
                //画面の幅に収まっている

                switch (alignment)
                {
                    case IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_LEFT:
                        d.dst_x1 = 0;
                        d.dst_x2 = d.dst_x1 + d.width;
                        break;
                    case IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_RIGHT:
                        d.dst_x1 = (scrnW - d.width);
                        d.dst_x2 = d.dst_x1 + d.width;
                        break;
                    case IMAGE_DISPLAY_ALIGNMENT_TYPE.IMAGE_DISPLAY__ALIGNMENT_CENTER:
                    default:
                        //幅に対してセンタリング
                        d.dst_x1 = (scrnW - d.width) / 2;
                        d.dst_x2 = d.dst_x1 + d.width;
                        break;
                 }

                //元画像の表示位置・幅
                d.src_x1 = 0;
                d.src_x2 = imgW - 1;
            }
            else
            {
                d.dst_x1 = 0;
                d.dst_x2 = scrnW - 1;

                //元画像の表示する部分の幅(x1,x2)の計算
                //d.src_x1 = 0;
                //d.src_x2 = scrnW * 100 / ratio;

                var rev = true;
                if (rev)
                {
                    var hamideru = (d.width - scrnW);
                    var hamideru2 = hamideru * DrawDimension.PER / ratio;

                    d.src_x1 = hamideru2 / 2;
                    d.src_x2 = imgW - d.src_x1;
                }
                else
                {
                    d.src_x1 = (imgW - scrnW) / 2;
                    d.src_x2 = imgW - d.src_x1;
                }
            }

            if (scrnH >= d.height)
            {
                //画面の高さに収まっている

                //高さに対してセンタリング
                d.dst_y1 = (scrnH - d.height) / 2;
                d.dst_y2 = d.dst_y1 + d.height;

                //元画像の表示位置・高さ
                d.src_y1 = 0;
                d.src_y2 = imgH - 1;
            }
            else
            {
                //表示部分が画面の高さに収まらない場合

                d.dst_y1 = 0;
                d.dst_y2 = scrnH - 1;

                //元画像の表示する部分の計算
                //d.src_y1 = 0;
                //d.src_y2 = scrnH * 100 / ratio;

                var f = true;
                if (f)
                {
                    var hamideru = (d.height - scrnH);
                    var hamideru2 = hamideru * DrawDimension.PER / ratio;
                    d.src_y1 = hamideru2 / 2;
                    d.src_y2 = imgH - d.src_y1;
                }
                else
                {
                    d.src_y1 = (imgH - scrnH) / 2;
                    d.src_y2 = imgH - d.src_y1;
                }
            }

            return d;
        }
        //---------------------------------------------------------------------
        // 表示倍率の計算
        //---------------------------------------------------------------------
        private static int GetDispRatio(
            int scrnW,
            int scrnH,
            int imgW,
            int imgH,
            IMAGE_DISPLAY_MAGNIFICATION_TYPE magType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND
            )
        {
            int ratio = DrawDimension.PER;

            int ratio_w = scrnW * DrawDimension.PER / imgW;
            int ratio_h = scrnH * DrawDimension.PER / imgH;

            switch (magType)
            {
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND:      /* 縦も横も画面内に収める(はみ出さない。画面より小さい場合は等倍表示 */
                    if (ratio_w > ratio_h)
                    {
                        ratio = ratio_h;
                    }
                    else
                    {
                        ratio = ratio_w;
                    }
                    ratio = ratio > DrawDimension.PER ? DrawDimension.PER : ratio;
                    break;
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN:                /* 縦も横も画面内に収める(はみ出さない。小さい画像は拡大する) */
                    if (ratio_w > ratio_h)
                    {
                        ratio = ratio_h;
                    }
                    else
                    {
                        ratio = ratio_w;
                    }
                    break;
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_OPTIMIZE_MAX:              /* 縦か横を画面に合わせる（縦長、横長による）。はみ出る場合あり。 */
                    if (ratio_w < ratio_h)
                    {
                        ratio = ratio_h;
                    }
                    else
                    {
                        ratio = ratio_w;
                    }
                    break;
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_WIDTH:                 /* 幅に合わせて拡大縮小(縦にはみ出る場合がある) */
                    ratio = ratio_w;
                    break;
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_HEIGHT:				/* 高さを画面に合わせて拡大縮小(横がはみ出る場合がある */
                    ratio = ratio_h;
                    break;
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_SPECIFY:                   /* 拡大縮小率を指定 */
                case IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_AS_IS:                     /* 拡大縮小を行わない */
                default:
                    ratio = DrawDimension.PER;
                    break;
            }

            return ratio;
        }
    }
}
