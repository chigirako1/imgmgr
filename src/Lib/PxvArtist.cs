using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureManagerApp.src.Lib
{
    public class PxvArtist
    {
        public long PxvID { set; get; }
        public string PxvName { set; get; }
        public long Rating { set; get; }
        public string R18 { set; get; }
        public string Feature { set; get; }

        public long Filenum { set; get; }
        public string Status { set; get; }

        public string Warnings { set; get; }


        public PxvArtist()
        {

        }

        public PxvArtist(string path)
        {
            var pattern = @"\(#?(\d+)\)";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(path, pattern);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                var pxvid = Int32.Parse(m.Groups[1].Value);
                Sqlite.GetPxvArtistInfo(pxvid, this);
                break;
            }
        }

        public void SetPxvArtistParam(SQLiteDataReader reader)
        {
            PxvID = (long)reader["pxvid"];
            PxvName = (string)reader["pxvname"];
            Rating = (long)reader["rating"];
            R18 = (string)reader["r18"];
            Feature = (string)reader["feature"];
            Filenum = (long)reader["filenum"];
            Status = (string)reader["status"];
            if (reader["warnings"] != DBNull.Value)
            {
                Warnings = (string)reader["warnings"];
            }
        }
    }
}
