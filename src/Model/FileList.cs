using PictureManagerApp.src.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureManagerApp.src.Model
{
    public class FileList
    {
        private List<FileItem> mFileList;

        public FileList()
        {
            mFileList = new List<FileItem>();
        }

        public void Add(FileItem fitem)
        {
            mFileList.Add(fitem);
        }

        public void Remove(FileItem fitem)
        {
            mFileList.Remove(fitem);
        }

        public FileItem this[int index]
        {
            set { mFileList[index] = value; }
            get { return mFileList[index]; }
        }

        public int Count
        {
            get { return mFileList.Count; }
        }

        public bool Contains(FileItem fitem)
        {
            return mFileList.Contains(fitem);
        }

        public void RemoveAt(int index)
        {
            mFileList.RemoveAt(index);
        }

        public void Batch(string rootpath)
        {
            foreach (var fitem in mFileList)
            {
                if (fitem.MarkRemove)
                {
                    MyFiles.moveToTrashDir(fitem.Path, rootpath);
                }
            }
        }
    }
}
