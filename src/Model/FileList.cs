using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        // クラスメソッド
        //=====================================================================
        public static IEnumerable<string> EnumerateFiles(string path)
        {
            var fileArray = Directory.GetFiles(
                path,
                "*.*",
                SearchOption.TopDirectoryOnly);

            string[] patterns = { ".zip", ".tsv" };
            var files = fileArray.Where(file => patterns.Any(pattern => file.ToLower().EndsWith(pattern)));
            return files;
        }

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

        public int CountIfHasThumbnail()
        {
            return mFileList.Count(x => x.HasThumbnailImage());
        }

        public int MarkCount()
        {
            return mFileList.Count(x => x.Mark);
        }

        public int GetDupCount()
        {
            return FileHashCnt.Count(pair => pair.Value >= 2);
        }

        public IEnumerable<FileItem> FileItems
        {
            get { return mFileList; }
        }

        public void Add(FileItem fitem, bool computeHash = false)
        {
            mFileList.Add(fitem);

            if (computeHash)
            {
                string hash = fitem.GetFileHash();//TODO:ファイルサイズチェックとかバイナリ比較まではしない？
                if (FileHashCnt.ContainsKey(hash))
                {
                    FileHashCnt[hash]++;
                    Log.log($"重複ファイル #{FileHashCnt.Count(pair => pair.Value >= 2)}='{fitem.FilePath}'");
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
                    var tweetinfo = Twt.GetTweetInfoFromPath(fitem.FilePath);
                    Log.log($"@{tweetinfo.ScreenName}/{tweetinfo.TweetID}-{tweetinfo.ImageNo}");

                    MyFiles.moveToTrashDir(fitem.FilePath, rootpath);
                    fitem.Removed = true;
                    cnt++;
                }
            }
            mFileList.RemoveAll(p => p.Removed);
            Log.log($"cnt={cnt}");
        }

        public void Swap(int idx1, int idx2)
        {
            (mFileList[idx1], mFileList[idx2]) = (mFileList[idx2], mFileList[idx1]);
        }

        public void multipleFileOnly(bool update_db = false)
        {
            mFileList.RemoveAll(IsSingle);

            Sort(SORT_TYPE.SORT_FILE_HASH);

            var hash_save = "";
            var filesize = 0L;
            var tweet_id_save = 0L;
            foreach (var fitem in mFileList)
            {
                var hash = fitem.GetFileHash();
                if (hash == hash_save)
                {
                    if (filesize == fitem.FileSize)
                    {
                        fitem.Mark = true;

                        if (update_db)
                        {
                            var ti = Twt.GetTweetInfoFromPath(fitem.FilePath);
                            if (ti.TweetID == tweet_id_save)
                            {
                                //同一のtweet idの場合は単純なDLミスなのでDB登録しない
                                // do nothing
                                Log.log($"単なる重複DL'{fitem.FilePath}'/{ti.TweetID}/@{ti.ScreenName}");
                            }
                            else if (ti.TweetID != 0)
                            {
                                //DB登録
                                Sqlite.UpdateTweetsTable(ti.TweetID, ti.ScreenName);
                                Log.log($"{ti.TweetID}/@{ti.ScreenName}/'{fitem.FilePath}'");
                            }
                            else
                            {
                                Log.trc($"???");
                            }
                            tweet_id_save = ti.TweetID;
                        }
                    }
                    else
                    {
                        Log.warning($"同じハッシュ値ですが、ファイルサイズが異なります。({filesize}:{fitem.FileSize})'{fitem.FilePath}'");
                    }
                }
                else
                {
                    if (update_db)
                    {
                        var ti = Twt.GetTweetInfoFromPath(fitem.FilePath);
                        tweet_id_save = ti.TweetID;
                    }
                }
                hash_save = hash;
                filesize = fitem.FileSize;
            }
        }

        private  bool IsSingle(FileItem fitem)
        {
            if (fitem.IsGroupEntry)
            {
                return true;
            }

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

        public void SearchAndMark(string search_word)
        {
            foreach (var fitem in mFileList)
            {
                if (fitem.FilePath.Contains(search_word))
                {
                    fitem.Mark = true;
                }
            }
        }

        public string TotalFileSizeOfSelectedFile()
        {
            var total = 0L;
            var slct = 0L;
            var not_slct = 0L;
            foreach (var fitem in mFileList)
            {
                total += fitem.FileSize;
                if (fitem.Mark)
                {
                    slct += fitem.FileSize;
                }
                else
                {
                    not_slct += fitem.FileSize;
                }
            }

            string str = $"total({mFileList.Count})=\t{MyFiles.FormatFileSize(total)}" + Environment.NewLine + 
                $"選択中({MarkCount()})=\t{MyFiles.FormatFileSize(slct)}({slct * 100 / total}%)" + Environment.NewLine+
                $"非選択({mFileList.Count - MarkCount()})=\t{MyFiles.FormatFileSize(not_slct)}({not_slct * 100 / total}%)";
            Log.log(str);

            return str;
        }

        public void WriteStatTsv(string ofilepath)
        {
            List<string> list = new ();

            using (var sw = new StreamWriter(ofilepath, false))
            {
                foreach (var fitem in mFileList)
                {
                    if (fitem.Mark)
                    {
                        var filename = Path.GetFileName(fitem.FilePath);
                        var filename_wo_ext = Path.GetFileNameWithoutExtension(fitem.FilePath);
                        list.Add(fitem.FilePath);
                        //list.Add(filename);
                        //list.Add(filename_wo_ext);
                        list.Add(fitem.FileSize.ToString());
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            var line = string.Join("\t", list);
                            sw.WriteLine(line);

                            list.Clear();
                        }

                        var filename = Path.GetFileName(fitem.FilePath);
                        var filename_wo_ext = Path.GetFileNameWithoutExtension(fitem.FilePath);
                        list.Add(fitem.FilePath);
                        //list.Add(filename);
                        //list.Add(filename_wo_ext);
                        list.Add(fitem.FileSize.ToString());
                    }
                }
            }
        }

        public void SavePicsInfo(string ofilepath)
        {
            using (var sw = new StreamWriter(ofilepath, false))
            {
                foreach (FileItem fitem in mFileList)
                {
                    if (fitem.IsGroupEntry)
                    {
                        //ignore
                    }
                    else
                    {
                        var line = fitem.GetPicInfoLine();
                        sw.WriteLine(line);
                    }
                }
            }
        }

        public IEnumerable<string> GetDirs()
        {
            //var dirs = mFileList.Select(x => Path.GetDirectoryName(x.FilePath)).Distinct();

            //もとのデータを変更できるように「ToHashSet」で複製する
            var dirs = mFileList.Select(x => Path.GetDirectoryName(x.FilePath)).ToHashSet();
            return dirs;
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
