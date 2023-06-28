using System;
using System.Drawing;
using System.IO;
using PictureManagerApp.src.Lib;

namespace PictureManagerApp.src.Model
{
    public class FileItem
    {
        //=====================================================================
        // private field
        //=====================================================================
        private readonly string mPath;
        private int mGroupNo;
        private Image mImage;
        private Image mThumbnail;


        public bool Mark { set; get; }
        public bool Removed { set; get; }

        //=====================================================================
        // public
        //=====================================================================
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public FileItem(string path, int groupNo = 0)
        {
            mPath = path;
            mGroupNo = groupNo;
        }

        public FileItem(FileItem org)
        {
            this.mPath = org.mPath;
            this.mGroupNo = org.mGroupNo;
            this.Mark = org.Mark;
        }

        public string Path
        {
            get { return mPath; }
        }

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

        public Image GetImage()
        {
            if (mImage == null)
            {
                string path = Path;
                mImage = ImageModule.GetImage(path);
                Log.trc(path);
            }
            return mImage;
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
                string path = Path;
                mThumbnail = ImageModule.GetImage(path, thumbWidth, thumbHeight);
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

        //=====================================================================
        // private method
        //=====================================================================
    }
}
