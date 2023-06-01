using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

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
        private FileList mFileList = new FileList();

        private int mIdx = -1;
        private DateTime? mDtFrom;
        private DateTime? mDtTo;

        public int UpDownCount { set; get; }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public PictureModel()
        {
            Log.trc("[S]");
            Log.trc("[E]");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void setDate(DateTime? dtFrom, DateTime? dtTo)
        {
            mDtFrom = dtFrom;
            mDtTo = dtTo;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void buildFileList(string path)
        {
            Log.log($"[S]:path={path}");

            mPath = path;

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

            mIdx = 0;
            Log.trc("[E]");
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
                mIdx = mIdx % UpDownCount;
            }
            Update();
            Log.trc($"[E]{mIdx}");
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
                default:
                    break;
            }
            Update();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public FileItem GetCurrentFileItem()
        {
            return mFileList[mIdx];
        }

        public string GetCurrentFilePath() => mFileList[mIdx].Path;

        public string GetPictureInfoText()
        {
            string txt;

            FileItem fi = mFileList[mIdx];
            txt = String.Format("[{0,3}/{1}] {2}", mIdx + 1, mFileList.Count, fi.Path);

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

        public Image GetImageByRelativeNo(int relativeNo)
        {
            var fitem = GetCurrentFileItemByRelativeIndex(relativeNo);
            return fitem.GetImage();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Batch()
        {
            mFileList.Batch(mPath);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void toggleRemoveMark()
        {
            mFileList[mIdx].MarkRemove = !mFileList[mIdx].MarkRemove;
        }

        public void RemoveCurrentFile()
        {
            string path = mFileList[mIdx].Path;

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
