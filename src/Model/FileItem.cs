using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using PictureManagerApp.src.Lib;

namespace PictureManagerApp.src.Model
{
    public class FileItem
    {
        //=====================================================================
        // private field
        //=====================================================================
        private readonly string mPath;
        private string mZipPath;
        private Size ImageSize;

        //private Image mImage;
        private Image mThumbnail;

        public bool Mark { set; get; }
        public bool Removed { set; get; }
        public bool IsZipEntry { private set; get; }

        public int FileSize { set; get; }


        //=====================================================================
        // 
        //=====================================================================
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public FileItem(string path, string zipPath = "")
        {
            mPath = path;
            mZipPath = zipPath;
            if (mZipPath != "")
            {
                IsZipEntry = true;
            }
        }

        public FileItem(FileItem org)
        {
            this.mPath = org.mPath;
            this.mZipPath = "";
            this.Mark = org.Mark;
            this.mThumbnail = org.mThumbnail;
        }

        public string FilePath
        {
            get { return mPath; }
        }

        public string GetRelativePath(string basePath)
        {
            if (mZipPath != "")
            {
                return mZipPath;
            }

            Uri basepath = new Uri(basePath + Path.DirectorySeparatorChar);
            string dirname = Path.GetDirectoryName(mPath);
            Uri dirUri = new Uri(dirname);

            Uri relUri = basepath.MakeRelativeUri(dirUri);

            return relUri.ToString();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static bool isSpecifiedFile(string filepath, DateTime? from, DateTime? to)
        {
            FileInfo fi = new FileInfo(filepath);
            DateTime dt = fi.LastWriteTime;
            if (from != null && dt.CompareTo(from) < 0)
            {
                return false;
            }

            if (to != null && dt.CompareTo(to) > 0)
            {
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public Image GetImage()
        {
            if (mZipPath == "")
            {
                return GetImageFromFile();
            }
            else
            {
                using (var archive = ZipFile.OpenRead(mZipPath))
                {
                    var ent = archive.GetEntry(FilePath);
                    var img = Image.FromStream(ent.Open());
                    ImageSize.Width = img.Width;
                    ImageSize.Height = img.Height;
                    return img;
                }
            }
        }

        private Image GetImageFromFile()
        {
#if false
//メモリ大量使用
            if (mImage == null)
            {
                string path = FilePath;
                mImage = ImageModule.GetImage(path);
                ImageSize.Width = mImage.Width;
                ImageSize.Height = mImage.Height;
                Log.trc(path);
            }
            return mImage;
#else
            var img = ImageModule.GetImage(this.FilePath);
            ImageSize.Width = img.Width;
            ImageSize.Height = img.Height;
            return img;
#endif
        }

        public Size GetImageSize()
        {
            return ImageSize;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public Image GetThumbnailImage(int thumbWidth, int thumbHeight, bool force = false)
        {
            if (force &&
                ((mThumbnail == null) ||
                (thumbWidth > mThumbnail.Width || thumbHeight > mThumbnail.Height)))
            {
                string path = FilePath;
                var img = GetImage();
                mThumbnail = ImageModule.GetThumbnailImage(img, thumbWidth, thumbHeight);
                Log.trc(path);
            }
            return mThumbnail;
        }

        public bool HasThumbnailImage()
        {
            if (mThumbnail == null)
            {
                return false;
            }
            return true;
        }
    }
}
