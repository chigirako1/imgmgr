using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    struct TweetInfo
    {
        public long TweetID;
        public int ImageNo;
        public string ScreenName;
        //public DateTime dlDate;

        override public string ToString()
        {
            return $"@{ScreenName}:{TweetID} {ImageNo}";
        }
    }

    public struct TwitterUserInfo
    {
        public string ScreenName;
        public string UserName;
        public long Rating;

        public TwitterUserInfo()
        {
            ScreenName = "";
            UserName = "";
            Rating = 0;
        }
    }

    class Twt
    {
        private const string TWT_CURRENT_DIR_TWT_DIR_PATH = @"D:\dl\AnkPixiv\Twitter";
        private const string TWT_ARCHIVE_DIR_TWT_FILEPATH = @"D:\r18\dlPic\twitter\dirlist.txt";
        private static Dictionary<string, string>  TwtArchivePathDic = new ();
        private static Dictionary<string, string> TwtCurrentPathDic = new();

        public static string[] GetTwtDirList()
        {
            var path = TWT_ARCHIVE_DIR_TWT_FILEPATH;
            if (File.Exists(path))
            {
                var lines = System.IO.File.ReadAllLines(path);
                return lines;
            }
            var s = new string[0];
            return s;
        }

        static Twt()
        {
            var lines = GetTwtDirList();
            foreach (var line in lines)
            {
                var scrn_name = GetScreenNameFromDirPath(line);
                if (scrn_name != "")
                {
                    //TBD:大文字・小文字するようにどっちかに統一する？
                    TwtArchivePathDic[scrn_name] = line;
                }
            }

            if (Directory.Exists(TWT_CURRENT_DIR_TWT_DIR_PATH))
            {
                var dirs = Directory.EnumerateDirectories(TWT_CURRENT_DIR_TWT_DIR_PATH);
                foreach (var dir in dirs)
                {
                    var sepa = Path.DirectorySeparatorChar;
                    var dirnames = dir.Split(sepa);
                    var dirname = dirnames[dirnames.Length - 1];

                    TwtCurrentPathDic[dirname] = dir;
                }
            }
        }

        public static string GetArchiveDirPath(string screen_name)
        {
            return GetPathFromDic(TwtArchivePathDic, screen_name);
        }

        public static string GetCurrentDirPath(string screen_name)
        {
            return GetPathFromDic(TwtCurrentPathDic, screen_name);
        }

        public static string GetPathFromDic(Dictionary<string, string> dic, string screen_name)
        {
            if (dic.ContainsKey(screen_name))
            {
                return dic[screen_name];
            }

            return "";
        }

        public static string GetScreenNameFromDirName(string dirname)
        {
            var screen_name = "";
            var pattern = @"^([\w_]+)";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(dirname, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                screen_name = m.Groups[1].Value;
                break;
            }
            //Log.trc($"'{path}':'{screen_name}'");
            return screen_name;
        }

        public static string GetScreenNameFromDirPath(string dirpath)
        {
            char[] sepa =
            [
                '/',
                Path.DirectorySeparatorChar,
            ];
            var dirnames = dirpath.Split(sepa);
            var dirname = dirnames[dirnames.Length - 1];

            return GetScreenNameFromDirName(dirname);
        }

        public static string GetScreenNameFromFilepath(string filepath)
        {
            var dirname = Path.GetDirectoryName(filepath);

            //TODO:パスからだとスクリーンネーム取得困難
            var dirname2 = Path.GetFileName(dirname);//BUG
            return GetScreenNameFromDirName(dirname2);
        }

        public static string GetScreenNameFromZipName(string path)
        {
            var pattern = @"@(\w+)";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(path, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                var screen_name = m.Groups[1].Value;
                return screen_name;
            }
            return null;
        }

        public static TweetInfo GetTweetInfoFromPath(string path)
        {
            var filename = Path.GetFileName(path);
            var screenname = GetScreenNameFromFilepath(path);
            var tweetinfo = new TweetInfo();
            tweetinfo.ScreenName = screenname;

            var pattern = @"(\d+)\s+(\d+)\s+(\d+-\d+\d+)";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(filename, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                tweetinfo.TweetID = long.Parse(m.Groups[1].Value);
                tweetinfo.ImageNo = int.Parse(m.Groups[2].Value);
                //tweetinfo.dlDate = DateTime.Parse(m.Groups[3].Value);
                return tweetinfo;
                //break;
            }

            //20231108 17150226604498206x9 0
            var pattern2 = @"(\d{8})\s+(\d+)\s+(\d)";
            var mc2 = System.Text.RegularExpressions.Regex.Matches(filename, pattern2);
            foreach (System.Text.RegularExpressions.Match m in mc2)
            {
                //tweetinfo.dlDate = DateTime.Parse(m.Groups[1].Value);
                tweetinfo.TweetID = long.Parse(m.Groups[2].Value);
                tweetinfo.ImageNo = int.Parse(m.Groups[3].Value);
                return tweetinfo;
                //break;
            }

            //Log.dbg($"'{path}':'{tweetinfo}'");
            return tweetinfo;
        }
    }
}
