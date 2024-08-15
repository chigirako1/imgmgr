using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    public class Pxv
    {
        private const string PXV_ARCHIVE_DIR_PATH = @"D:\r18\dlPic\pxv";
        private const string PXV_ARCHIVE_DIR_TXT_FILEPATH = @"D:\r18\dlPic\pxv\dirlist.txt";

        public static string[] GetPxvDirList()
        {
            return System.IO.File.ReadAllLines(PXV_ARCHIVE_DIR_TXT_FILEPATH);
        }

        public static List<(string, long, long)> GetDirPaths(Dictionary<long, PxvArtist> dic)
        {
            var lines = GetPxvDirList();
            var dirPaths = new List<(string, long, long)>();
            foreach (var line in lines)
            {
                var pxvid = GetPxvID(line);
                if (dic.ContainsKey(pxvid))
                {
                    var d = dic[pxvid];
                    dirPaths.Add((line, d.Rating, d.Filenum));
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
                .OrderBy(x => -(x.Item2))
                .ThenBy(x => -(x.Item3))
                .ToList();
            List<string> stringList = a.ConvertAll(t => t.Item1);
            return stringList;
        }

        public static string Conv((string, long) x)
        {
            return x.Item1;
        }

        private static long GetPxvID(string path)
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
    }
}
