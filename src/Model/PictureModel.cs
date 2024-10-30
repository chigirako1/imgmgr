using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using PictureManagerApp.src.Lib;

using static System.Windows.Forms.LinkLabel;
using static PictureManagerApp.src.Model.FileList;

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

    public enum PIC_ORIENT_TYPE
    {
        PIC_ORINET_ALL,
        PIC_ORINET_PORTRAIT,
        PIC_ORINET_LANDSCAPE,

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
        THUMBNAIL_VIEW_TILE,
        THUMBNAIL_VIEW_OVERVIEW,
        THUMBNAIL_VIEW_LIST,
        THUMBNAIL_VIEW_NEXT,

        THUMBNAIL_VIEW_MAX,

        THUMBNAIL_VIEW_DEFAULT = THUMBNAIL_VIEW_TILE,

        THUMBNAIL_VIEW_NO_MAIN_SPLIT,

        //以下は除外
    }

    public enum FILTER_TYPE
    {
        FILTER_NONE,
        FILTER_HASH,

        FILTER_MAX,
    }

    public class PictureModel
    {
        private string mPath;
        //private SORT_TYPE mSortType;
        private FileList mFileList;
        private DATA_SOURCE_TYPE mDataSrcType;

        private int mIdx = -1;
        private DateTime? mDtFrom;
        private DateTime? mDtTo;
        private FILTER_TYPE FilterType = FILTER_TYPE.FILTER_NONE;

        private Size? mMaxPicSize = null;
        private int mMinFileSize = 0;
        private int mMaxFileSize = 0;
        private PIC_ORIENT_TYPE mTargetPicOrient = PIC_ORIENT_TYPE.PIC_ORINET_ALL;

        private static readonly string DEL_LIST_TXT_FILENAME = @"del_list.tsv";
        private string mExtList = ".jpg,.jpeg,.png,.gif,.bmp";
        private string mSeachWord = "";

        private static readonly string STR_METHOD_DEL = "DEL";
        private static readonly string STR_METHOD_FAV = "FAV";

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

        private TsvRowList mRowList;

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

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void SetMaxPicSize(Size size)
        {
            Log.log($"max pic size={size}");
            mMaxPicSize = size;
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


        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void BuildFileListFromText(string filelist, string rootPath = "")
        {
            Log.log($"[S] root=#{rootPath}");
            mPath = rootPath;

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
                    Log.err($"存在しないファイルを無視します #{pathtmp}");
                    continue;
                }
                var fi = new FileItem(pathtmp);
                mFileList.Add(fi);
            }

            Log.trc("[E]");
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

        public void BuildFileList(string path)
        {
            Log.log($"[S]:path={path}");

            mPath = path;

            if (mPath.Contains("PxDl") || mPath.Contains("/pxv/"))
            {
                mDataSrcType = DATA_SOURCE_TYPE.DATA_SOURCE_PXV;
            }
            else if (mPath.Contains("Twitter") || mPath.Contains("/twt/"))
            {
                mDataSrcType = DATA_SOURCE_TYPE.DATA_SOURCE_TWT;
            }
            else
            {
                mDataSrcType = DATA_SOURCE_TYPE.DATA_SOURCE_UNKNOWN;
            }


            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                BuildFileList_Dir();

                if (FilterType == FILTER_TYPE.FILTER_HASH)
                {
                    mFileList.multipleFileOnly();
                    mMarkCount = mFileList.MarkCount();
                }
            }
            else
            {
                BuildFileList_Zip();
            }

            Log.trc("[E]");
        }

        private void BuildFileList_Dir()
        {
            var fileArray = Directory.GetFiles(
                            mPath,
                            "*.*",//"*.jpg",
                            SearchOption.AllDirectories);

#if false
            var files = new List<string>();
            var extlist = mExtList.Split(",");
            foreach (var ext in extlist)
            {
                var tmpfiles = fileArray.Where(e => e.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
                files.AddRange(tmpfiles);
            }
#else
            var extensions = mExtList.Split(",");
            var files = fileArray.Where(f => extensions.Contains(Path.GetExtension(f)));
#endif

            if (mSeachWord != "")
            {
                files = files.Where(e => e.Contains(mSeachWord));
            }

            var total = files.Count();
            //Log.log($"ファイル数={total}");
            var cnt = 0;
            foreach (var f in files.OrderBy(x => x))
            {
                cnt++;
                if (cnt % 1000 == 0)
                {
                    Log.log($"#{cnt}/{total}");
                }

                if (SpecFileP(f))
                {
                    var fi = new FileItem(f);
                    if (fi.isSpecifiedSizeImage(mMaxPicSize) && fi.isSpecifiedPicOrinet(mTargetPicOrient))
                    {
                        //Log.log($"対象ファイル={f}");
                        bool computeHash;
                        if (FilterType == FILTER_TYPE.FILTER_HASH)
                        {
                            computeHash = true;
                        }
                        else
                        {
                            computeHash = false;
                        }
                        mFileList.Add(fi, computeHash);

                        if (mSeachWord != "")
                        {
                            var tmp = f.Replace(mSeachWord, "");
                            Log.log($"w2x:{tmp}/{mSeachWord}");
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
                    }
                    else
                    {
                        //Log.log($"非対象ファイル={f}");
                    }
                }
            }
        }

        private bool SpecFileP(string filename)
        {
            if (FileItem.isSpecifiedDateFile(filename, mDtFrom, mDtTo) == false)
            {
                return false;
            }

            if (FileItem.isAboveOfMaxFilesizeImage(filename, mMinFileSize) == false)
            {
                return false;
            }

            if (FileItem.isBelowOfMaxFilesizeImage(filename, mMaxFileSize) == false)
            {
                return false;
            }

            return true;
        }

        private void BuildFileList_Zip()
        {
            var filelist = MyFiles.GetZipEntryList(mPath);

            mFileList.ZipList = true;
            foreach (var f in filelist)//.OrderBy(x => x))
            {
                //if (FileItem.isSpecifiedFile(f, mDtFrom, mDtTo))
                //var fi = new FileItem(f);
                var fi = new FileItem(f, mPath);
                if (fi.isSpecifiedSizeImage(mMaxPicSize) && fi.isSpecifiedPicOrinet(mTargetPicOrient))
                {
                    mFileList.Add(fi);
                }
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
                return true;
            }

            FileItem fitem = mFileList[idx];
            if (fitem.HasThumbnailImage())
            {
                Log.err($"??? !!! ??? #{idx}.");
            }
            else
            {
                //Log.log($"making thumbnail #{idx}.");
                Image thumbImg = fitem.GetThumbnailImage(thumWidth, thumHeight, true);
            }

            //Log.trc("thumbnail make cont.");
            return false;
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
        public void PageUp()
        {
            Log.trc($"[S]{mIdx}");
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
            Log.trc($"[E]{mIdx}");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        public void PageDown()
        {
            Log.trc($"[S]{mIdx}");
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

        public string GetPictureInfoText(bool simple = false)
        {
            string txt;
            FileItem fi = mFileList[mIdx];
            string fpath;
            if (simple)
            {
                fpath = fi.GetFilename();
            }
            else
            {
                fpath = fi.GetRelativePath(WorkingRootPath, true);
            }
            txt = String.Format("[{0,3}/{1}] {2}", mIdx + 1, mFileList.Count, fpath);

            return txt;
        }

        public string GetZipEntryname()
        {
            var fi = mFileList[mIdx];
            var txt = fi.GetZipEntryname();
            return txt;
        }

        public string GetArtistInfoFromDB()
        {
            var item = GetCurrentFileItem();
            var pxv = new PxvArtist(item.FilePath);

            string txt;
            if (pxv.PxvID != 0)
            {
                 txt = String.Format("DB情報:【{0}】{1}|{2}<{3}({4})>{5}",
                     pxv.Rating,
                     pxv.R18,
                     pxv.Status,
                     pxv.PxvName,
                     pxv.PxvID,
                     pxv.Warnings
                  );
                //pxv.PxvName, pxv.PxvID, pxv.Rating, pxv.Status, pxv.R18);
            }
            else
            {
                txt = "★未登録★";
            }

            return txt;
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

        /*
        public Image GetImageByRelativeNo(int relativeNo)
        {
            var fitem = GetCurrentFileItemByRelativeIndex(relativeNo);
            return fitem.GetImage();
        }*/

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
        public void Batch()
        {
            if (FilterType == FILTER_TYPE.FILTER_HASH)
            {
                mFileList.RegisterTweetDB();
            }

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
        public void UpdateListFile()
        {
            var filename = DEL_LIST_TXT_FILENAME;
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

        public void WriteRatingInfo()
        {
            var filename = DEL_LIST_TXT_FILENAME;
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

        public void tsv()
        {
            long s_pxvid = 0;
            string s_twtid = "";

            if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_PXV)
            {
                s_pxvid = Pxv.GetPxvID(mPath);
            }
            else if (mDataSrcType == DATA_SOURCE_TYPE.DATA_SOURCE_TWT)
            {
                //s_twtid = Twt.GetScreenNameFromDirName(mPath);
                s_twtid = Twt.GetScreenNameFromPath(mPath);
            }

            var m = new TsvRowList(@"D:\download\del_list.tsv");
            foreach (var x in m.hoge())
            {
                var filename = Path.GetFileName(x.FileName);
                if (s_pxvid != 0 && filename.StartsWith("px-"))
                {
                    var pxvid = Pxv.GetPxvID(filename);
                    if (pxvid != 0 && pxvid == s_pxvid)
                    {
                        Log.trc(x.EntryName);
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
