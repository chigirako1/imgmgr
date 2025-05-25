using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Policy;
using System.Xml.Linq;
using PictureManagerApp.src.Lib;

namespace PictureManagerApp.src.Model
{
    public class FileItem
    {
        //=====================================================================
        // private field
        //=====================================================================
        protected readonly string mPath;
        protected string mZipPath;
        protected Size ImageSize;
        private bool mThumbnailFail;

        //private Image mImage;
        private Image mThumbnail;

        //private string FileHash = null;//"";
        public string FileHash { private set; get; } = null;

        public bool Mark {
            set; get;
        }
        public bool Removed { set; get; }
        public bool IsZipEntry { private set; get; }
        public bool IsGroupEntry { private set; get; }

        public long FileSize { set; get; }
        public long CompressedLength { set; get; }
        public DateTime LastWriteTime { set; get; }

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
                using (var archive = ZipFile.OpenRead(mZipPath))
                {
                    var ent = archive.GetEntry(FilePath);
                    FileSize = ent.Length;//非圧縮のサイズ
                    CompressedLength = ent.CompressedLength;
                    LastWriteTime = ent.LastWriteTime.LocalDateTime; ;
                }
            }
            else
            {
                System.IO.FileInfo fi = new(path);

                if (((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory))
                {
                    IsGroupEntry = true;
                }
                else
                {
                    FileSize = fi.Length;
                }
                LastWriteTime = fi.LastWriteTime;
            }
            //FileHash = "";
        }

        public FileItem(string path, long filesize, int width, int height, string hash)
        {
            this.mPath = path;
            this.mZipPath = "";

            System.IO.FileInfo fi = new(path);
            if (filesize != fi.Length)
            {
                throw new ArgumentException();
            }
            this.FileSize = fi.Length;
            this.LastWriteTime = fi.LastWriteTime;

            this.ImageSize.Width = width;
            this.ImageSize.Height = height;
            this.FileHash = hash;
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

        public virtual string GetZipEntryname()
        {
            /*if (mZipPath != "")
            {
                return this.FilePath;
            }*/
            return null;
        }

        public string GetTxtPath()
        {
            return mZipPath + "\t" + mPath;
        }

        virtual public string GetFilename()
        {
            /*if (mZipPath != "")
            {
                return Path.GetFileName(mZipPath);
            }
            else*/
            {
                return Path.GetFileName(mPath);
            }
        }

        public string GetRelativePath(string basePath, bool filename_p = false)
        {
            if (mZipPath != "")
            {
                return mZipPath;
            }

            var hoge = false;
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
            else if (hoge)
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
                var result = relUri.ToString();

                return result;
            }
            else
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
                var relativePath = Path.GetRelativePath(basePath, dirname);
                return relativePath;
            }
        }

        public bool IsFileSizeBigger()
        {
            return FileSize > 1024 * 1024;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public static bool isSpecifiedDateFile(string filepath, DateTime? from, DateTime? to)
        {
            System.IO.FileInfo fi = new(filepath);
            return isSpecifiedDateFile(fi, from, to);
        }

        public static bool isSpecifiedDateFile(System.IO.FileInfo fi, DateTime? from, DateTime? to)
        {
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

            System.IO.FileInfo fi = new(filepath);
            return isAboveOfMaxFilesizeImage(fi, minFileSize);
        }

        public static bool isAboveOfMaxFilesizeImage(System.IO.FileInfo fi, int minFileSize)
        {
            if (minFileSize == 0)
            {
                return true;
            }

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

            System.IO.FileInfo fi = new(filepath);
            return isBelowOfMaxFilesizeImage(fi, maxFileSize);
        }

        public static bool isBelowOfMaxFilesizeImage(System.IO.FileInfo fi, int maxFileSize)
        {
            if (maxFileSize == 0)
            {
                return true;
            }

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


            int img_w, img_h;
            if (ImageSize.Width == 0 || ImageSize.Height == 0)
            {
                using var img = GetImage();
                img_w = img.Width;
                img_h = img.Height;
            }
            else
            {
                img_w = ImageSize.Width;
                img_h = ImageSize.Height;
            }

            switch (orient)
            {
                case PIC_ORIENT_TYPE.PIC_ORINET_PORTRAIT:
                    if (img_w <= img_h)
                    {
                        return true;
                    }
                    break;
                case PIC_ORIENT_TYPE.PIC_ORINET_LANDSCAPE:
                    if (img_w >= img_h)
                    {
                        return true;
                    }
                    break;
                case PIC_ORIENT_TYPE.PIC_ORINET_LANDSCAPE_ONLY:
                    if (img_w > img_h)
                    {
                        return true;
                    }
                    break;
                case PIC_ORIENT_TYPE.PIC_ORINET_SQUARE:
                    //if (img_w == img_h)
                    var n = Math.Abs(img_w - img_h);
                    if (n < 100)
                    {
                        return true;
                    }
                    break;
                case PIC_ORIENT_TYPE.PIC_ORINET_LONG:
                    float rat = img_w * 100 / img_h;
                    //まあアスペクト比ソートでいいかも
                    if (rat > 200)//TODO:
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
                    if (ImageSize.Width == 0 && ImageSize.Height == 0)
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
                    else
                    {
                        var pixel = size.Width * size.Height;
                        if (pixel != 0 && ImageSize.Width * ImageSize.Height > pixel)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public virtual Image GetImage()
        {
            if (mZipPath == "")
            {
                //if (IsGroupEntry)
                {
                    //return GetImage();
                }
                //else
                {
                    return GetImageFromFile();
                }
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
                Log.err($"GetImageFromFile fail. '{this.FilePath}'");
                img = null;
                ImageSize.Width = 0;
                ImageSize.Height = 0;
            }

            return img;
#endif
        }

        private Image GetImageFromZipFile()
        {
            var img = MyFiles.GetImageFromZipFile(mZipPath, FilePath);
            ImageSize.Width = img.Width;
            ImageSize.Height = img.Height;
            return img;
        }

        public Size GetImageSize()
        {
            return ImageSize;
        }
                
        public float GetAspectRatio()
        {
            //return (float)ImageSize.Height / (float)ImageSize.Width;
            return ImageModule.GetAspectRatio(ImageSize.Width, ImageSize.Height);
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
                    //Log.trc($"GetImage null");
                    mThumbnailFail = true;
                    mThumbnail = null;
                }
                //Log.trc(path);
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

        public string GetFileHash()
        {
            if (FileHash != null)
            {
                return FileHash;
            }

            if (mZipPath == "")
            {
                FileHash = MyFiles.ComputeFileHash(mPath);
                return FileHash;
            }

            return null;
        }

        public string GetPicInfoLine()
        {
            string line = $"{FilePath}\t{FileSize}\t{ImageSize.Width}\t{ImageSize.Height}\t{FileHash}";

            return line;
        }
    }
}
