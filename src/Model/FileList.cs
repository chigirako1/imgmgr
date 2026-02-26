using PictureManagerApp.src.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace PictureManagerApp.src.Model
{
    public class FileList : IEnumerable, IEnumerator
    {
        public enum SORT_TYPE
        {
            SORT_PATH,
            SORT_PATH2,
            SORT_FILENAME,
            SORT_FILESIZE,
            SORT_IMAGESIZE,
            SORT_LAST_WRITE_TIME,
            SORT_NUM_PIXEL,
            SORT_ASPECT_RATIO,
            SORT_FILE_HASH,
            SORT_TITLE,
            SORT_ARTWORK_ID,

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

        public int DstStrCount()
        {
            return mFileList.Count(x => x.DestinationStr != "");
        }

        public int MarkCount()
        {
            return mFileList.Count(x => x.Mark);
        }

        internal void UnmarkAll()
        {
            foreach (var file in mFileList)
            {
                file.Mark = false;
            }
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
                    System.Diagnostics.Debug.WriteLine("");
                    Log.log($"重複ファイル #{FileHashCnt.Count(pair => pair.Value >= 2)}='{fitem.FilePath}'({fitem.FileSizeDisp})");
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

        public int RemoveAllSelectedFiles()
        {
            return mFileList.RemoveAll(p => p.Mark);
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
                case SORT_TYPE.SORT_FILENAME:
                    comp = new FileItemNameComparer();
                    break;
                case SORT_TYPE.SORT_TITLE:
                    comp = new FileItemTitleComparer();
                    break;
                case SORT_TYPE.SORT_PATH:
                    comp = new FileItemFilePathNatComparer();
                    break;
                case SORT_TYPE.SORT_ARTWORK_ID:
                    comp = new FileItemIdComparer();
                    break;
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

        public int Batch(string rootpath)
        {
            var fail_list = new List<string>();
            int cnt = 1;
            foreach (var fitem in mFileList)
            {
                if (fitem.Mark)
                {
                    //var tweetinfo = Twt.GetTweetInfoFromPath(fitem.FilePath);
                    //Log.log($"@{tweetinfo.ScreenName}/{tweetinfo.TweetID}-{tweetinfo.ImageNo}");

                    Log.log($"{cnt}:");
                    var rmv_sccess = MyFiles.moveToTrashDir(fitem.FilePath, rootpath);
                    if (rmv_sccess)
                    {
                        fitem.Removed = true;
                        cnt++;
                    }
                    else
                    {
                        fail_list.Add(fitem.FilePath);
                    }
                }
                else if (fitem.DestinationStr != "")
                {
                    var rmv_sccess = MyFiles.moveToTrashDir(fitem.FilePath, rootpath, fitem.DestinationStr);
                }
            }
            var rmv_cnt = mFileList.RemoveAll(p => p.Removed);
            Log.log($"移動したファイルの数:#{cnt - 1}/{rmv_cnt}");

            foreach (var filename in fail_list)
            {
                Log.err($"移動失敗:'{filename}'");
            }

            return rmv_cnt;
        }

        public void RemoveMiddleFiles()
        {
            //mFileList
            foreach (var fitem in mFileList)
            {

            }
        }

        public void Swap(int idx1, int idx2)
        {
            (mFileList[idx1], mFileList[idx2]) = (mFileList[idx2], mFileList[idx1]);
        }

        public void multipleFileOnly(bool update_db)
        {
            mFileList.RemoveAll(IsSingle);

            Sort(SORT_TYPE.SORT_FILE_HASH);

            var hash_save = "";
            var filesize = 0L;
            var tweet_id_save = 0L;
            var parent_dir_save = "";
            foreach (var fitem in mFileList)
            {
                var hash = fitem.GetFileHash();
                if (hash == hash_save)
                {
                    if (filesize == fitem.FileSize)
                    {
                        if (!update_db && fitem.DirectoryName == parent_dir_save)
                        {
                            //pxvで同一ディレクトリの場合はマークしない

                            //TODO: 先の方のファイルをマーク。例"01.jpg"と"05.jpg"なら01をマーク
                        }
                        else
                        {
                            fitem.Mark = true;
                        }

                        if (update_db)
                        {
                            var ti = Twt.GetTweetInfoFromPath(fitem.FilePath);
                            if (ti.TweetID == tweet_id_save)
                            {
                                //同一のtweet idの場合は単純なDLミスなのでDB登録しない
                                // do nothing
                                Log.log($"単なる重複DL:'{fitem.FilePath}'/{ti.TweetID}/@{ti.ScreenName}");
                            }
                            else if (ti.TweetID != 0)
                            {
                                //DB登録
                                Sqlite.UpdateTweetsTable(ti.TweetID, ti.ScreenName);
                                Log.log($"{ti.TweetID}/@{ti.ScreenName}/'{fitem.FilePath}'");
                            }
                            else
                            {
                                //TODO:tweet id取得できてない場合の対応（特殊なファイル名）
                                Log.trc($"???");
                            }
                            tweet_id_save = ti.TweetID;
                        }
                    }
                    else
                    {
                        Log.warning($"同じハッシュ値ですが、ファイルサイズが異なります。({filesize}:{fitem.FileSize})'{fitem.FilePath}'");
                        //Environment.Exit(-1);
                        throw new InvalidOperationException();
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
                parent_dir_save = fitem.DirectoryName;
            }
        }
        
        public void MarkNoSameHashValue()
        {
            Dictionary<string, List<FileItem>> fs_dic = [];

            foreach (var fitem in mFileList)
            {
                var key = fitem.GetFileHash();
                if (fs_dic.ContainsKey(key))
                {
                }
                else
                {
                    fs_dic[key] = new List<FileItem>();
                }
                fs_dic[key].Add(fitem);
            }

            fs_dic = fs_dic.Where(pair => pair.Value.Count <= 1)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var x in fs_dic)
            {
                x.Value[0].Mark = true;
            }
        }

        public void MarkNoSameFileSize()
        {
            Dictionary<long, List<FileItem>> fs_dic = [];

            foreach (var fitem in mFileList)
            {
                var key = fitem.FileSize;
                if (fs_dic.ContainsKey(key))
                {
                }
                else
                {
                    fs_dic[key] = new List<FileItem>();
                }
                fs_dic[key].Add(fitem);
            }

            fs_dic = fs_dic.Where(pair => pair.Value.Count <= 1)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var x in fs_dic)
            {
                x.Value[0].Mark = true;
            }
        }

        public void MarkSameHashFiles()
        {
            FileItem fitem_save = null;
            var hash_save = "";
            var parent_dir_save = "";
            foreach (var fitem in mFileList)
            {
                var hash = fitem.GetFileHash();
                if (hash == hash_save)
                {
                    //fitem_save.Mark = true;
                    fitem.Mark = true;
                }
                else
                {
                }
                fitem_save = fitem;
                hash_save = hash;
                parent_dir_save = fitem.DirectoryName;
            }
        }

        private bool IsSingle(FileItem fitem)
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
            var slct_min = long.MaxValue;
            var slct_max = 0L;
            var not_slct_min = long.MaxValue;
            var not_slct_max = 0L;

            foreach (var fitem in mFileList)
            {
                if (fitem.FileSize == 0)
                {
                    Log.log("filesize=0");
                    throw new InvalidOperationException();
                }

                total += fitem.FileSize;
                if (fitem.Mark)
                {
                    if (fitem.FileSize > slct_max)
                    {
                        slct_max = fitem.FileSize;
                    }

                    if (fitem.FileSize < slct_min)
                    {
                        slct_min = fitem.FileSize;
                    }
                    slct += fitem.FileSize;
                }
                else
                {
                    if (fitem.FileSize > not_slct_max)
                    {
                        not_slct_max = fitem.FileSize;
                    }

                    if (fitem.FileSize < not_slct_min)
                    {
                        not_slct_min = fitem.FileSize;
                    }
                    not_slct += fitem.FileSize;
                }
            }

            long tmp;
            if  (slct > not_slct)
            {
                tmp = slct * 100 / not_slct;
            }
            else
            {
                tmp = not_slct * 100 / slct;
            }
            tmp = not_slct * 100 / slct;

            var avg_slct = slct / MarkCount();
            var avg_non_slct = not_slct / (mFileList.Count - MarkCount());
            var texts = new List<string>();
            texts.Add($"total({mFileList.Count})={MyFiles.FormatFileSize(total)}");
            texts.Add($"選択中({MarkCount()})={MyFiles.FormatFileSize(slct)}({slct * 100 / total}%)" +
                $"{MyFiles.FormatFileSize(avg_slct)}" +
                $"({MyFiles.FormatFileSize(slct_max)}" +
                $"/{MyFiles.FormatFileSize(slct_min)})" 
                );
            texts.Add($"非選択({mFileList.Count - MarkCount()})={MyFiles.FormatFileSize(not_slct)}({not_slct * 100 / total}%)" +
                $"{MyFiles.FormatFileSize(avg_non_slct)}" +
                $"({MyFiles.FormatFileSize(not_slct_max)}" +
                $"/{MyFiles.FormatFileSize(not_slct_min)})"
                );
            //texts.Add($"{tmp}%");
            texts.Add($"{MyFiles.FormatFileSize(not_slct)}(非選択中) / {MyFiles.FormatFileSize(slct)}(選択中) = {tmp}%");

            var strb = new StringBuilder();
            foreach (var text in texts)
            {
                strb.AppendLine(text);
            }
            var str = strb.ToString();
            Log.log(str);

            return str;
        }

#if false
        public void WriteStatTsv(string ofilepath)
        {
            List<string> list = new ();
            var fs_a = 0L;
            var fs_b = 0L;

            //TODO: ファイルが開かれているときなど例外の対応
            using (var sw = new StreamWriter(ofilepath, false))
            {
                //見出し行
                sw.WriteLine("a");

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
                        fs_a = fitem.FileSize;
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            list.Add($"{fs_b * 100 / fs_a}");
                            list.Add($"{fs_a - fs_b}");

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
                        fs_b = fitem.FileSize;
                    }
                }

                if (list.Count > 0)
                {
                    list.Add($"{fs_b * 100 / fs_a}");
                    var line = string.Join("\t", list);
                    sw.WriteLine(line);

                    list.Clear();
                }
            }
        }
#endif
        private void WriteStatTsv_AddList(List<string> list, FileItem fitem)
        {
            var filename = Path.GetFileName(fitem.FilePath);
            var filename_wo_ext = Path.GetFileNameWithoutExtension(fitem.FilePath);
            list.Add(fitem.FilePath);
            //list.Add(filename);
            //list.Add(filename_wo_ext);
            list.Add(fitem.FileSize.ToString());
            list.Add(fitem.Mark.ToString());
        }

        private void WriteStatTsv_WriteLine(List<string> list, StreamWriter sw, long fs_a, long fs_b)
        {
            list.Add($"{fs_b * 100 / fs_a}");
            list.Add($"{fs_a - fs_b}");

            var line = string.Join("\t", list);
            sw.WriteLine(line);

            list.Clear();
        }

        public void WriteStatTsv(string ofilepath)
        {
            var list = new List<string>();

            //TODO: ファイルが開かれているときなど例外の対応
            using (var sw = new StreamWriter(ofilepath, false))
            {
                //見出し行
                var line = string.Join("\t", ["a","b","c","d","e","f", "g", "h"]);
                sw.WriteLine(line);

                var i = 0;
                var fs_a = 0L;
                var fs_b = 0L;

                foreach (var fitem in mFileList)
                {
                    WriteStatTsv_AddList(list, fitem);
                    if (i == 0)
                    {
                        fs_b = fitem.FileSize;
                    }
                    else
                    {
                        fs_a = fitem.FileSize;
                    }
                    i++;

                    if (i > 1)
                    {
                        WriteStatTsv_WriteLine(list, sw, fs_a, fs_b);
                        i = 0;
                    }
                }

                if (list.Count > 0)
                {
                    WriteStatTsv_WriteLine(list, sw, fs_a, fs_b);
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

    public sealed class FileItemFilePathNatComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var d1 = Path.GetDirectoryName(a.FilePath) ?? "";
            var d2 = Path.GetDirectoryName(b.FilePath) ?? "";
            var dc = String.Compare(d1, d2);
            if (dc != 0)
            {
                return dc;
            }

            var f1 = Path.GetFileName(a.FilePath) ?? "";
            var f2 = Path.GetFileName(b.FilePath) ?? "";
            return String.Compare(f1, f2);
        }
    }

    public sealed class FileItemNameComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var f_a = a.GetFilename() + a.FilePath;
            var f_b = b.GetFilename() + b.FilePath;
            return String.Compare(f_a, f_b);
        }
    }

    public sealed class FileItemTitleComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var title_a = a.GetTitle() + a.FilePath;
            var title_b = b.GetTitle() + b.FilePath;
            return String.Compare(title_a, title_b);
        }
    }

    public sealed class FileItemIdComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var id_a = a.GetArtworkID();
            var id_b = b.GetArtworkID();
            var result = id_a - id_b;
            if (result != 0)
            {
                return result;
            }

            var p_a = a.FilePath;
            var p_b = b.FilePath;
            return String.Compare(p_a, p_b);
        }
    }

    public sealed class FileItemArtworkIdComparer : IComparer<FileItem>
    {
        public int Compare(FileItem a, FileItem b)
        {
            var id_a = a.GetArtworkID();
            var id_b = b.GetArtworkID();
            if (id_a > id_b)
            {
                return 1;
            }
            else if (id_a < id_b)
            {
                return -1;
            }
            else
            {
                return String.Compare(a.FilePath, b.FilePath);
            }
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
                //return 0;
                return String.Compare(a.FilePath, b.FilePath);
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
                //return 0;
                return String.Compare(a.FilePath, b.FilePath);
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
                //return 0;
                return String.Compare(a.FilePath, b.FilePath);
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
