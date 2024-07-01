using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using PictureManagerApp.src.Lib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace PictureManagerApp.src.Model
{
    public class FileList : IEnumerable, IEnumerator
    {
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
            foreach (var fitem in mFileList)
            {
                if (fitem.Mark)
                {
                    MyFiles.moveToTrashDir(fitem.FilePath, rootpath);
                    fitem.Removed = true;
                }
            }
            mFileList.RemoveAll(p => p.Removed);
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
}
