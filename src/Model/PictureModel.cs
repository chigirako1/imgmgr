using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using PictureManagerApp.src.Lib;

using static System.Windows.Forms.LinkLabel;
using static PictureManagerApp.src.Lib.PicEvalRow;
using static PictureManagerApp.src.Model.FileList;

namespace PictureManagerApp.src.Model
{
    public enum ACT_MODE_TYPE
    {
        ACT_MODE_AUTO,
        ACT_MODE_COMIC,
        ACT_MODE_PXV,
        ACT_MODE_TWT,

        ACT_MODE_MAX
    }

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

    public enum PIC_ORIENT_TYPE
    {
        PIC_ORINET_ALL,
        PIC_ORINET_PORTRAIT,
        PIC_ORINET_LANDSCAPE,
        PIC_ORINET_LANDSCAPE_ONLY,
        PIC_ORINET_SQUARE,
        PIC_ORINET_LONG,
        PIC_ORINET_CUSTOM,

        PIC_ORINET_MAX
    }

    public enum DATA_SOURCE_TYPE
    {
        DATA_SOURCE_UNKNOWN,
        DATA_SOURCE_PXV,
        DATA_SOURCE_TWT,
        DATA_SOURCE_NJE,
        DATA_SOURCE_WEB,
        DATA_SOURCE_MIX,
        DATA_SOURCE_GROUP,

        DATA_SOURCE_MAX
    }

    public enum ACTION_TYPE
    {
        ACTION_DO_NOTHING,

        ACTION_QUIT_CONF,

        ACTION_MOV_NEXT,
        ACTION_MOV_NEXT_CONT_START,
        ACTION_MOV_NEXT_CONT_STOP,
        ACTION_MOV_SLIDESHOW,
        ACTION_MOV_PREV,

        ACTION_MOV_NEXT_DIR,
        ACTION_MOV_PREV_DIR,

        ACTION_ADD_DEL_LIST,
        ACTION_ADD_FAV_LIST,

        ACTION_MAX
    }

    public enum THUMBNAIL_VIEW_TYPE
    {
        THUMBNAIL_VIEW_TILE,        //並べる
        THUMBNAIL_VIEW_MANGA,       //漫画用。２画像並べて表示。左に進む（TBD.進行方向は別にしたほうが良さそう

        THUMBNAIL_VIEW_LIST,        //同一フォルダを横一列に並べる(実装中

        THUMBNAIL_VIEW_OVERVIEW,    //これはなに？

        THUMBNAIL_VIEW_MAX,


        //以下は除外

        THUMBNAIL_VIEW_GROUP,       //(実装中

        //???
        THUMBNAIL_VIEW_NEXT,        //次の画像をでっかく表示
        THUMBNAIL_VIEW_DIRECTORY,   //同一フォルダを一画面に全部


        THUMBNAIL_VIEW_NO_MAIN_SPLIT,

        THUMBNAIL_VIEW_DEFAULT = THUMBNAIL_VIEW_TILE,
    }

    public enum DISP_ROT_TYPE
    {
        DISP_ROT_NONE,
        DISP_ROT_R090,
        DISP_ROT_R180,
        DISP_ROT_R270,

        DISP_ROT_DEFAULT = DISP_ROT_NONE,

        DISP_ROT_MAX
    }

    public enum FILTER_TYPE
    {
        FILTER_NONE,
        FILTER_HASH,

        FILTER_MAX,
    }

    public enum HOGE_TYPE
    {
        HOGE_NONE,
        HOGE_HEAD,
        HOGE_TAIL,
        HOGE_HEAD_AND_TAIL,

        HOGE_MAX
    }

    public class PictureModel
    {
        //定数

        private const int GROUP_FILE_NUM = 300;

        private const string EXT_MEMORY_DIR_PATH = @"work\r18";
        private static readonly string[] CMBBOX_DIR_PATHS = [
            @"D:\download\PxDl-",
            @"D:\download\PxDl",
            @"D:\download\PxDl-0trash",
            @"D:\download\PxDl--0trash",

            @"D:\dl\AnkPixiv\Twitter-",
            @"D:\dl\AnkPixiv\Twitter",
            @"D:\dl\AnkPixiv\Twitter-0trash",
            @"D:\dl\AnkPixiv\Twitter--0trash",

            @"D:\r18\dlPic\pxv",
            @"D:\r18\dlPic\twitter",
#if DEBUG
            @"D:\download\PxDl-\!pic_infos!.tsv",
            @"D:\dl\AnkPixiv\Twitter-\!pic_infos!.tsv",
#endif
        ];

        private static readonly string FILELIST_FILENAME = "!filelist!.txt";
        private static readonly string DEL_LIST_TXT_FILENAME = @"del_list.tsv";
        private static readonly string DEL_LIST_TXT_PATH = @"D:\download\" + DEL_LIST_TXT_FILENAME;
        //private static readonly string DEL_LIST_TXT_FILENAME = @"del_list.tsv";

        private static readonly string PIC_INFOS_FILENAME = @"!pic_infos!.tsv";
        private static readonly string PIC_COMPARE_FILENAME = @"!pic_compare!.tsv";

        private static readonly string STR_METHOD_DEL = "DEL";
        private static readonly string STR_METHOD_FAV = "FAV";


