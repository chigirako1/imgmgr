using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            SORT_ASPECT_RATIO,
            SORT_FILE_HASH,

            SORT_MAX
        }

        //=====================================================================
        // 
        //=====================================================================
        private List<FileItem> mFileList;
        public bool ZipList { set; get; }
        private readonly Dictionary<string, int> FileHashCnt = [];

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

        public int MarkCount()
        {
            return mFileList.Count(x => x.Mark);
        }

        public void Add(FileItem fitem, bool computeHash = false)
        {
            mFileList.Add(fitem);

            if (computeHash)
            {
                string hash = fitem.GetFileHash();
                if (FileHashCnt.ContainsKey(hash))
                {
                    FileHashCnt[hash]++;
                    Log.log($"重複ファイル{fitem.FilePath}");
                }
                else
                {
                    FileHashCnt.Add(hash, 1);
                }
            }
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
                case SORT_TYPE.SORT_ASPECT_RATIO:
                    comp = new FileItemAspectRatioComparer();
                    break;
                case SORT_TYPE.SORT_FILE_HASH:
                    comp = new FileItemHashComparer();
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
                    var tweetinfo = Twt.GetTweetIdFromPath(fitem.FilePath);
                    Log.log($"@{tweetinfo.ScreenName}/{tweetinfo.TweetID}-{tweetinfo.ImageNo}");

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

        public void multipleFileOnly()
        {
            mFileList.RemoveAll(chkHash);

            Sort(SORT_TYPE.SORT_FILE_HASH);

            string hash_save = "";
            foreach (var fitem in mFileList)
            {
                var hash = fitem.GetFileHash();
                if (hash == hash_save)
                {
                    fitem.Mark = true;
                }
                hash_save = hash;
            }
        }

        private  bool chkHash(FileItem fitem)
        {
            var hashv = fitem.GetFileHash();
            var cnt = FileHashCnt[hashv];
            if (cnt < 2)
            {
                return true;
            }
            return false;
        }

        public void RegisterTweetDB()
        {
            foreach (var fitem in mFileList)
            {
                if (fitem.Mark)
                {
                    //not implemented Sqlite.UpdateTweetsTable();
                    
                    //Log.log($"重複ファイル{fitem.FilePath}");
                }
            }
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

    public sealed class FileItemAspectRatioComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var ratio_a = a.GetAspectRatio();
            var ratio_b = b.GetAspectRatio();
            if (ratio_a > ratio_b)
            {
                return 1;
            }
            else if (ratio_a < ratio_b)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public sealed class FileItemHashComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var x_a = a.GetFileHash() + a.FilePath;
            var x_b = b.GetFileHash() + b.FilePath;

            return x_a.CompareTo(x_b);
        }
    }
}
