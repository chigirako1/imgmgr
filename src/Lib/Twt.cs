using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    class Twt
    {
        private const string TWT_CURRENT_DIR_TWT_DIR_PATH = @"D:\dl\AnkPixiv\Twitter";
        private const string TWT_ARCHIVE_DIR_TWT_FILEPATH = @"D:\r18\dlPic\twitter\dirlist.txt";
        private static Dictionary<string, string>  TwtArchivePathDic = new ();
        private static Dictionary<string, string> TwtCurrentPathDic = new();

        public static string[] GetTwtDirList()
        {
            return System.IO.File.ReadAllLines(TWT_ARCHIVE_DIR_TWT_FILEPATH);
        }

        static Twt()
        {
            TwtArchivePathDic = new Dictionary<string, string>();

            var lines = GetTwtDirList();
            foreach (var line in lines)
            {
                var scrn_name = GetScreenNameFromPath(line);
                if (scrn_name != "")
                {
                    //TBD:大文字・小文字するようにどっちかに統一する？
                    TwtArchivePathDic[scrn_name] = line;
                }
            }


            var dirs = Directory.EnumerateDirectories(TWT_CURRENT_DIR_TWT_DIR_PATH);
            foreach (var dir in dirs)
            {
                var sepa = Path.DirectorySeparatorChar;
                var dirnames = dir.Split(sepa);
                var dirname = dirnames[dirnames.Length - 1];

                TwtCurrentPathDic[dirname] = dir;
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

        private static string GetScreenNameFromPath(string path)
        {
            var dirnames = path.Split('/');
            var dirname = dirnames[dirnames.Length - 1];

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
    }
}
