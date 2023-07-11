using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using PictureManagerApp.src.Lib;

namespace PictureManagerApp.src.Model
{
    public enum POS_MOVE_TYPE
    {
        MOVE_NONE,
        MOVE_NEXT_FILE,
        MOVE_PREV_FILE,
        MOVE_HOME,
        MOVE_LAST,
        MOVE_NEXT_DIR,
        MOVE_PREV_DIR,
        MOVE_NEXT_ARTIST,
        MOVE_PREV_ARTIST,
        MOVE_NEXT_MARKED_FILE,
        MOVE_PREV_MARKED_FILE,
        MOVE_MAX
    }

    public enum SORT_TYPE
    {
        SORT_PATH,
        SORT_FILESIZE,
        SORT_IMAGESIZE,
        SORT_LAST_WRITE_TIME,
        SORT_MAX
    }

    public class PictureModel
    {
        private string mPath;
        //private SORT_TYPE mSortType;
        private FileList mFileList;

        private int mIdx = -1;
        private DateTime? mDtFrom;
        private DateTime? mDtTo;

        public int mMarkCount { private set; get; }//都度集計したほうが良いのでは？漏れ
        public int UpDownCount { set; get; }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public PictureModel()
        {
            Log.trc("[S]");
            mFileList = new FileList();
            mMarkCount = 0;
            mIdx = 0;
            Log.trc("[E]");
        }

