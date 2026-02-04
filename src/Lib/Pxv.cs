using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    public class Pxv
    {
        private const string PXV_CURRENT_DIR_PXV_DIR_PATH = @"D:\download\PxDl";
        private const string PXV_ARCHIVE_DIR_PATH = @"D:\r18\dlPic\pxv";
        private const string PXV_ARCHIVE_DIR_TXT_FILEPATH = @"D:\r18\dlPic\pxv\dirlist.txt";

        private static Dictionary<long, string> PxvArchivePathDic = new();
        private static Dictionary<long, string> PxvCurrentPathDic = new();

        public static string[] GetPxvDirList()
        {
            var path = PXV_ARCHIVE_DIR_TXT_FILEPATH;
            if (File.Exists(path))
            {
                return System.IO.File.ReadAllLines(path);
            }
            var s = new string[0];
            return s;
        }

        static Pxv()
        {
            var lines = GetPxvDirList();
            foreach (var line in lines)
            {
                var pxv_usr_id = GetPxvID(line);
                if (pxv_usr_id != 0)
                {
                    PxvArchivePathDic[pxv_usr_id] = line;
                }
            }


            var dirs = Directory.EnumerateDirectories(PXV_CURRENT_DIR_PXV_DIR_PATH);
            foreach (var dir in dirs)
            {
                var pxv_usr_id = GetPxvID(dir);
                if (pxv_usr_id != 0)
                {
                    PxvCurrentPathDic[pxv_usr_id] = dir;
                }
                
            }
        }

        public static List<(string, long, long, string)> GetDirPaths(Dictionary<long, PxvArtist> dic)
        {
            var lines = GetPxvDirList();
            var dirPaths = new List<(string, long, long, string)>();
            foreach (var line in lines)
            {
                var pxvid = GetPxvID(line);
                if (dic.ContainsKey(pxvid))
                {
                    var d = dic[pxvid];
                    dirPaths.Add((line, d.Rating, d.Filenum, d.Status));
                }
            }

            return dirPaths;
        }

        public static List<string> GetDirPaths(string where)
        {
            var dic = new Dictionary<long, PxvArtist>();
            var pxv_artists = Sqlite.GetPxvArtists(where);
            foreach (var artist in pxv_artists)
            {
                dic.Add(artist.PxvID, artist);
            }

            var dirPaths = GetDirPaths(dic);
            var a = dirPaths
                .OrderBy(x => x.Item4)
                .ThenBy(x => -(x.Item2))
                .ThenBy(x => -(x.Item3))
                .ToList();
            List<string> stringList = a.ConvertAll(t => t.Item1);
            return stringList;
        }

        public static string Conv((string, long) x)
        {
            return x.Item1;
        }

        public static string GetArchiveDirPath(long pxv_user_id)
        {
            return GetPathFromDic(PxvArchivePathDic, pxv_user_id);
        }

        public static string GetCurrentDirPath(long pxv_user_id)
        {
            return GetPathFromDic(PxvCurrentPathDic, pxv_user_id);
        }

        public static string GetPathFromDic(Dictionary<long, string> dic, long pxv_user_id)
        {
            if (dic.ContainsKey(pxv_user_id))
            {
                return dic[pxv_user_id];
            }

            return "";
        }

        public static long GetPxvID(string path)
        {
            var pxvid = 0L;
            var pattern = @"\(#?(\d+)\)";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(path, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                pxvid = Int64.Parse(m.Groups[1].Value);

                return pxvid;
            }
            return 0;
        }

        public static string GuessPxvUserName(string path)
        {
            var pattern = @"\d{8}-(.+) \[\d+\]";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(path, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                var pxv_user_name = m.Groups[1].Value;

                return pxv_user_name;
            }
            return "";
        }
    }
}
