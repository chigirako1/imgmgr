using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PictureManagerApp.src.Lib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace PictureManagerApp.src.Model
{
    public class FileList : IEnumerable, IEnumerator
    {

        public enum SORT_TYPE
        {
            SORT_PATH,
            SORT_FILESIZE,
            SORT_IMAGESIZE,
            SORT_LAST_WRITE_TIME,
            SORT_NUM_PIXEL,

            SORT_MAX
        }

        //=====================================================================
        // 
        //=====================================================================
        private List<FileItem> mFileList;
        public bool ZipList { set; get; }

        //=====================================================================
        // 
        //=====================================================================
        public FileList()
        {
            mFileList = [];
        }

        public FileItem this[int index]
        {
            //set { mFileList[index] = value; }
            get { return mFileList[index]; }
        }

        public int Count
        {
            get { return mFileList.Count; }
        }

        public void Add(FileItem fitem)
        {
            mFileList.Add(fitem);
        }

        public void Remove(FileItem fitem)
        {
            mFileList.Remove(fitem);
        }

        public void RemoveAt(int index)
        {
            mFileList.RemoveAt(index);
        }

        public bool Contains(FileItem fitem)
        {
            return mFileList.Contains(fitem);
        }

        public void Sort(SORT_TYPE sort_type)
        {
            IComparer<FileItem> comp;

            switch (sort_type)
            {
                case SORT_TYPE.SORT_FILESIZE:
                    comp = new FileItemFileSizeComparer();
                    break;
                case SORT_TYPE.SORT_NUM_PIXEL:
                    comp = new FileItemNumPixelComparer();
                    break;
                case SORT_TYPE.SORT_PATH:
                default:
                    comp = new FileItemFilePathComparer();
                    break;
            }
            mFileList.Sort(comp);
        }

        public FileList DupSel()
        {
            FileList flist = [];
            foreach (FileItem item in mFileList)
            {
                if (item.Mark)
                {
                    var fitem = new FileItem(item);
                    fitem.Mark = false;
                    flist.Add(fitem);
                }
            }
            return flist;
        }

        public void Batch(string rootpath)
        {
            int cnt = 0;
            foreach (var fitem in mFileList)
            {
                if (fitem.Mark)
                {
                    MyFiles.moveToTrashDir(fitem.FilePath, rootpath);
                    fitem.Removed = true;
                    cnt++;
                }
            }
            mFileList.RemoveAll(p => p.Removed);
            Log.log($"ctn={cnt}");
        }

        public void Swap(int idx1, int idx2)
        {
            (mFileList[idx1], mFileList[idx2]) = (mFileList[idx2], mFileList[idx1]);
        }


        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private int CurrPos = -1;
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        bool IEnumerator.MoveNext()
        {
            if (CurrPos < mFileList.Count - 1)
            {
                CurrPos++;
                return true;
            }
            return false;
        }

        void IEnumerator.Reset()
        {
            CurrPos = -1;
        }

        object IEnumerator.Current
        {
            get { return mFileList[CurrPos]; }
        }
    }

    public sealed class FileItemFilePathComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            return String.Compare(a.FilePath, b.FilePath);
        }
    }

    public sealed class FileItemFileSizeComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            if (a.FileSize > b.FileSize)
            {
                return 1;
            }
            else if (a.FileSize < b.FileSize)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
    public sealed class FileItemNumPixelComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var asize = a.GetImageSize();
            var bsize = b.GetImageSize();
            var apixel = asize.Height * asize.Width;
            var bpixel = bsize.Height * bsize.Width;
            if (apixel > bpixel)
            {
                return 1;
            }
            else if (apixel < bpixel)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
