using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Policy;
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
        private bool mThumbnailFail;

        //private Image mImage;
        private Image mThumbnail;

        public bool Mark { set; get; }
        public bool Removed { set; get; }
        public bool IsZipEntry { private set; get; }

        public long FileSize { set; get; }

        public bool Fav {  set; get; }
        public bool Del { set; get; }

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
            else
            {
                FileInfo fi = new(path);
                FileSize = fi.Length;
            }
        }

        public FileItem(FileItem org)
        {
            this.mPath = org.mPath;
            //this.mZipPath = "";
            this.mZipPath = org.mZipPath;
            this.IsZipEntry = org.IsZipEntry;
            this.Mark = org.Mark;
            this.mThumbnail = org.mThumbnail;
            this.ImageSize = org.ImageSize;
        }

        public string FilePath
        {
            get { return mPath; }
        }

        public string DirectoryName
        {
            get { return Path.GetDirectoryName(mPath); }
        }

        public string GetZipEntryname()
        {
            if (mZipPath != "")
            {
                return this.FilePath;
            }
            return null;
        }

        public string GetTxtPath()
        {
            return mZipPath + "\t" + mPath;
        }

        public string GetRelativePath(string basePath, bool filename_p = false)
        {
            if (mZipPath != "")
            {
                return mZipPath;
            }

            if (basePath == "")
            {
                string dirname;
                if (filename_p)
                {
                    dirname = mPath;
                }
                else
                {
                    dirname = Path.GetDirectoryName(mPath);
                }

                //Uri dirUri = new Uri(dirname);
                //return dirUri.ToString();
                return dirname;
            }
            else
            {
                Uri basepath = new(basePath + Path.DirectorySeparatorChar);

                string dirname;
                if (filename_p)
                {
                    dirname = mPath;
                }
                else
                {
                    dirname = Path.GetDirectoryName(mPath);
                }

                Uri dirUri = new(dirname);
                Uri relUri = basepath.MakeRelativeUri(dirUri);
                return relUri.ToString();
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static bool isSpecifiedDateFile(string filepath, DateTime? from, DateTime? to)
        {
            FileInfo fi = new(filepath);
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
        public static bool isAboveOfMaxFilesizeImage(string filepath, int minFileSize)
        {
            if (minFileSize == 0)
            {
                return true;
            }

            FileInfo fi = new(filepath);
            if (fi.Length >= minFileSize)
            {
                return true;
            }

            return false;
        }
        

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static bool isBelowOfMaxFilesizeImage(string filepath, int maxFileSize)
        {
            if (maxFileSize == 0)
            {
                return true;
            }

            FileInfo fi = new(filepath);
            if (fi.Length <= maxFileSize)
            {
                return true;
            }

            return false;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public bool isSpecifiedPicOrinet(PIC_ORIENT_TYPE orient)
        {
            if (orient == PIC_ORIENT_TYPE.PIC_ORINET_ALL)
            {
                return true;
            }

            using var img = GetImageFromFile();
            switch (orient)
            {
                case PIC_ORIENT_TYPE.PIC_ORINET_PORTRAIT:
                    if (img.Width <= img.Height)
                    {
                        return true;
                    }
                    break;
                case PIC_ORIENT_TYPE.PIC_ORINET_LANDSCAPE:
                    if (img.Width >= img.Height)
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }


        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public bool isSpecifiedSizeImage(Size? mMaxPicSize)
        {
            if (mMaxPicSize.HasValue)
            {
                Size size = mMaxPicSize.Value;
                if (IsZipEntry)
                {
                    return true;
                }
                else
                {
                    using var img = GetImageFromFile();
#if false
                    if (size.Width != 0 && img.Width > size.Width)
                    {
                        return false;
                    }
                    if (size.Height != 0 && img.Height > size.Height)
                    {
                        return false;
                    }
#else
                    if (img == null) return false;
                    var pixel = size.Width * size.Height;
                    if (pixel != 0 && img.Width * img.Height > pixel)
                    {
                        return false;
                    }
#endif
                }
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
                return GetImageFromZipFile();
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
            Image img = null;
            try {
                img = ImageModule.GetImage(this.FilePath);
                ImageSize.Width = img.Width;
                ImageSize.Height = img.Height;
            } catch {
                Log.err($"GetImageFromFile fail");
                img = null;
                ImageSize.Width = 0;
                ImageSize.Height = 0;
            }

            return img;
#endif
        }

        private Image GetImageFromZipFile()
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
                if (img != null)
                {
                    mThumbnail = ImageModule.GetThumbnailImage(img, thumbWidth, thumbHeight);
                }else
                {
                    Log.trc($"GetImage null");
                    mThumbnailFail = true;
                    mThumbnail = null;
                }
                Log.trc(path);
            }
            return mThumbnail;
        }

        public bool HasThumbnailImage()
        {
            if (mThumbnailFail) return true;

            if (mThumbnail == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
