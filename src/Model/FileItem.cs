using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool MarkRemove { set; get; }

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
            }
            return mImage;
        }

        //=====================================================================
        // private method
        //=====================================================================
    }
}
