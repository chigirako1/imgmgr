using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    public class PxvArtwork
    {
        private int _artwork_id;
        private string _artwork_title;
        private DateTime _artwork_date;

        public PxvArtwork(string filepath)
        {
            var (date, title, artwork_id) = GetPxvArtworkInfoFromPath(filepath);
            _artwork_date = (DateTime)date;
            _artwork_title = title;
            _artwork_id = artwork_id;
        }

        static public string GetPxvArtworkTitleFromPath(string path)
        {
            var (_, title, _) = GetPxvArtworkInfoFromPath(path);
            return title;
        }

        static public (DateTime?, string, int) GetPxvArtworkInfoFromPath(string path, string regex_str = @"(\d\d-\d\d-\d\d)\s+(.*)\(\d+\)")
        {
            Regex rgx = new Regex(regex_str);
            Match m = rgx.Match(path);
            if (m.Success)
            {
                var date = DateTime.Parse(m.Groups[1].Value);
                var title = m.Groups[2].Value;
                var artwork_id = int.Parse(m.Groups[3].Value);
                Log.dbg($"{title}\t{path}\t{date}");
                return (date, title, artwork_id);
            }
            else
            {
                Log.warning($"rgx no match:'{path}'");
                return (null, "", 0);
            }
        }
    }
}