        private string mPath;
        internal string GetPath() => mPath;
        private string mDstRootPath;
        //private SORT_TYPE mSortType;
        private FileList mFileList;
        private DATA_SOURCE_TYPE mDataSrcType;

        private int mIdx = -1;
        private DateTime? mDtFrom;
        private DateTime? mDtTo;
        private FILTER_TYPE FilterType = FILTER_TYPE.FILTER_NONE;

        private Size? mMaxPicSize = null;
        private int? mMaxPixelCnt = null;
        private int mMinFileSize = 0;
        private int mMaxFileSize = 0;
        private PIC_ORIENT_TYPE mTargetPicOrient = PIC_ORIENT_TYPE.PIC_ORINET_ALL;
        private DISP_ROT_TYPE _rot_type = DISP_ROT_TYPE.DISP_ROT_NONE;
        public DISP_ROT_TYPE ROT_TYPE
        {
            set
            {
                _rot_type = value;
            }
            get
            {
                return _rot_type;
            }
        }

        private string mExtList = ".jpg,.jpeg,.png,.gif,.bmp";
        private string mSeachWord = "";

        private string DelListPath;

        private HOGE_TYPE _Hoge = HOGE_TYPE.HOGE_NONE;


        public THUMBNAIL_VIEW_TYPE ThumbViewType { private set; get; }
        public int mMarkCount {
            private set
            {
                //mMarkCount = value;
            }
            get {
                return mFileList.MarkCount();
            }
        }
        public int UpDownCount { set; get; }
        public int PageCount { set; get; }

        //private TsvRowList mRowList;

        internal static long GetPxvIdFromPath(string filename)
        {
            var pxvid = Pxv.GetPxvID(filename);
            if (pxvid == 0)
            {
                var name = Pxv.GuessPxvUserName(filename);
                pxvid = Sqlite.GetPxvArtistIdFromName(name);
                if (pxvid == 0)
                {
                    Log.warning($"PXVIDが不明です:'{filename}'");
                }
                else
                {
                }
            }
            else
            {
            }
            return pxvid;
        }

        internal static TsvRowList MakeTsvRowList()
        {
            return new TsvRowList(DEL_LIST_TXT_PATH);
        }
        