        public PictureModel(FileList flist): this()
        {
            Log.trc("[S]");

            mFileList = flist.DupSel();

            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void SetDate(DateTime? dtFrom, DateTime? dtTo)
        {
            mDtFrom = dtFrom;
            mDtTo = dtTo;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void BuildFileList(string path)
        {
            Log.log($"[S]:path={path}");

            mPath = path;
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                BuildFileList_Dir(path);
            }
            else
            {
                BuildFileList_Zip(path);
            }

            Log.trc("[E]");
        }

        private void BuildFileList_Dir(string path)
        {
            var fileArray = Directory.GetFiles(
                            mPath,
                            "*.jpg",
                            SearchOption.AllDirectories);

            foreach (var f in fileArray.OrderBy(x => x))
            {
                if (FileItem.isSpecifiedFile(f, mDtFrom, mDtTo))
                {
                    FileItem fi = new FileItem(f);
                    mFileList.Add(fi);
                }
            }
        }

        private void BuildFileList_Zip(string path)
        {
            var encode = "utf-8";
            //encode = "sjis";
            //encode = "shift_jis";
            //using (var archive = ZipFile.OpenRead(path))
            using (ZipArchive archive = ZipFile.Open(path,
                ZipArchiveMode.Read,
                System.Text.Encoding.GetEncoding(encode)))
            {
                foreach (var e in archive.Entries)
                {
                    //filelist.Add(e.FullName);
                    //Console.WriteLine("名前       : {0}", e.Name);
                    Console.WriteLine("フルパス   : {0}", e.FullName);
                    //Console.WriteLine("サイズ     : {0}", e.Length);
                    //Console.WriteLine("圧縮サイズ : {0}", e.CompressedLength);
                    //Console.WriteLine("更新日時   : {0}", e.LastWriteTime);
                }

                var files = archive.Entries.OrderBy(e => e.FullName).
                    Where(e =>
                        e.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                );

                List<string> filelist = new List<string>();
                files.ToList().ForEach(f => filelist.Add(f.FullName));

                mFileList.ZipList = true;
                foreach (var f in filelist.OrderBy(x => x))
                {
                    //if (FileItem.isSpecifiedFile(f, mDtFrom, mDtTo))
                    {
                        FileItem fi = new FileItem(f, path);
                        mFileList.Add(fi);
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public PictureModel DuplicateSelectOnly()
        {
            PictureModel newObj = new PictureModel(mFileList);
            newObj.mPath = mPath;
            
            return newObj;
        }

        public bool MakeThumbnail(int thumWidth, int thumHeight)
        {
            if (thumWidth == 0 || thumHeight == 0) return false;

            int idx = mIdx;
            int cnt;
            for (cnt = 0; cnt < mFileList.Count; cnt++)
            {
                FileItem item = mFileList[idx];
                if (item.HasThumbnailImage())
                {
                }
                else
                {
                    Log.log($"#{idx}:no thumbnail.");
                    break;
                }
                idx++;
                if (idx == mFileList.Count)
                {
                    idx = 0;
                }
            }
            if (cnt >= mFileList.Count)
            {
                Log.trc("thumbnail make done");
                return true;
            }

            FileItem fitem = mFileList[idx];
            if (fitem.HasThumbnailImage())
            {
                Log.err($"??? !!! ??? #{idx}.");
            }
            else
            {
                Log.log($"making thumbnail #{idx}.");
                Image thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight, true);
            }

            Log.trc("thumbnail make cont.");
            return false;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Prev()
        {
            if (mIdx == 0)
            {
                mIdx = mFileList.Count;
            }
            mIdx--;

            Update();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Next()
        {
            mIdx++;
            if (mIdx == mFileList.Count)
            {
                mIdx = 0;
            }
            Update();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Up()
        {
            Log.trc($"[S]{mIdx}");
            mIdx -= UpDownCount;
            if (mIdx < 0)
            {
                mIdx += mFileList.Count;
                if (mIdx < 0)
                {
                    mIdx = 0;
                }
            }

            Update();
            Log.trc($"[E]{mIdx}");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Down()
        {
            Log.trc($"[S]{mIdx}");
            mIdx += UpDownCount;
            if (mIdx >= mFileList.Count)
            {
                mIdx = mIdx - mFileList.Count;
                if (mIdx >= mFileList.Count)
                {
                    mIdx = mFileList.Count - 1;
                }
            }
            Update();
            Log.trc($"[E]{mIdx}");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void NextMarkedImage()
        {
            mIdx++;
            if (mIdx == mFileList.Count)
            {
                mIdx = 0;
            }
            Update();
        }
        
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void MovePos(POS_MOVE_TYPE movType)
        {
            switch (movType)
            {
                case POS_MOVE_TYPE.MOVE_NEXT_FILE:
                    Next();
                    break;
                case POS_MOVE_TYPE.MOVE_PREV_FILE:
                    Prev();
                    break;
                case POS_MOVE_TYPE.MOVE_HOME:
                    mIdx = 0;
                    break;
                case POS_MOVE_TYPE.MOVE_LAST:
                    mIdx = mFileList.Count - 1;
                    break;
                case POS_MOVE_TYPE.MOVE_NEXT_DIR:
                    break;
                case POS_MOVE_TYPE.MOVE_PREV_DIR:
                    break;
                case POS_MOVE_TYPE.MOVE_NEXT_ARTIST:
                    break;
                case POS_MOVE_TYPE.MOVE_PREV_ARTIST:
                    break;
                case POS_MOVE_TYPE.MOVE_NEXT_MARKED_FILE:
                    break;
                case POS_MOVE_TYPE.MOVE_PREV_MARKED_FILE:
                    break;
                default:
                    break;
            }
            Update();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void ChangeOrderForward()
        {
            int idx = mIdx;
            int dstIdx = idx - 1;
            if (dstIdx < 0)
            {
                dstIdx = mFileList.Count - 1;
            }
            //Collections.swap(mFileList, idx, dstIdx);
            mFileList.Swap(idx, dstIdx);

            mIdx--;
            if (mIdx < 0)
            { 
                mIdx = mFileList.Count - 1;
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public FileItem GetCurrentFileItem()
        {
            return mFileList[mIdx];
        }

        public string GetCurrentFilePath() => mFileList[mIdx].FilePath;

        public string GetPictureInfoText()
        {
            string txt;

            FileItem fi = mFileList[mIdx];
            var fpath = fi.GetRelativePath(Path);
            txt = String.Format("[{0,3}/{1}] {2}", mIdx + 1, mFileList.Count, fpath);

            return txt;
        }

        public FileItem GetCurrentFileItemByRelativeIndex(int relativeIndex)
        {
            int idx = mIdx + relativeIndex;
            if (idx >= 0)
            {
                idx = idx % mFileList.Count;
            }

            return mFileList[idx];
        }

        /*
        public Image GetImageByRelativeNo(int relativeNo)
        {
            var fitem = GetCurrentFileItemByRelativeIndex(relativeNo);
            return fitem.GetImage();
        }*/

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Batch()
        {
            mFileList.Batch(mPath);
            mMarkCount = 0;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void toggleMark()
        {
            mFileList[mIdx].Mark = !mFileList[mIdx].Mark;
            if (mFileList[mIdx].Mark)
            {
                mMarkCount++;
            }
            else
            {
                mMarkCount--;
            }
        }

        public void RemoveCurrentFile()
        {
            string path = mFileList[mIdx].FilePath;

            MyFiles.moveToTrashDir(path, mPath);

            mFileList.RemoveAt(mIdx);
        }

        public string Path
        {
            get { return mPath; }
        }

        public int PictureNumber
        {
            get { return mIdx; }
        }

        public int PictureTotalNumber
        {
            get { return mFileList.Count; }
        }

        //=====================================================================
        // private method
        //=====================================================================
        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void Update()
        {
            NorifyObservers();
        }

        private void NorifyObservers()
        {
            //observers.ForEach(observer => observer.Update(this));
        }
    }
}
