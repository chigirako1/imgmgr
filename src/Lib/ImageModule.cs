using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public struct DrawDimension
    {
        public int width, height;

        public int src_x1, src_y1;
        public int src_x2, src_y2;

        public int dst_x1, dst_y1;
        public int dst_x2, dst_y2;

        public int ratio;
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
            Graphics g = Graphics.FromImage(bmpCanvas);

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
        public static DrawDimension DrawImage(
            Graphics g,
            int x,
            int y,
            int w,
            int h,
            Image img
            )
        {
            DrawDimension d = getDispParam(w, h, img.Width, img.Height, IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND);
            var dstRect = new Rectangle(
                d.dst_x1 + x, 
                d.dst_y1 + y, 
                d.dst_x2 - d.dst_x1, 
                d.dst_y2 - d.dst_y1);
            g.DrawImage(img,
                dstRect,
                0, 0, img.Width, img.Height,
                GraphicsUnit.Pixel);
            return d;
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
            IMAGE_DISPLAY_MAGNIFICATION_TYPE magType = IMAGE_DISPLAY_MAGNIFICATION_TYPE.IMG_DISP_MAG_FIT_SCREEN_NO_EXPAND
            )
        {
            int ratio = GetDispRatio(scrnW, scrnH, imgW, imgH, magType);

            DrawDimension d;

            // 画面に表示するサイズ
            d.width = imgW * ratio / 100;
            d.height = imgH * ratio / 100;

            if (scrnW >= d.width)
            {
                //画面の幅に収まっている

                //幅に対してセンタリング
                d.dst_x1 = (scrnW - d.width) / 2;
                d.dst_x2 = d.dst_x1 + d.width;

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
                d.src_x1 = (imgW - scrnW) / 2;
                d.src_x2 = imgW - d.src_x1;
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
                d.src_y1 = (imgH - scrnH) / 2;
                d.src_y2 = imgH - d.src_y1;
            }

            d.ratio = ratio;
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
            int ratio = 100;

            int ratio_w = scrnW * 100 / imgW;
            int ratio_h = scrnH * 100 / imgH;

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
                    ratio = ratio > 100 ? 100 : ratio;
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
                    ratio = 100;
                    break;
            }

            return ratio;
        }
    }
}
