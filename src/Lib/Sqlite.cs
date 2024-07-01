using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;
using static System.Windows.Forms.LinkLabel;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PictureManagerApp.src.Lib
{
    public class PxvArtist
    {
        public long PxvID { set; get; }
        public string PxvName { set;  get; }
        public long Rating{ set; get; }
        public string R18 { set; get; }
        public string Feature { set; get; }

        public long Filenum { set; get; }
        public string Status { set; get; }


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
        }
    }

    public class Pxv
    {
        private const string PXV_ARCHIVE_DIR_PATH = @"D:\r18\dlPic\pxv";
        private const string PXV_ARCHIVE_DIR_TXT_FILEPATH = @"D:\r18\dlPic\pxv\dirlist.txt";

        public static List<string> GetDirPaths(string where)
        {
            var dic = new Dictionary<long, PxvArtist>();
            var pxv_artists = Sqlite.GetPxvArtists(where);
            foreach (var artist in pxv_artists)
            {
                dic.Add(artist.PxvID, artist);
            }

            string[] lines = System.IO.File.ReadAllLines(PXV_ARCHIVE_DIR_TXT_FILEPATH);
            var dirPaths = new List<(string, long)>();
            foreach (var line in lines)
            {
                var pxvid = GetPxvID(line);
                if (dic.ContainsKey(pxvid))
                {
                    var d = dic[pxvid];
                    dirPaths.Add((line, d.Filenum));
                }
            }

            var a = dirPaths.OrderBy(x => -(x.Item2)).ToList();
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

    public class Sqlite
    {
        //private const string DATA_SRC = @"D:\data\src\vs_cs\development.sqlite3";
        //private const string DATA_SRC = @"D:\data\src\ror\myapp\db\development - bak240324.sqlite3";
        private const string DATA_SRC = @"D:\data\src\ror\myapp\db\development.sqlite3";

        public static void GetPxvArtistInfo(int pxvid, PxvArtist pxvartist)
        {
            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = DATA_SRC };
            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    //cmd.CommandText = "select sqlite_version()";
                    //result = (string)cmd.ExecuteScalar();

                    cmd.CommandText = $"SELECT * FROM artists WHERE pxvid = {pxvid}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        SetPxvArtistInfo(reader, pxvartist);
                    }
                }
            }
        }

        public static string SQL_STATEMENT_WHERE_NO_UPDATE = "status != ''";
        public static string SQL_STATEMENT_WHERE_3D = "feature = '3D'";
        public static string SQL_STATEMENT_WHERE_AI = "feature = 'AI'";
        public static string SQL_STATEMENT_WHERE_AI_SUSPEND = "feature = 'AI' and status = '停止'";
        public static string SQL_STATEMENT_WHERE_AI_NO_UPDATE = "feature = 'AI' and status != ''";

        public static List<PxvArtist> GetPxvArtists(string where_phrase)
        {
            var pxv_artists = new List<PxvArtist>();

            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = DATA_SRC };
            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = $"SELECT * FROM artists WHERE " + where_phrase;
                    using (var reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            int cnt = 0;
                            while (reader.Read())
                            {
                                //var pxvid = (long)reader["pxvid"];
                                //var recent_filenum = (long)reader["recent_filenum"];
                                //var filenum = (long)reader["filenum"];
                                cnt++;

                                var pxvartist = new PxvArtist();
                                pxvartist.SetPxvArtistParam(reader);
                                pxv_artists.Add(pxvartist);

                                //Log.trc($"{cnt} {pxvartist.PxvName}({pxvartist.PxvID})=|{{{pxvartist.Filenum}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.trc(ex.Message);
                        }
                    }
                }
            }

            return pxv_artists;
        }

        private static void SetPxvArtistInfo(SQLiteDataReader reader, PxvArtist pxvartist)
        {
            try
            {
                if (reader.Read())
                {
                    pxvartist.SetPxvArtistParam(reader);
                }
                else
                {
                    Log.trc("no read ");
                }
            }
            catch (Exception ex)
            {
                Log.trc(ex.Message);
            }
        }

    }
}