        internal static List<string> GetDirPathList()
        {
            var list = new List<string>();

            var cdir = System.Environment.CurrentDirectory;
            //var dri = System.IO.Path.GetDirectoryName(cdir);
            var root_path = Path.GetPathRoot(cdir);
            var extmem_path = System.IO.Path.Combine(root_path, EXT_MEMORY_DIR_PATH);

            if (Directory.Exists(extmem_path))
            {
                var dirs = Directory.EnumerateDirectories(extmem_path);
                foreach (var dir in dirs.OrderBy(x => x))
                {
                    list.Add(dir);
                }
            }
            else
            {
                foreach (string path in CMBBOX_DIR_PATHS)
                {
                    if (Directory.Exists(path))
                    {
                        list.Add(path);
                    }
#if DEBUG
                    else if (System.IO.File.Exists(path))
                    {
                        list.Add(path);
                    }
#endif
                }
            }

            return list;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public PictureModel()
        {
            Log.trc("[S]");
            mFileList = [];
            mMarkCount = 0;
            mIdx = 0;
            ThumbViewType = THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_DEFAULT;

            //DelListPath = DEL_LIST_TXT_FILENAME;

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
            Log.log($"date from={dtFrom}");
            Log.log($"date to={dtTo}");
            mDtFrom = dtFrom;
            mDtTo = dtTo;
        }

        public void SetFilter(FILTER_TYPE filter)
        {
            Log.log($"filter={filter}");
            FilterType = filter;
        }

        public void SetHoge(HOGE_TYPE hoge)
        {
            this._Hoge = hoge;
            Log.log($"hoge={this._Hoge}");
        }

        public void SetDelListPath(string delListPath)
        {
            DelListPath = delListPath;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void SetMaxPicSize(Size size)
        {
            Log.log($"max pic size={size}");
            mMaxPicSize = size;
        }

        public void SetMaxPixelCount(int pixel)
        {
            Log.log($"max pixel count={pixel}");
            mMaxPixelCnt = pixel;
        }
        

        public void SetMinFileSize(int minfilesize)
        {
            Log.log($"min file size={minfilesize}");
            mMinFileSize = minfilesize;
        }

        public void SetMaxFileSize(int maxfilesize)
        {
            Log.log($"max file size={maxfilesize} bytes");
            mMaxFileSize = maxfilesize;
        }

        public void SetPicOrient(PIC_ORIENT_TYPE targetPicOrient)
        {
            Log.log($"orinet={targetPicOrient}");
            mTargetPicOrient = targetPicOrient;
        }
        
        public void SetExt(string ext)
        {
            mExtList = ext;
        }

        public void SetSeachWord(string word)
        {
            mSeachWord = word;
        }

        public void SetThumbView(THUMBNAIL_VIEW_TYPE thumbViewType)
        {
            ThumbViewType = thumbViewType;
        }

        public void ToggleThumbView()
        {
            ThumbViewType++;
            if (ThumbViewType == THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_MAX)
            {
                ThumbViewType = THUMBNAIL_VIEW_TYPE.THUMBNAIL_VIEW_DEFAULT;
            }
            Log.trc($"ThumbViewType={ThumbViewType}");
        }

        internal void SetDstRootPath(string dstPath)
        {
            mDstRootPath = dstPath;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void BuildFileListFromText(string filelist, string rootPath = "")
        {
            Log.log($"[S] root=#{rootPath}");
            mPath = rootPath;
            var no_file_cnt = 0;

            string[] filelistarray = filelist.Split(
                            Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);
            foreach (var file in filelistarray)
            {
                var pathtmp = "";
                if (mPath == "")
                {
                    pathtmp = file;
                }
                else
                {
                    pathtmp = Path.Combine(rootPath, file);
                }
                if (!File.Exists(pathtmp))
                {
                    Log.warning($"存在しないファイルを無視します #{pathtmp}");
                    no_file_cnt++;
                    continue;
                }
                var fi = new FileItem(pathtmp);
                mFileList.Add(fi);
            }

            Log.trc($"[E] 存在しないファイル={no_file_cnt} files");
        }

        public void SetFileList(FileList filelist)
        {
            Log.log($"[S]");

            mPath = "";
            mFileList = filelist;

            Log.trc("[E]");
        }

        public bool IsZip()
        {
            if (mPath != "" && File.GetAttributes(mPath).HasFlag(FileAttributes.Directory))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool HasInvalidFile()
        {
            foreach (var fi in mFileList.FileItems)
            {
                if (fi.InvalidFile)
                {
                    return true;
                }
            }
            return false;
        }

        private DATA_SOURCE_TYPE GetSourceType(string path)
        {
            DATA_SOURCE_TYPE result;

            if (path.Contains("PxDl") || path.Contains(@"\pxv\") || path.Contains("/pxv/"))
            {
                result = DATA_SOURCE_TYPE.DATA_SOURCE_PXV;
            }
            else if (path.ToLower().Contains("twitter") || path.Contains("/twt/"))
            {
                result = DATA_SOURCE_TYPE.DATA_SOURCE_TWT;
            }
            else
            {
                result = DATA_SOURCE_TYPE.DATA_SOURCE_UNKNOWN;
            }

            return result;
        }

        /*
         * フォルダ内のファイルをファイル名でソートしたときの最初と最後のファイルのみを残す
         */
        public void RemoveMiddleFiles()
        {
            mFileList.RemoveMiddleFiles();
        }

        public void BuildFileList(string path)
        {
            Log.log($"[S]:path={path}");

            mPath = path;
            mDataSrcType = GetSourceType(path);

            if (MyFiles.IsDirectory(path))
            {
                BuildFileList_Directory();
            }
            else if (MyFiles.IsExt(path, ".zip"))
            {
                BuildFileList_Zip();
            }
            else if (MyFiles.IsExt(path, ".tsv"))
            {
                BuildFileList_Tsv(path);
            }
            else
            {
                Log.err($"'{path}'不正");
            }

            Log.trc("[E]");
        }

        public void BuildFileList_Directory()
        {
            var files = MyFiles.GetFileList(mPath, mExtList, mSeachWord);

            if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_PXV && this._Hoge != HOGE_TYPE.HOGE_NONE)
            {
                var hoge = 0;
                switch (this._Hoge)
                {
                    case HOGE_TYPE.HOGE_HEAD:
                        //hoge = -1;//前を残す,未実装
                        break;
                    case HOGE_TYPE.HOGE_TAIL:
                        hoge = 1;//後ろを残す
                        break;
                    case HOGE_TYPE.HOGE_HEAD_AND_TAIL:
                        hoge = 0; //前と後ろを残す
                        break;
                    case HOGE_TYPE.HOGE_NONE:
                    default:
                        break;
                }
                files = MyFiles.RemoveSpecFiles(files, hoge);
            }

            var add_group = false;
            BuildFileList_Dir(files, add_group);

            if (FilterType == FILTER_TYPE.FILTER_HASH)
            {
                var ofilepath = Path.Combine(mPath, PIC_INFOS_FILENAME);
                mFileList.SavePicsInfo(ofilepath);

                bool update_db;
                if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_TWT)
                {
                    update_db = true;
                }
                else
                {
                    update_db = false;
                }

                mFileList.multipleFileOnly(update_db);
                mMarkCount = mFileList.MarkCount();
            }
        }

        public void BuildFileList_Tsv(string path)
        {
            mPath = Path.GetDirectoryName(path);
            //mDataSrcType = GetSourceType(path);

            BuildFileList_Tsv_Core(path);

            if (FilterType == FILTER_TYPE.FILTER_HASH)
            {
                bool update_db;
                if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_TWT)
                {
                    update_db = true;
                }
                else
                {
                    update_db = false;
                }

                mFileList.multipleFileOnly(update_db);
                mMarkCount = mFileList.MarkCount();
            }
        }

        private void BuildFileList_Dir(IEnumerable<string> files, bool add_group)
        {
            var total = files.Count();
            Log.log($"ファイル数={total}");
            var cnt = 0;

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            foreach (var f in files.OrderBy(x => x))
            {
                if (SpecFileP(f))
                {
                    var fi = new FileItem(f);
                    if (IsTargetPic(fi))
                    {
                        //Log.log($"対象ファイル={f}");
                        bool computeHash;
                        if (FilterType == FILTER_TYPE.FILTER_HASH)
                        {
                            computeHash = true;

                            var img = fi.GetImage();
                            if (img == null)
                            {

                            }
                            else
                            {
                                //TODO: ついでに画像サイズも設定しておく？

                            }
                        }
                        else
                        {
                            computeHash = false;
                        }
                        mFileList.Add(fi, computeHash);

                        //関連するファイルの追加
                        if (mSeachWord != "")
                        {
                            AddRelativeFile(f);
                        }
                    }
                    else
                    {
                        //Log.log($"非対象ファイル={f}");
                    }
                }

                cnt++;
                if (cnt % 1000 == 0)
                {
                    var dupCnt = mFileList.GetDupCount();

                    Log.log($"#{cnt}/{total}({cnt * 100 / total}%) 重複={dupCnt}\t({mFileList.Count})");

                    sw.Stop();
                    var ts_elapsed = sw.Elapsed;
                    Log.trc($"");
                    Log.trc($"途中経過：{ts_elapsed}");

                    sw.Start();
                }
                else if (cnt % 100 == 0)
                {
                    //Console.Error.Write(".");
                    System.Diagnostics.Debug.Write(".");
                    Log.print(".");
                }
            }

            var group = false;
            if (add_group && mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_PXV)
            {
                if (mFileList.Count > GROUP_FILE_NUM)
                {
                    group = true;
                }
            }
            else
            {
                group = false;
            }

            if (group)
            {
                var dirs = mFileList.GetDirs();

                foreach (var d in dirs)
                {
                    if (Directory.Exists(d))
                    {
                        var gi = new GroupItem(d);
                        // Log.trc($"{gi.FilePath}");

                        foreach (var file in mFileList.FileItems)
                        {
                            if (file.IsGroupEntry)
                            {
                                continue;
                            }

                            if (gi.FilePath == file.DirectoryName)
                            {
                                gi.AddFileItem(file);
                            }
                        }

                        if (gi.MemberCount() > 4)
                        {
                            mFileList.Add(gi);
                        }
                    }
                }
            }

            mFileList.Sort(SORT_TYPE.SORT_PATH);

            sw.Stop();
            var ts = sw.Elapsed;
            Log.trc($"ファイルリスト構築所要時間：{ts}");
        }

        private void AddRelativeFile(string f)
        {
            //TODO: ファイルサイズを比較し、ファイルサイズが大きい方を選択する？？？

            var tmp = f.Replace(mSeachWord, "");
            Log.log($"ファイル名:'{tmp}'/置換語:'{mSeachWord}'");
            if (File.Exists(tmp))
            {
                var tmpfi = new FileItem(tmp);
                tmpfi.Mark = true;
                mMarkCount++;
                mFileList.Add(tmpfi);
            }
            else
            {
                if (Path.GetExtension(tmp) == ".jpg")
                {
                    tmp = tmp.Replace(".jpg", ".png");
                }
                if (File.Exists(tmp))
                {
                    var tmpfi = new FileItem(tmp);
                    tmpfi.Mark = true;
                    mMarkCount++;
                    mFileList.Add(tmpfi);
                }
                else
                {
                    if (Path.GetExtension(tmp) == ".png")
                    {
                        tmp = tmp.Replace(".jpg", "");
                    }
                    if (File.Exists(tmp))
                    {
                        var tmpfi = new FileItem(tmp);
                        tmpfi.Mark = true;
                        mMarkCount++;
                        mFileList.Add(tmpfi);
                    }

                }
            }
        }

        private bool SpecFileP(string filename)
        {
            System.IO.FileInfo fi = new(filename);

            if (FileItem.isSpecifiedDateFile(fi, mDtFrom, mDtTo) == false)
            {
                return false;
            }

            if (FileItem.isAboveOfMaxFilesizeImage(fi, mMinFileSize) == false)
            {
                return false;
            }

            if (FileItem.isBelowOfMaxFilesizeImage(fi, mMaxFileSize) == false)
            {
                return false;
            }

            return true;
        }

        private bool IsTargetPic(FileItem fi)
        {
            if (fi.isSpecifiedSizeImage(mMaxPicSize) &&
                //fi.isSpecifiedPixelCount(mMaxPixelCnt) &&  ???
                fi.isSpecifiedPicOrinet(mTargetPicOrient))
            {
                return true;
            }
            return false;
        }

        private void BuildFileList_Zip()
        {
            //ZipEncodingChecker.DisplayZipEntryEncodings(mPath);

            var filelist = MyFiles.GetZipEntryList(mPath);

            mFileList.ZipList = true;
            foreach (var f in filelist)
            {
                //var fi = new FileItem(f, mPath);
                var fi = new ZipEntryItem(f, mPath);
                if (IsTargetPic(fi))
                {
                    mFileList.Add(fi);
                }
            }
        }

        private void BuildFileList_Tsv_Core(string tsv_file_path)
        {
            var line_cnt = 0;
            var missing_file_cnt = 0;
            var table = new Table(tsv_file_path);
            foreach (var line in table)
            {
                line_cnt++;

                /*
                var values = line.Split('\t');
                var picpath = values[0];
                var filesize = long.Parse(values[1]);
                var width = int.Parse(values[2]);
                var height = int.Parse(values[3]);
                var hash = values[4];
                */

                string picpath;
                long filesize;
                int width;
                int height;
                string hash;

                Table.SetParams(line,
                    out picpath,
                    out filesize,
                    out width,
                    out height,
                    out hash);

                try
                {
                    if (mSeachWord != "" && picpath.Contains(mSeachWord) == false)
                    {
                        continue;
                    }

                    if (!File.Exists(picpath))
                    {
                        missing_file_cnt++;
                        //Log.warning($"存在しないファイルを無視します '{picpath}'(#{missing_file_cnt})");
                        continue;
                    }

                    System.IO.FileInfo fi = new(picpath);
                    if (fi.Length != filesize)
                    {
                        Log.warning($"ファイルサイズが変更されています '{picpath}'({filesize} => {fi.Length})");
                    }

                    var fitem = new FileItem(picpath, filesize, width, height, hash);
                    if (SpecFileP(picpath) && IsTargetPic(fitem))
                    {
                        bool computeHash;
                        if (FilterType == FILTER_TYPE.FILTER_HASH)
                        {
                            computeHash = true;
                        }
                        else
                        {
                            computeHash = false;
                        }

                        mFileList.Add(fitem, computeHash);
                    }
                }
                catch (Exception ex)
                {
                    Log.err($"path='{picpath}'{ex}");
                }
            }

            if (missing_file_cnt > 0)
            {
                var percent = missing_file_cnt * 100 / line_cnt;
                Log.trc($"{line_cnt}行中{missing_file_cnt}ファイル({percent}%)が存在しませんでした。(存在={line_cnt - missing_file_cnt})");
            }
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public PictureModel DuplicateSelectOnly()
        {
            PictureModel newObj = new(mFileList);
            newObj.mPath = mPath;
            
            return newObj;
        }

        public FileList GetSelectedPic()
        {
            return mFileList.DupSel();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public int MakeThumbnail(int thumWidth, int thumHeight)
        {
            if (thumWidth == 0 || thumHeight == 0) return 0;// false;

            int idx = mIdx;
            int cnt;
            for (cnt = 0; cnt < mFileList.Count; cnt++)
            {
                FileItem item = mFileList[idx];
                //TODO: サムネイル再作成要否チェック（w,h)
                if (item.HasThumbnailImage())
                {
                }
                else
                {
                    //Log.log($"#{idx}:no thumbnail.");
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
                return -1;
            }

            var fitem = mFileList[idx];
            if (fitem.HasThumbnailImage())
            {
                Log.err($"??? !!! ??? #{idx}.");
            }
            else
            {
                //Log.log($"making thumbnail #{idx}.");
                var thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight, true);
            }

            //Log.trc("thumbnail make cont.");
            return idx;
        }

        public int CountIfHasThumbnail()
        {
            return this.mFileList.CountIfHasThumbnail();
        }

        //---------------------------------------------------------------------
        // 同一ディレクトリ内のファイルで前後に移動
        //---------------------------------------------------------------------
        public void ListPrev()
        {
            //if (false)
            {
                //Prev();//zantei
            }
            //else
            {
                mIdx = GetIdxOfPrevFileInSameDir();
                Update();
            }

        }

        public void ListNext()
        {
            //if (false)
            {
                ///Next();//zantei
            }
            //else
            {
                //次のファイルに移動（ディレクトリ内をループ）
                mIdx = GetIdxOfNextFileInSameDir();
                Update();
            }
        }

        //---------------------------------------------------------------------
        // 前のディレクトリの先頭ファイルに移動
        //---------------------------------------------------------------------
        public void ListUp()
        {

            PrevDirImage();
        }

        //---------------------------------------------------------------------
        // 次のディレクトリの先頭ファイルに移動
        //---------------------------------------------------------------------
        public void ListDown()
        {
            NextDirImage();
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
        public bool HasNext()
        {
            if (mIdx == mFileList.Count - 1)
            {
                return false;
            }
            return true;
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

        public void NextIfHasNext()
        {
            if (HasNext())
            {
                Next();
            }
        }

        public void PrevPage()
        {
            Prev();
        }

        public void NextPage()
        {
            var fitem1 = GetCurrentFileItem();
            var fitem2 = GetCurrentFileItemByRelativeIndex(1);

            var size1 = fitem1.GetImageSize();
            var size2 = fitem2.GetImageSize();

            if (PictureModel.IsMihiraki(size1, size2))
            {
                mIdx += 2;
            }
            else
            {
                mIdx++;
            }
                
            if (mIdx >= mFileList.Count)
            {
                mIdx = 0;
            }
            Update();
        }

        public static bool IsMihiraki(Size size1, Size size2)
        {
            return (size1.Width < size1.Height && size2.Width < size2.Height);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Up()
        {
            Log.trc($"[S] mIdx={mIdx}");
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
            Log.trc($"[E] mIdx={mIdx}");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void Down()
        {
            Log.trc($"[S] mIdx={mIdx}");
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
            Log.trc($"[E] mIdx={mIdx}");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void PageUp()
        {
            Log.trc($"[S] mIdx={mIdx}");
            mIdx -= PageCount;
            if (mIdx < 0)
            {
                mIdx += mFileList.Count;
                if (mIdx < 0)
                {
                    mIdx = 0;
                }
            }

            Update();
            Log.trc($"[E] mIdx={mIdx}");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void PageDown()
        {
            Log.trc($"[S] mIdx={mIdx}");
            mIdx += PageCount;
            if (mIdx >= mFileList.Count)
            {
                mIdx = mIdx - mFileList.Count;
                if (mIdx >= mFileList.Count)
                {
                    mIdx = mFileList.Count - 1;
                }
            }

            Update();
            Log.trc($"[E] mIdx={mIdx}");
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
        /// <summary>
        /// TODO: 現在表示中のフォルダの先頭のファイルに移動。すでに先頭だったならばその前のフォルダの先頭のファイルに移動
        /// </summary>
        //---------------------------------------------------------------------
        public void PrevDirImage()
        {
            string dirname = mFileList[mIdx].DirectoryName;

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                if (idx == 0)
                {
                    idx = mFileList.Count;
                }
                idx--;

                var dirname2 = mFileList[idx].DirectoryName;
                if (dirname2 != dirname)
                {
                    Log.trc($"{dirname}:{dirname2}");
                    mIdx = idx;
                    break;
                }
            } while (bak != idx);

            Update();
        }

        public void PrevDirTopImage()
        {
            PrevDirImage();//暫定
        }


        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void NextDirImage()
        {
            /*if (mFileList.Count == 1)
            {
                return;
            }

            string dirname = mFileList[mIdx].DirectoryName;

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                idx++;
                if (idx == mFileList.Count)
                {
                    idx = 0;
                }

                var dirname2 = mFileList[idx].DirectoryName;
                if (dirname2 != dirname)
                {
                    Log.trc($"{dirname}:{dirname2}");
                    mIdx = idx;
                    break;
                }
            } while (bak != idx);
            */
            mIdx = GetNextDiffDirFileIdx();

            Update();
        }

        private int GetNextDiffDirFileIdx()
        {
            var targetIdx = mIdx;
            var fi = mFileList[mIdx];
            string dirname;
            if (fi.IsGroupEntry)
            {
                dirname = fi.FilePath;
            }
            else
            {
                dirname = fi.DirectoryName;
            }

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                idx++;
                if (idx == mFileList.Count)
                {
                    idx = 0;
                }

                var dirname2 = mFileList[idx].DirectoryName;
                if (dirname2 != dirname)
                {
                    Log.trc($"[{mIdx}]:{dirname}:[{idx}]{dirname2}");
                    targetIdx = idx;
                    break;
                }
            } while (bak != idx);

            return targetIdx;
        }

        private int GetIdxOfNextFileInSameDir()
        {
            var targetIdx = mIdx;
            string dirname = mFileList[mIdx].DirectoryName;

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                idx++;
                if (idx == mFileList.Count)
                {
                    idx = 0;
                }

                var dirname2 = mFileList[idx].DirectoryName;
                if (dirname2 == dirname)
                {
                    Log.trc($"[{mIdx}]:{dirname}:[{idx}]{dirname2}");
                    targetIdx = idx;
                    break;
                }
                else
                {
                    //TODO: 先頭のファイルに移動
                }
            } while (bak != idx);

            return targetIdx;
        }

        private int GetIdxOfPrevFileInSameDir()
        {
            var targetIdx = mIdx;
            string dirname = mFileList[mIdx].DirectoryName;

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                if (idx == 0)
                {
                    idx = mFileList.Count;
                }
                idx--;

                var dirname2 = mFileList[idx].DirectoryName;
                if (dirname2 == dirname)
                {
                    Log.trc($"[{mIdx}]:{dirname}:[{idx}]{dirname2}");
                    targetIdx = idx;
                    break;
                }
                else
                {
                    //TODO: 末尾のファイルに移動
                }
            } while (bak != idx);


            return targetIdx;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void MoveToDirTopImage()
        {
            // TODO:

            Update();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void MoveToDirEndImage()
        {
            /*if (mFileList.Count == 1)
            {
                return;
            }*/

            string dirname;
            var fi = mFileList[mIdx];
            if (fi.IsGroupEntry)
            {
                dirname = fi.FilePath;
            }
            else
            {
                dirname = fi.DirectoryName;
            }

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                idx++;
                if (idx == mFileList.Count)
                {
                    idx = 0;
                }

                var dirname2 = mFileList[idx].DirectoryName;
                if (dirname2 != dirname)
                {
                    Log.trc($"{dirname}:{dirname2}");
                    mIdx = idx - 1;
                    if (mIdx < 0)
                    {
                        mIdx = 0;
                    }
                    break;
                }
            } while (bak != idx);

            // :現在ディレクトリ最後のファイルだった場合は、次のディレクトリに移動
            if (mIdx == bak)
            {
                mIdx++;
                if (mIdx == mFileList.Count)
                {
                    mIdx = 0;
                }
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

        public string GetRootPath() => mPath;

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public FileItem GetFileItem(int idx)
        {
            return mFileList[idx];
        }

        public FileItem GetCurrentFileItem()
        {
            return GetFileItem(mIdx);//mFileList[mIdx];
        }

        public string GetCurrentFilePath() => mFileList[mIdx].FilePath;

        public int GetCurrentFileIndex() => (mIdx + 1);

        public void SetCurrentFileIndex(int idx)
        {
            if (idx < 0 || idx > mFileList.Count)
            {
                Log.err($"idx={idx}");
                //throw new ArgumentOutOfRangeException(nameof(idx));
                mIdx = 0;
            }
            mIdx = idx;
        }

        public (int, int) GetIndex()
        {
            return (mIdx + 1, mFileList.Count);
        }

        public int GetAbsIdx(int relativeIndex)
        {
            int idx = mIdx + relativeIndex;
            if (idx >= 0)
            {
                idx = idx % mFileList.Count;
            }

            return idx;
        }

        public FileItem GetCurrentFileItemByRelativeIndex(int relativeIndex)
        {
            var i = GetAbsIdx(relativeIndex);

            return mFileList[i];
        }

        public bool IsLastNow()
        {
            return (mIdx >= mFileList.Count - 1);
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public string GetPictureInfoText(bool simple = false)
        {
            string txt;
            var fi = mFileList[mIdx];
            string fpath;
            if (simple)
            {
                fpath = fi.GetFilename();
            }
            else
            {
                fpath = fi.GetRelativePath(WorkingRootPath, true);
            }
            txt = String.Format("[{0,4}/{1}] {2} ({3})", mIdx + 1, mFileList.Count, fpath, fi.FileSizeDisp);

            return txt;
        }

        public string GetRelDirName(FileItem fi)
        {
            return fi.GetRelativePath(WorkingRootPath, false);
        }

        public string GetZipEntryname()
        {
            var fi = mFileList[mIdx];
            var txt = fi.GetZipEntryname();
            return txt;
        }

        public string GetArtistInfoFromTwtDB()
        {
            var item = GetCurrentFileItem();
            var ti = Twt.GetTweetInfoFromPath(item.FilePath);

            //TODO: DBから情報を取ってくるようにする

            var str = $"【】{ti.ScreenName}";
            return str;
        }

        public string GetArtistInfoFromPxvDB()
        {
            var item = GetCurrentFileItem();
            var pxv = new PxvArtist(item.FilePath);
            string txt;
            if (pxv.PxvID == 0)
            {
                txt = "★未登録★";
            }
            else
            {
                txt = String.Format(
                    $"DB情報:【{pxv.Rating}】{pxv.R18}|{pxv.Status}|{pxv.Warnings}|{pxv.DelInfo}|<{pxv.PxvName}({pxv.PxvID})>"
                 );
            }
            return txt;
        }

        public string GetArtistInfoFromDB()
        {
            switch (mDataSrcType)
            {
                case DATA_SOURCE_TYPE.DATA_SOURCE_PXV:
                    return GetArtistInfoFromPxvDB();
                case DATA_SOURCE_TYPE.DATA_SOURCE_TWT:
                    return GetArtistInfoFromTwtDB();
                default:
                    return "";
            }
        }


        public bool IsDiffDirNext(FileItem fitem, int relativeIndex)
        {
            var i = GetAbsIdx(relativeIndex);
            var fitem2 = mFileList[i];
            if (fitem.DirectoryName != fitem2.DirectoryName)
            {
                return true;
            }
            return false;
        }

        //毎回これやるのはいくらなんでも馬鹿すぎる
        public List<FileItem> GetSameDirFileItemList(int idx, ref int offsetoffset, out int currpos)
        {
            var fi = GetFileItem(idx);

            Log.trc($"idx={idx}");
            var startIdx = idx - 1;
            if (startIdx < 0)
            {
                startIdx = 0;
            }
            else
            {
                int i = idx - 1;
                for (; i >= 0; i--)
                {
                    var fi2 = GetFileItem(i);
                    if (fi.DirectoryName == fi2.DirectoryName)
                    {
                        startIdx = i;
                    }
                    else
                    {
                        startIdx = i + 1;
                        break;
                    }
                }
            }
            Log.trc($"startIdx={startIdx}");

            var endIdx = idx;
            for (int i = idx + 1; i < mFileList.Count; i++)
            {
                offsetoffset++;
                var fi2 = GetFileItem(i);
                if (fi.DirectoryName != fi2.DirectoryName)
                {
                    endIdx = i;
                    break;
                }
                else
                {
                    endIdx = i;
                }
            }
            Log.trc($"endIdx={endIdx}");

            List<FileItem> filelist = new List<FileItem>();
            for (int i = startIdx; i >= 0 && i < endIdx; i++)
            {
                filelist.Add(GetFileItem(i));
            }
            Log.trc($"size={filelist.Count}");

            currpos = idx - startIdx;

            return filelist;
        }

        public List<FileItem> GetSameDirFileItemList2(int idx)
        {
            List<FileItem> filelist = new List<FileItem>();

            var fi = GetFileItem(idx);

            for (int i = idx; i < mFileList.Count; i++)
            {
                var fi2 = GetFileItem(i);
                if (fi.DirectoryName != fi2.DirectoryName)
                {
                    break;
                }
                filelist.Add(GetFileItem(i));
            }
            return filelist;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public int Batch()
        {
            if (FilterType == FILTER_TYPE.FILTER_HASH)
            {
                //不要？mFileList.RegisterTweetDB();
            }

            string dst_root_path;
            if (mDstRootPath != null)
            {
                dst_root_path = mDstRootPath;
            }
            else
            {
                dst_root_path = mPath;
            }

            var cnt = mFileList.Batch(dst_root_path);
            mMarkCount = 0;
            return cnt;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void toggleMark()
        {
            var item = mFileList[mIdx];
            if (item.IsGroupEntry)
            {
                item.toggleMark();

                mMarkCount = mFileList.MarkCount();
            }
            else
            {
                //item.Mark = !item.Mark;
                item.toggleMark();

                if (item.Mark)
                {
                    mMarkCount++;
                }
                else
                {
                    mMarkCount--;
                }
            }
        }

        public void MarkAllSameDirFiles()
        {
            var fi = mFileList[mIdx];
            fi.Mark = true;
            mMarkCount++;

            var dirname = mFileList[mIdx].DirectoryName;

            var bak = mIdx;
            var idx = mIdx;
            do
            {
                idx++;
                if (idx == mFileList.Count)
                {
                    idx = 0;
                }

                fi = mFileList[idx];
                var dirname2 = fi.DirectoryName;
                if (dirname2 != dirname)
                {
                    Log.trc($"[{mIdx}]:{dirname}:[{idx}]{dirname2}");
                    break;
                }
                fi.Mark = true;
                mMarkCount++;
            } while (bak != idx);
        }

        internal void UnmarkAll()
        {
            mFileList.UnmarkAll();
        }

        public void MarkSameHashFiles()
        {
            mFileList.MarkSameHashFiles();
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void AddDelList()
        {
            mFileList[mIdx].Del = !mFileList[mIdx].Del;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void AddFavList()
        {
            mFileList[mIdx].Fav = !mFileList[mIdx].Fav;
        }

        public void LoadTsvFile()
        {

        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void UpdateListFile(bool desktop)
        {
            var path = DEL_LIST_TXT_FILENAME;//@"del_list.tsv";
            if (desktop)
            {
                var desktop_path = MyFiles.GetDesktopPath();//System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                path = MyFiles.PathCombine(desktop_path, path);
            }
            UpdateListFile(path);
        }

        public void UpdateListFile(string tsv_filepath)
        {
            var filename = tsv_filepath;
            var append = true;
            using (var writer = new StreamWriter(filename, append))
            {
                var sDate = GetTodayString();

                for (int cnt = 0; cnt < mFileList.Count; cnt++)
                {
                    var markStr = "";
                    var item = mFileList[cnt];
                    if (item.Del)
                    {
                        markStr = STR_METHOD_DEL;
                    }
                    else if (item.Fav)
                    {
                        markStr = STR_METHOD_FAV;
                    }

                    if (markStr != "")
                    {
                        var txt = item.GetTxtPath();
                        WriteFile(writer, txt, markStr, sDate);
                    }
                }
            }
        }

        public void WriteRatingInfo(string filename)
        {
            //var filename = DEL_LIST_TXT_FILENAME;
            var append = true;
            using (var writer = new StreamWriter(filename, append))
            {
                string txt = "";
                WriteFile(writer, txt, "UP");
            }
        }

        private void WriteFile(StreamWriter writer, string txt, string kwd, string date_str=null)
        {
            string sDate;
            if (date_str == null)
            {
                
                sDate = GetTodayString();
            }
            else
            {
                sDate = date_str;
            }

            writer.WriteLine(txt + "\t" + kwd + "\t" + sDate);
        }

        private string GetTodayString()
        {
            DateTime dt = DateTime.Now;
            var sDate = dt.ToString("yyyy/MM/dd HH:mm:ss");
            return sDate;
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void RemoveCurrentFile()
        {
            string path = mFileList[mIdx].FilePath;

            //MyFiles.moveToTrashDir(path, mPath);

            mFileList.RemoveAt(mIdx);
            //mMarkCount--;
        }

        public string WorkingRootPath
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

        public void Sort(SORT_TYPE sort_type)
        {
            mFileList.Sort(sort_type);
        }

        public string Stat()
        {
            var str = mFileList.TotalFileSizeOfSelectedFile();

            var ofilepath = Path.Combine(mPath, PIC_COMPARE_FILENAME);
            mFileList.WriteStatTsv(ofilepath);

            return str;
        }

        public void SaveFilesPath()
        {
            var txtpath = Path.Combine(mPath, FILELIST_FILENAME);
            using (var sw = new StreamWriter(txtpath, false))
            {
                foreach (FileItem file in mFileList)
                {
                    //Log.trc(file.FilePath);
                    sw.WriteLine(file.FilePath);
                }
            }
        }

        public void tsv()
        {
            long s_pxvid = 0;
            string s_twtid = "";

            if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_PXV)
            {
                //s_pxvid = Pxv.GetPxvID(mPath);
                s_pxvid = PictureModel.GetPxvIdFromPath(mPath);
            }
            else if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_TWT)
            {
                //s_twtid = Twt.GetScreenNameFromDirName(mPath);
                s_twtid = Twt.GetScreenNameFromPath(mPath);
            }

            var m = new TsvRowList(DEL_LIST_TXT_PATH);
            foreach (var x in m.GetRowList())
            {
                var filename = Path.GetFileName(x.FileName);
                if (s_pxvid != 0 && filename.StartsWith("px-"))
                {
                    //var pxvid = Pxv.GetPxvID(filename);
                    var pxvid = PictureModel.GetPxvIdFromPath(filename);
                    if (pxvid != 0 && pxvid == s_pxvid)
                    {
                        Log.trc(x.EntryName);
                        // pxv artwork idと画像番号の取得
                        // TODO:
                    }
                }
                else if (s_twtid != "" && filename.StartsWith("tw-"))
                {
                    var screen_name = Twt.GetScreenNameFromDirName(filename);
                    if (screen_name != null && s_twtid == screen_name)
                    {
                        var twt_info = Twt.GetTweetInfoFromPath(x.EntryName);
                        //Log.trc(twt_info.TweetID.ToString());

                        mFileList.SearchAndMark($"{twt_info.TweetID} {twt_info.ImageNo}");
                    }
                }
                else
                {
                }
            }

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
